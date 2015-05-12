using System;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class MouseMovement : Movement
	{
		float xSpeed = 0f;
		float ySpeed = 0f;	
		float zSpeed = 0f;

		Vector3 mousePosition;
		bool mouseUpdated = false;
		Vector3 scale = new Vector3(1f/256f, -1f/256f, 0.01f);
		Vector3 center = new Vector3(256f, 256f, 0f);
		float mouseGain = 4f;
		
		private float NewValue(float oldValue, float maxMovement, float lowLimit, 
		                       float highLimit, float speed)
		{
			oldValue = oldValue + speed * maxMovement;
			if (oldValue < lowLimit) oldValue = lowLimit;
			if (oldValue > highLimit) oldValue = highLimit;
			return oldValue;
		}

		public override Vector3 NewOffset(Vector3 oldOffset)
		{
			if (mouseUpdated)
			{
				xSpeed = (mousePosition.X  * scale.X - currentPosition.X) * mouseGain;
				ySpeed = (mousePosition.Y  * scale.Y - currentPosition.Y) * mouseGain;
			}
			oldOffset.X = NewValue(oldOffset.X, maxXmovement, xLimitLow, xLimitHigh, xSpeed);
			oldOffset.Y = NewValue(oldOffset.Y, maxYmovement, yLimitLow, yLimitHigh, ySpeed);
			oldOffset.Z = NewValue(oldOffset.Z, maxZmovement, zLimitLow, zLimitHigh, zSpeed);
			currentPosition = oldOffset;
			return oldOffset;
		}
			
		public void SetScale(Vector3 v)
		{
			scale = v;
		}
		public void MouseMotion(int mouseX, int mouseY)
		{
			mousePosition = new Vector3((mouseX - center.X), (mouseY - center.Y), 0f);
			mouseUpdated = true;
		}
	}
}

