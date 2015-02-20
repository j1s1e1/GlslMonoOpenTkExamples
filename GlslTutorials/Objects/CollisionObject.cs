using System;
using OpenTK;

namespace GlslTutorials
{
	public class CollisionObject : ICollision
	{
		public virtual Vector3 GetOffset()
		{
			return new Vector3();
		}

		public virtual Vector3 GetNormal()
		{
			return new Vector3(0f, 0f, 1f);
		}
	}
}

