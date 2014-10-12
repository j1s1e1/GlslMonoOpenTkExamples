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
		
		protected override void init()
	    {
			string BlenderFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/Blender/";
			blender = new Blender();
			blender.ReadFile(BlenderFilesDirectory + "test.obj");
			//blender.ReadFile(BlenderFilesDirectory + "SingleSideClean.obj");
			//blender.ReadFile(BlenderFilesDirectory + "SingleSideCleanFirst2.obj");
			//blender.ReadFile(BlenderFilesDirectory + "SingleSideOriginal2.obj");
			//blender.ReadFile(BlenderFilesDirectory + "Simpler.obj");
			
			//GL.Enable(EnableCap.CullFace);
	        //GL.CullFace(CullFaceMode.Back);
	        //GL.FrontFace(FrontFaceDirection.Cw);
	
	        //GL.Enable(EnableCap.DepthTest);
	        //GL.DepthMask(true);
	        //GL.DepthFunc(DepthFunction.Lequal);
	        //GL.DepthRange(0.0f, 1.0f);
	        
		}
		
		public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			blender.Draw();
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
					break;
	        }
	        return result.ToString();
	    }
		
	}
}

