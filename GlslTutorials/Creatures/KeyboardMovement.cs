using System;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class KeyboardMovement : Movement
	{
		public KeyboardMovement()
		{
		}
		
		float xSpeed = 0f;
		float ySpeed = 0f;	
		float zSpeed = 0f;
		
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
			oldOffset.X = NewValue(oldOffset.X, maxXmovement, xLimitLow, xLimitHigh, xSpeed);
			oldOffset.Y = NewValue(oldOffset.Y, maxYmovement, yLimitLow, yLimitHigh, ySpeed);
			oldOffset.Z = NewValue(oldOffset.Z, maxZmovement, zLimitLow, zLimitHigh, zSpeed);
			return oldOffset;
		}
		
		public void keyboard(Keys keyCode)
		{
			switch (keyCode)
			{
				case Keys.NumPad4: xSpeed = xSpeed - 0.01f; break;
				case Keys.NumPad6: xSpeed = xSpeed + 0.01f; break;
				case Keys.NumPad8: ySpeed = ySpeed + 0.01f; break;
				case Keys.NumPad2: ySpeed = ySpeed - 0.01f; break;
			}
		}
	}
}

