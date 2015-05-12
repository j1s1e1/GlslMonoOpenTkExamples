using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class ElasticMovement : Movement
	{
		public ElasticMovement()
		{
			otherObjects = new List<CollisionObject>();
			speed = new Vector3(minSpeed + random.Next(100)/100f, 
			        minSpeed + random.Next(100)/100f, minSpeed + random.Next(100)/100f);
		}
		
		static Random random = new Random();
		List<CollisionObject> otherObjects;
		float minSpeed = 0.25f;
		
		private float NewValue(float oldValue, float maxMovement, float lowLimit, 
		                       float highLimit, float speed)
		{
			oldValue = oldValue + speed * maxMovement;
			if (oldValue < lowLimit) oldValue = lowLimit;
			if (oldValue > highLimit) oldValue = highLimit;
			return oldValue;
		}
		
		private void Bounce(Vector3 oldOffset)
		{
			for (int i = 0; i <  otherObjects.Count; i++)
			{
				CollisionObject p = otherObjects[i];
				Vector3 offset = p.GetOffset();
				Vector3 difference = oldOffset - offset;
				if (difference.Length < 0.55f)
				{
					Vector3 normal = p.GetNormal();
					Vector3 normalComponent = Vector3.Dot(speed, normal) * normal;
					// verify speed and normal are in the opposite direction
					if ((normalComponent + normal).Length < normal.Length)
					{
						speed = speed - 2 * normalComponent;
						Tut_PaintBox.PaddleHitBall(i);
					}
				}
			}
			if (oldOffset.X == xLimitLow) speed.X = Math.Abs(speed.X);
			if (oldOffset.Y == yLimitLow) speed.Y = Math.Abs(speed.Y);
			if (oldOffset.Z == zLimitLow) speed.Z = Math.Abs(speed.Z);
			if (oldOffset.X == xLimitHigh) speed.X = -Math.Abs(speed.X);
			if (oldOffset.Y == yLimitHigh) speed.Y = -Math.Abs(speed.Y);
			if (oldOffset.Z == zLimitHigh) speed.Z = -Math.Abs(speed.Z);

		}
		
		public override Vector3 NewOffset(Vector3 oldOffset)
		{
			oldOffset.X = NewValue(oldOffset.X, maxXmovement, xLimitLow, xLimitHigh, speed.X);
			oldOffset.Y = NewValue(oldOffset.Y, maxYmovement, yLimitLow, yLimitHigh, speed.Y);
			oldOffset.Z = NewValue(oldOffset.Z, maxZmovement, zLimitLow, zLimitHigh, speed.Z);
			Bounce(oldOffset);
			return oldOffset;
		}
		
		public void SetPaddles(List<CollisionObject> paddles)
		{
			otherObjects = paddles;
		}

		public void SetSpeed(Vector3 speedIn)
		{
			speed = speedIn;
		}

		public Vector3 GetSpeed()
		{
			return speed;
		}
		                    
	}
}

