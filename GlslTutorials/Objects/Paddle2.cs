using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Paddle2 : CollisionObject
	{	
		Shape body;
		static Random random = new Random();
		Movement movement = new RandomMovement();
		Collisions collision = new Collisions();
		int framesPerMove = 10;
		int frameCount;
		float scale = 0.5f;
		bool dead = false;
		Vector3 lowLimits = new Vector3(-1f, -1f, 0f);
		Vector3 highLimits = new Vector3(1f, 1f, 1f);
		public Paddle2()
		{
			body = new LitMatrixSphere2(0.1f);
			float xOffset = random.Next(20)/10f - 1f;
			float yOffset = random.Next(20)/10f - 1f;
			float zOffset = random.Next(10)/10f - 0.5f;
			int colorSelection = random.Next(3);
			switch (colorSelection)
			{
				case 0:	
					body.SetColor(Colors.RED_COLOR);
					break;
				case 1:
					body.SetColor(Colors.GREEN_COLOR); 
					break;
				case 2: body.SetColor(Colors.BLUE_COLOR);
					break;
				default: 
					body.SetColor(Colors.YELLOW_COLOR);
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
			body.SetLightPosition(body.GetOffset() - new Vector3(5f, 5f, 5f));
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
		
		public void SetProgram(int newProgram)
		{
			body.SetProgram(newProgram);
		}
		
		public void SetRandomControl()
		{
			movement = new RandomMovement();
			movement.SetLimits(lowLimits, highLimits);
		}
		
		public void SetKeyboardControl()
		{
			movement = new KeyboardMovement();
			movement.SetLimits(lowLimits, highLimits);
		}
		
		public void SetSocketControl()
		{
			movement = new SocketMovement();
			movement.SetLimits(lowLimits, highLimits);
		}	

		public void SetMouseControl()
		{
			movement = new MouseMovement();
			movement.SetLimits(lowLimits, highLimits);
		}

		public void SetChaseControl()
		{
			movement = new ChaseObjectMovement();
			movement.SetLimits(lowLimits, highLimits);
		}

		public void SetRemoteControl()
		{
			movement = new RemoteMovement();
			movement.SetLimits(lowLimits, highLimits);
		}

		public void SetChaseObject(CollisionObject c)
		{
			if (movement is ChaseObjectMovement)
			{
				((ChaseObjectMovement) movement).SetChaseObject(c);
			}
		}

		public void SetChaseSpeed(float cs)
		{
			if (movement is ChaseObjectMovement)
			{
				((ChaseObjectMovement) movement).SetSpeedLimit(cs);
			}
		}
		
		public void keyboard(Keys keyCode)
		{
			if (movement is KeyboardMovement)
			{
				KeyboardMovement keyboardMovement = (KeyboardMovement) movement;
				keyboardMovement.keyboard(keyCode);
			}
		}

		public void MouseMotion(int mouseX, int mouseY)
		{
			if (movement is MouseMovement)
			{
				MouseMovement mouseMovement = (MouseMovement) movement;
				mouseMovement.MouseMotion(mouseX, mouseY);
			}
		}
		
		public void SetLimits(Vector3 low, Vector3 high)
		{
			lowLimits = low;
			highLimits = high;
			movement.SetLimits(lowLimits, highLimits);
		}
		
		public override Vector3 GetOffset()
		{
			return body.GetOffset();
		}

		public void SetOffset(Vector3 offset)
		{
			body.SetOffset(offset);
		}

		Vector3 normal = new Vector3(0f, 0f, -1f);

		public void SetNormal(Vector3 n)
		{
			normal = n;
		}

		public override Vector3 GetNormal()
		{
			return normal;
		}

		public void UseBlock(Vector3 size)
		{
			int colorSelection = random.Next(3);
			float[] color;
			switch (colorSelection)
			{
			case 0:
				color = Colors.RED_COLOR;
				break;
			case 1:
				color = Colors.GREEN_COLOR;
				break;
			case 2:
				color = Colors.BLUE_COLOR;
				break;
			default:
				color = Colors.YELLOW_COLOR;
				break;
			}
			body = new LitMatrixBlock2(size, color);
		}
	}
}

