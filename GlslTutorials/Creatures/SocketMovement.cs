using System;
using OpenTK;

namespace GlslTutorials
{
	public class SocketMovement : Movement
	{
		public SocketMovement ()
		{
			receiveSocket = new ReceiveSocket(ProcessData);
		}
		
		float xSpeed = 0f;
		float ySpeed = 0f;	
		float zSpeed = 0f;
		
		ReceiveSocket receiveSocket;
		
		public void ProcessData(byte[] data)
		{
			string message = System.Text.Encoding.Default.GetString(data);
			string[] words = message.Split(' ');
			if (words.Length >= 2)
			{
				switch (words[0])
				{
				 	case "X+": xSpeed = xSpeed + float.Parse(words[1]); break;
					case "X-": xSpeed = xSpeed - float.Parse(words[1]);break;
					case "Y+": ySpeed = ySpeed + float.Parse(words[1]);break;
					case "Y-": ySpeed = ySpeed - float.Parse(words[1]);break;
				}
			}
		}
		
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
	}
}

