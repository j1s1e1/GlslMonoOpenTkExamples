using System;
using OpenTK;

namespace GlslTutorials
{
	public class ChaseObjectMovement : Movement
	{
		CollisionObject objectToChase;
		float speedLimit = 0.05f;

		public ChaseObjectMovement ()
		{
		}

		public void SetChaseObject(CollisionObject c)
		{
			objectToChase = c;
		}

		public void SetSpeedLimit(float sl)
		{
			speedLimit = sl;
		}

		public override Vector3 NewOffset(Vector3 oldOffset)
		{
			Vector3 result = objectToChase.GetOffset();
			Vector3 difference = result - oldOffset;
			if (difference.X > speedLimit)  difference.X = speedLimit;
			if (difference.X < -speedLimit)  difference.X = -speedLimit;
			if (difference.Y > speedLimit)  difference.Y = speedLimit;
			if (difference.Y < -speedLimit)  difference.Y = -speedLimit;
			if (difference.Z > speedLimit)  difference.Z = speedLimit;
			if (difference.Z < -speedLimit)  difference.Z = -speedLimit;
			result = oldOffset + difference;
			if (result.X > xLimitHigh)  result.X = xLimitHigh;
			if (result.X < xLimitLow)  result.X = xLimitLow;
			if (result.Y > yLimitHigh)  result.Y = yLimitHigh;
			if (result.Y < yLimitLow)  result.Y = yLimitLow;
			if (result.Z > zLimitHigh)  result.Z = zLimitHigh;
			if (result.Z < zLimitLow)  result.Z = zLimitLow;
			return result;
		}
	}
}

