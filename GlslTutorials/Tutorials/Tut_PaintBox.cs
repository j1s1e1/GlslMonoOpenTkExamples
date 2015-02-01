﻿using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_PaintBox : TutorialBase
	{
		static PaintBox paintBox;
		Random random = new Random();
		Ball ball;
		float ballRadius = 0.25f;
		float ballSpeedFactor = 1f;
		static float ballLimit = 0.9f;
		static Vector3 ballOffset = new Vector3(0f, 0f, -1f);
		Vector3 ballLimitLow = ballOffset + new Vector3(-ballLimit, -ballLimit, -ballLimit);
		Vector3 ballLimitHigh = ballOffset + new Vector3(ballLimit, ballLimit, ballLimit);
		Vector3 ballSpeed;

		float perspectiveAngle = 90f;
		float newPerspectiveAngle = 90f;

		float textureRotation = -90f;
		float epsilon = 0.01f;

		float moveZ = -1f;

		bool rotateWorld = false;

		protected override void init ()
		{
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

			paintBox = new PaintBox();
			ball = new Ball();
			ball = new Ball(ballRadius);
			ball.SetLimits(ballLimitLow, ballLimitHigh);
			ballSpeed = new Vector3(
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble(),
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble(),
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble());
			ball.SetSpeed(ballSpeed);
			ball.SetLightPosition(new Vector3(0f, 0f, -1f));

			paintBox.SetLimits(ballLimitLow, ballLimitHigh, new Vector3(epsilon, epsilon, epsilon));

			SetupDepthAndCull();
			Textures.EnableTextures();
			g_fzNear = 0.5f;
			g_fzFar = 100f;
			reshape();
		}

		static private void SetGlobalMatrices()
		{
			Shape.SetCameraToClipMatrix(cameraToClipMatrix);
			Shape.SetWorldToCameraMatrix(worldToCameraMatrix);
		}

		public override void reshape()
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(perspectiveAngle, (width / (float) height), g_fzNear, g_fzFar);

			worldToCameraMatrix = persMatrix.Top();

			cameraToClipMatrix = Matrix4.Identity;

			SetGlobalMatrices();

			GL.Viewport(0, 0, width, height);

		}

		public override void display()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			paintBox.Draw();
			ball.Draw();
			if (perspectiveAngle != newPerspectiveAngle)
			{
				perspectiveAngle = newPerspectiveAngle;
				reshape();
			}
			paintBox.Paint(ball.GetOffset());
			UpdateDisplayOptions();
			if (rotateWorld)
			{
				RotateWorldSub();
			}
		}

		static private void SetGlobalMatrices(ProgramData program)
		{
			Shape.worldToCamera = worldToCameraMatrix;
			Shape.cameraToClip = cameraToClipMatrix;

			//GL.UseProgram(program.theProgram);
			//GL.UniformMatrix4(program.cameraToClipMatrixUnif, false, ref cameraToClipMatrix);  // this one is first
			//GL.UniformMatrix4(program.worldToCameraMatrixUnif, false, ref worldToCameraMatrix); // this is the second one
			//GL.UseProgram(0);
		}

		private void RotateWorldSub()
		{
			Matrix4 rotX = Matrix4.CreateRotationX(0.05f * (float)random.NextDouble());
			Matrix4 rotY = Matrix4.CreateRotationY(0.05f * (float)random.NextDouble());
			Matrix4 rotZ = Matrix4.CreateRotationZ(0.05f * (float)random.NextDouble());
			worldToCameraMatrix = Matrix4.Mult(rotX, worldToCameraMatrix);
			worldToCameraMatrix = Matrix4.Mult(rotY, worldToCameraMatrix);
			worldToCameraMatrix = Matrix4.Mult(rotZ, worldToCameraMatrix);
			//SetGlobalMatrices(currentProgram);
			Shape.worldToCamera = worldToCameraMatrix;
			Shape.cameraToClip = cameraToClipMatrix;
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
					paintBox.Move(new Vector3(0f, 0f, 0.1f));
					break;
				case Keys.D2:
					paintBox.Move(new Vector3(0f, 0f, -0.1f));
					break;
				case Keys.D3:
					paintBox.MoveFront(new Vector3(0f, 0f, 0.1f));
					break;
				case Keys.D4:
					paintBox.MoveFront(new Vector3(0f, 0f, -0.1f));
					break;
				case Keys.D5:
					Shape.RotateWorld(Vector3.UnitX, 5f);
					result.AppendLine("RotateWorld 5X");
					break;
				case Keys.D6:
					Shape.RotateWorld(Vector3.UnitY, 5f);
					result.AppendLine("RotateWorld 5Y");
					break;
				case Keys.D7:
					Shape.RotateWorld(Vector3.UnitZ, 5f);
					result.AppendLine("RotateWorld 5Z");
					break;
				case Keys.D8:
					break;
				case Keys.D9:
					break;
				case Keys.D0:
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
					result.AppendLine("g_fzNear = " + g_fzNear.ToString());
					result.AppendLine("g_fzFar = " + g_fzFar.ToString());
					result.AppendLine("perspectiveAngle = " + perspectiveAngle.ToString());
					result.AppendLine("textureRotation = " + textureRotation.ToString());
					break;
				case Keys.P:
					newPerspectiveAngle = perspectiveAngle + 5f;
					if (newPerspectiveAngle > 170f) {
						newPerspectiveAngle = 30f;
					}
					break;
				case Keys.R:
					if (rotateWorld)
					{
						rotateWorld = false;
						result.AppendLine("rotateWorld disabled");
					}
					else
					{
						rotateWorld = true;
						result.AppendLine("rotateWorld enabled");
					}
					break;
				}
			}
			return result.ToString();
		}
	}
}
