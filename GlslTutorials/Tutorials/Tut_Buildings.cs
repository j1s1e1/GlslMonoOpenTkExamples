using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tut_Buildings : TutorialBase
	{
		List<Building> buildings;
		int count = 1000;
		protected override void init ()
		{
			Building.SetOffsetScale(new Vector3(10f, 10f, 10f));
			buildings = new List<Building>();
			for (int i = 0; i < count; i++)
			{
				buildings.Add(new Building());
			}
			SetupDepthAndCull();
			Matrix4 worldToCamera = Matrix4.CreateScale(0.05f);
			Shape.SetWorldToCameraMatrix(worldToCamera);
			Shape.SetCameraOffset(new Vector3(0f, 0f, -0.5f));
		}

		public override void display()
		{
			ClearDisplay();
			foreach (Building b in buildings)
			{
				b.Draw();
			}
		}

		Vector3 focalPoint = new Vector3(0f, 0f, 0.5f);

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			switch (keyCode)
			{
			case Keys.D1:
				Shape.RotateWorld(Vector3.UnitX, 5f);
				result.AppendLine("RotateWorld 5X");
				result.AppendLine(AnalysisTools.CalculateMatrixEffects(Shape.worldToCamera));
				break;
			case Keys.D2:
				Shape.RotateWorld(Vector3.UnitY, 5f);
				result.AppendLine("RotateWorld 5Y");
				break;
			case Keys.D3:
				Shape.RotateWorld(Vector3.UnitZ, 5f);
				result.AppendLine("RotateWorld 5Z");
				break;
			case Keys.D4:
				Shape.RotateCamera(focalPoint, Vector3.UnitX, 5f);
				result.AppendLine("RotateWorld 5X");
				break;
			case Keys.D5:
				Shape.RotateWorld(Vector3.UnitY, 5f);
				result.AppendLine("RotateWorld 5Y");
				break;
			case Keys.D6:
				Shape.RotateWorld(Vector3.UnitZ, 5f);
				result.AppendLine("RotateWorld 5Z");
				break;
			}
			return result.ToString();
		}
	}
}

