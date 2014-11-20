using System;
using OpenTK;

namespace GlslTutorials
{
	public class Vector3IDistance : IDistance<Vector3IDistance>
	{
		Vector3 vector3Value;
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
	}
}

