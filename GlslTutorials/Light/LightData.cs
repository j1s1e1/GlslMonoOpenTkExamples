using System;
using OpenTK;

namespace GlslTutorials
{
	public class LightData
	{
		public Vector4 first;
		public float second;
		
		public LightData(Vector4 firstIn, float secondIn)
		{
			first = firstIn;
			second = secondIn;
		}
		
		Vector4 GetValue() 
		{
			return first;
		}
		
		float GetTime() 
		{
			return second;
		}
	}
}

