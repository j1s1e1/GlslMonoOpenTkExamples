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
		
		virtual public Vector3 NewOffset(Vector3 oldOffset)
		{
			return oldOffset;
		}
	}
}

