using System;
using OpenTK;

namespace GlslTutorials
{
	public class Movement
	{
		public Movement ()
		{
		}
		
		protected float xLimitLow = -1.0f;
		protected float yLimitLow = -1.0f;
		protected float zLimitLow = 0f;
		
		protected float xLimitHigh = 1.0f;
		protected float yLimitHigh = 1.0f;
		protected float zLimitHigh = 1.0f;
		
		protected float maxXmovement = 0.1f;
		protected float maxYmovement = 0.1f;
		protected float maxZmovement = 0.1f;
		
		protected Vector3 speed = new Vector3(0f, 0f, 0f);
		protected Vector3 speedLimit = new Vector3(0.05f, 0.05f, 0.05f);
		protected Vector3 currentPosition = new Vector3(0f, 0f, 0f);

		protected Matrix4 systemMatrix = Matrix4.Identity;
		
		virtual public Vector3 NewOffset(Vector3 oldOffset)
		{
			return oldOffset;
		}
		
		public void SetXlimits(float xLow, float xHigh)
		{
			xLimitLow = xLow;
			xLimitHigh = xHigh;
		}
		
		public void SetYlimits(float yLow, float yHigh)
		{
			yLimitLow = yLow;
			yLimitHigh = yHigh;
		}
		
		public void SetZlimits(float zLow, float zHigh)
		{
			zLimitLow = zLow;
			zLimitHigh = zHigh;
		}
		
		public void SetLimits(Vector3 low, Vector3 high)
		{
			xLimitLow = low.X;
			xLimitHigh = high.X;
			yLimitLow = low.Y;
			yLimitHigh = high.Y;
			zLimitLow = low.Z;
			zLimitHigh = high.Z;
		}

		public void MoveLimits(Vector3 v)
		{
			xLimitLow += v.X;
			xLimitHigh += v.X;
			yLimitLow += v.Y;
			yLimitHigh += v.Y;
			zLimitLow += v.Z;
			zLimitHigh += v.Z;
		}

		public String GetLimits()
		{
			Vector3 low = new Vector3(xLimitLow, yLimitLow, zLimitLow);
			Vector3 high = new Vector3(xLimitHigh, yLimitHigh, zLimitHigh);
			return low.ToString() + " " + high.ToString();
		}

		public Matrix4 GetSystemMatrix()
		{
			return systemMatrix;
		}

		public virtual string MovementInfo()
		{
			return "Position = " + currentPosition.ToString();
		}

		public virtual void Translate(Vector3 offset)
		{
		}
	}
}

