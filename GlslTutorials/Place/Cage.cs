using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Cage : Exhibit
	{
		public Cage ()
		{
			bars = new List<LitMatrixBlock3>();
			float height = 0.8f;
			float xstart = -0.8f;
			float xend = 0.8f;
			float xstep = 0.2f;
			for (float x = xstart; x <= xend; x += xstep)
			{
				LitMatrixBlock3 newBar = new LitMatrixBlock3(new Vector3(0.05f, 2 * height, 0.5f), Colors.RED_COLOR);
				newBar.Move(new Vector3(x, 0f, 0.5f));
				bars.Add(newBar);
			}
			for (float x = xstart; x <= xend; x += xstep)
			{
				LitMatrixBlock3 newBar = new LitMatrixBlock3(new Vector3(0.05f, 2 * height, 0.5f), Colors.YELLOW_COLOR);
				newBar.Move(new Vector3(x, 0f, -0.5f));
				bars.Add(newBar);
			}
			LitMatrixBlock3 floor = new LitMatrixBlock3(new Vector3(1.8f, 0.1f,1.8f), Colors.GREEN_COLOR);
			floor.Move(new Vector3(0, -0.8f, 0f));
			bars.Add(floor);
		}

		public override void Draw()
		{
			foreach (LitMatrixBlock3 l in bars)
			{
				l.Draw();
			}
		}
	}
}

