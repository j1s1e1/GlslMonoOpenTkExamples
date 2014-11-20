using System;
using OpenTK;

namespace GlslTutorials
{
	public class LightData : IGetValueTime<Vector4IDistance>
	{ 
		public Vector4IDistance first;
		public float second;
		
		public LightData(Vector4 firstIn, float secondIn)
		{
			first = new Vector4IDistance(firstIn);
			second = secondIn;
		}		
		
		public LightData(Vector4IDistance firstIn, float secondIn)
		{
			first = firstIn;
			second = secondIn;
		}
		
		public Vector4IDistance GetValue() 
		{
			return first;
		}
		
		public float GetTime() 
		{
			return second;
		}
	}
}

