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

        public Vector3 GetMovementInput();

        public Vector2 GetRotationInput();

        public bool GetJumpInput();
    }
}
