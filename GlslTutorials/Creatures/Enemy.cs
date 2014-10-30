using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Enemy
	{
		Blender tieFighter;
		float radius = 0.05f;
		static Random random = new Random();
		RandomMovement movement = new RandomMovement();
		Collisions collision = new Collisions();
		int framesPerMove = 10;
		int frameCount;
		float scale = 0.5f;
		bool dead = false;
		public Enemy ()
		{
			tieFighter = new Blender();
			tieFighter.ReadBinaryFile("test_with_normals.bin");
			tieFighter.Scale(new Vector3(0.02f, 0.02f, 0.02f));
			float xOffset = random.Next(20)/10f - 1f;
			float yOffset = random.Next(20)/10f - 1f;
			float zOffset = random.Next(10)/10f - 0.5f;
			int colorSelection = random.Next(3);
			switch (colorSelection)
			{
				case 0:	tieFighter.SetColor(Colors.RED_COLOR); break;
				case 1: tieFighter.SetColor(Colors.GREEN_COLOR); break;
				case 2: tieFighter.SetColor(Colors.BLUE_COLOR); break;
				default: tieFighter.SetColor(Colors.YELLOW_COLOR); break;
			}
			xOffset = xOffset * scale;
			yOffset = yOffset * scale;
			zOffset = zOffset * scale;
			tieFighter.SetOffset(new Vector3(xOffset, yOffset, zOffset));
		}
		
		public bool isDead()
		{
			return dead;
		}
		
		public void Draw()
		{
			tieFighter.Draw();
			if (frameCount < framesPerMove)
			{
				frameCount++;
			}
			else
			{
				frameCount = 0;
				Vector3 oldOffset = tieFighter.GetOffset();
				tieFighter.SetOffset(movement.NewOffset(tieFighter.GetOffset()));
				Vector3 newOffset = tieFighter.GetOffset();
				Vector3 direction = newOffset - oldOffset;
				tieFighter.Face(direction);
			}
		}
		 
		public void FireOn(List<Missle> missles)
		{
			foreach (Missle m in missles)
			{
				if (collision.DetectColisions(tieFighter.GetOffset(), m.GetOffsets()))
				{
					dead = true;
					break;
				}
			}
		}
		
		public void SetProgram(int newProgram)
		{
			tieFighter.SetProgram(newProgram);
		}
	}
}

