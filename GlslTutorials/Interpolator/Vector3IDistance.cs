using System;
using OpenTK;

namespace GlslTutorials
{
	public class Vector3IDistance : IDistance<Vector3IDistance>, ILinearInterpolate<Vector3IDistance>
	{
		Vector3 vector3Value;

		public Vector3IDistance(float x, float y, float z)
		{
			vector3Value = new Vector3(x, y, z);
		}

		public Vector3IDistance(Vector3 vector3ValueIn)
		{
			vector3Value = vector3ValueIn;
		}
		
		public Vector3 GetValue()
		{
			return vector3Value;
		}
		
		float IDistance<Vector3IDistance>.Distance(Vector3IDistance a, Vector3IDistance b)
		{
			return Vector3Extensions.Distance(a.GetValue(), b.GetValue());
		}
		
		Vector3IDistance ILinearInterpolate<Vector3IDistance>.LinearInterpolate(Vector3IDistance a, Vector3IDistance b, float sectionAlpha)
		{
			Vector3 interpolatedValue = (1f - sectionAlpha) * a.GetValue() + sectionAlpha * b.GetValue();
			Vector3IDistance result = new Vector3IDistance(interpolatedValue);
			return result;
		}
	}
}

