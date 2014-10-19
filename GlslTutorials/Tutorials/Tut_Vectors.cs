using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Vectors : TutorialBase
	{
		LitMatrixBlock2 lmb2;
		Vector3 axis;	
		Vector3 angles = new Vector3(0f, 0f, 0f);
		
		
		public Tut_Vectors ()
		{
		}
		
		protected override void init()
	    {
			lmb2 = new LitMatrixBlock2(new Vector3 (0.05f, 1f, 0.05f), Colors.GREEN_COLOR);
			axis = new Vector3(0f, 1f, 0f);
			lmb2.SetAxis(axis);
		}
		
		public override void display()
	    {
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			lmb2.Draw();
		}
		
		public void Rotate(Vector3 rotationAxis, float angle)
		{
			lmb2.RotateShape(rotationAxis, angle);
		}
		
		public override String keyboard(Keys keyCode, int x, int y)
	    {
	        StringBuilder result = new StringBuilder();
	        switch (keyCode) 
			{
				case Keys.D1:
					Rotate(Vector3.UnitX, 5f);
					angles.X += 5f;
					break;
				case Keys.D2:
					Rotate(Vector3.UnitX, -5f);
					angles.X += -5f;
					break;
				case Keys.D3:
					Rotate(Vector3.UnitY, 5f);
					angles.Y += 5f;
					break;
				case Keys.D4:
					Rotate(Vector3.UnitY, -5f);
					angles.Y += -5f;
					break;
				case Keys.D5:
					Rotate(Vector3.UnitZ, 5f);
					angles.Z += 5f;
					break;
				case Keys.D6:
					Rotate(Vector3.UnitZ, -5f);
					angles.Z += -5f;
					break;		
				case Keys.I:
					break;
				case Keys.Space:
					break;
	        }
			if (angles.X > 180f) angles.X -= 360f;
			if (angles.Y > 180f) angles.Y -= 360f;
			if (angles.Z > 180f) angles.Z -= 360f;
			if (angles.X < -180f) angles.X += 360f;
			if (angles.Y < -180f) angles.Y += 360f;
			if (angles.Z < -180f) angles.Z += 360f;
			result.AppendLine("angles = " + angles.ToString());
	        return result.ToString();
	    }                                       
	}
}

