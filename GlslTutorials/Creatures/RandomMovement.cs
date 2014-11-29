using System;
using OpenTK;

namespace GlslTutorials
{
	public class RandomMovement : Movement
	{
		public RandomMovement ()
		{
		}
		
		static Random random = new Random();
		
		private float NewValue(float oldValue, float maxMovement, float lowLimit, float highLimit)
		{
			oldValue = oldValue + random.Next(100)/100f * 2f * maxMovement - maxMovement;
			if (oldValue < lowLimit) oldValue = lowLimit;
			if (oldValue > highLimit) oldValue = highLimit;
			return oldValue;
		}
		
		public override Vector3 NewOffset(Vector3 oldOffset)
		{
			oldOffset.X = NewValue(oldOffset.X, maxXmovement, xLimitLow, xLimitHigh);
			oldOffset.Y = NewValue(oldOffset.Y, maxYmovement, yLimitLow, yLimitHigh);
			oldOffset.Z = NewValue(oldOffset.Z, maxZmovement, zLimitLow, zLimitHigh);
			return oldOffset;
		}
	}
}

