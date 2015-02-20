using System;
using OpenTK;

namespace GlslTutorials
{
	public class Wing
	{
		LitMatrixBlock3 lmb;
		public Wing ()
		{
			lmb = new LitMatrixBlock3(new Vector3(0.4f, 0.05f, 0.15f), Colors.GREEN_COLOR);
		}

		public void Draw()
		{
			lmb.Draw();
		}

		public void Move(Vector3 v)
		{
			lmb.Move(v);
		}

		public void RotateShape(Vector3 axis, float angleDeg)
		{
			lmb.RotateShape(axis, angleDeg);
		}

		public void RotateShape(Vector3 offset, Vector3 axis, float angleDeg)
		{
			lmb.RotateShape(offset, axis, angleDeg);
		}
	}
}

