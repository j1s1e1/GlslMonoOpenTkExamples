using System;
using OpenTK;

namespace GlslTutorials
{
	public class Planet
	{
		TextureSphere ts;
		Vector3 axis = new Vector3(0f, 0f, 1f);
		float angleStep = 1f;
		public Planet (float radiusIn, string textureIn = "")
		{
			ts = new TextureSphere(radiusIn, textureIn);
		}

		public void SetAngleStep(float f)
		{
			angleStep = f;
		}

		public void UpdatePosition()
		{
			ts.RotateShape(axis, angleStep);
		}

		public void Draw()
		{
			ts.Draw();
		}

		public void Move(Vector3 v)
		{
			ts.Move(v);
		}

		public void SetProgram(int program)
		{
			ts.SetProgram(program);
		}

		public Vector3 GetLocation()
		{
			return ts.GetOffset();
		}
	}
}

