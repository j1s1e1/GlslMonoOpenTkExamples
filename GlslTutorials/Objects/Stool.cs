using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Stool
	{
		List<LitMatrixBlock3> lmbs = new List<LitMatrixBlock3>();
		public Stool()
		{
			LitMatrixBlock3 leftFront = new LitMatrixBlock3(new Vector3(0.05f, 0.2f, 0.05f), Colors.BLUE_COLOR);
			leftFront.Move(new Vector3(-0.25f, -0.1f, -0.25f));
			lmbs.Add(leftFront);
			LitMatrixBlock3 rightFront = new LitMatrixBlock3(new Vector3(0.05f, 0.2f, 0.05f), Colors.BLUE_COLOR);
			rightFront.Move(new Vector3(0.25f, -0.1f, -0.25f));
			lmbs.Add(rightFront);
			LitMatrixBlock3 leftBack = new LitMatrixBlock3(new Vector3(0.05f, 0.2f, 0.05f), Colors.BLUE_COLOR);
			leftBack.Move(new Vector3(-0.25f, -0.1f, -0.75f));
			lmbs.Add(leftBack);
			LitMatrixBlock3 rightBack = new LitMatrixBlock3(new Vector3(0.05f, 0.2f, 0.05f), Colors.BLUE_COLOR);
			rightBack.Move(new Vector3(0.25f, -0.1f, -0.75f));
			lmbs.Add(rightBack);
			LitMatrixBlock3 top = new LitMatrixBlock3(new Vector3(0.55f, 0.05f, 0.55f), Colors.BLUE_COLOR);
			top.Move(new Vector3(0.0f, -0.0475f, 0.0f));
			lmbs.Add(top);
		}

		public void Move(Vector3 v)
		{
			foreach (LitMatrixBlock3 lmb in lmbs)
			{
				lmb.Move(v);
			}
		}

		public void Draw()
		{
			foreach (LitMatrixBlock3 lmb in lmbs)
			{
				lmb.Draw();
			}
		}
	}
}

