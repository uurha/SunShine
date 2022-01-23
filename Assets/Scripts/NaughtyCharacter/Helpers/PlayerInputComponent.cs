using CorePlugin.ReferenceDistribution.Interface;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NaughtyCharacter.Helpers
{ 
    [RequireComponent(typeof(PlayerInput))]
	public class PlayerInputComponent : MonoBehaviour, IDistributingReference
    {
        public Vector2 MoveInput { get; private set; }
		public Vector2 LastMoveInput { get; private set; }
		public Vector2 CameraInput { get; private set; }
		public bool JumpInput { get; private set; }
		public bool DoubleJumpInput { get; private set; }
		public bool HasMoveInput { get; private set; }
		public bool CrouchInput { get; private set; }
        
		public bool SprintInput { get; private set; }

        public void OnMoveEvent(InputAction.CallbackContext context)
		{
			var moveInput = context.ReadValue<Vector2>();

			var hasMoveInput = moveInput.sqrMagnitude > 0.0f;
			if (HasMoveInput && !hasMoveInput)
			{
				LastMoveInput = MoveInput;
			}

			MoveInput = moveInput;
			HasMoveInput = hasMoveInput;
		}
        
        public void OnDoubleJumpEvent(InputAction.CallbackContext context)
		{
            if (context.started || context.performed)
            {
                DoubleJumpInput = true;
            }
            else if (context.canceled)
            {
                DoubleJumpInput = false;
            }
		}

        public void OnCrouchEvent(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
            {
                CrouchInput = true;
            }
            else if (context.canceled)
            {
                CrouchInput = false;
            }
        }
        
        public void OnSprintEvent(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
            {
                SprintInput = true;
            }
            else if (context.canceled)
            {
                SprintInput = false;
            }
        }

        public void OnLookEvent(InputAction.CallbackContext context)
		{
			CameraInput = context.ReadValue<Vector2>();
		}

		public void OnJumpEvent(InputAction.CallbackContext context)
		{
			if (context.started || context.performed)
			{
				JumpInput = true;
			}
			else if (context.canceled)
			{
				JumpInput = false;
			}
		}
	}
}
