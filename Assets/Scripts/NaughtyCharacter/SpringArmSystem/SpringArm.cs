using UnityEngine;

namespace NaughtyCharacter.SpringArmSystem
{
	public class SpringArm : MonoBehaviour
	{
		[SerializeField] private float targetLength = 3.0f;
		[SerializeField] private float speedDamp = 0.0f;
		[SerializeField] private Transform collisionSocket;
		[SerializeField] private float collisionRadius = 0.25f;
		[SerializeField] private LayerMask collisionMask = 0;
		[SerializeField] private Camera cameraReference;
		[SerializeField] private float cameraViewportExtentsMultiplier = 1.0f;

		private Vector3 _socketVelocity;
        private Transform _thisTransform;
        private const float MinRadius = 0.001f;

        private void Awake()
        {
            _thisTransform = transform;
        }

        private void LateUpdate()
		{
			if (cameraReference != null)
			{
				collisionRadius = GetCollisionRadiusForCamera(cameraReference);
				cameraReference.transform.localPosition = -Vector3.forward * cameraReference.nearClipPlane;
			}

			UpdateLength();
		}

		private float GetCollisionRadiusForCamera(Camera cam)
		{
			var halfFOV = (cam.fieldOfView / 2.0f) * Mathf.Deg2Rad; // vertical FOV in radians
			var nearClipPlaneHalfHeight = Mathf.Tan(halfFOV) * cam.nearClipPlane * cameraViewportExtentsMultiplier;
			var nearClipPlaneHalfWidth = nearClipPlaneHalfHeight * cam.aspect;
			var buffer = new Vector2(nearClipPlaneHalfWidth, nearClipPlaneHalfHeight).magnitude; // Pythagoras

			return buffer;
		}

		private float GetDesiredTargetLength()
		{
			var ray = new Ray(_thisTransform.position, -_thisTransform.forward);
            return Physics.SphereCast(ray, Mathf.Max(MinRadius, collisionRadius), out var hit, targetLength, collisionMask) ? hit.distance : targetLength;
		}

		private void UpdateLength()
		{
			var desiredLength = GetDesiredTargetLength();
			var newSocketLocalPosition = -Vector3.forward * desiredLength;

			collisionSocket.localPosition = Vector3.SmoothDamp(
				collisionSocket.localPosition, newSocketLocalPosition, ref _socketVelocity, speedDamp);
		}

		private void OnDrawGizmos()
        {
            if (collisionSocket == null) return;
            Gizmos.color = Color.green;
            var position = collisionSocket.transform.position;
            Gizmos.DrawLine(transform.position, position);
            DrawGizmoSphere(position, collisionRadius);
        }

		private void DrawGizmoSphere(Vector3 pos, float radius)
		{
			var rot = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

			const int alphaSteps = 8;
			const int betaSteps = 16;

			const float deltaAlpha = Mathf.PI / alphaSteps;
			const float deltaBeta = 2.0f * Mathf.PI / betaSteps;

			for (var a = 0; a < alphaSteps; a++)
			{
				for (var b = 0; b < betaSteps; b++)
				{
					var alpha = a * deltaAlpha;
					var beta = b * deltaBeta;

					var p1 = pos + rot * GetSphericalPoint(alpha, beta, radius);
					var p2 = pos + rot * GetSphericalPoint(alpha + deltaAlpha, beta, radius);
					var p3 = pos + rot * GetSphericalPoint(alpha + deltaAlpha, beta - deltaBeta, radius);

					Gizmos.DrawLine(p1, p2);
					Gizmos.DrawLine(p2, p3);
				}
			}
		}

		private Vector3 GetSphericalPoint(float alpha, float beta, float radius)
		{
			Vector3 point;
			point.x = radius * Mathf.Sin(alpha) * Mathf.Cos(beta);
			point.y = radius * Mathf.Sin(alpha) * Mathf.Sin(beta);
			point.z = radius * Mathf.Cos(alpha);

			return point;
		}
	}
}
