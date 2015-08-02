using System;
using OpenTK;

namespace GlslTutorials
{
	public class BugMovement3D : Movement
	{
		Random random = new Random();
		int move_count = 0;
		int repeat_count = 0;
		int repeat_limit = 50;

		public BugMovement3D(Vector3 speedLimitIn)
		{
			speedLimit = speedLimitIn;
			speed = new Vector3(NewSpeed(speedLimit.X), NewSpeed(speedLimit.Y), NewSpeed(speedLimit.Z));
		}

		private float NewSpeed(float limit)
		{
			return (float)random.NextDouble() * limit * 2f - limit;
		}

		public override Vector3 NewOffset(Vector3 oldOffset)
		{
			if (repeat_count < repeat_limit)
			{
				repeat_count++;
			}
			else
			{
				speed = new Vector3(NewSpeed(speedLimit.X), NewSpeed(speedLimit.Y), NewSpeed(speedLimit.Z));
				repeat_count = random.Next(repeat_limit/2);
			}
			oldOffset.X = oldOffset.X + speed.X;
			if (oldOffset.X < xLimitLow) oldOffset.X = xLimitLow;
			if (oldOffset.X > xLimitHigh) oldOffset.X = xLimitHigh;
			oldOffset.Y = oldOffset.Y + speed.Y;
			if (oldOffset.Y < yLimitLow) oldOffset.Y = yLimitLow;
			if (oldOffset.Y > yLimitHigh) oldOffset.Y = yLimitHigh;
			oldOffset.Z = oldOffset.Z + speed.Z;
			if (oldOffset.Z < zLimitLow) oldOffset.Z = zLimitLow;
			if (oldOffset.Z > zLimitHigh) oldOffset.Z = zLimitHigh;
			move_count++;
			currentPosition = oldOffset;
			return oldOffset;
		}
	}
}

