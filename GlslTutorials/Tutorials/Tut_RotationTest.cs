using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_RotationTest : TutorialBase
	{
		LitMatrixBlock3 lmb;
		Vector3 center = new Vector3();

		protected override void init()
		{
			lmb = new LitMatrixBlock3(new Vector3(0.5f, 0.05f, 0.05f), Colors.RED_COLOR);
		}

		public override void display()
		{
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			GL.ClearDepth(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			lmb.Draw();
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			if (displayOptions)
			{
				SetDisplayOptions(keyCode);
			}
			else {
				switch (keyCode) {
				case Keys.Enter:
					displayOptions = true;
					break;
				case Keys.D1:
					center += new Vector3(0.1f, 0.0f, 0.0f);
					lmb.Move(new Vector3(0.1f, 0.0f, 0.0f));
					break;
				case Keys.D2:
					center += new Vector3(0.0f, 0.1f, 0.0f);
					lmb.Move(new Vector3(0.0f, 0.1f, 0.0f));
					break;
				case Keys.D3:
					center += new Vector3(0.0f, 0.0f, 0.1f);
					lmb.Move(new Vector3(0.0f, 0.0f, 0.1f));
					break;
				case Keys.D4:
					lmb.RotateShape(Vector3.UnitX, 5f);
					result.AppendLine("RotateShape 5X");
					break;
				case Keys.D5:
					lmb.RotateShape(Vector3.UnitY, 5f);
					result.AppendLine("RotateShape 5Y");
					break;
				case Keys.D6:
					lmb.RotateShape(Vector3.UnitZ, 5f);
					result.AppendLine("RotateShape 5Z");
					break;
				case Keys.A:
					break;
				case Keys.B:
					break;
				case Keys.C:
					break;
				case Keys.D:
					break;
				case Keys.F:
					break;
				case Keys.I:
					result.AppendLine("worldToCamera");
					result.AppendLine(Shape.worldToCamera.ToString());
					result.AppendLine("modelToWorld");
					result.AppendLine(lmb.modelToWorld.ToString());
					result.AppendLine(AnalysisTools.CalculateMatrixEffects(lmb.modelToWorld));
					break;
				case Keys.P:
					break;
				case Keys.R:
					break;
				}
			}
			return result.ToString();
		}
	}
}

