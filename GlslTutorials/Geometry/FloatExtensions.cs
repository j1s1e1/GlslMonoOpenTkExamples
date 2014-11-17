using System;

namespace GlslTutorials
{
	public class FloatExtensions
	{
		public FloatExtensions ()
		{
		}
		
		public static float Distance(float a, float b)
		{
			return (float)Math.Abs(a - b);
		}
	}
}

