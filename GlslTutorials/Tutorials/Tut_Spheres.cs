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
		LitMatrixSphere2 lms2;
		TextureSphere ts;
		
		protected override void init()
	    {
			lms1 = new LitMatrixSphere(0.2f);	
			lms2 = new LitMatrixSphere2(0.2f);
			ts = new TextureSphere(0.3f);
			lms2.SetColor(Colors.BLUE_COLOR);
			SetupDepthAndCull();
		}
		
		float angle = 0;
		Vector3 tsAxis = new Vector3(0.1f, 1f, 0f);
		
		private void MoveSpheres()
		{
			float sin = (float) Math.Sin(angle);
			float cos = (float) Math.Cos (angle);
			lms1.SetOffset(new Vector3(sin, cos, 0f));
			lms2.SetOffset(new Vector3(cos, sin, 0f));
			ts.RotateShape(tsAxis, angle);
			angle += 0.02f;
		}
		
		
		public override void display()
	    {
	        ClearDisplay();
			lms1.Draw();
			lms2.Draw();
			ts.Draw();
			MoveSpheres();
		}
	
	}
}

