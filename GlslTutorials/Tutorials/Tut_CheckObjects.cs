using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace GlslTutorials
{
	public class Tut_CheckObjects : TutorialBase 
	{
		public Tut_CheckObjects ()
		{
		}
		
		LitMatrixBlock lmb;
		
		protected override void init()
	    {
			lmb = new LitMatrixBlock(new Vector3(0.1f, 0.1f, 0.1f), Colors.BLUE_COLOR);
			MessageBox.Show (lmb.CheckRotations(new Vector3(0,0,0)) + lmb.CheckExtents());
		}
	}
}

