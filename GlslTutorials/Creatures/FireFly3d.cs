using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class FireFly3d : BugClass3d
	{
		int flash = 10;
		int flash_count = 0;
		LitMatrixSphere2[] body;

		float sizef;
		float wingOffset = 0f;
		
		public FireFly3d(int x_in = 0, int y_in = 0, int z_in = 0): base(x_in, y_in, z_in)
		{
			scale = 0.002f;
			body = new LitMatrixSphere2[7];
			sizef = size * scale;
			body[0] = new LitMatrixSphere2(sizef, 2);
			body[1] = new LitMatrixSphere2(sizef, 2);
			body[2] = new LitMatrixSphere2(sizef, 2);
			body[3] = new LitMatrixSphere2(sizef, 2);
			body[4] = new LitMatrixSphere2(2 * sizef, 2);
			body[5] = new LitMatrixSphere2(2 * sizef, 2);
			body[6] = new LitMatrixSphere2(sizef, 2);
			body[0].SetColor(Color.SaddleBrown);
			body[1].SetColor(Color.SaddleBrown);
			body[2].SetColor(Color.SaddleBrown);
			body[3].SetColor(Color.FromArgb(0xB2f932)); // flash color
			body[4].SetColor(Color.SaddleBrown);
			body[5].SetColor(Color.SaddleBrown);
			body[6].SetColor(Color.SaddleBrown);
			body[3].LighterColor(15f);
			SetOffsets();
		}
		
		private void SetOffsets()
		{
			body[0].SetOffset(x - sizef - scale * 1, y, z);
			body[1].SetOffset(x, y, z);
			body[2].SetOffset(x + sizef, y, z);
			body[3].SetOffset(x + 2 * sizef, y, z);
			body[4].SetOffset(x + sizef - wingOffset, y - wingOffset, z);
			body[5].SetOffset(x + sizef - wingOffset, y + wingOffset , z);
			body[6].SetOffset(x + 2 * sizef, y, z);
			
			body[4].SetAngles(0, 0, -wingOffset * 10);
			body[5].SetAngles(80, 0, wingOffset * 10);
		}
		
		public override void Draw()
		{
			body[0].Draw();
			body[1].Draw();
			body[2].Draw();	
			
			if (flash_count == 0) 
			{
				flash = random.Next(100);
				if (flash < 3) {
					body[3].Draw();
					flash_count = 10 + random.Next(20);
				} else {
					body[6].Draw();
				}
			} else {
				flash_count--;
				body[3].Draw();
			}
							
			body[4].DrawSemi(0, 5);	
			body[5].DrawSemi(0, 5);	
		
			if (wing_step < 3) {
				wing_step++;
			} else {
				wing_step = 0;
			}
			wingOffset = wing_step * scale * 5;
			SetOffsets();
			if (autoMove)
			{
				Move();
			}
		}
     
	}
}

