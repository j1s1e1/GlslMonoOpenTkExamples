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
		Vector3 offset = new Vector3(0.0f, 0f, 0f);
		Vector3 rotationAxis = Vector3.UnitY;
		int flapCount = 0;
		bool flapClosed = true;
		bool flapEnabled = true;
		float flapAngle = 5f;

		public Wing (string wingTexture = "Butterfly/swallowtail256.png")
		{
			te = new TextureElement2(wingTexture);
			te.Scale(scale);
			te.Move(new Vector3(-1f * 0.2f, 0f, 0f));
			//offset = new Vector3(0.25f, 0f, 0f);
		}

		private void Flap()
		{
			if (flapClosed)
			{
				Rotate(rotationAxis, flapAngle);
			}
			else
			{
				Rotate(rotationAxis, -flapAngle);
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

		public void SetRotationAxis(Vector3 v)
		{
			rotationAxis = v;
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
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(axis, (float)Math.PI / 180.0f * angleDeg);
			te.RotateShape(offset, rotation);
			rotationAxis = Vector3.Transform(rotationAxis, rotation);
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

