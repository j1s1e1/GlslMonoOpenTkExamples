using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tut_Plane : TutorialBase
	{
		Plane plane;

		protected override void init()
		{
			plane = new Plane();
		}

		public override void display()
		{
			ClearDisplay();
			plane.Draw();
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
				case Keys.NumPad6:
					plane.Move(new Vector3(0.1f, 0.0f, 0.0f));
					break;
				case Keys.NumPad4:
					plane.Move(new Vector3(-0.1f, 0.0f, 0.0f));
					break;
				case Keys.NumPad8:
					plane.Move(new Vector3(0.0f, 0.1f, 0.0f));
					break;
				case Keys.NumPad2:
					plane.Move(new Vector3(0.0f, -0.1f, 0.0f));
					break;
				case Keys.NumPad7:
					plane.Move(new Vector3(0.0f, 0.0f, 0.1f));
					break;
				case Keys.NumPad3:
					plane.Move(new Vector3(0.0f, 0.0f, -0.1f));
					break;
				case Keys.D1:
					break;
				case Keys.D2:
					break;
				case Keys.D3:
					break;
				case Keys.D4:
					plane.Rotate(Vector3.UnitX, 5f);
					break;
				case Keys.D5:
					plane.Rotate(Vector3.UnitY, 5f);
					break;
				case Keys.D6:
					plane.Rotate(Vector3.UnitZ, 5f);
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

