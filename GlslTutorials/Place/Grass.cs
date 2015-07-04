using System;
using OpenTK;

namespace GlslTutorials
{
	public class Grass : Exhibit
	{
		TextureElement grass;

		public Grass ()
		{
			grass = new TextureElement("grass.jpg");
			grass.Scale(1.0f);
			grass.RotateShape(Vector3.UnitX, 45f);
			grass.Move(0f, -0.8f, 0f);
		}

		public override void Draw ()
		{
			grass.Draw();
		}
	}
}

