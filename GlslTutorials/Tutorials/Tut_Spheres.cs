using System;
using System.Drawing;
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
		ShadedSphere ss;
		TextureSphere ts;
		TextureHeightSphere heightMapPlanet;
		
		protected override void init()
	    {
			lms1 = new LitMatrixSphere(0.2f);	
			lms2 = new LitMatrixSphere2(0.2f);
			ts = new TextureSphere(0.2f);
			lms2.SetColor(Colors.BLUE_COLOR);
			
			ss = new ShadedSphere(0.3f);

			Color deep = Color.FromArgb(0, 0, 128);
			Color shalow = Color.FromArgb(0, 0, 255);
			Color shore = Color.FromArgb(0, 128, 255);
			Color sand = Color.FromArgb(240, 240,  64);
			Color grass = Color.FromArgb(32, 160,   0);
			Color dirt = Color.FromArgb(224, 224,   0);
			Color rock = Color.FromArgb(128, 128, 128);
			Color snow = Color.FromArgb(255, 255, 255);
			Color[] colors = new Color[]{deep, shalow, shore, sand, grass, dirt, rock, snow};
			float[] heights = new float[]{0.8f, 0.8f, 0.85f, 0.9f, 0.95f, 1.0f, 1.05f, 1.2f};
			heightMapPlanet = new TextureHeightSphere(0.2f, colors, heights, "heightmap.bmp");
			
			SetupDepthAndCull();
		}

		float angle = 0;
		Vector3 tsAxis = new Vector3(0.1f, 1f, 0f);
		Vector3 hmAxis = new Vector3(0.1f, 1f, 0.1f);
		
		private void MoveSpheres()
		{
			float sin = (float) Math.Sin(angle);
			float cos = (float) Math.Cos (angle);
			lms1.SetOffset(new Vector3(sin, cos, 0f));
			lms2.SetOffset(new Vector3(cos, sin, 0f));
			ts.SetOffset(new Vector3(-sin, cos, 0f));
			heightMapPlanet.SetOffset(new Vector3(-sin, -cos, 0f));
			ts.RotateShape(tsAxis, angle/4f);
			ss.RotateShape(tsAxis, angle);
			heightMapPlanet.RotateShape(hmAxis, angle/8f);
			angle += 0.02f;
		}
		
		
		public override void display()
	    {
	        ClearDisplay();
			lms1.Draw();
			lms2.Draw();
			ts.Draw();
			ss.Draw();
			heightMapPlanet.Draw();
			MoveSpheres();
		}
	
	}
}

