using System;
using OpenTK;

namespace GlslTutorials
{
	public class FloatIDistance : IDistance<FloatIDistance>
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
	}
}

