using System;
using System.Drawing;
using OpenTK;

namespace GlslTutorials
{
	public class Dandelions : Exhibit
	{
		Blender grass;
		Blender[] dandelions;
		Random random = new Random();

		private Blender CreateDandelion()
		{
			Blender dandelion = new Blender();
			dandelion.ReadBinaryFile("dandelion.bin");
			dandelion.SetColor(Colors.YELLOW_COLOR);
			dandelion.RotateShape(Vector3.UnitX, 45f);
			float xOffset = (random.Next(20) - 10) / 10f;
			float yOffset = (random.Next(20) - 10) / 100f;
			float zOffset = (random.Next(20) - 10) / 10f;
			dandelion.SetOffset(new Vector3(xOffset, -1f + yOffset, zOffset));
			int rotation = random.Next(360);
			float scale = (random.Next(15) + 10) / 10f;
			dandelion.Scale(new Vector3(scale, scale, scale));
			dandelion.RotateShape(Vector3.UnitY, rotation);
			dandelion.Scale(new Vector3(0.2f, 0.2f, 0.2f));
			return dandelion;
		}

		public Dandelions ()
		{
			dandelions = new Blender[20];
			for (int i = 0; i < dandelions.Length; i++)
			{
				dandelions[i] = CreateDandelion();
			}
			grass = new Blender();
			grass.ReadFile("grassonly.obj");
			grass.Scale(new Vector3(1f, 3f, 1f));
			grass.RotateShape(Vector3.UnitX, 45f);
			grass.SetColor(Colors.GREEN_COLOR);
			grass.SetOffset(0f, -1.1f, 0f);
		}

		public override void Draw ()
		{
			grass.Draw();
			foreach (Blender b in dandelions)
			{
				b.Draw();
			}
		}
	}
}

