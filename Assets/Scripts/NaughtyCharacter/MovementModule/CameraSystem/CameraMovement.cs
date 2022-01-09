using System;
using NaughtyCharacter.MovementModule.CameraSystem.Interfaces;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.CameraSystem
{
    [Serializable]
    public class CameraMovement : ICameraMovement
    {
        [Tooltip("How fast the camera rotates around the pivot. Value <= 0 are interpreted as instant rotation")]
        [SerializeField] private float rotationSpeed = 25f;

        [SerializeField] private float positionSmoothDamp = 0.025f;

        /// <summary>
        /// The root transform of the camera rig
        /// </summary>
        private ICameraRigProvider _rig;

        /// <summary>
        /// The point at which the camera pivots around
        /// </summary>
        private ICameraPivotProvider _pivot;

        private Vector3 _cameraVelocity;

        public void Inject(ICameraRigProvider reference)
        {
            _rig = reference;
        }

        public void Inject(ICameraPivotProvider reference)
        {
            _pivot = reference;
        }

        public void SetPosition(Vector3 position)
        {
            _rig.Provided.position =
                Vector3.SmoothDamp(_rig.Provided.position, position, ref _cameraVelocity, positionSmoothDamp);
        }

        public void SetControlRotation(Vector2 controlRotation)
        {
            // Y Rotation (Yaw Rotation)
            var rigTargetLocalRotation = Quaternion.Euler(0.0f, controlRotation.y, 0.0f);

            // X Rotation (Pitch Rotation)
            var pivotTargetLocalRotation = Quaternion.Euler(controlRotation.x, 0.0f, 0.0f);

            if (rotationSpeed > 0.0f)
            {
                _rig.Provided.localRotation = Quaternion.Slerp(_rig.Provided.localRotation, rigTargetLocalRotation,
                                                               rotationSpeed * Time.deltaTime);

                _pivot.Provided.localRotation = Quaternion.Slerp(_pivot.Provided.localRotation,
                                                                 pivotTargetLocalRotation,
                                                                 rotationSpeed * Time.deltaTime);
            }
            else
            {
                _rig.Provided.localRotation = rigTargetLocalRotation;
                _pivot.Provided.localRotation = pivotTargetLocalRotation;
            }
        }
    }
}
