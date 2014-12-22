using System;
using OpenTK;

namespace GlslTutorials
{
	public class SunlightValue
	{
		public float normTime;
		public Vector4 ambient;
		public Vector4 sunlightIntensity;
		public Vector4 backgroundColor;
		public float maxIntensity;
		public bool HDR = true;

		public SunlightValue (float normTimeIn, Vector4 ambientIn, Vector4 sunlightIntensityIn, 
			Vector4 backgroundColorIn)
		{
			normTime = normTimeIn;
			ambient = ambientIn;
			sunlightIntensity = sunlightIntensityIn;
			backgroundColor = backgroundColorIn;
			HDR = false;
		}
		public SunlightValue (float normTimeIn, Vector4 ambientIn, Vector4 sunlightIntensityIn, 
			Vector4 backgroundColorIn, float maxIntensityIn)
		{
			normTime = normTimeIn;
			ambient = ambientIn;
			sunlightIntensity = sunlightIntensityIn;
			backgroundColor = backgroundColorIn;
			maxIntensity = maxIntensityIn;
		}
	}
}

