using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_3D_Shooter : TutorialBase
	{
		public Tut_3D_Shooter ()
		{
		}
		
		Blender ship;
		List<Missle> missles = new List<Missle>();
	 	bool addMissle = false;
				
		protected override void init()
	    {
			string BlenderFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/Blender/";
			ship = new Blender();
			ship.ReadFile(BlenderFilesDirectory + "X_Wing3.obj");
			ship.SetColor(Colors.WHITE_COLOR);
			ship.Scale(new Vector3(0.1f, 0.1f, 0.1f));

			GL.Enable(EnableCap.CullFace);
	        GL.CullFace(CullFaceMode.Back);
	        GL.FrontFace(FrontFaceDirection.Cw);
	
	        GL.Enable(EnableCap.DepthTest);
	        GL.DepthMask(true);
	        GL.DepthFunc(DepthFunction.Lequal);
	        GL.DepthRange(0.0f, 1.0f);   
		}
		
		double anglehorizontal = 0;
		double anglevertical = 0;
		
		Vector3 axis = new Vector3(0f, 0f, 1f);
		Vector3 up = new Vector3(0f, 0.125f, 0f);
		Vector3 right = new Vector3(0.25f, 0f,0f);
		
		
		public override void display()
	    {
			List<int> deadMissles = new List<int>();
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			ship.Draw();
			anglehorizontal = anglehorizontal + 0.02f;
			anglevertical = anglevertical + 0.01f;
			for (int i = 0; i < missles.Count; i++)
			{
				if (missles[i].Firing())
				{
					missles[i].Draw();
				} 
				else
				{
					if (missles[i].Finished())
					{
						deadMissles.Add(i);
					}
				}
			}

   			if (deadMissles.Count > 0)
			{
				missles.RemoveAt(deadMissles[0]);
			}
			
			if (addMissle) 
			{
 	            missles.Add(new Missle(axis, up, right));
	            addMissle = false;
        	}
		}
		
		private void Rotate(Vector3 rotationAxis, float angle)
		{
			ship.RotateShapes(rotationAxis, angle);
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angle);
			axis = Vector3.Transform(axis, rotation);
			up = Vector3.Transform(up, rotation);
			right = Vector3.Transform(right, rotation);
		}
		
	    public override String keyboard(Keys keyCode, int x, int y)
	    {
	        StringBuilder result = new StringBuilder();
	        switch (keyCode) 
			{
				case Keys.D1:
					Rotate(Vector3.UnitX, 5f);
					break;
				case Keys.D2:
					Rotate(Vector3.UnitX, -5f);
					break;
				case Keys.D3:
					Rotate(Vector3.UnitY, 5f);
					break;
				case Keys.D4:
					Rotate(Vector3.UnitY, -5f);
					break;
				case Keys.D5:
					Rotate(Vector3.UnitZ, 5f);
					break;
				case Keys.D6:
					Rotate(Vector3.UnitZ, -5f);
					break;		
				case Keys.I:
					result.AppendLine("Found " + ship.ObjectCount().ToString() + " objects in ship file.");
					break;
				case Keys.Space:
					if (addMissle == false)
					{
						if (missles.Count < 10)
						{
							addMissle = true;
						}
					}
					result.AppendLine("axis " + axis.ToString());
					result.AppendLine("up " + up.ToString());
					result.AppendLine("right " + right.ToString());
					result.AppendLine("missles.Count " + missles.Count.ToString());
					break;
	        }
	        return result.ToString();
	    }
	}
}

