using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace GlslTutorials
{
	public class Tut_SphericalCoordinates : TutorialBase
	{
		Dragonfly3d dragonFly;
		float totalScale = 1.0f;
		float minScale = 0.1f;
		float maxScale = 10f;

		float r = 2f;
		float theta = 0f;
		float phi = 0f;

		int systemMovementMatrixUnif;
		int rotationMatrixUnif;

		int dragonflyProgram;

		Matrix4 dragonflyMatrix = Matrix4.Identity;
		Matrix4 dragonflyScale = Matrix4.Identity;
		Matrix4 dragonflyTranslation = Matrix4.Identity;
		Matrix4 dragonflyRotation = Matrix4.Identity;

		Matrix4 rotationTheta = Matrix4.Identity;
		Matrix4 rotationPhi = Matrix4.Identity;
		Matrix4 rotationMatrix = Matrix4.Identity;

		protected override void init()
		{
			dragonFly = new Dragonfly3d(0, 0, 0);
			dragonFly.ClearAutoMove();
			dragonflyProgram = Programs.AddProgram(VertexShaders.spherical_lms, FragmentShaders.lms_fragmentShaderCode);
			dragonflyMatrix = Matrix4.CreateRotationY((float)Math.PI);
			dragonflyMatrix = Matrix4.Identity;
			SetSystemMatrix(dragonflyMatrix, dragonflyProgram);
			dragonFly.SetProgram(dragonflyProgram);
			dragonflyScale = Matrix4.CreateScale(0.25f);
			dragonflyTranslation = Matrix4.CreateTranslation(r, 0.0f, 0.0f);
			UpdateDragonflyMatrix();
			systemMovementMatrixUnif = GL.GetUniformLocation(Programs.GetProgram(dragonflyProgram), "systemMovementMatrix");
			rotationMatrixUnif = GL.GetUniformLocation(Programs.GetProgram(dragonflyProgram), "rotationMatrix");
			SetRotationMatrix(rotationMatrix, dragonflyProgram);
		}

		private void SetSystemMatrix(Matrix4 matrix, int program)
		{
			GL.UseProgram(Programs.GetProgram(program));
			GL.UniformMatrix4(systemMovementMatrixUnif, false, ref matrix);
			GL.UseProgram(0);	
		}

		private void SetRotationMatrix(Matrix4 matrix, int program)
		{
			GL.UseProgram(Programs.GetProgram(program));
			GL.UniformMatrix4(rotationMatrixUnif, false, ref matrix);
			GL.UseProgram(0);	
		}

		public override void display()
		{
			ClearDisplay();
			dragonFly.Draw();
			SetSystemMatrix(dragonflyMatrix, dragonflyProgram);
			SetRotationMatrix(rotationMatrix, dragonflyProgram);
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

		private void UpdateDragonflyMatrix()
		{
			dragonflyMatrix = Matrix4.Mult(dragonflyTranslation, dragonflyScale);
			dragonflyMatrix = Matrix4.Mult(dragonflyRotation, dragonflyMatrix);
		}

		private void Rotate(Vector3 rotationAxis, float angle)
		{
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angle);
			dragonflyRotation = Matrix4.Mult(rotation, dragonflyRotation);
			UpdateDragonflyMatrix();
		}

		private void Translate(Vector3 translation)
		{
			Matrix4 translationMatrix = Matrix4.CreateTranslation(translation);
			dragonflyTranslation = Matrix4.Mult(translationMatrix, dragonflyTranslation);
			UpdateDragonflyMatrix();
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
			dragonflyTranslation = Matrix4.Mult(translationMatrix, dragonflyTranslation);
			UpdateDragonflyMatrix();
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
			case Keys.D1: ChangeRadius(0.05f); break;
			case Keys.D2: ChangeRadius(-0.05f); break;
			case Keys.D3: ChangeTheta(5f); break;
			case Keys.D4: ChangeTheta(-5f); break;
			case Keys.D5: ChangePhi(5f); break;
			case Keys.D6: ChangePhi(-5f); break;
			case Keys.I:
				result.AppendLine("dragonflyMatrix = " + dragonflyMatrix.ToString());
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

			}
			return result.ToString();
		}
	}
}

