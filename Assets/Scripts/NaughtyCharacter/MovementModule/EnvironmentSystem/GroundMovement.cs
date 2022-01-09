using System;
using NaughtyCharacter.AnimationSystem;
using NaughtyCharacter.CharacterSystem;
using NaughtyCharacter.MovementModule.EnvironmentSystem.Interfaces;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.EnvironmentSystem
{
    [Serializable]
    public class GroundMovement : MovementEnvironment
    {
        private bool _isGrounded = true;

        public override void Initialize()
        {
            base.Initialize();
            _animatorStateProvider.SetState<BooleanState>(CharacterAnimatorParamId.IsGrounded, true);
        }

        private protected override void UpdateVerticalSpeed(float deltaTime)
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

        private protected override void UpdateHorizontalSpeed(float deltaTime)
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

        private bool CheckGrounded(Vector3 position)
        {
            var spherePosition = position;

            var isGrounded = Physics.CheckSphere(spherePosition, _environmentSettings.SphereCastRadius,
                                                 _environmentSettings.CheckLayer, QueryTriggerInteraction.Ignore);
            
            return isGrounded;
        }

        private protected override void UpdateState(Character.CharacterData data)
        {
            base.UpdateState(data);
            _justWalkedOffEdge = false;
            var isGrounded = CheckGrounded(data.Position);

            if (_isGrounded && !isGrounded && !_jumpInput)
            {
                _justWalkedOffEdge = true;
            }
            _isGrounded = isGrounded;
            
            _animatorStateProvider.SetState<BooleanState>(CharacterAnimatorParamId.IsGrounded, isGrounded);
        }
    }
}
