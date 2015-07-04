using System;
using OpenTK;

namespace GlslTutorials
{
	public class River : Exhibit
	{
		TextureElement water;
		public River ()
		{
			water = new TextureElement("water1.jpg");
			water.Scale(1.0f);
			water.RotateShape(Vector3.UnitX, 45f);
			water.Move(0f, -0.8f, 0f);
		}

		public override void Move (Vector3 v)
		{
			water.Move(v);
		}

		public override void Draw ()
		{
			water.Draw();
		}
	}
}

