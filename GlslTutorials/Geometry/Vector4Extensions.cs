using System;
using OpenTK;

namespace GlslTutorials
{
	public static class Vector4Extensions
	{
		public static Vector4 Clone(this Vector4 a)
		{
			return new Vector4(a.X, a.Y, a.Z, a.W);
		}
		
		public static float[] ToFloat(this Vector4 a)
		{
			return new float[]{a.X, a.Y, a.Z, a.W};
		}
		
		public static float Distance(Vector4 a, Vector4 b)
		{
			return (a - b).Length;
		}
	}
}

