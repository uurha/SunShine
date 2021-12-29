using CorePlugin.Attributes.EditorAddons.SelectAttributes;
using NaughtyCharacter.Scripts.CharacterSettingsModel;
using NaughtyCharacter.Scripts.PlayerControllerSystem.Interfaces;
using NaughtyCharacter.Scripts.Utility;
using UnityEngine;

namespace NaughtyCharacter.Scripts.CharacterSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private PlayerInputComponent playerInputComponent;
        [SerializeField] private Animator playerAnimator;
		[SerializeReference] [SelectImplementation] private IPlayerController controller; // The controller that controls the character
        [SerializeReference] [SelectImplementation] private IPlayerCamera playerCamera;
        [SerializeReference] [SelectImplementation] private ICharacterAnimator characterAnimator;
		[SerializeField] private MovementSettings movementSettings;
		[SerializeField] private GravitySettings gravitySettings;
		[SerializeField] private RotationSettings rotationSettings;
		[SerializeField] private GroundSettings groundSettings;

		private CharacterController _characterController; // The Unity's CharacterController
		
        private Data _data;

		private float _targetHorizontalSpeed; // In meters/second
		private float _horizontalSpeed; // In meters/second
		private float _verticalSpeed; // In meters/second
		private bool _justWalkedOffALedge;

		private Vector2 _controlRotation; // X (Pitch), Y (Yaw)
		private Vector3 _movementInput;
		private Vector3 _lastMovementInput;
		private bool _hasMovementInput;
		private bool _jumpInput;
        private bool _isGrounded = true;

        public struct Data
        {
            public Vector3 Velocity { get; }
            public Vector3 HorizontalVelocity  { get; }
            public Vector3 VerticalVelocity  { get; }
            public bool IsGrounded { get; }

            public Data(CharacterController character, bool isGrounded)
            {
                var velocity = character.velocity;
                Velocity = velocity;
                IsGrounded = isGrounded;
                HorizontalVelocity = velocity.SetY(0.0f);;
                VerticalVelocity = Vector3.Scale(velocity,Vector3.up);;
            }
        }

        private void Awake()
		{
			controller.Inject(this);
			controller.Inject(playerInputComponent);

			_characterController = GetComponent<CharacterController>();
            _data = new Data(_characterController, true);
            characterAnimator.Inject(playerAnimator);
            characterAnimator.Inject(movementSettings);
		}

		private void Update()
		{
			controller?.OnCharacterUpdate();
		}

		private void FixedUpdate()
		{
			Tick(Time.deltaTime);
            playerCamera?.SetPosition(transform.position);
            playerCamera?.SetControlRotation(GetControlRotation());
		}

		private void Tick(float deltaTime)
		{
			UpdateHorizontalSpeed(deltaTime);
			UpdateVerticalSpeed(deltaTime);

			var movement = _horizontalSpeed * GetMovementDirection() + _verticalSpeed * Vector3.up;
			_characterController.Move(movement * deltaTime);

			OrientToTargetRotation(movement.SetY(0.0f), deltaTime);

			UpdateGrounded();
            _data = new Data(_characterController, _isGrounded);
			characterAnimator.UpdateState(_data);
		}

		public void SetMovementInput(Vector3 movementInput)
		{
			var hasMovementInput = movementInput.sqrMagnitude > 0.0f;

			if (_hasMovementInput && !hasMovementInput)
			{
				_lastMovementInput = _movementInput;
			}

			_movementInput = movementInput;
			_hasMovementInput = hasMovementInput;
		}

		public void SetJumpInput(bool jumpInput)
		{
			_jumpInput = jumpInput;
		}

		public Vector2 GetControlRotation()
		{
			return _controlRotation;
		}

		public void SetControlRotation(Vector2 controlRotation)
		{
			// Adjust the pitch angle (X Rotation)
			var pitchAngle = controlRotation.x;
			pitchAngle %= 360.0f;
			pitchAngle = Mathf.Clamp(pitchAngle, rotationSettings.MinPitchAngle, rotationSettings.MaxPitchAngle);

			// Adjust the yaw angle (Y Rotation)
			var yawAngle = controlRotation.y;
			yawAngle %= 360.0f;

			_controlRotation = new Vector2(pitchAngle, yawAngle);
		}

		private bool CheckGrounded()
		{
			var spherePosition = transform.position;
			spherePosition.y = spherePosition.y + groundSettings.SphereCastRadius - groundSettings.SphereCastDistance;
			var isGrounded = Physics.CheckSphere(spherePosition, groundSettings.SphereCastRadius, groundSettings.GroundLayers, QueryTriggerInteraction.Ignore);

			return isGrounded;
		}

		private void UpdateGrounded()
		{
			_justWalkedOffALedge = false;

			var isGrounded = CheckGrounded();
			if (_data.IsGrounded && !isGrounded && !_jumpInput)
			{
				_justWalkedOffALedge = true;
			}

            _isGrounded = isGrounded;
		}

		private void UpdateHorizontalSpeed(float deltaTime)
		{
			var movementInput = _movementInput;
			if (movementInput.sqrMagnitude > 1.0f)
			{
				movementInput.Normalize();
			}

			_targetHorizontalSpeed = movementInput.magnitude * movementSettings.MaxHorizontalSpeed;
			var acceleration = _hasMovementInput ? movementSettings.Acceleration : movementSettings.Deceleration;

			_horizontalSpeed = Mathf.MoveTowards(_horizontalSpeed, _targetHorizontalSpeed, acceleration * deltaTime);
		}

		private void UpdateVerticalSpeed(float deltaTime)
		{
			if (_data.IsGrounded)
			{
				_verticalSpeed = -gravitySettings.GroundedGravity;
                if (!_jumpInput) return;
                _verticalSpeed = movementSettings.JumpSpeed;
                _isGrounded = false;
            }
			else
			{
				if (!_jumpInput && _verticalSpeed > 0.0f)
				{
					// This is what causes holding jump to jump higher than tapping jump.
					_verticalSpeed = Mathf.MoveTowards(_verticalSpeed, -gravitySettings.MaxFallSpeed, movementSettings.JumpAbortSpeed * deltaTime);
				}
				else if (_justWalkedOffALedge)
				{
					_verticalSpeed = 0.0f;
				}

				_verticalSpeed = Mathf.MoveTowards(_verticalSpeed, -gravitySettings.MaxFallSpeed, gravitySettings.Gravity * deltaTime);
			}
		}

		private Vector3 GetMovementDirection()
		{
			var moveDir = _hasMovementInput ? _movementInput : _lastMovementInput;
			if (moveDir.sqrMagnitude > 1f)
			{
				moveDir.Normalize();
			}

			return moveDir;
		}

		private void OrientToTargetRotation(Vector3 horizontalMovement, float deltaTime)
        {
            switch (rotationSettings.RotationBehavior)
            {
                case RotationBehavior.OrientRotationToMovement when horizontalMovement.sqrMagnitude > 0.0f:
                {
                    var rotationSpeed = Mathf.Lerp(
                                                   rotationSettings.MaxRotationSpeed, rotationSettings.MinRotationSpeed, _horizontalSpeed / _targetHorizontalSpeed);

                    var targetRotation = Quaternion.LookRotation(horizontalMovement, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * deltaTime);
                    break;
                }
                case RotationBehavior.UseControlRotation:
                {
                    var targetRotation = Quaternion.Euler(0.0f, _controlRotation.y, 0.0f);
                    transform.rotation = targetRotation;
                    break;
                }
            }
        }
	}
}
