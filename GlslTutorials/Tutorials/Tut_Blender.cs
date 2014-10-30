using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Blender : TutorialBase
	{
		public Tut_Blender ()
		{
		}
		
		Blender blender;
		Blender blender2;
		Blender blender3;
		
		TextClass credit1;
		TextClass credit2;
		bool staticText = true;
		
		protected override void init()
	    {
			blender = new Blender();
			blender.ReadFile("test.obj");
			blender.Scale(new Vector3(0.05f, 0.05f, 0.05f));
			blender2 = new Blender();
			blender2.ReadFile("test.obj");
			blender2.SetColor(Colors.BLUE_COLOR);
			blender2.Scale(new Vector3(0.07f, 0.05f, 0.05f));
			blender3 = new Blender();
			blender3.ReadFile("X_Wing3.obj");
			blender3.SetColor(Colors.WHITE_COLOR);
			blender3.Scale(new Vector3(0.1f, 0.1f, 0.1f));

			credit1 = new TextClass("X-Wing Model based on Blender model by", 0.4f, 0.04f, staticText);
        	credit1.SetOffset(new Vector3(-0.75f, -0.65f, 0.0f));

			credit2 = new TextClass("Angel David Guzman of PixelOz Designs", 0.4f, 0.04f, staticText);
        	credit2.SetOffset(new Vector3(-0.75f, -0.75f, 0.0f));
			
			SetupDepthAndCull();
		}
		
		double anglehorizontal = 0;
		double anglevertical = 0;
		float xoffset;
		float yoffset;
		float zoffset;
		
		
		public override void display()
	    {
	        ClearDisplay();
			blender.Draw();
			blender2.Draw();
			blender3.Draw();
			xoffset = (float) (Math.Cos (anglevertical) * Math.Cos (anglehorizontal));
			yoffset = (float) (Math.Cos (anglevertical) * Math.Sin (anglehorizontal));
			zoffset = (float) (Math.Sin (anglevertical)) / 10f;
			blender.SetOffset(new Vector3(xoffset, yoffset, zoffset));
			blender2.SetOffset(new Vector3(-xoffset, yoffset, -zoffset));
			anglehorizontal = anglehorizontal + 0.02f;
			anglevertical = anglevertical + 0.01f;
			credit1.Draw();
			credit2.Draw();
		}
		
	    public override String keyboard(Keys keyCode, int x, int y)
	    {
	        StringBuilder result = new StringBuilder();
	        switch (keyCode) 
			{
				case Keys.D1:
					Shape.RotateWorld(Vector3.UnitX, 5f);
					break;
				case Keys.D2:
					Shape.RotateWorld(Vector3.UnitX, -5f);
					break;
				case Keys.D3:
					Shape.RotateWorld(Vector3.UnitY, 5f);
					break;
				case Keys.D4:
					Shape.RotateWorld(Vector3.UnitY, -5f);
					break;
				case Keys.D5:
					Shape.RotateWorld(Vector3.UnitZ, 5f);
					break;
				case Keys.D6:
					Shape.RotateWorld(Vector3.UnitZ, -5f);
					break;		
				case Keys.I:
					result.AppendLine("Found " + blender.ObjectCount().ToString() + " objects in Blender file.");
					result.AppendLine("Found " + blender2.ObjectCount().ToString() + " objects in Blender 2 file.");
					result.AppendLine("Found " + blender3.ObjectCount().ToString() + " objects in Blender 3 file.");
					break;
				case Keys.S:
					blender.SaveBinaryBlenderObjects("blenderObject1.bin");
				    blender3.SaveBinaryBlenderObjects("xwing3.bin");
					Blender xwing_with_normals = new Blender();
					xwing_with_normals.ReadFile("xwing_with_normals.obj");
					xwing_with_normals.SaveBinaryBlenderObjects("xwng_with_normals.bin");
					break;
	        }
	        return result.ToString();
	    }
		
	}
}

