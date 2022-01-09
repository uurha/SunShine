using NaughtyCharacter.AnimationSystem;
using NaughtyCharacter.CharacterSystem;
using NaughtyCharacter.CharacterSystem.Models;
using NaughtyCharacter.Helpers;
using NaughtyCharacter.MovementModule.EnvironmentSystem.Interfaces;
using NaughtyCharacter.MovementModule.PlayerSystem.Interfaces;
using NaughtyCharacter.Utility;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.EnvironmentSystem
{
    public abstract class MovementEnvironment : IMovementEnvironment
    {
        /// <summary>
        /// In meters/second
        /// </summary>
        private protected float _targetHorizontalSpeed;

        /// <summary>
        /// In meters/second
        /// </summary>
        private protected float _horizontalSpeed;

        /// <summary>
        /// In meters/second
        /// </summary>
        private protected float _verticalSpeed;

        /// <summary>
        /// In meters/second
        /// </summary>
        private protected bool _justWalkedOffEdge;

        private protected Vector3 _movementInput;
        private protected Vector2 _rotationInput;
        private protected bool _hasMovementInput;
        private protected bool _jumpInput;

        private protected Vector3 _lastMovementInput;
        private protected Vector3 _movementVector;

        private protected MovementSettings _movementSettings;
        private protected GravitySettings _gravitySettings;
        private protected RotationSettings _rotationSettings;
        private protected EnvironmentSettings _environmentSettings;
        private protected float _deltaTime;
        private protected IInputAnalyzer.InputAnalyzedData _analyzedInputData;
        private protected IAnimatorStateProvider<int> _animatorStateProvider;

        public IAnimatorStateProvider<int> StateProvider => _animatorStateProvider;

        public void Inject(EnvironmentSettings reference)
        {
            _environmentSettings = reference;
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

        public virtual void Initialize()
        {
            _animatorStateProvider = new AnimatorStateProvider();
            _animatorStateProvider.SetState<FloatState>(CharacterAnimatorParamId.HorizontalSpeed, 0f);
            _animatorStateProvider.SetState<FloatState>(CharacterAnimatorParamId.VerticalSpeed, 0f);
        }

        public virtual void OnPreMove(float deltaTime)
        {
            _deltaTime = deltaTime;
            UpdateHorizontalSpeed(_deltaTime);
            UpdateVerticalSpeed(_deltaTime);
        }

        public virtual void OnUpdate(IInputAnalyzer.InputAnalyzedData data)
        {
            _analyzedInputData = data;
            SetMovementInput(_analyzedInputData.MovementInput);
            SetRotationInput(_analyzedInputData.RotationInput);
            _jumpInput = _analyzedInputData.JumpInput;
        }

        public virtual Vector3 CalculateMove()
        {
            return _movementVector =
                       (_horizontalSpeed * GetMovementDirection() + _verticalSpeed * Vector3.up) * _deltaTime;
        }

        private protected virtual Vector3 GetMovementDirection()
        {
            var moveDir = _hasMovementInput ? _movementInput : _lastMovementInput;

            if (moveDir.sqrMagnitude > 1f)
            {
                moveDir.Normalize();
            }
            return moveDir;
        }

        private protected virtual void UpdateState(Character.CharacterData data)
        {
            var normHorizontalSpeed = data.HorizontalVelocity.magnitude / _movementSettings.MaxHorizontalSpeed;
            _animatorStateProvider.SetState<FloatState>(CharacterAnimatorParamId.HorizontalSpeed, normHorizontalSpeed);

            var jumpSpeed = _movementSettings.JumpSpeed;
            var normVerticalSpeed = data.VerticalVelocity.y.Remap(-jumpSpeed, jumpSpeed, -1.0f, 1.0f);
            _animatorStateProvider.SetState<FloatState>(CharacterAnimatorParamId.VerticalSpeed, normVerticalSpeed);
        }

        public virtual void OnPostMove(Character.CharacterData data)
        {
            UpdateState(data);
        }

        private protected virtual void SetRotationInput(Vector2 rotationInput)
        {
            _rotationInput = rotationInput;
        }

        private protected virtual void SetMovementInput(Vector3 movementInput)
        {
            var hasMovementInput = movementInput.sqrMagnitude > 0.0f;

            if (_hasMovementInput && !hasMovementInput)
            {
                _lastMovementInput = _movementInput;
            }
            _movementInput = movementInput;
            _hasMovementInput = hasMovementInput;
        }

        private protected abstract void UpdateVerticalSpeed(float deltaTime);

        private protected abstract void UpdateHorizontalSpeed(float deltaTime);

        public virtual Quaternion CalculateRotation(Quaternion currentRotation)
        {
            return OrientToTargetRotation(_movementVector.SetY(0.0f), currentRotation, _deltaTime);
        }

        private protected virtual Quaternion OrientToTargetRotation(Vector3 horizontalMovement, Quaternion currentRotation,
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

        public virtual Vector2 ClampRotationFunc(Vector2 rotationInput)
        {
            var pitchAngle = rotationInput.x;
            pitchAngle = Mathf.Clamp(pitchAngle, _rotationSettings.MinPitchAngle, _rotationSettings.MaxPitchAngle);
            return rotationInput.SetX(pitchAngle);
        }

        private protected virtual float RotationSpeed()
        {
            return Mathf.Lerp(_rotationSettings.MaxRotationSpeed, _rotationSettings.MinRotationSpeed,
                              _horizontalSpeed / _targetHorizontalSpeed);
        }

        public virtual Vector3 ClampMovementFunc(Vector3 movementInput)
        {
            return movementInput;
        }
    }
}
