using System;

namespace GlslTutorials
{
	public class MaxIntensityData: IGetValueTime<FloatIDistance>
	{
		public FloatIDistance first;
		public float second;
		
		public MaxIntensityData(float firstIn, float secondIn)
		{
			first = new FloatIDistance(firstIn);
			second = secondIn;
		}
		
		public MaxIntensityData(FloatIDistance firstIn, float secondIn)
		{
			first = firstIn;
			second = secondIn;
		}
		
		public FloatIDistance GetValue() 
		{
			return first;
		}
		
		public float GetFloat() 
		{
			return first.GetValue();
		}
		
		public float GetTime() 
		{
			return second;
		}
	}
}

