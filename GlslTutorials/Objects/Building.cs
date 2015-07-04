using System;
using OpenTK;
using System.Collections.Generic;

namespace GlslTutorials
{
	public class Building
	{
		static int maxStories = 10;
		int stories;
		static Random random = new Random();
		List<LitMatrixBlock2> floors;
		Vector3 center;
		static Vector3 offsetScale = new Vector3(1f, 1f, 1f);

		public Building()
		{
			floors = new List<LitMatrixBlock2>();
			float x = (float)random.NextDouble() * 2f - 1f;
			float z = (float)random.NextDouble() * 2;
			center = new Vector3(x * offsetScale.X, 0, z * offsetScale.Z);
			stories = random.Next(maxStories);
			for (int i = 0; i < stories; i++)
			{
				LitMatrixBlock2 newFloor = new LitMatrixBlock2(new Vector3(0.2f, 0.2f, 0.2f), Colors.BLUE_COLOR);
				newFloor.SetOffset(center + new Vector3(0f, i * 0.2f, 0f));
				floors.Add(newFloor);

			}

			x = (float)random.NextDouble()/4;
			z = (float)random.NextDouble()/8;
		}

		public void Draw()
		{
			for (int i = 0; i < stories; i++)
			{
				floors[i].Draw();
			}
		}

		public static void SetOffsetScale(Vector3 v)
		{
			offsetScale = v;
		}
	}
}

