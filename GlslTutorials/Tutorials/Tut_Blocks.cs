using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Blocks : TutorialBase
	{
		public Tut_Blocks ()
		{
		}
		
		List<LitMatrixBlock> lmbs;
		
		
		protected override void init()
	    {
			lmbs = new List<LitMatrixBlock>();
			LitMatrixBlock lmb1 = new LitMatrixBlock(new Vector3(0.05f, 0.05f, 0.05f), Colors.BLUE_COLOR);
			lmb1.SetOffset(new Vector3(5f, 0.25f, 0f));
			lmbs.Add(lmb1);	
			lmbs.Add(new LitMatrixBlock(new Vector3(0.15f, 0.15f, 0.15f), Colors.RED_COLOR));	
			lmbs.Add(new LitMatrixBlock(new Vector3(0.25f, 0.25f, 0.25f), Colors.GREEN_COLOR));	
		}
		
		private float angle = 0;
		
		public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			foreach (LitMatrixBlock  lmb in lmbs)
			{
				lmb.Draw();
				lmb.UpdateAngle(angle++);
			}
		}
	}
}

