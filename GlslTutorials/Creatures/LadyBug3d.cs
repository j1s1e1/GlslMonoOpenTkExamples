using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class LadyBug3d : BugClass3d
	{
	
		LitMatrixSphere2 sphere;
		LitMatrixSphere2[] wings;
	
		public LadyBug3d(int x_in = 0, int y_in = 0, int z_in = 0): base(x_in, y_in, z_in)
		{
			sphere = new LitMatrixSphere2(scale * 20, 2);
			sphere.SetColor(Color.Pink);
			wings = new LitMatrixSphere2[2];
			wings[0] = new LitMatrixSphere2(scale * 30, 2);
			wings[1] = new LitMatrixSphere2(scale * 30, 2);
			wings[0].SetColor(Color.Red);
			wings[1].SetColor(Color.Red);
			SetOffsets();
		}
		
		private void SetOffsets()
		{
			sphere.SetOffset(position);
			wings[0].SetOffset(position + new Vector3(0f, - wing_angle, 0f));
			wings[1].SetOffset(position + new Vector3(0f, + wing_angle, 0f));
			wings[0].SetAngles(0, 0, -wing_angle * 10);
			wings[1].SetAngles(80, 0, wing_angle * 10);
		}
		
		public override void Draw()
		{
			if (wing_step < 4) {
				wing_step++;
			} else {
				wing_step = 0;
			}
			wing_angle = wing_step * 5 * scale;
			SetOffsets();
			sphere.Draw();
			wings[0].DrawSemi(0, 7);
			wings[1].DrawSemi(0, 7);
			if (autoMove)
			{
				Move();
			}
		}

		public override void SetProgram(int program)
		{
			base.SetProgram(program);
			sphere.SetProgram(program);

			foreach(LitMatrixSphere2 w in wings)
			{
				w.SetProgram(program);
			}
		}
	}
}

