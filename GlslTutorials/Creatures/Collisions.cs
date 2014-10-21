using System;
using OpenTK;

namespace GlslTutorials
{
	public class Collisions
	{
		public Collisions ()
		{
		}
		
		float collisionDistance = 0.1f;
		
		private bool DetectCollision(Vector3 position1, Vector3 position2)
		{
			if ((position1 - position2).Length < collisionDistance)
				return true;
			else
				return false;
		}
		
		public bool DetectColisions(Vector3 position, Vector3[] otherPositions)
		{
			foreach (Vector3 v in otherPositions)
			{
				if (DetectCollision(position, v))
				    return true;
			}
			return false;
		}
	}
}

