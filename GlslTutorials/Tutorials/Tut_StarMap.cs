using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_StarMap : TutorialBase
	{
		TextureSphere ts;
		int program;

		bool setScale = false;
		float scale = 1f;

		protected override void init()
		{
			//TextureSphere.reverseNormals = true;
			ts = new TextureSphere(50f, "starmap.png");
			program = Programs.AddProgram(VertexShaders.MatrixTexture,
				FragmentShaders.MatrixTextureScale);
			Programs.SetUniformScale(program, 200000f);
			ts.SetProgram(program);
			//SetupDepthAndCull();
			g_fzNear = 0.1f;
			g_fzFar = 100f;
			worldToCameraMatrix = Matrix4.Identity;
			reshape();
		}

		float perspectiveAngle = 90f;

		static private void SetGlobalMatrices()
		{
			Shape.SetCameraToClipMatrix(cameraToClipMatrix);
			Shape.SetWorldToCameraMatrix(worldToCameraMatrix);
		}

		public override void reshape()
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(perspectiveAngle, (width / (float) height), g_fzNear, g_fzFar);

			cameraToClipMatrix = persMatrix.Top();

			SetGlobalMatrices();

			GL.Viewport(0, 0, width, height);
		}

		public override void display()
		{
			ClearDisplay();
			ts.Draw();
			if (setScale)
			{
				setScale = false;
				Shape.SetScale(scale);
			}
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
				case Keys.D4:
					ts.RotateAboutCenter(Vector3.UnitX, 5f);
					result.AppendLine("RotateShape 5X");
					break;
				case Keys.D5:
					ts.RotateAboutCenter(Vector3.UnitY, 5f);
					result.AppendLine("RotateShape 5Y");
					break;
				case Keys.D6:
					ts.RotateAboutCenter(Vector3.UnitZ, 5f);
					result.AppendLine("RotateShape 5Z");
					break;
				case Keys.NumPad6:
					ts.Move(new Vector3(0.1f, 0.0f, 0.0f));
					break;
				case Keys.NumPad4:
					ts.Move(new Vector3(-0.1f, 0.0f, 0.0f));
					break;
				case Keys.NumPad8:
					ts.Move(new Vector3(0.0f, 0.1f, 0.0f));
					break;
				case Keys.NumPad2:
					ts.Move(new Vector3(0.0f, -0.1f, 0.0f));
					break;
				case Keys.NumPad7:
					ts.Move(new Vector3(0.0f, 0.0f, 0.1f));
					break;
				case Keys.NumPad3:
					ts.Move(new Vector3(0.0f, 0.0f, -0.1f));
					break;
				case Keys.A:
					break;
				case Keys.B:
					break;
				case Keys.C:
					break;
				case Keys.D:
					scale *= 0.8f;
					setScale = true;
					break;
				case Keys.U:
					scale *= 1.2f;
					setScale = true;
					break;
				case Keys.F:
					break;
				case Keys.I:
					result.AppendLine("worldToCamera");
					result.AppendLine(Shape.worldToCamera.ToString());
					result.AppendLine("modelToWorld");
					result.AppendLine(ts.modelToWorld.ToString());
					result.AppendLine(AnalysisTools.CalculateMatrixEffects(ts.modelToWorld));
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

