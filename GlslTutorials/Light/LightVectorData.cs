using System;
using OpenTK;

namespace GlslTutorials
{
	public class LightVectorData : IDistance<LightVectorData>, IGetValueTime<LightVectorData>, ILinearInterpolate<LightVectorData>
	{
		public Vector4 first;
		public float second;

		public Vector4 GetValue()
		{
			return first;
		}

		LightVectorData IGetValueTime<LightVectorData>.GetValue()
		{
			return this;
		}

		float IGetValueTime<LightVectorData>.GetTime()
		{
			return second;
		}

		float IDistance<LightVectorData>.Distance(LightVectorData a, LightVectorData b)
		{
			return Vector4Extensions.Distance(a.GetValue(), b.GetValue());
		}

		public LightVectorData(Vector4 firstIn, float secondIn)
		{
			first = firstIn;
			second = secondIn;
		}

		LightVectorData ILinearInterpolate<LightVectorData>.LinearInterpolate(LightVectorData a, LightVectorData b, float sectionAlpha)
		{
			Vector4 interpolatedValue = (1f - sectionAlpha) * a.GetValue() + sectionAlpha * b.GetValue();
			float interp2 = 0f;
			LightVectorData result = new LightVectorData(interpolatedValue, interp2);
			return result;
		}
	}
}

