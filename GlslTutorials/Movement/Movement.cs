using System;
using OpenTK;

namespace GlslTutorials
{
	public class Movement
	{
		public Movement ()
		{
		}
		
		protected float xLimitLow = -1.0f;
		protected float yLimitLow = -1.0f;
		protected float zLimitLow = 0f;
		
		protected float xLimitHigh = 1.0f;
		protected float yLimitHigh = 1.0f;
		protected float zLimitHigh = 1.0f;
		
		protected float maxXmovement = 0.1f;
		protected float maxYmovement = 0.1f;
		protected float maxZmovement = 0.1f;
		
		protected Vector3 speed = new Vector3(0f, 0f, 0f);
		
		virtual public Vector3 NewOffset(Vector3 oldOffset)
		{
			return oldOffset;
		}
		
		public void SetXlimits(float xLow, float xHigh)
		{
			xLimitLow = xLow;
			xLimitHigh = xHigh;
		}
		
		public void SetYlimits(float yLow, float yHigh)
		{
			yLimitLow = yLow;
			yLimitHigh = yHigh;
		}
		
		public void SetZlimits(float zLow, float zHigh)
		{
			zLimitLow = zLow;
			zLimitHigh = zHigh;
		}
		
		public void SetLimits(Vector3 low, Vector3 high)
		{
			xLimitLow = low.X;
			xLimitHigh = high.X;
			yLimitLow = low.Y;
			yLimitHigh = high.Y;
			zLimitLow = low.Z;
			zLimitHigh = high.Z;
		}
	}
}

