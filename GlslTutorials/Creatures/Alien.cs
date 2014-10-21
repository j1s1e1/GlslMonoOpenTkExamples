using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Alien
	{
		LitMatrixSphere2 body;
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
			float xOffset = random.Next(20)/10f - 1f;
			float yOffset = random.Next(20)/10f - 1f;
			float zOffset = random.Next(10)/10f - 0.5f;
			int colorSelection = random.Next(3);
			switch (colorSelection)
			{
				case 0:	body.SetColor(Colors.RED_COLOR); break;
				case 1: body.SetColor(Colors.GREEN_COLOR); break;
				case 2: body.SetColor(Colors.BLUE_COLOR); break;
				default: body.SetColor(Colors.YELLOW_COLOR); break;
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
			if (frameCount < framesPerMove)
			{
				frameCount++;
			}
			else
			{
				frameCount = 0;
				body.SetOffset(movement.NewOffset(body.GetOffset()));
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
	}
}

