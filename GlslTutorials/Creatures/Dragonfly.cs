using System;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Dragonfly : Bug
	{
		float abs_x_step = 0.05f;
		float abs_y_direction = 0.04f;
		int direction = 0;
		float x_step = 0.05f;
		float y_direction = 0.04f;
		int wing_step = 0;
		float wing_offset = 0;
		float wing_length = 0.100f;

		LitMatrixSphere[] body;
		Triangle3d[] wings;

		public Dragonfly(int x_in = 0, int y_in = 0, int z_in = 0) : base(x_in, y_in, z_in)
		{
			body = new LitMatrixSphere[6];
			body[0] = new LitMatrixSphere(0.05f);
			body[1] = new LitMatrixSphere(0.04f);
			body[2] = new LitMatrixSphere(0.04f);
			body[3] = new LitMatrixSphere(0.04f);
			body[4] = new LitMatrixSphere(0.04f);
			body[5] = new LitMatrixSphere(0.03f);
			body[0].SetColor(Colors.GREEN_COLOR);
			body[1].SetColor(Colors.DarkOliveGreen);
			body[2].SetColor(Colors.LimeGreen);
			body[3].SetColor(Colors.DarkOliveGreen);
			body[4].SetColor(Colors.LimeGreen);
			body[5].SetColor(Colors.LimeGreen);
			wings = new Triangle3d[2];
			Vector3 a = new Vector3(x - x_step, y + wing_offset + wing_length, z + 0.05f);
			Vector3 b = new Vector3(x, y + wing_offset, z);
			Vector3 c = new Vector3(x + x_step, y + wing_offset + wing_length, z -  0.05f);
			wings[0] = new Triangle3d(a, b, c, true);
			wings[0].SetColor(Colors.LimeGreen);
			Vector3 d = new Vector3(x -x_step, y - wing_offset - wing_length, z -  0.05f);
			Vector3 e = new Vector3(x, y - wing_offset, z);
			Vector3 f = new Vector3(x + x_step, y - wing_offset - wing_length, z +  0.05f);
			wings[1] = new Triangle3d(d,e,f, true);
			wings[1].SetColor(Colors.LimeGreen);
			SetOffsets();
		}

		private void SetOffsets()
		{
			body[0].SetOffset(new Vector3(x - x_step, y, z));
			body[1].SetOffset(new Vector3(x, y + y_direction, z));
			body[2].SetOffset(new Vector3(x + 1 * x_step, y + 2 * y_direction, z));
			body[3].SetOffset(new Vector3(x + 2 * x_step, y + 3 * y_direction, z));
			body[4].SetOffset(new Vector3(x + 3 * x_step, y + 4 * y_direction, z));
			body[5].SetOffset(new Vector3(x, y + wing_offset / 2000, z + 0.100f));
			wings[0].SetOffset(new Vector3(x, y, z));
			wings[1].SetOffset(new Vector3(x, y, z));
		}

		private void SetRotations()
		{
			float rotation = (x % 256) - 128;
			Shape.global_y_rotate = rotation;
			wings[0].SetAngles(90 + wing_offset/5, 0, 0);
			wings[1].SetAngles(90 - wing_offset/5, 0, 0);
			/*
			body[0].SetAngles(0, rotation, 0);
			body[1].SetAngles(0, rotation, 0);;
			body[2].SetAngles(0, rotation, 0);
			body[3].SetAngles(0, rotation, 0);
			body[4].SetAngles(0, rotation, 0);
			body[5].SetAngles(0, rotation, 0);
			*/
		}
			
		public override void Draw()
		{
			x_step = abs_x_step;
			if (direction == 1)
			{
				x_step = -abs_x_step;
			}
			//GraphicsClass.DrawTriangle(Color.LimeGreen, x + x_step, y  + y_direction, x , y + wing_offset, x + 2 * x_step, y + wing_offset);
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
			wing_offset = wing_step/500f;
			SetOffsets();
			SetRotations();
			y_direction = 0.0f;
			body[0].Draw();
			body[1].Draw();
			body[2].Draw();
			body[3].Draw();
			body[4].Draw();
			//body[5].Draw();
			wings[0].Draw();
			wings[1].Draw();
		}

		public void KeyPress(Keys key)
		{
			switch (key) {
			case Keys.NumPad8:
				{
					if (y < 240) {
						y += 2;
						y_direction = -abs_y_direction;
					}
					break;
				}
			case Keys.NumPad2:
				{
					if (y > -240) {
						y -= 2;
						y_direction = abs_y_direction;
					}
					break;
				}
			}
		}

		public void NewWorldPosition(int wp)
		{
			x = wp;
			//ShapeClass.global_x_offset = wp;
		}

		public void Move(float x_in, float y_in, float z_in)
		{
			x = x + x_in / 100f;
			if (y_in < 0)
			{
				if (y > -240) {
					y -= 0.02f;
					y_direction = abs_y_direction;
				}
			}
			if (y_in > 0)
			{
				if (y < 240) {
					y += 0.02f;
					y_direction = -abs_y_direction;
				}
			}

		}
	}
}

