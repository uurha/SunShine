using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyCharacter.AnimationSystem;
using NaughtyCharacter.CharacterSystem;
using NaughtyCharacter.MovementModule.EnvironmentSystem.Interfaces;
using NaughtyCharacter.MovementModule.EnvironmentSystem.Models;
using NaughtyCharacter.MovementModule.PlayerSystem.Interfaces;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.EnvironmentSystem
{
    [Serializable]
    public class LiquidMovement : MovementEnvironment
    {
        private bool _isSwimming = true;

        public override void Initialize()
        {
            base.Initialize();
            _animatorStateProvider.SetState<BooleanState>(CharacterAnimatorParamId.IsSwimming, _isSwimming);
            _animatorStateProvider.SetState<BooleanState>(CharacterAnimatorParamId.IsGrounded, false);
        }

        public override EnvironmentTransferState GetTransferState()
        {
            var verticalSpeed = _movementSettings.JumpSpeed;
            return new EnvironmentTransferState(verticalSpeed, _horizontalSpeed, _analyzedInputData);
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

            _horizontalSpeed =
                Mathf.MoveTowards(_horizontalSpeed, _targetHorizontalSpeed, acceleration * deltaTime);
        }

        private protected override void SetMovementInput(Vector3 movementInput)
        {
            var hasMovementInput = movementInput.sqrMagnitude > 0.0f;

            if (_hasMovementInput && !hasMovementInput)
            {
                _lastMovementInput = _movementInput;
            }
            _movementInput = movementInput;
            _hasMovementInput = hasMovementInput;
        }

        private protected override void UpdateVerticalSpeed(float deltaTime)
        {
            if (_analyzedInputData.JumpInput)
            {
                _verticalSpeed = _movementSettings.JumpSpeed;
            }
            else if (_analyzedInputData.CrouchInput)
            {
                _verticalSpeed = -_movementSettings.JumpSpeed;
            }

            if (Mathf.Abs(_verticalSpeed) > 0.0f)
                _verticalSpeed =
                    Mathf.MoveTowards(_verticalSpeed, 0, deltaTime) * _environmentSettings.EnvironmentDensity;
        }

        private protected override void UpdateState(Character.CharacterData data)
        {
            base.UpdateState(data);
            _isSwimming = true;

            _animatorStateProvider.SetState<BooleanState>(CharacterAnimatorParamId.IsSwimming, _isSwimming);
            _animatorStateProvider.SetState<BooleanState>(CharacterAnimatorParamId.IsGrounded, false);
        }

        private bool CheckGrounded(Vector3 position)
        {
            var spherePosition = position;

            spherePosition.y = spherePosition.y + _environmentSettings.SphereCastRadius -
                               _environmentSettings.SphereCastDistance;

            var isGrounded = Physics.CheckSphere(spherePosition, _environmentSettings.SphereCastRadius,
                                                 _environmentSettings.CheckLayer, QueryTriggerInteraction.Ignore);
            return isGrounded;
        }
    }
}
