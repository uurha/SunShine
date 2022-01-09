using System;
using Base.Inject;
using NaughtyCharacter.Helpers;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.PlayerSystem.Interfaces
{
    public interface IInputAnalyzer : IInjectReceiver<PlayerInputComponent>
    {
        public void OnUpdate();

        public void SetMovementClampFunction(Func<Vector3, Vector3> clampFunction);
        
        public void SetRotationClampFunction(Func<Vector2, Vector2> clampFunction);

        public InputAnalyzedData GetAnalyzedData();
        
        public readonly struct InputAnalyzedData
        {
            public InputAnalyzedData(Vector2 rotationInput, Vector3 movementInput, bool jumpInput, bool crouchInput,
                                     bool sprintInput)
            {
                RotationInput = rotationInput;
                MovementInput = movementInput;
                JumpInput = jumpInput;
                CrouchInput = crouchInput;
                SprintInput = sprintInput;
            }

            public Vector2 RotationInput { get; } // X (Pitch), Y (Yaw)
            public Vector3 MovementInput { get; }
            public bool JumpInput { get; }
            public bool CrouchInput { get; }
            public bool SprintInput { get; }
        }
    }
}
