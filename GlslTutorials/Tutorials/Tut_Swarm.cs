using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tut_Swarm : TutorialBase
	{
		TextureSphere planet;
		List<Animal> animals;

		int sphericalProgram;

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

		Vector3 offset = new Vector3(0f, -2f, 1f);

		protected override void init()
		{
			sphericalProgram = Programs.AddProgram(VertexShaders.spherical_lms, FragmentShaders.lms_fragmentShaderCode);

			planet = new TextureSphere(2f, 0.0f);
			//planet.Move(offset);
			Shape.MoveWorld(offset);
			SetupDepthAndCull();

			sphericalScale = Matrix4.CreateScale(0.25f);
			sphericalTranslation = Matrix4.CreateTranslation(r, 0f, 0f);  // -8 due to scale

			animals = new List<Animal>();
			for (int i = 0; i < 100; i++)
			{
				AddDragonfly();
			}
		}

		private void SetupSphericalAnimal(Animal animal)
		{
			animal.ClearAutoMove();
			animal.SetProgram(sphericalProgram);
			animal.SetSystemMatrix(spericalTransform);
			animal.SetRotationMatrix(rotationMatrix);
		}

		private void AddDragonfly()
		{
			Dragonfly3d dragonFly = new Dragonfly3d(0, 0, 0);
			SetupSphericalAnimal(dragonFly);
			animals.Add(dragonFly);
		}

		public override void display()
		{
			ClearDisplay();
			planet.Draw();
			foreach (Animal a in animals)
			{
				ChangePhi(3.6f);
				ChangeTheta(1.8f);
				UpdateSphericalMatrix();
				a.SetSystemMatrix(spericalTransform);
				a.SetRotationMatrix(rotationMatrix);
				a.Draw();
			}
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			if (displayOptions)
			{
				SetDisplayOptions(keyCode);
			}
			else {
				switch (keyCode) {
				case Keys.Enter:
					displayOptions = true;
					break;
				case Keys.D1:
					Shape.RotateWorld(-offset, Vector3.UnitX, 1f);
					break;
				case Keys.D2:
					Shape.RotateWorld(-offset, Vector3.UnitX, -1f);
					break;
				case Keys.D3:
					Shape.RotateWorld(-offset, Vector3.UnitY, 1f);
					break;
				case Keys.D4:
					Shape.RotateWorld(-offset, Vector3.UnitY, -1f);
					break;
				case Keys.D5:
					Shape.RotateWorld(-offset, Vector3.UnitZ, 1f);
					break;
				case Keys.D6:
					Shape.RotateWorld(-offset, Vector3.UnitZ, -1f);
					break;
				case Keys.D0:
					ChangePhi(3.6f);
					ChangeTheta(1.8f);
					UpdateSphericalMatrix();
					break;
				case Keys.NumPad6:
					planet.Move(new Vector3(0.1f, 0.0f, 0.0f));
					break;
				case Keys.NumPad4:
					planet.Move(new Vector3(-0.1f, 0.0f, 0.0f));
					break;
				case Keys.NumPad8:
					planet.Move(new Vector3(0.0f, 0.1f, 0.0f));
					break;
				case Keys.NumPad2:
					planet.Move(new Vector3(0.0f, -0.1f, 0.0f));
					break;
				case Keys.NumPad7:
					planet.Move(new Vector3(0.0f, 0.0f, 0.1f));
					break;
				case Keys.NumPad3:
					planet.Move(new Vector3(0.0f, 0.0f, -0.1f));
					break;
				case Keys.A:
					break;
				case Keys.B:
					break;
				case Keys.C:
					break;
				case Keys.D:
					break;
				case Keys.F:
					break;
				case Keys.I:
					result.AppendLine("worldToCamera");
					result.AppendLine(Shape.worldToCamera.ToString());
					result.AppendLine("modelToWorld");
					result.AppendLine(planet.modelToWorld.ToString());
					result.AppendLine(AnalysisTools.CalculateMatrixEffects(planet.modelToWorld));
					break;
				case Keys.P:
					break;
				case Keys.R:
					break;
				}
			}
			return result.ToString();
		}

		private void UpdateSphericalMatrix()
		{
			spericalTransform = Matrix4.Mult(sphericalTranslation, sphericalScale);
			spericalTransform = Matrix4.Mult(sphericalRotation, spericalTransform);
		}

		private void Rotate(Vector3 rotationAxis, float angle)
		{
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angle);
			sphericalRotation = Matrix4.Mult(rotation, sphericalRotation);
			UpdateSphericalMatrix();
		}

		private void Translate(Vector3 translation)
		{
			Matrix4 translationMatrix = Matrix4.CreateTranslation(translation);
			sphericalTranslation = Matrix4.Mult(translationMatrix, sphericalTranslation);
			UpdateSphericalMatrix();
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

		private void ChangePhi(float phiChange)
		{
			phi = phi +  (float)Math.PI / 180.0f * phiChange;
			rotationPhi = Matrix4.CreateFromAxisAngle(
				new Vector3((float)Math.Sin(theta), 0f, (float)Math.Cos(theta)), phi);				
			rotationMatrix = Matrix4.Mult(rotationTheta, rotationPhi);
		}
	}
}

