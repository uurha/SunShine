using Base.Inject;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.CameraSystem.Interfaces
{
    public interface ICameraMovement : IInjectReceiver<ICameraRigProvider>, IInjectReceiver<ICameraPivotProvider>
    {
        public void SetPosition(Vector3 position);

        public void SetControlRotation(Vector2 controlRotation);
    }
}
