using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tut_Horizon : TutorialBase
	{
		Shape planet;
		TextureSphere defaultPlanet;
		TextureHeightSphere heightMapPlanet;

		Vector3 offset = new Vector3(0f, -2f, 1f);

		bool useHeightMap = false;
		bool usingHeightMap = false;

		protected override void init()
		{
			defaultPlanet = new TextureSphere(2f, 0.0f);
			defaultPlanet.Move(offset);
			planet = defaultPlanet;

			Color deep = Color.FromArgb(0, 0, 128);
			Color shalow = Color.FromArgb(0, 0, 255);
			Color shore = Color.FromArgb(0, 128, 255);
			Color sand = Color.FromArgb(240, 240,  64);
			Color grass = Color.FromArgb(32, 160,   0);
			Color dirt = Color.FromArgb(224, 224,   0);
			Color rock = Color.FromArgb(128, 128, 128);
			Color snow = Color.FromArgb(255, 255, 255);

			/*
			renderer.AddGradientPoint (-1.0000, utils::Color (  0,   0, 128, 255)); // deeps
			renderer.AddGradientPoint (-0.2500, utils::Color (  0,   0, 255, 255)); // shallow
			renderer.AddGradientPoint ( 0.0000, utils::Color (  0, 128, 255, 255)); // shore
			renderer.AddGradientPoint ( 0.0625, utils::Color (240, 240,  64, 255)); // sand
			renderer.AddGradientPoint ( 0.1250, utils::Color ( 32, 160,   0, 255)); // grass
			renderer.AddGradientPoint ( 0.3750, utils::Color (224, 224,   0, 255)); // dirt
			renderer.AddGradientPoint ( 0.7500, utils::Color (128, 128, 128, 255)); // rock
			renderer.AddGradientPoint ( 1.0000, utils::Color (255, 255, 255, 255)); // snow
			*/
			Color[] colors = new Color[]{deep, shalow, shore, sand, grass, dirt, rock, snow};
			float[] heights = new float[]{0.8f, 0.8f, 0.85f, 0.9f, 0.95f, 1.0f, 1.05f, 1.2f};
			heightMapPlanet = new TextureHeightSphere(2f, colors, heights, "heightmap.bmp");
			heightMapPlanet.Move(offset);
			SetupDepthAndCull();
		}

		public override void display()
		{
			ClearDisplay();
			planet.Draw();
			if (useHeightMap)
			{
				if (!usingHeightMap)
				{
					usingHeightMap = true;
					planet = heightMapPlanet;
				}
			}
			else
			{
				if (usingHeightMap)
				{
					usingHeightMap = false;
					planet = defaultPlanet;
				}
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
					Shape.RotateWorld(-offset, Vector3.UnitX, 1f);
					break;
				case Keys.D2:
					Shape.RotateWorld(-offset, Vector3.UnitX, -1f);
					break;
				case Keys.D3:
					Shape.RotateWorld(-offset, Vector3.UnitY, 1f);
					break;
				case Keys.D4:
					Shape.RotateWorld(-offset, Vector3.UnitY, -1f);
					break;
				case Keys.D5:
					Shape.RotateWorld(-offset, Vector3.UnitZ, 1f);
					break;
				case Keys.D6:
					Shape.RotateWorld(-offset, Vector3.UnitZ, -1f);
					break;
				case Keys.NumPad6:
					planet.Move(new Vector3(0.1f, 0.0f, 0.0f));
					break;
				case Keys.NumPad4:
					planet.Move(new Vector3(-0.1f, 0.0f, 0.0f));
					break;
				case Keys.NumPad8:
					planet.Move(new Vector3(0.0f, 0.1f, 0.0f));
					break;
				case Keys.NumPad2:
					planet.Move(new Vector3(0.0f, -0.1f, 0.0f));
					break;
				case Keys.NumPad7:
					planet.Move(new Vector3(0.0f, 0.0f, 0.1f));
					break;
				case Keys.NumPad3:
					planet.Move(new Vector3(0.0f, 0.0f, -0.1f));
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
				case Keys.H:
					if (useHeightMap)
					{
						useHeightMap = false;
					}
					else
					{
						useHeightMap = true;
					}
					break;
				case Keys.I:
					result.AppendLine("worldToCamera");
					result.AppendLine(Shape.worldToCamera.ToString());
					result.AppendLine("modelToWorld");
					result.AppendLine(planet.modelToWorld.ToString());
					result.AppendLine(AnalysisTools.CalculateMatrixEffects(planet.modelToWorld));
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

