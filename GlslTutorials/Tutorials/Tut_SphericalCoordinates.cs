using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_SphericalCoordinates : TutorialBase
	{
		List<Animal> animals;
		float totalScale = 1.0f;
		float minScale = 0.1f;
		float maxScale = 10f;

		float r = 2f;
		float theta = 0f;
		float phi = 0f;

		int sphericalProgram;

		Matrix4 spericalTransform = Matrix4.Identity;
		Matrix4 sphericalScale = Matrix4.Identity;
		Matrix4 sphericalTranslation = Matrix4.Identity;
		Matrix4 sphericalRotation = Matrix4.Identity;

		Matrix4 rotationTheta = Matrix4.Identity;
		Matrix4 rotationPhi = Matrix4.Identity;
		Matrix4 rotationMatrix = Matrix4.Identity;

		int currentAnimal = 0;
		bool nextAnimal = false;
		bool addAnimal = false;

		enum animal_enum
		{
			DRAGONFLY,
			LADYBUG,
			FIREFLY,
			NUM_ANIMALS,
		}

		private void SetupSphericalAnimal(Animal animal)
		{
			animal.ClearAutoMove();
			animal.SetProgram(sphericalProgram);
			animal.SetSystemMatrix(spericalTransform);
			animal.SetRotationMatrix(rotationMatrix);
		}

		protected override void init()
		{
			
			animals = new List<Animal>();
			Animal animal = new Dragonfly3d();

			sphericalProgram = Programs.AddProgram(VertexShaders.spherical_lms, FragmentShaders.lms_fragmentShaderCode);
			spericalTransform = Matrix4.CreateRotationY((float)Math.PI);
			spericalTransform = Matrix4.Identity;

			sphericalScale = Matrix4.CreateScale(0.25f);
			sphericalTranslation = Matrix4.CreateTranslation(r, 0.0f, 0.0f);
			UpdateSphericalMatrix();
			SetupSphericalAnimal(animal);
			animals.Add(animal);
		}

		public override void display()
		{
			ClearDisplay();
			float separationAngle = 360 / animals.Count;
			foreach (Animal a in animals)
			{
				a.Draw();
				a.SetSystemMatrix(spericalTransform);
				a.SetRotationMatrix(rotationMatrix);
				ChangeTheta(separationAngle);
			}
			if (nextAnimal)
			{				
				nextAnimal = false;
				animals = new List<Animal>();
				IncrementAnimal();
				AddAnimal();
			}
			if (addAnimal)
			{
				addAnimal = false;
				IncrementAnimal();
				AddAnimal();
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

		public void SetScale(float scale) {
			if ((totalScale * scale) > minScale)
			{
				if ((totalScale * scale) < maxScale) {
					totalScale = totalScale * scale;
					Shape.ScaleWorldToCameraMatrix(scale);
				}
			}
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

		public void receiveMessage(String message)
		{
			String[] words = message.Split(' ');
			switch (words[0])
			{
			case "ZoomIn": SetScale(1.05f); break;
			case "ZoomOut": SetScale(1f / 1.05f); break;
			case "RotateX":  Rotate(Vector3.UnitX, float.Parse(words[1])); break;
			case "RotateY":  Rotate(Vector3.UnitY, float.Parse(words[1])); break;
			case "RotateZ":  Rotate(Vector3.UnitZ, float.Parse(words[1])); break;
			case "RotateX+": Rotate(Vector3.UnitX, 5f); break;
			case "RotateX-": Rotate(Vector3.UnitX, -5f); break;
			case "RotateY+": Rotate(Vector3.UnitY, 5f); break;
			case "RotateY-": Rotate(Vector3.UnitY, -5f); break;
			case "RotateZ+": Rotate(Vector3.UnitZ, 5f); break;
			case "RotateZ-": Rotate(Vector3.UnitZ, -5f); break;
			case "SetScale":
				if (words.Length == 2) {
					Shape.ResetWorldToCameraMatrix();
					Shape.ScaleWorldToCameraMatrix(float.Parse(words[1]));
				}
				break;
			case "SetScaleLimit":
				if (words.Length == 3) {
					minScale = float.Parse(words[1]);
					maxScale = float.Parse(words[2]);
				}
				break;
			}
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

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			switch (keyCode)
			{
			case Keys.A: addAnimal = true; break;
			case Keys.D1: ChangeRadius(0.05f); break;
			case Keys.D2: ChangeRadius(-0.05f); break;
			case Keys.D3: ChangeTheta(5f); break;
			case Keys.D4: ChangeTheta(-5f); break;
			case Keys.D5: ChangePhi(5f); break;
			case Keys.D6: ChangePhi(-5f); break;
			case Keys.I:
				result.AppendLine("dragonflyMatrix = " + spericalTransform.ToString());
				result.AppendLine("rotationMatrix = " + rotationMatrix.ToString());
				break;
			case Keys.NumPad8: Translate(new Vector3(0f, 0.05f, 0f)); break;
			case Keys.NumPad2: Translate(new Vector3(0f, -0.05f, 0f)); break;
			case Keys.NumPad6: Translate(new Vector3(0.05f, 0f, 0f)); break;
			case Keys.NumPad4: Translate(new Vector3(-0.05f, 0f, 0f)); break;

			case Keys.D7: Rotate(Vector3.UnitY, 5f); break;
			case Keys.D8: Rotate(Vector3.UnitY, -5f); break;
			case Keys.D9: Rotate(Vector3.UnitX, 5f); break;
			case Keys.D0: Rotate(Vector3.UnitX, -5f); break;
			case Keys.N:
				nextAnimal = true;
				break;

			}
			return result.ToString();
		}
	}
}

