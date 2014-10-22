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
		
		LitMatrixSphere2 lms1;
		LitMatrixSphere2 lms2;
		LitMatrixBlock2 lmb2;
		
		float lmb2_angle = 0f;
		
		protected override void init()
	    {
			lms1 = new LitMatrixSphere2(0.2f);
			lms2 = new LitMatrixSphere2(0.2f);
			lms2.SetOffset(new Vector3(-0.5f, 0f, 0f));
			lms2.SetColor(0f, 1f, 0f);
			
			Vector3 size = new Vector3(0.1f, 0.1f, 0.1f);
			Vector3 offset = new Vector3(0.5f, 0.025f, 0f);
			Vector3 axis = new Vector3(1f, 1f, 1f);
			
			lmb2 = new LitMatrixBlock2(size, Colors.BLUE_COLOR);
			lmb2.SetOffset(offset);
			lmb2.SetAxis(axis);
			
			SetupDepthAndCull();
		}
		
		public override void display()
	    {
	       	ClearDisplay();
			lms1.Draw();
			lms2.Draw();
			lmb2.Draw();
			lmb2.UpdateAngle(lmb2_angle++);
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

