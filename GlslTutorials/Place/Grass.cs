using System;
using OpenTK;

namespace GlslTutorials
{
	public class Grass : Exhibit
	{
		Blender grass;

		public Grass ()
		{
			grass = new Blender();
			grass.ReadFile("grassonly.obj");
			grass.Scale(new Vector3(1f, 3f, 1f));
			grass.RotateShape(Vector3.UnitX, 45f);
			grass.SetColor(Colors.GREEN_COLOR);
			grass.SetOffset(0f, -0.8f, 0f);
		}

		public override void Draw ()
		{
			grass.Draw();
		}
	}
}

