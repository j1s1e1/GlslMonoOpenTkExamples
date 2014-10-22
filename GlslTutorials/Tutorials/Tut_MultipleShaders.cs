using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace GlslTutorials
{
	public class Tut_MultipleShaders : TutorialBase
	{
		public Tut_MultipleShaders ()
		{
		}
		
		List<LitMatrixSphere2> spheres = new List<LitMatrixSphere2>();
		List<Alien> aliens = new List<Alien>();
		protected override void init()
	    {		
			int lms_program = Programs.AddProgram(VertexShaders.lms_vertexShaderCode, 
              	FragmentShaders.lms_fragmentShaderCode);
			
			Programs.SetUniformColor(lms_program, new Vector4(0f, 0.5f, 0.5f, 1f));
			Programs.SetLightPosition(lms_program, new Vector3(0f, 0.5f, -0.5f));
			
			int greenprog = Programs.AddProgram(VertexShaders.lms_vertexShaderCode, 
            	FragmentShaders.solid_green_with_normals_frag);
			
			int DirVertexLighting_PN = Programs.AddProgram(VertexShaders.DirVertexLighting_PN_vert,
			 	FragmentShaders.ColorPassthrough_frag);
			Programs.SetUniformColor(DirVertexLighting_PN, new Vector4(0.75f, 0.25f, 0.0f, 1f));
			Vector3 dirToLight = new Vector3(0.5f, 0.5f, 0.0f);
			dirToLight.Normalize();
			Programs.SetDirectionToLight(DirVertexLighting_PN, dirToLight);
			Programs.SetLightIntensity(DirVertexLighting_PN, new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
			Programs.SetNormalModelToCameraMatrix(DirVertexLighting_PN, Matrix3.Identity);
			Programs.SetModelToCameraMatrix(DirVertexLighting_PN, Matrix4.Identity);
			
			Alien alien;
			LitMatrixSphere2 lms;

			lms = new LitMatrixSphere2(0.1f);
			lms.SetOffset(new Vector3(-0.5f, 0.0f, 0.0f));
			spheres.Add(lms);
			
			lms = new LitMatrixSphere2(0.025f);
			//lms.SetOffset(new Vector3(0.5f, 0.0f, 0.0f));
			lms.SetProgram(DirVertexLighting_PN);
			
			spheres.Add(lms);		
			
			lms = new LitMatrixSphere2(0.1f);
			lms.SetOffset(new Vector3(0.0f, 0.5f, 0.0f));
			lms.SetProgram(lms_program);
			spheres.Add(lms);		
			

			
			alien = new Alien();
			alien.SetProgram(DirVertexLighting_PN);
			aliens.Add(alien);
			
			alien = new Alien();
			alien.SetProgram(greenprog);
			
			aliens.Add(alien);
			
			
			SetupDepthAndCull();
		}
		
		public override void display()
	    {
			ClearDisplay();
			foreach (LitMatrixSphere2 sphere in spheres)
			{
				sphere.Draw();
			}
			
			foreach (Alien a in aliens)
			{
				a.Draw();
			}
		}
	}
}

