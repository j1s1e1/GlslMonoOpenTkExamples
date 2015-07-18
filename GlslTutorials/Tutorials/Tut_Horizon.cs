using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tut_Horizon : TutorialBase
	{
		TextureSphere planet;

		Vector3 offset = new Vector3(0f, -2f, 1f);

		protected override void init()
		{
			planet = new TextureSphere(2f, 0.0f);
			planet.Move(offset);
			SetupDepthAndCull();
		}

		public override void display()
		{
			ClearDisplay();
			planet.Draw();
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
					Shape.RotateWorld(-offset, Vector3.UnitY, 1f);
					break;
				case Keys.D3:
					Shape.RotateWorld(-offset, Vector3.UnitZ, 1f);
					break;
				case Keys.D4:
					planet.RotateAboutCenter(Vector3.UnitX, 5f);
					result.AppendLine("RotateShape 5X");
					break;
				case Keys.D5:
					planet.RotateAboutCenter(Vector3.UnitY, 5f);
					result.AppendLine("RotateShape 5Y");
					break;
				case Keys.D6:
					planet.RotateAboutCenter(Vector3.UnitZ, 5f);
					result.AppendLine("RotateShape 5Z");
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

