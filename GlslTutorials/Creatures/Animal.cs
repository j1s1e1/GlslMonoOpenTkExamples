using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Animal
	{
		int programNumber = -1;
		int theProgram = -1;
		int systemMovementMatrixUnif = -1;
		int rotationMatrixUnif = -1;
		LitMatrixSphere2 s;

		protected bool autoMove = true;

		public Animal ()
		{
			s = new LitMatrixSphere2(0.1f);
		}

		public virtual void Move(Vector3 v)
		{
			s.Move(v);
		}


		public virtual void Draw()
		{
			s.Draw();
		}      

		public virtual void SetProgram(int program)
		{
			programNumber = program;
			theProgram = Programs.GetProgram(programNumber);
			GL.UseProgram(theProgram);
			systemMovementMatrixUnif = GL.GetUniformLocation(theProgram, "systemMovementMatrix");
			rotationMatrixUnif = GL.GetUniformLocation(theProgram, "rotationMatrix");
			GL.UseProgram(0);
		}

		public void SetSystemMatrix(Matrix4 matrix)
		{
			GL.UseProgram(theProgram);
			GL.UniformMatrix4(systemMovementMatrixUnif, false, ref matrix);
			GL.UseProgram(0);	
		}

		public void SetRotationMatrix(Matrix4 matrix)
		{
			GL.UseProgram(theProgram);
			GL.UniformMatrix4(rotationMatrixUnif, false, ref matrix);
			GL.UseProgram(0);	
		}

		public void SetAutoMove()
		{
			autoMove = true;
		}

		public void ClearAutoMove()
		{
			autoMove = false;
		}
	}
}

