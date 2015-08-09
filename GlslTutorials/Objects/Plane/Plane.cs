using System;
using OpenTK;

namespace GlslTutorials
{
	public class Plane
	{
		LitMatrixBlock3 body;
		LitMatrixBlock3 tail;
		PlaneWing rightWing;
		PlaneWing leftWing;
		Vector3 offset = new Vector3(0f, 0f, 0f);

		public Plane ()
		{
			body = new LitMatrixBlock3(new Vector3(0.1f, 0.1f, 0.6f), Colors.GREEN_COLOR);
			tail = new LitMatrixBlock3(new Vector3(0.1f, 0.1f, 0.1f), Colors.GREEN_COLOR);
			tail.Move(new Vector3(0f, 0.05f, -0.25f));
			rightWing = new PlaneWing();
			rightWing.Move(new Vector3(0.05f, 0f, 0f));

			leftWing = new PlaneWing();
			leftWing.RotateShape(new Vector3(0f, 0f, 1f), 180f);
			leftWing.Move(new Vector3(-0.05f, 0f, 0f));
		}

		public void Draw()
		{
			body.Draw();
			tail.Draw();
			rightWing.Draw();
			leftWing.Draw();
		}

		public void Move(Vector3 v)
		{
			offset  += v;
			body.Move(v);
			tail.Move(v);
			rightWing.Move(v);
			leftWing.Move(v);
		}

		public void Rotate(Vector3 axis, float angleDeg)
		{
			body.RotateShape(offset, axis, angleDeg);
			tail.RotateShape(offset, axis, angleDeg);
			rightWing.RotateShape(offset, axis, angleDeg);
			leftWing.RotateShape(offset, axis, angleDeg);
		}
	}
}

