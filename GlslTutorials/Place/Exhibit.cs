using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Exhibit
	{
		protected List<LitMatrixBlock3> bars;
		private Vector3 offset = new Vector3(); 

		public Exhibit ()
		{
			bars = new List<LitMatrixBlock3>();
			float xstart = -0.8f;
			float xend = 0.8f;
			float xstep = 0.2f;
			for (float x = xstart; x <= xend; x += xstep)
			{
				LitMatrixBlock3 newBar = new LitMatrixBlock3(new Vector3(0.05f, 1.6f, 0f), Colors.BLUE_COLOR);
				newBar.Move(new Vector3(x, 0f, 0f));
				bars.Add(newBar);
			}
		}

		public virtual void Move(Vector3 v)
		{
			foreach (LitMatrixBlock3 l in bars)
			{
				l.Move(v);
			}
			offset = offset + v;
		}

		public Vector3 GetOffset()
		{
			return offset;
		}

		public virtual void Draw()
		{
			foreach (LitMatrixBlock3 l in bars)
			{
				l.Draw();
			}
		}
	}
}

