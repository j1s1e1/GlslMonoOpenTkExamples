using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Blocks : TutorialBase
	{
		public Tut_Blocks ()
		{
		}
		
		LitMatrixBlock lmb1;
		
		protected override void init()
	    {
			lmb1 = new LitMatrixBlock(new Vector3(0.10f, 0.10f, 0.10f), Colors.BLUE_COLOR);	
		}
		
		private float angle = 0;
		
		public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			lmb1.Draw();
			lmb1.UpdateAngle(angle++);
		}
	}
}

