using System;
using CorePlugin.Logger;
using NaughtyCharacter.CharacterSettingsModel;
using NaughtyCharacter.Helpers;
using NaughtyCharacter.MovementModule.PlayerSystem.Interfaces;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.PlayerSystem
{
    [Serializable]
    public class DefaultInputAnalyzer : IInputAnalyzer
	{
		[SerializeField] private float verticalRotationSensitivity = 0.5f;
		[SerializeField] private float horizontalRotationSensitivity = 0.5f;
        
		private PlayerInputComponent _playerInput;
        private Vector2 _controlRotation; // X (Pitch), Y (Yaw)
        private Vector3 _movementInput;
        private Func<Vector2, Vector2> _rotationClampFunction;
        private Func<Vector3, Vector3> _movementClampFunction;

        public void Inject(PlayerInputComponent reference)
        {
            if (reference == null)
            {
                DebugLogger.LogException(new NullReferenceException($"{nameof(PlayerInputComponent)} missing in scene"));
            }
            _playerInput = reference;
        }

        public void OnUpdate()
		{
			UpdateControlRotation();
            UpdateControlMovement();
		}

        public void SetMovementClampFunction(Func<Vector3, Vector3> clampFunction)
        {
            _movementClampFunction = clampFunction;
        }

        public void SetRotationClampFunction(Func<Vector2, Vector2> clampFunction)
        {
            _rotationClampFunction = clampFunction;
        }

        private void UpdateControlRotation()
		{
			var camInput = _playerInput.CameraInput;
            var controlRotation = _controlRotation;

			// Adjust the pitch angle (X Rotation)
			var pitchAngle = controlRotation.x;
			pitchAngle -= camInput.y * verticalRotationSensitivity;

			// Adjust the yaw angle (Y Rotation)
			var yawAngle = controlRotation.y;
			yawAngle += camInput.x * horizontalRotationSensitivity;

            pitchAngle %= 360.0f;

            yawAngle %= 360.0f;
            _controlRotation = new Vector2(pitchAngle, yawAngle);
		}
        
        private void UpdateControlMovement()
		{
            var yawRotation = Quaternion.Euler(0.0f, _controlRotation.y, 0.0f);
            var forward = yawRotation * Vector3.forward;
            var right = yawRotation * Vector3.right;
            _movementInput = (forward * _playerInput.MoveInput.y + right * _playerInput.MoveInput.x);

            if (_movementInput.sqrMagnitude > 1f)
            {
                _movementInput.Normalize();
            }
		}

        public Vector2 GetRotationInput()
        {
            return _rotationClampFunction == null ? _controlRotation : _controlRotation = _rotationClampFunction.Invoke(_controlRotation);
        }

        public bool GetJumpInput()
        {
            return _playerInput.JumpInput;
        }

        public Vector3 GetMovementInput()
		{
            return _movementClampFunction == null ? _movementInput : _movementInput = _movementClampFunction.Invoke(_movementInput);
		}
    }
}