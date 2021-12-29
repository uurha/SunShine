using System;
using UnityEngine;

namespace NaughtyCharacter.Scripts
{
    public interface IPlayerCamera
    {
        public void SetPosition(Vector3 position);

        public void SetControlRotation(Vector2 controlRotation);
    }
    
    [Serializable]
	public class PlayerCamera : IPlayerCamera
	{
		[Tooltip("How fast the camera rotates around the pivot. Value <= 0 are interpreted as instant rotation")]
		[SerializeField] private float rotationSpeed = 25f;
		[SerializeField] private float positionSmoothDamp = 0.025f;
		[SerializeField] private Transform rig; // The root transform of the camera rig
		[SerializeField] private Transform pivot; // The point at which the camera pivots around

		private Vector3 _cameraVelocity;

		public void SetPosition(Vector3 position)
		{
			rig.position = Vector3.SmoothDamp(rig.position, position, ref _cameraVelocity, positionSmoothDamp);
		}

		public void SetControlRotation(Vector2 controlRotation)
		{
			// Y Rotation (Yaw Rotation)
			var rigTargetLocalRotation = Quaternion.Euler(0.0f, controlRotation.y, 0.0f);

			// X Rotation (Pitch Rotation)
			var pivotTargetLocalRotation = Quaternion.Euler(controlRotation.x, 0.0f, 0.0f);

			if (rotationSpeed > 0.0f)
			{
				rig.localRotation = Quaternion.Slerp(rig.localRotation, rigTargetLocalRotation, rotationSpeed * Time.deltaTime);
				pivot.localRotation = Quaternion.Slerp(pivot.localRotation, pivotTargetLocalRotation, rotationSpeed * Time.deltaTime);
			}
			else
			{
				rig.localRotation = rigTargetLocalRotation;
				pivot.localRotation = pivotTargetLocalRotation;
			}
		}
	}
}
