using System;
using OpenTK;

namespace GlslTutorials
{
	public class FloatIDistance : IDistance<FloatIDistance>, ILinearInterpolate<FloatIDistance>
	{
		float floatValue;
		public FloatIDistance(float floatValueIn)
		{
			floatValue = floatValueIn;
		}
		
		public float GetValue()
		{
			return floatValue;
		}
		
		float IDistance<FloatIDistance>.Distance(FloatIDistance a, FloatIDistance b)
		{
			return FloatExtensions.Distance(a.GetValue(), b.GetValue());
		}
		
		FloatIDistance ILinearInterpolate<FloatIDistance>.LinearInterpolate(FloatIDistance a, FloatIDistance b, float sectionAlpha)
		{
			float interpolatedValue = (1f - sectionAlpha) * a.GetValue() + sectionAlpha * b.GetValue();
			FloatIDistance result = new FloatIDistance(interpolatedValue);
			return result;
		}
		
	}
}

