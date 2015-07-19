using System;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class SphericalMovement : Movement
	{
		int programNumber = -1;
		int theProgram = -1;
		int systemMovementMatrixUnif = -1;
		int rotationMatrixUnif = -1;
		Matrix4 spericalTransform = Matrix4.Identity;
		Matrix4 sphericalScale = Matrix4.Identity;
		Matrix4 sphericalTranslation = Matrix4.Identity;
		Matrix4 sphericalRotation = Matrix4.Identity;

		Matrix4 rotationTheta = Matrix4.Identity;
		Matrix4 rotationPhi = Matrix4.Identity;
		Matrix4 rotationMatrix = Matrix4.Identity;

		float r = 3f;
		float theta = 0f;
		float phi = 0f;

		float thetaStep = 1f;
		float phiStep = 1f;

		Vector3 radiusBasis;
		Vector3 thetaBasis;
		Vector3 phiBasis;

		public SphericalMovement(int programNumberIn, float thetaStepIn = 1f, float phiStepIn = 1f)
		{
			programNumber = programNumberIn;
			thetaStep = thetaStepIn;
			phiStep = phiStepIn;
			theProgram = Programs.GetProgram(programNumber);
			GL.UseProgram(theProgram);
			systemMovementMatrixUnif = GL.GetUniformLocation(theProgram, "systemMovementMatrix");
			rotationMatrixUnif = GL.GetUniformLocation(theProgram, "rotationMatrix");
			GL.UseProgram(0);
			sphericalTranslation = Matrix4.CreateTranslation(r, 0.0f, 0.0f);
			SetSystemMatrix(sphericalTranslation);
			SetRotationMatrix(Matrix4.Identity);
			SetScale(0.25f);
		}

		private void UpdateSphericalMatrix()
		{
			spericalTransform = Matrix4.Mult(sphericalTranslation, sphericalScale);
			spericalTransform = Matrix4.Mult(sphericalRotation, spericalTransform);
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

		public override Vector3 NewOffset(Vector3 oldOffset)
		{
			ChangeRadius(0f);
			ChangePhi(phiStep);
			ChangeTheta(thetaStep);
			sphericalRotation = Matrix4.CreateRotationY(180f);
			UpdateSphericalMatrix();
			SetSystemMatrix(spericalTransform);
			SetRotationMatrix(rotationMatrix);
			CalculatePosition();
			return oldOffset;
		}

		private void ChangeRadius(float rChange)
		{
			r = r + rChange;
			Matrix4 translationMatrix = Matrix4.CreateTranslation(rChange, 0f, 0f);
			sphericalTranslation = Matrix4.Mult(translationMatrix, sphericalTranslation);
			UpdateSphericalMatrix();
		}

		private void ChangeTheta(float thetaChange)
		{
			theta = theta +  (float)Math.PI / 180.0f * thetaChange;
			rotationTheta = Matrix4.CreateRotationY(theta);		
			rotationPhi = Matrix4.CreateFromAxisAngle(
				new Vector3((float)Math.Sin(theta), 0f, (float)Math.Cos(theta)), phi);	
			rotationMatrix = Matrix4.Mult(rotationTheta, rotationPhi);
		}

		private void SetThetaOffset(float thetaoffset)
		{
			float thetaOffset = theta +  (float)Math.PI / 180.0f * thetaoffset;
			rotationTheta = Matrix4.CreateRotationY(thetaOffset);		
			rotationPhi = Matrix4.CreateFromAxisAngle(
				new Vector3((float)Math.Sin(thetaOffset), 0f, (float)Math.Cos(thetaOffset)), phi);	
			rotationMatrix = Matrix4.Mult(rotationTheta, rotationPhi);
		}

		private void SetThetaPhiOffset(float thetaOffset, float phiOffset)
		{
			thetaOffset = theta + (float)Math.PI / 180.0f * thetaOffset;
			phiOffset = phi + (float)Math.PI / 180.0f * phiOffset;
			rotationTheta = Matrix4.CreateRotationY(thetaOffset);		
			rotationPhi = Matrix4.CreateFromAxisAngle(
				new Vector3((float)Math.Sin(thetaOffset), 0f, (float)Math.Cos(thetaOffset)), phiOffset);	
			rotationMatrix = Matrix4.Mult(rotationTheta, rotationPhi);
		}

		private void ChangePhi(float phiChange)
		{
			phi = phi +  (float)Math.PI / 180.0f * phiChange;
			rotationPhi = Matrix4.CreateFromAxisAngle(
				new Vector3((float)Math.Sin(theta), 0f, (float)Math.Cos(theta)), phi);				
			rotationMatrix = Matrix4.Mult(rotationTheta, rotationPhi);
		}

		private void SetScale(float scale)
		{
			sphericalScale = Matrix4.CreateScale(scale);
		}

		private void CalculatePosition()
		{
			currentPosition.X = (float)(r * Math.Cos(theta)*Math.Cos(phi));
			currentPosition.Y = (float)(r * Math.Sin(theta)*Math.Cos(phi));
			currentPosition.Z = (float)(r * Math.Sin(phi));
		}

		private void CalculateBasisVectors()
		{
			radiusBasis.X = (float)(Math.Cos(theta)*Math.Cos(phi));
			radiusBasis.Y = (float)(Math.Sin(theta)*Math.Cos(phi));
			radiusBasis.Z = (float)(Math.Sin(phi));
			thetaBasis.X = (float)(-Math.Sin(theta));
			thetaBasis.Y = (float)(Math.Cos(theta));
			thetaBasis.Z = 0;
			phiBasis.X = (float)(-Math.Cos(theta)*Math.Sin(phi));
			phiBasis.Y = (float)(-Math.Sin(theta)*Math.Sin(phi));
			phiBasis.Z = (float)(Math.Cos(phi));
		}

		public override string MovementInfo()
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine("Position = " + currentPosition.ToString());
			result.AppendLine("r = " + r.ToString());
			result.AppendLine("theta = " + theta.ToString("###.###") + " " + ((theta * 180f / Math.PI) % 360).ToString("###.###"));
			result.AppendLine("phi = " + phi.ToString("###.###")+ " " + ((phi * 180f / Math.PI) % 360).ToString("###.###"));
			return result.ToString();
		}
	}
}

