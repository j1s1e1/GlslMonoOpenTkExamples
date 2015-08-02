using System;
using OpenTK;

namespace GlslTutorials
{
	public class BugMovement2D : Movement
	{
		Random random = new Random();
		int direction = 0;
		int move_count = 0;
		int repeat_count = 0;
		int repeat_limit = 50;

		public BugMovement2D(Vector3 speedIn)
		{
			speed = speedIn;
		}

		public override Vector3 NewOffset(Vector3 oldOffset)
		{
			if (repeat_count < repeat_limit)
			{
				repeat_count++;
			}
			else
			{
				direction = random.Next(5);
				repeat_count = random.Next(repeat_limit/2);
			}
			if (direction == 1)
			{
				oldOffset.X = oldOffset.X - speed.X;
				if (oldOffset.X < xLimitLow) repeat_count = repeat_limit;
			}
			if (direction == 2)
			{
				oldOffset.X = oldOffset.X + speed.X;
				if (oldOffset.X > xLimitHigh) repeat_count = repeat_limit;
			}
			if (direction == 3)
			{
				oldOffset.Y = oldOffset.Y - speed.Y;
				if (oldOffset.Y < yLimitLow) repeat_count = repeat_limit;
			}
			if (direction == 4)
			{
				oldOffset.Y = oldOffset.Y + speed.Y;
				if (oldOffset.Y > yLimitHigh) repeat_count = repeat_limit;
			}
			move_count++;
			currentPosition = oldOffset;
			return oldOffset;
		}

		public int GetDirection()
		{
			return direction;
		}
	}
}

