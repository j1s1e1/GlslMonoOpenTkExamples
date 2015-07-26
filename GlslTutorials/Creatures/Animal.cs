using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Animal
	{
		protected Vector3 lastPosition = new Vector3();
		protected Vector3 position = new Vector3();
		
		protected int programNumber = -1;
		protected int theProgram = -1;
		int systemMovementMatrixUnif = -1;
		int rotationMatrixUnif = -1;
		LitMatrixSphere2 s;

		protected bool autoMove = true;
		protected Movement movement;

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

		public virtual int GetProgram()
		{
			return programNumber;
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

		public bool GetAutoMove()
		{
			return autoMove;
		}

		public void SetAutoMove()
		{
			autoMove = true;
		}

		public void ClearAutoMove()
		{
			autoMove = false;
		}

		public void SetSphericalMovement(int programNumber, float thetaStep = 1f, float phiStep = 1f)
		{
			movement = new SphericalMovement(programNumber, thetaStep, phiStep);
		}

		public string GetMovementInfo()
		{
			return movement.MovementInfo();
		}

		public void Translate(Vector3 offset)
		{
			movement.Translate(offset);
		}

		public void ChangeRadius(float radiusChange)
		{
			if (movement is SphericalMovement)
			{
				((SphericalMovement)movement).ChangeRadius(radiusChange);
			}
		}
	}
}

