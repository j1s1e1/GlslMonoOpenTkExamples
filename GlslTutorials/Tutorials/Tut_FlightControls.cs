using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace GlslTutorials
{
	public class Tut_FlightControls : TutorialBase
	{
		Dragonfly3d dragonFly;
		float totalScale = 1.0f;
		float minScale = 0.1f;
		float maxScale = 10f;

		int systemMovementMatrixUnif;

		int dragonflyProgram;

		Matrix4 dragonflyMatrix = Matrix4.Identity;
		Matrix4 dragonflyTranslation = Matrix4.Identity;
		Matrix4 dragonflyRotation = Matrix4.Identity;

		protected override void init()
		{
			dragonFly = new Dragonfly3d(0, 0, 0);
			dragonFly.ClearAutoMove();
			dragonflyProgram = Programs.AddProgram(VertexShaders.flightControl_lms, FragmentShaders.lms_fragmentShaderCode);
			dragonflyMatrix = Matrix4.CreateRotationY((float)Math.PI);
			dragonflyMatrix = Matrix4.Identity;
			SetSystemMatrix(dragonflyMatrix, dragonflyProgram);
			dragonFly.SetProgram(dragonflyProgram);
		}

		private void SetSystemMatrix(Matrix4 matrix, int program)
		{
			GL.UseProgram(Programs.GetProgram(program));
			systemMovementMatrixUnif = GL.GetUniformLocation(Programs.GetProgram(program), "systemMovementMatrix");
			GL.UniformMatrix4(systemMovementMatrixUnif, false, ref matrix);
			GL.UseProgram(0);	
		}

		public override void display()
		{
			ClearDisplay();
			dragonFly.Draw();
			SetSystemMatrix(dragonflyMatrix, dragonflyProgram);
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

		private void Rotate(Vector3 rotationAxis, float angle)
		{
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angle);
			dragonflyRotation = Matrix4.Mult(rotation, dragonflyRotation);
			dragonflyMatrix = Matrix4.Mult(dragonflyRotation, dragonflyTranslation);
		}

		private void Translate(Vector3 translation)
		{
			Matrix4 translationMatrix = Matrix4.CreateTranslation(translation);
			dragonflyTranslation = Matrix4.Mult(translationMatrix, dragonflyTranslation);
			dragonflyMatrix = Matrix4.Mult(dragonflyRotation, dragonflyTranslation);
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

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			switch (keyCode)
			{
			case Keys.D1: Rotate(Vector3.UnitX, 5f); break;
			case Keys.D2: Rotate(Vector3.UnitY, 5f); break;
			case Keys.D3: Rotate(Vector3.UnitZ, 5f); break;
			case Keys.NumPad8: Translate(new Vector3(0f, 0.05f, 0f)); break;
			case Keys.NumPad2: Translate(new Vector3(0f, -0.05f, 0f)); break;
			case Keys.NumPad6: Translate(new Vector3(0.05f, 0f, 0f)); break;
			case Keys.NumPad4: Translate(new Vector3(-0.05f, 0f, 0f)); break;
				
			}
			return result.ToString();
		}
	}
}

