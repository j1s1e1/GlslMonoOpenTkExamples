using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace GlslTutorials
{
	public class Missle
	{
		LitMatrixSphere2 topLeft;
		LitMatrixSphere2 topRight;
		LitMatrixSphere2 bottomLeft;
		LitMatrixSphere2 bottomRight;
		float radius = 0.01f;
		bool started = false;
		bool finished = false;
		bool fire = false;
		
		float[] color = Colors.BLUE_COLOR;
		
		Vector3 topLeftDirection;
		Vector3 topRightDirection;
		Vector3 bottomLeftDirection;
		Vector3 bottomRightDirection;
		
		float stepMultiple = 0.04f;
		
		Vector3 axis;
		
		Timer timer = new Timer();
		
		public Missle (Vector3 axisIn, Vector3 up, Vector3 right)
		{
			axis = axisIn;
			topLeft = new LitMatrixSphere2(radius);
			topRight = new LitMatrixSphere2(radius);
			bottomLeft = new LitMatrixSphere2(radius);
			bottomRight = new LitMatrixSphere2(radius);
			
			topLeft.SetColor(color);
			topRight.SetColor(color);
			bottomLeft.SetColor(color);
			bottomRight.SetColor(color);
			
			topLeft.SetOffset(-right + up);
			topRight.SetOffset(right + up);
			bottomLeft.SetOffset(-right - up);
			bottomRight.SetOffset(right - up);
			
			topLeftDirection = Vector3.Multiply(axis - topLeft.GetOffset(), stepMultiple);
			topRightDirection = Vector3.Multiply(axis - topRight.GetOffset(), stepMultiple);
			bottomLeftDirection = Vector3.Multiply(axis - bottomLeft.GetOffset(), stepMultiple);
			bottomRightDirection = Vector3.Multiply(axis - bottomRight.GetOffset(), stepMultiple);
						
			timer.Interval = 50;
			timer.Tick += TimerTick;
			timer.Start();
		}
		
		void TimerTick(Object o, EventArgs e)
		{
			if (started)
			{
				fire = false;
				finished = true;
			}
			else
			{
				started = true;
				fire = true;
			}
			timer.Stop();	
		}
		
		public bool Started()
		{
			return started;
		}
		
		public bool Firing()
		{
			return fire;
		}
		
		public bool Finished()
		{
			return finished;
		}
		
		public void Clear()
		{
			fire = false;
		}
		
		public void DrawMissle(LitMatrixSphere2 missle, Vector3 step)
		{
			missle.Draw();
			Vector3 offset = missle.GetOffset();
			missle.SetOffset(offset + step);
		}
		
		public void Draw()
		{

			Vector3 offset = topLeft.GetOffset();
			if ((offset - axis).Length < 0.01)
			{
				if (finished == false)
				{
					timer.Interval = 500;
					timer.Start();
				}
			}
			else
			{
				DrawMissle(topLeft, topLeftDirection);
				DrawMissle(topRight, topRightDirection);
				DrawMissle(bottomLeft, bottomLeftDirection);
				DrawMissle(bottomRight, bottomRightDirection);
			}
		}

	}
}

