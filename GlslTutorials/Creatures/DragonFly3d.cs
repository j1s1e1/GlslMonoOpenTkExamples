using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Dragonfly3d : BugClass3d
	{	
	    int x_step = 0;
		float x_offset = 0f;
	    float y_direction = 0;
		float wing_offset = 0;
		
		LitMatrixSphere2[] body;
		
		public Dragonfly3d(int x_in = 0, int y_in = 0, int z_in = 0): base(x_in, y_in, z_in)
		{
			scale = 0.005f;
			body = new LitMatrixSphere2[6];
			body[0] = new LitMatrixSphere2(scale * 20, 2);
			body[1] = new LitMatrixSphere2(scale * 15, 2);
			body[2] = new LitMatrixSphere2(scale * 15, 2);
			body[3] = new LitMatrixSphere2(scale * 15, 2);
			body[4] = new LitMatrixSphere2(scale * 15, 2);
			body[5] = new LitMatrixSphere2(scale * 14, 2);
			body[0].SetColor(Color.Green);
			body[1].SetColor(Color.Green);
			body[2].SetColor(Color.Green);
			body[3].SetColor(Color.Green);
			body[4].SetColor(Color.Green);
			body[5].SetColor(Color.LimeGreen);
			
			SetOffsets();
			programNumber = body[0].GetProgram();
			theProgram = Programs.GetProgram(programNumber);
		}
		
		private void SetOffsets()
		{
			body[0].SetOffset(position);
			body[1].SetOffset(position + new Vector3(+ x_offset, + y_direction, 0f));
			body[2].SetOffset(position + new Vector3(+ 2 * x_offset, + 2 * y_direction, 0f));
			body[3].SetOffset(position + new Vector3(+ 3 * x_offset, + 3 * y_direction, 0f));
			body[4].SetOffset(position + new Vector3(+ 4 * x_offset, + 4 * y_direction, 0f));
			body[5].SetOffset(position + new Vector3(+ x_offset, + y_direction + wing_offset, 0f));
		}
		
		public override void Draw()
        {
            x_step = 20;
            if (direction == 2)
            {
              x_step = -20;
            }
			x_offset = scale * x_step;
            // FIXME GraphicsClass.DrawTriangle(Color.LimeGreen, x + x_step, y  + y_direction, x , y + wing_offset, x + 2 * x_step, y + wing_offset);
			switch(wing_step)
            {
			case -40:  wing_step = -30; break;
			case -30:  wing_step = -20; break;
			case -20:  wing_step = -10; break;
			case -10:  wing_step = 0; break;
			case 0:  wing_step = 10; break;
			case 10: wing_step = 20; break;
			case 20: wing_step = 30; break;
			case 30: wing_step = 40; break;
			case 40: wing_step = -40; break;
            }
			wing_offset = scale * wing_step;
            SetOffsets();
            y_direction = 0;
            for (int i = 0; i < body.Length; i++)
            {
            	body[i].Draw();
            }
			if (autoMove)
			{
				Move();
				switch (direction)
				{
				case 3: y_direction = scale * 5; break;
				case 4: y_direction = scale * -5; break;
				default: y_direction = 0; break;
				}
			}
        }

		public void KeyPress(System.Windows.Forms.Keys key)
		{
			switch (key) {
			case System.Windows.Forms.Keys.NumPad8:
				{
					if (position.Y < 500) {
						position.Y += 2;
						y_direction = -5;
					}
					break;
				}
			case System.Windows.Forms.Keys.NumPad2:
				{
					if (position.Y > 12) {
						position.Y -= 2;
						y_direction = 5;
					}
					break;
				}
			}
		}
		
	    public void NewWorldPosition(int wp)
		{
			if (wp > position.X - 256) {
				direction = 1;
			}
			if (wp < position.X - 256) {
				direction = 2;
			}
			position.X = wp + 256;
		}

		public override void SetProgram(int program)
		{
			base.SetProgram(program);
			foreach(LitMatrixSphere2 l in body)
			{
				l.SetProgram(program);
			}
		}
	}
}

