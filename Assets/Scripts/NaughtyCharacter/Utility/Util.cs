using UnityEngine;

namespace NaughtyCharacter.Utility
{
	public static class Util
	{
		public static Vector3 SetX(this Vector3 vec, float x)
		{
			return new Vector3(x, vec.y, vec.z);
		}

		public static Vector3 SetY(this Vector3 vec, float y)
		{
			return new Vector3(vec.x, y, vec.z);
		}

		public static Vector3 SetZ(this Vector3 vec, float z)
		{
			return new Vector3(vec.x, vec.y, z);
		}
        
        public static Vector2 SetX(this Vector2 vec, float x)
		{
			return new Vector2(x, vec.y);
		}

		public static Vector2 SetY(this Vector2 vec, float y)
		{
			return new Vector2(vec.x, y);
		}

        public static Vector3 Clamp(this Vector3 vec, Vector3 min, Vector3 max)
		{
			vec.x = Mathf.Clamp(vec.x, min.x, max.x);
			vec.y = Mathf.Clamp(vec.y, min.y, max.y);
			vec.z = Mathf.Clamp(vec.z, min.z, max.z);

			return vec;
		}

		public static float Remap(this float f, float fromMin, float fromMax, float toMin, float toMax)
		{
			var t = (f - fromMin) / (fromMax - fromMin);
			return Mathf.LerpUnclamped(toMin, toMax, t);
		}
	}
}
