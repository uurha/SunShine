using System;
using Base.Inject;
using NaughtyCharacter.CharacterSettingsModel;
using NaughtyCharacter.Helpers;
using NaughtyCharacter.MovementModule.PlayerSystem.Interfaces;
using NaughtyCharacter.Utility;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.EnvironmentSystem.Interfaces
{
    public interface IMovementEnvironment : IInjectReceiver<GroundSettings>,
                                            IInjectReceiver<RotationSettings>, IInjectReceiver<GravitySettings>,
                                            IInjectReceiver<MovementSettings>
    {

        public bool IsGrounded { get; }

        public void Initialize();

        public void OnPreMove(float deltaTime);

        public void OnUpdate(Vector3 movementInput, Vector2 rotationInput, bool jumpInput);

        public Vector3 CalculateMove();

        public void OnPostMove(Vector3 position);

        public Quaternion CalculateRotation(Quaternion transformRotation);

        Vector2 ClampRotationFunc(Vector2 rotationInput);

        Vector3 ClampMovementFunc(Vector3 movementInput);
    }

    public class GroundMovement : IMovementEnvironment
    {
        private bool _isGrounded = true;

        /// <summary>
        /// In meters/second
        /// </summary>
        private float _targetHorizontalSpeed;

        /// <summary>
        /// In meters/second
        /// </summary>
        private float _horizontalSpeed;

        /// <summary>
        /// In meters/second
        /// </summary>
        private float _verticalSpeed;

        /// <summary>
        /// In meters/second
        /// </summary>
        private bool _justWalkedOffEdge;

        private Vector3 _movementInput;
        private Vector2 _rotationInput;
        private bool _hasMovementInput;
        private bool _jumpInput;
        
        private Vector3 _lastMovementInput;
        private Vector3 _movementVector;

        private MovementSettings _movementSettings;
        private GravitySettings _gravitySettings;
        private RotationSettings _rotationSettings;
        private GroundSettings _groundSettings;
        private float _deltaTime;

        public bool IsGrounded => _isGrounded;

        public void Initialize()
        {
            
        }

        public void OnUpdate(Vector3 movementInput, Vector2 rotationInput, bool jumpInput)
        {
            SetMovementInput(movementInput);
            SetRotationInput(rotationInput);
            _jumpInput = jumpInput;
        }

        private void SetRotationInput(Vector2 rotationInput)
        {
            _rotationInput = rotationInput;
        }

        public void OnPreMove(float deltaTime)
        {
            _deltaTime = deltaTime;
            UpdateHorizontalSpeed(_deltaTime);
            UpdateVerticalSpeed(_deltaTime);
        }

        private void UpdateHorizontalSpeed(float deltaTime)
        {
            var movementInput = _movementInput;

            if (movementInput.sqrMagnitude > 1.0f)
            {
                movementInput.Normalize();
            }
            _targetHorizontalSpeed = movementInput.magnitude * _movementSettings.MaxHorizontalSpeed;
            var acceleration = _hasMovementInput ? _movementSettings.Acceleration : _movementSettings.Deceleration;
            _horizontalSpeed = Mathf.MoveTowards(_horizontalSpeed, _targetHorizontalSpeed, acceleration * deltaTime);
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

        public Quaternion CalculateRotation(Quaternion currentRotation)
        {
            return OrientToTargetRotation(_movementVector.SetY(0.0f), currentRotation, _deltaTime);
        }

        private Quaternion OrientToTargetRotation(Vector3 horizontalMovement, Quaternion currentRotation,
                                                  float deltaTime)
        {
            var targetRotation = Quaternion.identity;

            switch (_rotationSettings.RotationBehavior)
            {
                case RotationBehavior.OrientRotationToMovement when horizontalMovement.sqrMagnitude > 0.0f:
                {
                    var target = Quaternion.LookRotation(horizontalMovement, Vector3.up);

                    targetRotation =
                        Quaternion.RotateTowards(currentRotation, target, RotationSpeed() * deltaTime);
                    break;
                }
                case RotationBehavior.UseControlRotation:
                {
                    targetRotation = Quaternion.Euler(0.0f, _rotationInput.y, 0.0f);
                    break;
                }
            }
            return targetRotation;
        }

        private void SetMovementInput(Vector3 movementInput)
        {
            var hasMovementInput = movementInput.sqrMagnitude > 0.0f;

            if (_hasMovementInput && !hasMovementInput)
            {
                _lastMovementInput = _movementInput;
            }
            _movementInput = movementInput;
            _hasMovementInput = hasMovementInput;
        }

        private float RotationSpeed()
        {
            return Mathf.Lerp(_rotationSettings.MaxRotationSpeed, _rotationSettings.MinRotationSpeed,
                              _horizontalSpeed / _targetHorizontalSpeed);
        }

        private void UpdateVerticalSpeed(float deltaTime)
        {
            if (_isGrounded)
            {
                _verticalSpeed = -_gravitySettings.GroundedGravity;
                if (!_jumpInput) return;
                _verticalSpeed = _movementSettings.JumpSpeed;
                _isGrounded = false;
            }
            else
            {
                if (!_jumpInput &&
                    _verticalSpeed > 0.0f)
                {
                    // This is what causes holding jump to jump higher than tapping jump.
                    _verticalSpeed = Mathf.MoveTowards(_verticalSpeed, -_gravitySettings.MaxFallSpeed,
                                                       _movementSettings.JumpAbortSpeed * deltaTime);
                }
                else if (_justWalkedOffEdge)
                {
                    _verticalSpeed = 0.0f;
                }

                _verticalSpeed = Mathf.MoveTowards(_verticalSpeed, -_gravitySettings.MaxFallSpeed,
                                                   _gravitySettings.Gravity * deltaTime);
            }
        }

        public Vector3 CalculateMove()
        {
            return _movementVector = (_horizontalSpeed * GetMovementDirection() + _verticalSpeed * Vector3.up) * _deltaTime;
        }

        public void OnPostMove(Vector3 position)
        {
            UpdateGrounded(position);
        }

        private bool CheckGrounded(Vector3 position)
        {
            var spherePosition = position;
            spherePosition.y = spherePosition.y + _groundSettings.SphereCastRadius - _groundSettings.SphereCastDistance;

            var isGrounded = Physics.CheckSphere(spherePosition, _groundSettings.SphereCastRadius,
                                                 _groundSettings.GroundLayers, QueryTriggerInteraction.Ignore);
            return isGrounded;
        }

        private void UpdateGrounded(Vector3 position)
        {
            _justWalkedOffEdge = false;
            var isGrounded = CheckGrounded(position);

            if (_isGrounded &&
                !isGrounded &&
                !_jumpInput)
            {
                _justWalkedOffEdge = true;
            }
            _isGrounded = isGrounded;
        }

        public Vector2 ClampRotationFunc(Vector2 rotationInput)
        {
            var pitchAngle = rotationInput.x;
            pitchAngle = Mathf.Clamp(pitchAngle, _rotationSettings.MinPitchAngle, _rotationSettings.MaxPitchAngle);
            return rotationInput.SetX(pitchAngle);
        }

        public Vector3 ClampMovementFunc(Vector3 movementInput)
        {
            return movementInput;
        }
        
        public void Inject(GroundSettings reference)
        {
            _groundSettings = reference;
        }

        public void Inject(RotationSettings reference)
        {
            _rotationSettings = reference;
        }

        public void Inject(GravitySettings reference)
        {
            _gravitySettings = reference;
        }

        public void Inject(MovementSettings reference)
        {
            _movementSettings = reference;
        }
    }
}
