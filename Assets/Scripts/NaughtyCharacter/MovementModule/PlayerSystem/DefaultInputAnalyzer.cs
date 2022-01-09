using System;
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
        private IInputAnalyzer.InputAnalyzedData _data;

        private Func<Vector2, Vector2> _rotationClampFunction;
        private Func<Vector3, Vector3> _movementClampFunction;

        public void Inject(PlayerInputComponent reference)
        {
            _playerInput = reference;
        }

        public void OnUpdate()
        {
            var rotation = UpdateControlRotation();
            var movement = UpdateControlMovement();

            _data = new IInputAnalyzer.InputAnalyzedData(rotation, movement, _playerInput.JumpInput, _playerInput.CrouchInput,
                                                         _playerInput.SprintInput);
        }

        public IInputAnalyzer.InputAnalyzedData GetAnalyzedData()
        {
            return _data;
        }

        public void SetMovementClampFunction(Func<Vector3, Vector3> clampFunction)
        {
            _movementClampFunction = clampFunction;
        }

        public void SetRotationClampFunction(Func<Vector2, Vector2> clampFunction)
        {
            _rotationClampFunction = clampFunction;
        }

        private Vector2 UpdateControlRotation()
        {
            var camInput = _playerInput.CameraInput;
            var controlRotation = _data.RotationInput;

            // Adjust the pitch angle (X Rotation)
            var pitchAngle = controlRotation.x;
            pitchAngle -= camInput.y * verticalRotationSensitivity;

            // Adjust the yaw angle (Y Rotation)
            var yawAngle = controlRotation.y;
            yawAngle += camInput.x * horizontalRotationSensitivity;
            pitchAngle %= 360.0f;
            yawAngle %= 360.0f;
            var rotation = new Vector2(pitchAngle, yawAngle);
            return _rotationClampFunction?.Invoke(rotation) ?? rotation;
        }

        private Vector3 UpdateControlMovement()
        {
            var yawRotation = Quaternion.Euler(0.0f, _data.RotationInput.y, 0.0f);
            var forward = yawRotation * Vector3.forward;
            var right = yawRotation * Vector3.right;
            var movementInput = (forward * _playerInput.MoveInput.y + right * _playerInput.MoveInput.x);

            if (movementInput.sqrMagnitude > 1f)
            {
                movementInput.Normalize();
            }
            return _movementClampFunction?.Invoke(movementInput) ?? movementInput;;
        }
    }
}
