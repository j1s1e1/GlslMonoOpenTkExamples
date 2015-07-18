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

		float x_low = -1;
		float x_high = 1;
		float y_low = -1;
		float y_high = 1;

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
				if (oldOffset.X < x_low) repeat_count = repeat_limit;
			}
			if (direction == 2)
			{
				oldOffset.X = oldOffset.X + speed.X;
				if (oldOffset.X > x_high) repeat_count = repeat_limit;
			}
			if (direction == 3)
			{
				oldOffset.Y = oldOffset.Y - speed.Y;
				if (oldOffset.Y < y_low) repeat_count = repeat_limit;
			}
			if (direction == 4)
			{
				oldOffset.Y = oldOffset.Y + speed.Y;
				if (oldOffset.Y > y_high) repeat_count = repeat_limit;
			}
			move_count++;
			return oldOffset;
		}

		public int GetDirection()
		{
			return direction;
		}
	}
}

