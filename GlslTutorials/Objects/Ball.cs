using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Ball
	{
		LitMatrixSphere2 body;
		static Random random = new Random();
		Movement movement = new ElasticMovement();
		Collisions collision = new Collisions();
		int framesPerMove = 10;
		int frameCount;
		float scale = 1f;
		bool dead = false;
		Vector3 lowLimits = new Vector3(-1f, -1f, 0f);
		Vector3 highLimits = new Vector3(1f, 1f, 1f);
		List<Paddle> otherObjects;
		Vector3 speed;
		public Ball(float radius = 0.05f)
		{
			otherObjects = new List<Paddle>();
			body = new LitMatrixSphere2(radius);
			float xOffset = 0f; // random.Next(20)/10f - 1f;
			float yOffset = 0f; // random.Next(20)/10f - 1f;
			float zOffset = 0f; // random.Next(10)/10f - 0.5f;
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

		public void SetLightPosition(Vector3 lightPosition)
		{
			body.SetLightPosition(lightPosition);
		}
			
		public void SetElasticControl()
		{
			movement = new ElasticMovement();
			movement.SetLimits(lowLimits, highLimits);
			if (speed != null) SetSpeed(speed);
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
		
		public void keyboard(Keys keyCode)
		{
			if (movement is KeyboardMovement)
			{
				KeyboardMovement keyboardMovement = (KeyboardMovement) movement;
				keyboardMovement.keyboard(keyCode);
			}
		}
		
		public void SetLimits(Vector3 low, Vector3 high)
		{
			lowLimits = low;
			highLimits = high;
			movement.SetLimits(lowLimits, highLimits);
		}

		public void MoveLimits(Vector3 v)
		{
			lowLimits += v;
			highLimits += v;
			movement.MoveLimits(v);
		}
		
		public Vector3 GetOffset()
		{
			return body.GetOffset();
		}

		public String GetLimits()
		{
			return movement.GetLimits();
		}
		
		public void AddPaddle(Paddle paddle)
		{
			otherObjects.Add(paddle);
			ElasticMovement em = (ElasticMovement) movement;
			em.SetPaddles(otherObjects);
		}

		public void SetSpeed(Vector3 speedIn)
		{
			speed = speedIn;
			if (movement is ElasticMovement)
			{
				((ElasticMovement)movement).SetSpeed(speedIn);
			}
		}
	}
}

