using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_TexturePerspective : TutorialBase
	{
		static TextureElement wood;
		bool drawWood = true;

		float perspectiveAngle = 160f;
		float newPerspectiveAngle = 160f;

		protected override void init ()
		{
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			wood = new TextureElement("wood4_rotate.png");
			wood.Scale(0.5f);
			wood.RotateShape(new Vector3(1f, 0f, 0f), 60f);
			wood.Move(0f, 0f, -0.2f);

			SetupDepthAndCull();
			Textures.EnableTextures();
		}

		static private void SetGlobalMatrices()
		{
			wood.SetCameraToClipMatrix(cameraToClipMatrix);
			wood.SetWorldToCameraMatrix(worldToCameraMatrix);
		}

		public override void reshape()
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(perspectiveAngle, (width / (float)height), g_fzNear, g_fzFar);

			worldToCameraMatrix = persMatrix.Top();

			cameraToClipMatrix = Matrix4.Identity;
			cameraToClipMatrix.M34 = -1f;

			SetGlobalMatrices();

			GL.Viewport(0, 0, width, height);

		}

		public override void display()
		{
			ClearDisplay();
			if (drawWood) wood.Draw();
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
					wood.RotateShape(Vector3.UnitX, 5f);
					break;
				case Keys.D2:
					wood.RotateShape(Vector3.UnitY, 5f);
					break;
				case Keys.D3:
					wood.RotateShape(Vector3.UnitZ, 5f);
					break;
				case Keys.D4:
					wood.RotateShape(Vector3.UnitX, -5f);
					break;
				case Keys.D5:
					wood.RotateShape(Vector3.UnitY, -5f);
					break;
				case Keys.D6:
					wood.RotateShape(Vector3.UnitZ, -5f);
					break;
				case Keys.D7:
					break;
				case Keys.D8:
					break;
				case Keys.D9:
					break;
				case Keys.D0:
					wood.SetRotation(Matrix3.Identity);
					break;
				case Keys.F:
					break;
				case Keys.P:
					newPerspectiveAngle = perspectiveAngle + 5f;
					if (newPerspectiveAngle > 170f) {
						newPerspectiveAngle = 30f;
					}
					break;
				case Keys.W:
					if (drawWood)
						drawWood = false;
					else
						drawWood = true;
					break;
				}
			}
			return result.ToString();
		}
	}
}

