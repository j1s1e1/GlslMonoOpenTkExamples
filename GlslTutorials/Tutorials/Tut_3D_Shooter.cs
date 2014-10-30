using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_3D_Shooter : TutorialBase
	{
		public Tut_3D_Shooter ()
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
		Vector3 up = new Vector3(0f, 0.5f, 0f);
		Vector3 right = new Vector3(0.5f, 0f,0f);
		
		int defaultShader;
		int shaderWithNormals;
		int shaderDirVertexLighting_PN;
		int shaderAllGreen;
		int shaderFragWhiteDiffuseColor;
		
		Vector3 currentScale = new Vector3(0.05f, 0.05f, 0.05f);
		
		private void SetupShaders()
		{
			defaultShader = Programs.AddProgram(VertexShaders.lms_vertexShaderCode,
			                                    FragmentShaders.lms_fragmentShaderCode);
			shaderWithNormals = Programs.AddProgram(VertexShaders.DirAmbVertexLighting_PN_vert,
			                                       	FragmentShaders.ColorPassthrough_frag);
			
			shaderDirVertexLighting_PN = Programs.AddProgram(VertexShaders.DirVertexLighting_PN_vert,
			 	FragmentShaders.ColorPassthrough_frag);
			Vector3 dirToLight = new Vector3(0.5f, 0.5f, 0.0f);
			dirToLight.Normalize();
			Programs.SetDirectionToLight(shaderDirVertexLighting_PN, dirToLight);
			Programs.SetLightIntensity(shaderDirVertexLighting_PN, new Vector4(0.4f, 0.6f, 0.8f, 1.0f));
			Programs.SetNormalModelToCameraMatrix(shaderDirVertexLighting_PN, Matrix3.Identity);
			Programs.SetModelToCameraMatrix(shaderDirVertexLighting_PN, Matrix4.Identity);
			
			shaderAllGreen  = Programs.AddProgram(VertexShaders.lms_vertexShaderCode,
                                    FragmentShaders.solid_green_with_normals_frag);
			
			shaderFragWhiteDiffuseColor = Programs.AddProgram(VertexShaders.FragmentLighting_PN, 
			                        FragmentShaders.FragmentLighting);
			Programs.SetLightIntensity(shaderFragWhiteDiffuseColor, new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
			Programs.SetAmbientIntensity(shaderFragWhiteDiffuseColor, new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
			Matrix4 mm = Matrix4.Identity;
			mm.M11 = 0.05f;
			mm.M22 = 0.05f;
			mm.M33 = 0.05f;
			Programs.SetModelToCameraMatrix(shaderFragWhiteDiffuseColor, mm);
			
			
			Vector4 worldLightPos = new Vector4(-0.5f, -0.5f, -10f, 1.0f);
			Vector4 lightPosCameraSpace = Vector4.Transform(worldLightPos, mm);
			Matrix4 invTransform = mm.Inverted();
			Vector4 lightPosModelSpace = Vector4.Transform(lightPosCameraSpace, invTransform);
			Vector3 lightPos = new Vector3(lightPosModelSpace.X, lightPosModelSpace.Y, lightPosModelSpace.Z);
			
			Programs.SetModelSpaceLightPosition(shaderFragWhiteDiffuseColor, lightPos);			
		}
				
		protected override void init()
	    {
			ship = new Blender();
			ship.ReadFile("test_with_normals.obj");
			ship.SetColor(Colors.WHITE_COLOR);
			ship.Scale(currentScale);
			
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
			SetupShaders();
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
				case Keys.A:
					ship.SetProgram(defaultShader);
					break;
				case Keys.B:
					ship.SetProgram(shaderFragWhiteDiffuseColor);
					break;
				case Keys.C:
					ship.SetProgram(shaderDirVertexLighting_PN);
					break;
				case Keys.D:
					ship.SetProgram(shaderAllGreen);
					break;		
				case Keys.E:
					ship.SetProgram(shaderWithNormals);
					break;
				case Keys.X:
					ship.SetOffset(new Vector3(0f, 0f, 10f));
					break;	
				case Keys.V:
					ship.SetOffset(new Vector3(0f, 0f, 0f));
					break;	
				case Keys.Add:
					currentScale = Vector3.Multiply(currentScale, 1.05f);
					ship.Scale(currentScale);
					break;
				case Keys.Subtract:
					currentScale = Vector3.Divide(currentScale, 1.05f);
					ship.Scale(currentScale);
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

