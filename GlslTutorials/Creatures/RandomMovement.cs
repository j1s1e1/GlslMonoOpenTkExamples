using System;
using OpenTK;

namespace GlslTutorials
{
	public class RandomMovement
	{
		public RandomMovement ()
		{
		}
		
		static Random random = new Random();
		
		float xLimitLow = -1.0f;
		float yLimitLow = -1.0f;
		float zLimitLow = 0f;
		
		float xLimitHigh = 1.0f;
		float yLimitHigh = 1.0f;
		float zLimitHigh = 1.0f;
		
		float maxXmovement = 0.1f;
		float maxYmovement = 0.1f;
		float maxZmovement = 0.1f;
		
		private float NewValue(float oldValue, float maxMovement, float lowLimit, float highLimit)
		{
			oldValue = oldValue + random.Next(100)/100f * 2f * maxMovement - maxMovement;
			if (oldValue < lowLimit) oldValue = lowLimit;
			if (oldValue > highLimit) oldValue = highLimit;
			return oldValue;
		}
		
		public Vector3 NewOffset(Vector3 oldOffset)
		{
			oldOffset.X = NewValue(oldOffset.X, maxXmovement, xLimitLow, xLimitHigh);
			oldOffset.Y = NewValue(oldOffset.Y, maxYmovement, yLimitLow, yLimitHigh);
			oldOffset.Z = NewValue(oldOffset.Z, maxZmovement, zLimitLow, zLimitHigh);
			return oldOffset;
		}
	}
}

