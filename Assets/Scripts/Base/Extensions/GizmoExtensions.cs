using UnityEngine;

namespace Base.Extensions
{
    public static class GizmoExtensions
    {
        public static void DrawGizmoSphere(Vector3 pos, float radius)
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

        public static Vector3 GetSphericalPoint(float alpha, float beta, float radius)
        {
            Vector3 point;
            point.x = radius * Mathf.Sin(alpha) * Mathf.Cos(beta);
            point.y = radius * Mathf.Sin(alpha) * Mathf.Sin(beta);
            point.z = radius * Mathf.Cos(alpha);

            return point;
        }
    }
}