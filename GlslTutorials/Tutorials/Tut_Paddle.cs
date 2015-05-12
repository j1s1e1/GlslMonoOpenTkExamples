using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Paddle : TutorialBase
	{
		Paddle2 paddle;

		float perspectiveAngle = 90f;

		protected override void init ()
		{
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			SetupDepthAndCull();
			g_fzNear = 0.5f;
			g_fzFar = 100f;
			reshape();
			paddle = new Paddle2();
			paddle.SetLimits(new Vector3(-1f, -1f, -1f), new Vector3(1f, 1f, -1f));
			paddle.SetKeyboardControl();
		}

		static private void SetGlobalMatrices()
		{
			Shape.SetCameraToClipMatrix(cameraToClipMatrix);
			Shape.SetWorldToCameraMatrix(worldToCameraMatrix);
		}

		public override void reshape()
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(perspectiveAngle, (width / (float) height), g_fzNear, g_fzFar);

			worldToCameraMatrix = persMatrix.Top();

			cameraToClipMatrix = Matrix4.Identity;

			SetGlobalMatrices();

			GL.Viewport(0, 0, width, height);

		}

		public override void display()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			paddle.Draw();
			UpdateDisplayOptions();
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
					break;
				case Keys.D2:
					break;
				case Keys.D3:
					break;
				case Keys.D4:
					break;
				case Keys.D5:
					break;
				case Keys.D6:
					break;
				case Keys.D7:
					break;
				case Keys.D8:
					break;
				case Keys.D9:
					break;
				case Keys.D0:
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
					result.AppendLine("Paddle Position = " + paddle.GetOffset().ToString());
					break;
				case Keys.K:
					paddle.SetKeyboardControl();
					break;
				case Keys.M:
					paddle.SetMouseControl();
					break;
				case Keys.P:
					break;
				case Keys.R:
					break;
				}
			}
			paddle.keyboard(keyCode);
			return result.ToString();
		}

		public override void MouseMotion (int x, int y)
		{
			paddle.MouseMotion(x, y);
		}
	}
}

