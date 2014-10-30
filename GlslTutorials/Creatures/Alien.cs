using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Alien
	{
		LitMatrixSphere2 body;
		LitMatrixBlock2 leftWing;
		LitMatrixBlock2 rightWing;
		float radius = 0.05f;
		static Random random = new Random();
		RandomMovement movement = new RandomMovement();
		Collisions collision = new Collisions();
		int framesPerMove = 10;
		int frameCount;
		float scale = 0.5f;
		bool dead = false;
		public Alien ()
		{
			body = new LitMatrixSphere2(radius);
			leftWing = new LitMatrixBlock2(new Vector3(radius/5, 2 * radius, 2 * radius), Colors.RED_COLOR);
			leftWing.SetOffset(new Vector3(-radius, 0f, 0f));
			rightWing = new LitMatrixBlock2(new Vector3(radius/5, 2 * radius, 2 * radius), Colors.RED_COLOR);
			rightWing.SetOffset(new Vector3(-radius, 0f, 0f));
			float xOffset = random.Next(20)/10f - 1f;
			float yOffset = random.Next(20)/10f - 1f;
			float zOffset = random.Next(10)/10f - 0.5f;
			int colorSelection = random.Next(3);
			switch (colorSelection)
			{
				case 0:	
					body.SetColor(Colors.RED_COLOR);
					leftWing.SetColor(Colors.RED_COLOR);
					rightWing.SetColor(Colors.RED_COLOR);
					break;
				case 1:
					body.SetColor(Colors.GREEN_COLOR); 
					leftWing.SetColor(Colors.GREEN_COLOR);
					rightWing.SetColor(Colors.GREEN_COLOR);
					break;
				case 2: body.SetColor(Colors.BLUE_COLOR);
					leftWing.SetColor(Colors.BLUE_COLOR);
					rightWing.SetColor(Colors.BLUE_COLOR);
					break;
				default: 
					body.SetColor(Colors.YELLOW_COLOR);
					leftWing.SetColor(Colors.YELLOW_COLOR);
					rightWing.SetColor(Colors.YELLOW_COLOR);
					break;
			}
			xOffset = xOffset * scale;
			yOffset = yOffset * scale;
			zOffset = zOffset * scale;
			body.SetOffset(new Vector3(xOffset, yOffset, zOffset));
		}
		
		public bool isDead()
		{
			return dead;
		}
		
		public void Draw()
		{
			body.Draw();
			leftWing.Draw();
			rightWing.Draw();
			if (frameCount < framesPerMove)
			{
				frameCount++;
			}
			else
			{
				frameCount = 0;
				body.SetOffset(movement.NewOffset(body.GetOffset()));
				leftWing.SetOffset(Vector3.Add(body.GetOffset(), new Vector3(-radius, 0f, 0f)));
				rightWing.SetOffset(Vector3.Add(body.GetOffset(), new Vector3(radius, 0f, 0f)));
			}
		}
		 
		public void FireOn(List<Missle> missles)
		{
			foreach (Missle m in missles)
			{
				if (collision.DetectColisions(body.GetOffset(), m.GetOffsets()))
				{
					dead = true;
					break;
				}
			}
		}
		
		public void SetProgram(int newProgram)
		{
			body.SetProgram(newProgram);
			leftWing.SetProgram(newProgram);
			rightWing.SetProgram(newProgram);
		}
	}
}

