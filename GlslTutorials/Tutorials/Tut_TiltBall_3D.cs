using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_TiltBall3D : TutorialBase
	{
		public Tut_TiltBall3D()
		{
		}
		
		List<LitMatrixBlock3> maze;
		LitMatrixSphere2 ball;
		
		Vector3 ballPosition = Vector3.Zero;
		float ballLimit = 0.7f;
		
		private void AddBlock(Vector3 size, Vector3 offset, float[] color)
		{
			LitMatrixBlock3 lmb = new LitMatrixBlock3(size, color);
			lmb.SetOffset(offset);
			maze.Add(lmb);
		}

		protected override void init()
		{		
			ball = new LitMatrixSphere2(0.1f);
			ball.SetOffset(new Vector3(0f, 0f, -0.25f));
			maze = new List<LitMatrixBlock3>();
			AddBlock(new Vector3(2.0f, 2.0f, 0.1f), new Vector3(0.0f, 0.0f, 0.9f), Colors.BLUE_COLOR);
			AddBlock(new Vector3(0.2f, 2.0f, 0.1f), new Vector3(-0.9f, 0.0f, 0.8f), Colors.RED_COLOR);
			AddBlock(new Vector3(0.2f, 2.0f, 0.1f), new Vector3(0.9f, 0.0f, 0.8f), Colors.RED_COLOR);
			AddBlock(new Vector3(2.0f, 0.2f, 0.1f), new Vector3(0.0f, -0.9f, 0.8f), Colors.RED_COLOR);
			AddBlock(new Vector3(2.0f, 0.2f, 0.1f), new Vector3(0.0f, 0.9f, 0.8f), Colors.RED_COLOR);
		
				
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.FrontFace(FrontFaceDirection.Cw);
			
			GL.Enable(EnableCap.DepthTest);
			GL.DepthMask(true);
			GL.DepthFunc(DepthFunction.Lequal);
			GL.DepthRange(0.0f, 1.0f);
		}
		
		private void MoveBall()
		{
			if (xAngle > 0)
			{
				if (ballPosition.X < ballLimit) ballPosition.X = ballPosition.X + 0.01f;
			}
			if (xAngle < 0)
		    {
				if (ballPosition.X > -ballLimit) ballPosition.X = ballPosition.X - 0.01f;
			}
			if (yAngle > 0)
			{
				if (ballPosition.Y < ballLimit) ballPosition.Y = ballPosition.Y + 0.01f;
			}
			if (yAngle < 0)
		    {
				if (ballPosition.Y > -ballLimit) ballPosition.Y = ballPosition.Y - 0.01f;;
			}
			ball.SetOffset(ballPosition);
		}

		public override void display()
		{
			MoveBall();
			// Draw background color
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			GL.ClearDepth(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			ball.Draw();
			foreach (LitMatrixBlock3 lmb in maze) {
				lmb.Draw();
			}

			GlControl.SwapBuffers();
		}
		
		public override string keypress(System.Windows.Forms.Keys key, int x, int y)
		{
			return keyboard(key, x, y);
		}
		
		private float xAngle = 0f;
		private float yAngle = 0f;
				
		public override string keyboard(System.Windows.Forms.Keys key, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			switch (key) {
			case Keys.Escape:
				{
					timer.Stop();
					break;
				}
			case System.Windows.Forms.Keys.Up:			
			case System.Windows.Forms.Keys.NumPad8:
				{
					if (yAngle < 45) yAngle++;			
					break;
				}			
			case System.Windows.Forms.Keys.W:
				fzFar++;
				break;
			case System.Windows.Forms.Keys.S:
				fzFar--;
				break;		
			case System.Windows.Forms.Keys.E:
				break;
			case System.Windows.Forms.Keys.D:
				break;	
			case System.Windows.Forms.Keys.R:
				//Earth.SetAxis(1f, 0f, 0f);
				break;
			case System.Windows.Forms.Keys.F:
				//Earth.SetAxis(0f, 1f, 0f);
				break;								
			case System.Windows.Forms.Keys.V:
				//Earth.SetAxis(0f, 0f, 1f);
				break;	
			case System.Windows.Forms.Keys.P:
				if (timer.Enabled) {
					timer.Enabled = false;
				} else {
					timer.Enabled = true;
				}
				break;																			
			case System.Windows.Forms.Keys.Down:
			case System.Windows.Forms.Keys.NumPad2:
				{
					if (yAngle > -45) yAngle--;	
					break;
				}
				
			case System.Windows.Forms.Keys.Right:			
			case System.Windows.Forms.Keys.NumPad6:
				{
					if (xAngle < 45) xAngle++;
					break;
				}
			case System.Windows.Forms.Keys.Left:
			case System.Windows.Forms.Keys.NumPad4:
				{
					if (xAngle > -45) xAngle--;
					break;
				}
			case System.Windows.Forms.Keys.NumPad5:
				{
					
					break;
				}
			}
			result.AppendLine(
				"xAngle = " + xAngle.ToString() + 
				"\nyAngle = " + yAngle.ToString() + 
				"Offset = " + ballPosition.ToString()
				);
			RotateBlocks();
			return result.ToString();
		}

		private void RotateBlocks()
		{
			Shape.SetWorldToCameraRotation(-yAngle, xAngle, 0f);
		}
	}

}