using System;
using OpenTK;

namespace GlslTutorials
{
	public static class Vector3Extensions
	{	
		public static Vector3 Clone(this Vector3 a)
		{
			return new Vector3(a.X, a.Y, a.Z);
		}
		
		public static float[] ToFloat(this Vector3 a)
		{
			return new float[]{a.X, a.Y, a.Z};
		}
		
		public static Vector3 Midpoint(this Vector3 a, Vector3 b)
		{
			return (a + b) / 2;
		}
	}
}

