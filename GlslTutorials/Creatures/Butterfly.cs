using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Butterfly : BugClass3d
	{
		Wing[] wings;
		int rotateCount = 0;
		int rotateAngle = 0;
		int rotateDirection = 1;

		enum wingTextureEnum
		{
			SWALLOWTAIL,
			MONARCH,
			BUTTERFLY2,
			BUTTERFLY3,
			MOTH,
			GREEN,
			DRAGONFLY,
			WING_TEXTURE_COUNT
		}

		public Butterfly(int x_in = 0, int y_in = 0, int z_in = 0): base(x_in, y_in, z_in)
		{
			int scale = random.Next(5) + 5;
			wingTextureEnum wingTexture = (wingTextureEnum) random.Next((int)wingTextureEnum.WING_TEXTURE_COUNT);
			string wingTextureString = "Butterfly/";
			float flapAngle = 5f;
			switch (wingTexture)
			{
			case wingTextureEnum.MONARCH: wingTextureString = wingTextureString + "monarch.png"; break;
			case wingTextureEnum.SWALLOWTAIL: wingTextureString = wingTextureString + "swallowtail256.png"; break;
			case wingTextureEnum.BUTTERFLY2: wingTextureString = wingTextureString + "butterfly2.png"; break;
			case wingTextureEnum.BUTTERFLY3: wingTextureString = wingTextureString + "butterfly3.png"; break;
			case wingTextureEnum.MOTH: wingTextureString = wingTextureString + "moth1.png"; break;
			case wingTextureEnum.GREEN: wingTextureString = wingTextureString + "greenbutterfly.png"; break;
			case wingTextureEnum.DRAGONFLY: 
				wingTextureString = wingTextureString + "dragonfly1.png"; 
				flapAngle = 2f;
				break;
			}
			wings = new Wing[2];
			wings[0] = new Wing(wingTextureString);
			wings[0].Rotate(Vector3.UnitZ, 180f);
			wings[0].Scale(new Vector3(scale/10f, scale/10f, scale/10f));
			wings[0].SetFlapAngle(flapAngle);
			wings[1] = new Wing(wingTextureString);
			wings[1].Rotate(Vector3.UnitZ, 180f);
			wings[1].Rotate(Vector3.UnitY, 180f);
			wings[1].SetFlapAngle(-flapAngle);
			wings[1].SetRotationAxis(-Vector3.UnitY);
			wings[1].Scale(new Vector3(scale/10f, scale/10f, scale/10f));
			movement = new BugMovement3D(new Vector3(0.02f, 0.02f, 0.02f));
			movement.SetLimits(new Vector3(-0.6f, -0.6f, -0.6f), new Vector3(0.6f, 0.6f, 0.6f));
			wings[0].Rotate(Vector3.UnitX, 270f);
			wings[1].Rotate(Vector3.UnitX, 270f);
		}

		public void SetFlapEnable(bool flapEnable)
		{
			foreach (Wing w in wings)
			{
				w.SetFlapEnable(flapEnable);
			}
		}

		public override void Draw()
		{
			GL.Disable(EnableCap.CullFace);
			GL.Enable(EnableCap.AlphaTest);
			GL.AlphaFunc(AlphaFunction.Gequal, 0.1f);
			foreach (Wing w in wings)
			{
				w.Draw();
			}
			if (autoMove)
			{
				rotateCount++;
				foreach (Wing w in wings)
				{
					w.Move(Vector3.Subtract(position, lastPosition));
					if ((rotateCount % (18 * 1)) == 0)
					{
						rotateAngle++;
						if(rotateAngle > 135)
						{
							rotateAngle = 0;
							rotateDirection = -rotateDirection;
						}
						w.Rotate(Vector3.UnitX, rotateDirection);
					}
				}
				Move();
			}
		}
			
		public override void SetProgram(int program)
		{
			base.SetProgram(program);
			foreach (Wing w in wings)
			{
				w.SetProgram(program);
			}
		}
	}
}

