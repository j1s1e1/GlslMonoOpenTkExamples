using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_WireFramePerspective : TutorialBase
	{
		WireFrameBlock wireFrameBlock;
		bool drawWireFrame = true;

		float perspectiveAngle = 160f;
		float newPerspectiveAngle = 160f;

		protected override void init ()
		{
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			wireFrameBlock = new WireFrameBlock(new Vector3(5f, 5f, 5f), Colors.RED_COLOR);
			wireFrameBlock.RotateShape(new Vector3(1f, 0f, 0f), 60f);
			wireFrameBlock.Move(0f, 0f, -5f);

			SetupDepthAndCull();
			g_fzNear = 0.5f;
			g_fzFar = 10f;
			reshape();
		}

		private void SetGlobalMatrices()
		{
			Shape.SetCameraToClipMatrix(cameraToClipMatrix);
			Shape.SetWorldToCameraMatrix(worldToCameraMatrix);
		}

		public override void reshape()
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(perspectiveAngle, (width / (float)height), g_fzNear, g_fzFar);

			worldToCameraMatrix = persMatrix.Top();

			cameraToClipMatrix = Matrix4.Identity;
			//cameraToClipMatrix.M34 = -1f;

			SetGlobalMatrices();

			GL.Viewport(0, 0, width, height);

		}

		public override void display()
		{
			ClearDisplay();
			if (drawWireFrame) wireFrameBlock.Draw();
			if (perspectiveAngle != newPerspectiveAngle)
			{
				perspectiveAngle = newPerspectiveAngle;
				reshape();
			}
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
					wireFrameBlock.RotateShape(Vector3.UnitX, 5f);
					break;
				case Keys.D2:
					wireFrameBlock.RotateShape(Vector3.UnitY, 5f);
					break;
				case Keys.D3:
					wireFrameBlock.RotateShape(Vector3.UnitZ, 5f);
					break;
				case Keys.D4:
					wireFrameBlock.RotateShape(Vector3.UnitX, -5f);
					break;
				case Keys.D5:
					wireFrameBlock.RotateShape(Vector3.UnitY, -5f);
					break;
				case Keys.D6:
					wireFrameBlock.RotateShape(Vector3.UnitZ, -5f);
					break;
				case Keys.D7:
					break;
				case Keys.D8:
					break;
				case Keys.D9:
					break;
				case Keys.D0:
					wireFrameBlock.SetRotation(Matrix3.Identity);
					break;
				case Keys.F:
					break;
				case Keys.I:
					result.AppendLine("g_fzNear = " + g_fzNear.ToString());
					result.AppendLine("g_fzFar = " + g_fzFar.ToString());
					result.AppendLine("perspectiveAngle = " + perspectiveAngle.ToString());
					break;
				case Keys.P:
					newPerspectiveAngle = perspectiveAngle + 5f;
					if (newPerspectiveAngle > 170f) {
						newPerspectiveAngle = 30f;
					}
					break;
				case Keys.W:
					if (drawWireFrame)
						drawWireFrame = false;
					else
						drawWireFrame = true;
					break;
				case Keys.Z:
					wireFrameBlock.Scale(new Vector3(1.1f, 1.1f, 1.1f));
					break;
				}
			}
			return result.ToString();
		}
	}
}

