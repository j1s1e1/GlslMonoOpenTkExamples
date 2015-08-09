using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Wing
	{
		TextureElement2 te;
		Vector3 scale = new Vector3(0.2f, 0.2f, 0.2f);
		Vector3 offset = new Vector3(0.2f, 0f, 0f);
		int flapCount = 0;
		bool flapClosed = true;
		bool flapEnabled = true;
		float flapAngle = 5f;

		public Wing (string wingTexture = "Butterfly/swallowtail256.png")
		{
			te = new TextureElement2(wingTexture);
			te.Scale(scale);
		}

		private void Flap()
		{
			if (flapClosed)
			{
				Rotate(Vector3.UnitY, flapAngle);
			}
			else
			{
				Rotate(Vector3.UnitY, -flapAngle);
			}
			flapCount = flapCount + 5;
			if (flapCount == 90)
			{
				flapCount = 0;
				if (flapClosed)
				{
					flapClosed = false;
				}
				else
				{
					flapClosed = true;
				}
			}
		}

		public void SetFlapAngle(float f)
		{
			flapAngle = f;
		}

		public void SetFlapEnable(bool flapEnable)
		{
			flapEnabled = flapEnable;
		}

		public void Scale(Vector3 v)
		{
			te.Scale(v);
			offset = offset * scale;
		}

		public void Move(Vector3 v)
		{
			te.Move(v);
			offset = offset + v;
		}

		public void Rotate(Vector3 axis, float angleDeg)
		{
			te.RotateShape(offset, axis, angleDeg);
		}

		public Quaternion GetOrientation()
		{
			return Quaternion.FromMatrix(new Matrix3(te.modelToWorld));
		}

		public void Draw()
		{
			te.Draw();
			if (flapEnabled) Flap();
		}

		public void SetProgram(int program)
		{
			te.SetProgram(program);
		}
	}
}

