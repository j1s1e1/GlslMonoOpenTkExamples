using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_3D_Shooter2 : TutorialBase
	{
		public Tut_3D_Shooter2 ()
		{
		}
		
		Blender ship;
		List<Missle> missles = new List<Missle>();
	 	bool addMissle = false;
		
		List<Alien> aliens;
		
		TextClass credit1;
		TextClass credit2;
		
		int deadAliensCount = 0;
		TextClass deadAliensText;
		
		bool staticText = true;
		
		double anglehorizontal = 0;
		double anglevertical = 0;
		
		Vector3 axis = new Vector3(0f, 0f, 1f);
		Vector3 up = new Vector3(0f, 0.07f, 0f);
		Vector3 right = new Vector3(0.16f, 0f,0f);
				
		protected override void init()
	    {
			string BlenderFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/Blender/";
			ship = new Blender();
			ship.ReadFile(BlenderFilesDirectory + "X_Wing3.obj");
			ship.SetColor(Colors.WHITE_COLOR);
			ship.Scale(new Vector3(0.1f, 0.1f, 0.1f));
			
			aliens = new List<Alien>();
			
			for (int i = 0; i < 10; i++)
			{
				Alien alien = new Alien();
				aliens.Add(alien);
			}
			
			deadAliensText = new TextClass("Dead Aliens = " + deadAliensCount.ToString(), 0.4f, 0.04f, staticText);
			deadAliensText.SetOffset(new Vector3(-0.75f, +0.8f, 0.0f));
			
			credit1 = new TextClass("X-Wing Model based on Blender model by", 0.4f, 0.04f, staticText);
        	credit1.SetOffset(new Vector3(-0.75f, -0.65f, 0.0f));

			credit2 = new TextClass("Angel David Guzman of PixelOz Designs", 0.4f, 0.04f, staticText);
        	credit2.SetOffset(new Vector3(-0.75f, -0.75f, 0.0f));
			
			SetupDepthAndCull();
		}
		
		public override void display()
	    {
			List<int> deadMissles = new List<int>();
			List<int> deadAliens = new List<int>();
			ClearDisplay();
			ship.Draw();
			anglehorizontal = anglehorizontal + 0.02f;
			anglevertical = anglevertical + 0.01f;
			for (int i = 0; i < missles.Count; i++)
			{
				if (missles[i].Firing())
				{
					missles[i].Draw();
				} 
				else
				{
					if (missles[i].Finished())
					{
						deadMissles.Add(i);
					}
				}
			}

   			if (deadMissles.Count > 0)
			{
				missles.RemoveAt(deadMissles[0]);
			}
			
			if (addMissle) 
			{
 	            missles.Add(new Missle(axis, up, right));
	            addMissle = false;
        	}
			int dead = 0;
			for(int i = 0; i < aliens.Count; i++)
			{
				if (aliens[i].isDead())
				{
					dead++;
				}
				else
				{
					aliens[i].Draw();
					aliens[i].FireOn(missles);
				}
			}
			if (dead > deadAliensCount)
			{
				deadAliensCount = dead;
				deadAliensText.UpdateText("Dead Aliens = " + deadAliensCount.ToString());
			}
			deadAliensText.Draw();
			credit1.Draw();
			credit2.Draw();
		}
		
		private void Rotate(Vector3 rotationAxis, float angle)
		{
			ship.RotateShapes(rotationAxis, angle);
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angle);
			axis = Vector3.Transform(axis, rotation);
			up = Vector3.Transform(up, rotation);
			right = Vector3.Transform(right, rotation);
		}
		
	    public override String keyboard(Keys keyCode, int x, int y)
	    {
	        StringBuilder result = new StringBuilder();
	        switch (keyCode) 
			{
				case Keys.D1:
					Rotate(Vector3.UnitX, 5f);
					break;
				case Keys.D2:
					Rotate(Vector3.UnitX, -5f);
					break;
				case Keys.D3:
					Rotate(Vector3.UnitY, 5f);
					break;
				case Keys.D4:
					Rotate(Vector3.UnitY, -5f);
					break;
				case Keys.D5:
					Rotate(Vector3.UnitZ, 5f);
					break;
				case Keys.D6:
					Rotate(Vector3.UnitZ, -5f);
					break;		
				case Keys.I:
					result.AppendLine("Found " + ship.ObjectCount().ToString() + " objects in ship file.");
					break;
				case Keys.Space:
					if (addMissle == false)
					{
						if (missles.Count < 10)
						{
							addMissle = true;
						}
					}
					result.AppendLine("axis " + axis.ToString());
					result.AppendLine("up " + up.ToString());
					result.AppendLine("right " + right.ToString());
					result.AppendLine("missles.Count " + missles.Count.ToString());
					break;
	        }
	        return result.ToString();
	    }
	}
}

