using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Blocks : TutorialBase
	{
		List<LitMatrixBlock3> lmbs;
		
		private void AddBlock(Vector3 size, Vector3 offset, Vector3 axis, float[] color)
		{
			LitMatrixBlock3 lmb1 = new LitMatrixBlock3(size, color);
			lmb1.SetOffset(offset);
			lmb1.SetAxis(axis);
			lmbs.Add(lmb1);	
		}
		
		
		protected override void init()
	    {
			lmbs = new List<LitMatrixBlock3>();
			AddBlock(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.5f, 0.25f, 0.5f),
			         new Vector3(0f, 1f, 0f), Colors.BLUE_COLOR);

			AddBlock(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(-0.5f, -0.25f, 0.5f),
			         new Vector3(1f, 0f, 0f), Colors.GREEN_COLOR);
			
			AddBlock(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(-0.25f, -0.5f, 0.5f),
			         new Vector3(0f, 0f, 1f), Colors.CYAN_COLOR);
			
			AddBlock(new Vector3(0.15f, 0.1f, 0.15f), new Vector3(-0.5f, -0.5f, 0.5f),
			         new Vector3(0.2f, 0.2f, .2f), Colors.RED_COLOR);
				
			lmbs.Add(new LitMatrixBlock3(new Vector3(0.25f, 0.25f, 0.25f), Colors.GREEN_COLOR));	
		}

		public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			foreach (LitMatrixBlock3  lmb in lmbs)
			{
				lmb.Draw();
				lmb.RotateShapeAboutAxis(1f);
			}
		}
	}
}

