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

		float thetaStep = 15f;
		float phiStep = 45f;

		Vector3 offset = new Vector3(0f, -2f, 1f);

		bool rotateWorld = true;
		float worldRotationAngle = 1f;
		Vector3 worldRotationAxis = new Vector3(0f, 1f, 0.5f);

		int currentAnimal = 0;

		enum animal_enum
		{
			DRAGONFLY,
			LADYBUG,
			FIREFLY,
			NUM_ANIMALS,
		}

		protected override void init()
		{
			sphericalProgram = Programs.AddProgram(VertexShaders.spherical_lms, FragmentShaders.lms_fragmentShaderCode);

			planet = new TextureSphere(2f, 0.0f);
			Shape.MoveWorld(offset);
			SetupDepthAndCull();

			sphericalScale = Matrix4.CreateScale(0.25f);
			sphericalTranslation = Matrix4.CreateTranslation(r, -8f, 4f);  // -8 due to scale

			animals = new List<Animal>();
			for (int i = 0; i < 200; i++)
			{
				AddAnimal();
				IncrementAnimal();
			}
		}

		private void IncrementAnimal()
		{
			currentAnimal++;
			if (currentAnimal >= (int)animal_enum.NUM_ANIMALS)
			{
				currentAnimal = 0;
			}
		}

		private void AddAnimal()
		{
			Animal animal;
			switch(currentAnimal)
			{
			case (int)animal_enum.DRAGONFLY: animal = new Dragonfly3d(); break;
			case (int)animal_enum.LADYBUG: animal = new LadyBug3d(); break;
			case (int)animal_enum.FIREFLY: animal = new FireFly3d(); break;	
			default: animal = new Dragonfly3d(); break;
			}
			SetupSphericalAnimal(animal);
			animals.Add(animal);
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
			float thetaOffset = 0f;
			float phiOffset = 0f;
			planet.Draw();
			foreach (Animal a in animals)
			{
				a.SetSystemMatrix(spericalTransform);
				a.SetRotationMatrix(rotationMatrix);
				a.Draw();
				SetThetaPhiOffset(thetaOffset, phiOffset);
				thetaOffset = thetaOffset + thetaStep;
				phiOffset = phiOffset + phiStep;
				UpdateSphericalMatrix();
			}
			if (rotateWorld)
			{
				Shape.RotateWorld(-offset, worldRotationAxis, worldRotationAngle);
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
					phiStep += 5f;
					if(phiStep > 90f)
					{
						phiStep = 5f;
					}
					result.AppendLine("phiStep " + phiStep.ToString());
					break;
				case Keys.R:
					if (rotateWorld)
					{
						rotateWorld = false;
						result.AppendLine("rotateWorld = false");
					}
					else
					{
						rotateWorld = true;
						result.AppendLine("rotateWorld = true");
					}
					break;
				case Keys.T:
					thetaStep += 5f;
					if(thetaStep > 90f)
					{
						thetaStep = 5f;
					}
					result.AppendLine("thetaStep " + thetaStep.ToString());
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

		private void SetThetaPhiOffset(float thetaOffset, float phiOffset)
		{
			thetaOffset = theta + (float)Math.PI / 180.0f * thetaOffset;
			phiOffset = phi + (float)Math.PI / 180.0f * phiOffset;
			rotationTheta = Matrix4.CreateRotationY(thetaOffset);		
			rotationPhi = Matrix4.CreateFromAxisAngle(
				new Vector3((float)Math.Sin(thetaOffset), 0f, (float)Math.Cos(thetaOffset)), phiOffset);	
			rotationMatrix = Matrix4.Mult(rotationTheta, rotationPhi);
		}
	}
}

