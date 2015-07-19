using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class BugClass3d : Animal
	{   
		protected Vector3 lastPosition = new Vector3();
		protected Vector3 position = new Vector3();
        protected float x;
		protected float y;
		protected float z;
		protected int wing_step = 0;  
		protected float wing_angle = 0;
        public int size = 25;
        public Color color = Color.Red;
        bool alive;
        static protected Random random = new Random();

		protected float scale = 0.005f;
		protected Vector3 speed;

        public BugClass3d (int x_in, int y_in, int z_in)
        {
            x = x_in;
            y = y_in;
            z = z_in;
            alive = true;
			lastPosition = new Vector3(x, y, z);
			position = lastPosition;
			speed = new Vector3(scale, scale, scale);

			movement = new BugMovement2D(speed);
        } 
        
        private void DrawBug()
        {
        }

		public override void Draw()
        {
            if (alive == true)
            {
                DrawBug();
            }
        }
			
        protected int direction = 0;

        private void ChaseCheck()
        {
        /*
            int x_dif = Math.Abs(x - player_x);
            if (x_dif > last_frame_offset) x_dif = x_dif - last_frame_offset;
            int y_dif = Math.Abs(y - player_y);
            if (x_dif < 100)
            {
                speed = 2;
                if (x_dif < y_dif)
                {
                    if (x > player_x)
                    {
                       direction = 2;
                    }
                    else
                    {
                        direction = 1;
                    }
                }
                else
                {
                    if (y > player_y)
                    {
                        direction = 4;
                    }
                    else
                    {
                        direction = 3;
                    }
                }
            }
            */
        }

        private void Random_Move()
        {
			lastPosition = position;
			position = movement.NewOffset(position);
			x = position.X;
			y = position.Y; 
			z = position.Z;
			if (movement is BugMovement2D)
			{
				direction = ((BugMovement2D)movement).GetDirection();
			}
            ChaseCheck();
        }
			
        public static int player_x;
        public static int player_y;

        public static int player_distance = 10;

        private void Player_Hit()
        {
        	/*
            if (InLastFrame())
            {
                if (Math.Abs (x - player_x - last_frame_offset) < player_distance)
                {
                    if (Math.Abs (y - player_y) < player_distance)
                    {
                        alive = false;
                    }
                }
                if (Math.Abs (x - player_x + last_frame_offset) < player_distance)
                {
                    if (Math.Abs (y - player_y) < player_distance)
                    {
                        alive = false;
                    }
                }
            }
            if (Math.Abs (x - player_x) < player_distance)
            {
                if (Math.Abs (y - player_y) < player_distance)
                {
                    alive = false;
                }
            }
            */
        }

        public void Move()
        {
          if (alive)
          {
              Player_Hit();
              Random_Move();
          }
        }

		public void SetBug2DMovement()
		{
			movement = new BugMovement2D(speed);
		}
    }
}

