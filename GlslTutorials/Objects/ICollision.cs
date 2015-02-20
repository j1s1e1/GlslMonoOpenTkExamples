using System;
using OpenTK;

namespace GlslTutorials
{
	public interface ICollision
	{
		Vector3 GetOffset();
		Vector3 GetNormal();
	}
}

