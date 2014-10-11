using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Camera : TutorialBase
	{
		public Tut_Camera ()
		{
		}
		
		LitMatrixSphere2 lms2;
		LitMatrixBlock2 lmb2;
		
		protected override void init()
	    {
			lms2 = new LitMatrixSphere2(0.2f);
			
			Vector3 size = new Vector3(0.1f, 0.1f, 0.1f);
			Vector3 offset = new Vector3(0.5f, 0.025f, 0f);
			Vector3 axis = new Vector3(0f, 1f, 0f);
			
			lmb2 = new LitMatrixBlock2(size, Colors.BLUE_COLOR);
			lmb2.SetOffset(offset);
			lmb2.SetAxis(axis);
		}
		
		public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			lms2.Draw();
			lmb2.Draw();
		}
		
		public override String keyboard(Keys keyCode, int x, int y)
	    {
	        StringBuilder result = new StringBuilder();
	        switch (keyCode) 
			{
				case Keys.N:
					Shape.worldToCamera.M42 = Shape.worldToCamera.M42 + 0.01f;
					break;
				case Keys.S:
					Shape.worldToCamera.M42 = Shape.worldToCamera.M42 - 0.01f;
				    break;
				case Keys.E:
					Shape.worldToCamera.M41 = Shape.worldToCamera.M41 + 0.01f;
					break;
				case Keys.W:
					Shape.worldToCamera.M41 = Shape.worldToCamera.M41 - 0.01f;	
					break;
	            case Keys.Space:
	                break;
	        }
	        result.AppendLine(keyCode.ToString());
	        reshape();
	        display();
	        return result.ToString();
	    }
	
	}
}

