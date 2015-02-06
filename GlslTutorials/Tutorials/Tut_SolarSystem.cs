using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_SolarSystem : TutorialBase
	{
		List<TextureSphere> planets;

		private void AddPlanet(Vector3 offset)
		{
			TextureSphere planet = new TextureSphere(2f);
			planet.Move(offset);
			planets.Add(planet);
		}

		protected override void init()
		{
			planets = new List<TextureSphere>();
			AddPlanet(new Vector3(0f, -2f, 1f));
			AddPlanet(new Vector3(5f, -2f, 1f));
			SetupDepthAndCull();
		}

		public override void display()
		{
			ClearDisplay();
			foreach (TextureSphere planet in planets)
			{
				planet.Draw();
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
				case Keys.D1:
					Shape.MoveWorld(new Vector3(0.1f, 0f, 0f));
					break;
				case Keys.D2:
					Shape.MoveWorld(new Vector3(0.0f, 0.1f, 0f));
					break;
				case Keys.D3:
					Shape.MoveWorld(new Vector3(0f, 0f, 0.1f));
					break;
				case Keys.D4:
					//planet.RotateAboutCenter(Vector3.UnitX, 5f);
					result.AppendLine("RotateShape 5X");
					break;
				case Keys.D5:
					//planet.RotateAboutCenter(Vector3.UnitY, 5f);
					result.AppendLine("RotateShape 5Y");
					break;
				case Keys.D6:
					//planet.RotateAboutCenter(Vector3.UnitZ, 5f);
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
					//result.AppendLine(planet.modelToWorld.ToString());
					//result.AppendLine(AnalysisTools.CalculateMatrixEffects(planet.modelToWorld));
					break;
				case Keys.P:
					break;
				case Keys.R:
					break;
				}
			}
			return result.ToString();
		}

		static private void SetGlobalMatrices()
		{
			Shape.SetCameraToClipMatrix(cameraToClipMatrix);
			Shape.SetWorldToCameraMatrix(worldToCameraMatrix);
		}

		float perspectiveAngle = 90f;
		float newPerspectiveAngle = 90f;

		public override void reshape()
		{
			MatrixStack persMatrix = new MatrixStack();
			width = 1280;
			height = 800;
			persMatrix.Perspective(perspectiveAngle, (width / (float)height), g_fzNear, g_fzFar);

			//worldToCameraMatrix = persMatrix.Top();

			//cameraToClipMatrix = Matrix4.Identity;

			//SetGlobalMatrices();

			//GL.Viewport(0, 0, width, height);

		}
	}
}

