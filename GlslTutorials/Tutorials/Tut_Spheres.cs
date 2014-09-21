using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Spheres : TutorialBase
	{
		public Tut_Spheres ()
		{
		}
		
		LitMatrixSphere lms1;
		
		protected override void init()
	    {
			lms1 = new LitMatrixSphere(0.2f);	
		}
		
		public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			lms1.Draw();
		}
	
	}
}

