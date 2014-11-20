using System;
using OpenTK;

namespace GlslTutorials
{
	public class Vector4IDistance : IDistance<Vector4IDistance>
	{
		Vector4 vector4Value;
		public Vector4IDistance(Vector4 vector4ValueIn)
		{
			vector4Value = vector4ValueIn;
		}
		
		public Vector4 GetValue()
		{
			return vector4Value;
		}
		
		float IDistance<Vector4IDistance>.Distance(Vector4IDistance a, Vector4IDistance b)
		{
			return Vector4Extensions.Distance(a.GetValue(), b.GetValue());
		}
	}
}

