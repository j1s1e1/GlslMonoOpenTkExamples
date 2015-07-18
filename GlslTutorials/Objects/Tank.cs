using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tank
	{
		LitMatrixSphere2 body;
		LitMatrixBlock2 leftTread;
		LitMatrixBlock2 rightTread;

		LitMatrixSphere2 bullet;
		Vector3 bulletDirection = new Vector3();
		int shotCount = 0;

		float radius = 0.05f;
		static Random random = new Random();
		Movement movement = new RandomMovement();
		Collisions collision = new Collisions();
		int framesPerMove = 10;
		int frameCount;
		float scale = 0.5f;
		bool dead = false;
		public Tank()
		{
			body = new LitMatrixSphere2(radius);
			leftTread = new LitMatrixBlock2(new Vector3(radius/5, 2 * radius, 2 * radius), Colors.RED_COLOR);
			leftTread.SetOffset(new Vector3(-radius, 0f, 0f));
			rightTread = new LitMatrixBlock2(new Vector3(radius/5, 2 * radius, 2 * radius), Colors.RED_COLOR);
			rightTread.SetOffset(new Vector3(-radius, 0f, 0f));
			bullet = new LitMatrixSphere2(0.01f);
			bullet.SetColor(Colors.FLASH_COLOR);
			float xOffset = random.Next(20)/10f - 1f;
			float yOffset = random.Next(20)/10f - 1f;
			float zOffset = random.Next(10)/10f - 0.5f;
			int colorSelection = random.Next(3);
			switch (colorSelection)
			{
				case 0:	
					body.SetColor(Colors.RED_COLOR);
					leftTread.SetColor(Colors.RED_COLOR);
					rightTread.SetColor(Colors.RED_COLOR);
					break;
				case 1:
					body.SetColor(Colors.GREEN_COLOR); 
					leftTread.SetColor(Colors.GREEN_COLOR);
					rightTread.SetColor(Colors.GREEN_COLOR);
					break;
				case 2: body.SetColor(Colors.BLUE_COLOR);
					leftTread.SetColor(Colors.BLUE_COLOR);
					rightTread.SetColor(Colors.BLUE_COLOR);
					break;
				default: 
					body.SetColor(Colors.YELLOW_COLOR);
					leftTread.SetColor(Colors.YELLOW_COLOR);
					rightTread.SetColor(Colors.YELLOW_COLOR);
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
			leftTread.Draw();
			rightTread.Draw();
			if (shotCount > 0)
			{
				bullet.Draw();
				bullet.SetOffset(Vector3.Add(bullet.GetOffset(), bulletDirection));
				shotCount--;
			}
			if (frameCount < framesPerMove)
			{
				frameCount++;
			}
			else
			{
				frameCount = 0;
				body.SetOffset(movement.NewOffset(body.GetOffset()));
				leftTread.SetOffset(Vector3.Add(body.GetOffset(), new Vector3(-radius, 0f, 0f)));
				rightTread.SetOffset(Vector3.Add(body.GetOffset(), new Vector3(radius, 0f, 0f)));
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
			leftTread.SetProgram(newProgram);
			rightTread.SetProgram(newProgram);
		}
		
		public void SetRandomControl()
		{
			movement = new RandomMovement();
		}
		
		public void SetKeyboardControl()
		{
			movement = new KeyboardMovement();
		}
		
		public void SetSocketControl()
		{
			movement = new SocketMovement();
		}		

		private void Shoot()
		{
			shotCount = 100;
			bullet.SetOffset(body.GetOffset());
			bulletDirection = new Vector3(0f, 0.005f, 0f);
		}
		
		public void keyboard(Keys keyCode)
		{
			if (movement is KeyboardMovement)
			{
				KeyboardMovement keyboardMovement = (KeyboardMovement) movement;
				keyboardMovement.keyboard(keyCode);
			}
			if (keyCode == Keys.NumPad5)
			{
				Shoot();
			}
		}
	}
}

