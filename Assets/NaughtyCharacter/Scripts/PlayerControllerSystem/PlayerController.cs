using System;
using NaughtyCharacter.Scripts.CharacterSystem;
using NaughtyCharacter.Scripts.PlayerControllerSystem.Interfaces;
using UnityEngine;

namespace NaughtyCharacter.Scripts.PlayerControllerSystem
{
    [Serializable]
    public class PlayerController : IPlayerController
	{
		[SerializeField] private float controlRotationSensitivity = 2.0f;
        
        private Character _character;
		private PlayerInputComponent _playerInput;
        
        public void Inject(Character reference)
        {
            _character = reference;
        }

        public void Inject(PlayerInputComponent reference)
        {
            _playerInput = reference;
        }

        public void OnCharacterUpdate()
		{
			UpdateControlRotation();
            _character.SetMovementInput(GetMovementInput());
            _character.SetJumpInput(_playerInput.JumpInput);
		}

		private void UpdateControlRotation()
		{
			var camInput = _playerInput.CameraInput;
			var controlRotation = _character.GetControlRotation();

			// Adjust the pitch angle (X Rotation)
			var pitchAngle = controlRotation.x;
			pitchAngle -= camInput.y * controlRotationSensitivity;

			// Adjust the yaw angle (Y Rotation)
			var yawAngle = controlRotation.y;
			yawAngle += camInput.x * controlRotationSensitivity;

			controlRotation = new Vector2(pitchAngle, yawAngle);
            _character.SetControlRotation(controlRotation);
		}

		private Vector3 GetMovementInput()
		{
			// Calculate the move direction relative to the character's yaw rotation
			var yawRotation = Quaternion.Euler(0.0f, _character.GetControlRotation().y, 0.0f);
			var forward = yawRotation * Vector3.forward;
			var right = yawRotation * Vector3.right;
			var movementInput = (forward * _playerInput.MoveInput.y + right * _playerInput.MoveInput.x);

			if (movementInput.sqrMagnitude > 1f)
			{
				movementInput.Normalize();
			}

			return movementInput;
		}
    }
}