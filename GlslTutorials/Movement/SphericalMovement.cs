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

		float last_r = 3f;
		float last_theta = 0f;
		float last_phi = 0f;

		float r = 3f;
		float theta = 0f;
		float phi = 0f;

		float thetaStep = 1f;
		float phiStep = 1f;

		Vector3 radiusBasis;
		Vector3 thetaBasis;
		Vector3 phiBasis;

		Vector3 velocity;
		Vector3 radialVelocity;
		Vector3 thetaVelocity;
		Vector3 phiVelocity;

		Vector3 cross;

		float rMin = 0.5f;
		float rMax = 1.5f;

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
			last_r = r;
			last_theta = theta;
			last_phi = phi;
			ChangeRadius(0f);
			ChangePhi(phiStep);
			ChangeTheta(thetaStep);
			CalculatePosition();
			CalculateBasisVectors();
			CalculateVelocity();
			cross = Vector3.Cross(velocity, radiusBasis);
			sphericalRotation = Matrix4.Identity; 
			//Vector3 cluge = new Vector3(cross.X, -cross.Y, cross.Z);
			sphericalRotation = Matrix4.CreateFromAxisAngle(cross, (float)Math.PI * 1f/2f);
			//sphericalRotation = Matrix4.Mult(sphericalRotation, Matrix4.CreateRotationY((float)Math.PI * 3f/2f));
			//sphericalRotation = Matrix4.CreateFromAxisAngle(cross, (float)Math.PI * 1f/2f);
			//sphericalRotation = Matrix4.Mult(Matrix4.CreateFromAxisAngle(phiBasis, (float)Math.PI * 1f/2f), sphericalRotation);
//			Vector3 cluge = new Vector3(cross.X, cross.Z, cross.Z);
//			sphericalRotation = Matrix4.CreateFromAxisAngle(cluge, (float)Math.PI * 1f/2f);

//			sphericalRotation.M12 = -cross.Z;
//			sphericalRotation.M13 = cross.Y;
//			sphericalRotation.M21 = cross.Z;
//			sphericalRotation.M23 = -cross.X;
//			sphericalRotation.M31 = -cross.Y;
//			sphericalRotation.M32 = cross.X;
//			sphericalRotation.Normalize();
			//sphericalRotation = Matrix4.Mult(sphericalRotation, Matrix4.CreateRotationY((float)Math.PI * 3f/2f));
//			float dtheta = (float)Math.Pow(theta - last_theta, 2);
//			float dphi = (float)Math.Pow(phi - last_phi, 2);
//			float dtotal = dtheta + dphi;
//			float thetaRotation = dtheta / dtotal;
//			float phiRotation = dphi / dtotal;
//			sphericalRotation = Matrix4.Mult(sphericalRotation, Matrix4.CreateRotationY((float)Math.PI * 3f/2f * thetaRotation));
//			sphericalRotation = Matrix4.Mult(sphericalRotation, Matrix4.CreateRotationZ((float)Math.PI * 3f/2f * phiRotation));
			UpdateSphericalMatrix();
			SetSystemMatrix(spericalTransform);
			SetRotationMatrix(rotationMatrix);
			return oldOffset;
		}

		public void ChangeRadius(float rChange)
		{
			r = r + rChange;
			Matrix4 translationMatrix = Matrix4.CreateTranslation(rChange, 0f, 0f);
			sphericalTranslation = Matrix4.Mult(translationMatrix, sphericalTranslation);
			UpdateSphericalMatrix();
		}

		private void ChangeTheta(float thetaChange)
		{
			theta = theta +  (float)Math.PI / 180.0f * thetaChange;
			rotationTheta = Matrix4.CreateRotationZ(theta);		
			rotationPhi = Matrix4.CreateFromAxisAngle(
				new Vector3((float)Math.Sin(theta), (float)Math.Cos(theta), 0f), phi);	
			rotationMatrix = Matrix4.Mult(rotationTheta, rotationPhi);
		}

		private void SetThetaOffset(float thetaoffset)
		{
			float thetaOffset = theta +  (float)Math.PI / 180.0f * thetaoffset;
			rotationTheta = Matrix4.CreateRotationZ(thetaOffset);		
			rotationPhi = Matrix4.CreateFromAxisAngle(
				new Vector3((float)Math.Sin(thetaOffset), (float)Math.Cos(thetaOffset), 0f), phi);	
			rotationMatrix = Matrix4.Mult(rotationTheta, rotationPhi);
		}

		private void SetThetaPhiOffset(float thetaOffset, float phiOffset)
		{
			thetaOffset = theta + (float)Math.PI / 180.0f * thetaOffset;
			phiOffset = phi + (float)Math.PI / 180.0f * phiOffset;
			rotationTheta = Matrix4.CreateRotationZ(thetaOffset);		
			rotationPhi = Matrix4.CreateFromAxisAngle(
				new Vector3((float)Math.Sin(thetaOffset), (float)Math.Cos(thetaOffset), 0f), phiOffset);	
			rotationMatrix = Matrix4.Mult(rotationTheta, rotationPhi);
		}

		private void ChangePhi(float phiChange)
		{
			phi = phi +  (float)Math.PI / 180.0f * phiChange;
			rotationPhi = Matrix4.CreateFromAxisAngle(
				new Vector3((float)Math.Sin(theta), (float)Math.Cos(theta), 0f), phi);				
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

		private void CalculateVelocity()
		{
			radialVelocity = (r - last_r) * radiusBasis;
			thetaVelocity = (float)(r * (theta - last_theta) * Math.Cos(phi)) * thetaBasis;
			phiVelocity = r * (phi - last_phi) * phiBasis;
			velocity = radialVelocity + thetaVelocity + phiVelocity;
		}

		public override string MovementInfo()
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine("Position = " + currentPosition.ToString());
			result.AppendLine("r = " + r.ToString());
			result.AppendLine("theta = " + theta.ToString("000.000") + " " + ((theta * 180f / Math.PI) % 360).ToString("000.000"));
			result.AppendLine("phi = " + phi.ToString("000.000") + " " + ((phi * 180f / Math.PI) % 360).ToString("000.000"));
			result.AppendLine("velocity = " + velocity.ToString());
			result.AppendLine("radiusBasis = " + radiusBasis.ToString());
			result.AppendLine("thetaBasis = " + thetaBasis.ToString());
			result.AppendLine("phiBasis = " + phiBasis.ToString());
			result.AppendLine("cross = " + cross.ToString());
			return result.ToString();
		}

		public override void Translate(Vector3 offset)
		{
			sphericalTranslation = Matrix4.Mult(Matrix4.CreateTranslation(offset), sphericalTranslation);
		}
	}
}

