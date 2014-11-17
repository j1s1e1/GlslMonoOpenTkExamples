using System;

namespace GlslTutorials
{
	public class MaxIntensityData
	{
		public float first;
		public float second;
		
		public MaxIntensityData(float firstIn, float secondIn)
		{
			first = firstIn;
			second = secondIn;
		}
		
		float GetValue() 
		{
			return first;
		}
		
		float GetTime() 
		{
			return second;
		}
	}
}

