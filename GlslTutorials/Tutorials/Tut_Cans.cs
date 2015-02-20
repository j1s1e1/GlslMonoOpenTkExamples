using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Cans : TutorialBase
	{
		Ball ball;
		int ballProgram;
		int numberOfLights = 2;
		float ballRadius = 0.05f;
		static float ballLimit = 0.75f;
		static Vector3 ballOffset = new Vector3(0f, 0f, -2f);
		Vector3 ballLimitLow = ballOffset + new Vector3(-ballLimit, -ballLimit, -ballLimit);
		Vector3 ballLimitHigh = ballOffset + new Vector3(ballLimit, ballLimit, ballLimit);
		Vector3 ballSpeed;

		Stool stool = new Stool();
		List<Can> cans = new List<Can>();

		float xSeparation = 0.12f;
		float ySeparation = 0.1f;

		bool gravity = true;
		Vector3 gravityVector = new Vector3(0f, -0.005f, 0f);

		float perspectiveAngle = 90f;

		float canZ = -1.5f;

		protected override void init()
		{
			stool.Move(new Vector3(0f, -0.03f, -1f));
			ball = new Ball(ballRadius);
			ball.SetLimits(ballLimitLow, ballLimitHigh);
			ballSpeed = new Vector3(0.5f, 0.5f, 0.5f);
			ball.SetSpeed(ballSpeed);
			ball.SetLightPosition(new Vector3(0f, 0f, -1f));
			ball.SetOffset(new Vector3(-0.75f, 0.75f, -1f));
			for (int i = 0; i < 6; i++)
			{ 
				Can c = new Can();
				c.RotateShape(new Vector3(1f, 0f, 0f), 90f);
				c.SetColor(Colors.BLUE_COLOR);
				switch (i)
				{
					case 0: c.Move(new Vector3(-xSeparation, 0f, canZ)); break;
					case 1: c.Move(new Vector3(0f, 0f, canZ)); break;
					case 2: c.Move(new Vector3(xSeparation, 0f, canZ)); break;
					case 3: c.Move(new Vector3(-xSeparation/2, ySeparation, canZ)); break;
					case 4: c.Move(new Vector3(xSeparation/2, ySeparation, canZ)); break;
					case 5: c.Move(new Vector3(0f, 2*ySeparation, canZ)); break;
				}
				c.SetSpeed(new Vector3(0f, 0f, 0f));
				c.AddPaddle(ball);

				cans.Add(c);
			}
			SetupDepthAndCull();
			updateProgram();
		}


		private void updateProgram()
		{
			ballProgram = Programs.AddProgram(VertexShaders.HDR_PCN2, 
				FragmentShaders.DiffuseSpecularHDR);
			ball.SetProgram(ballProgram);
			Programs.SetUpLightBlock(ballProgram, numberOfLights);
			LightBlock	lightBlock = new LightBlock(numberOfLights);
			lightBlock.ambientIntensity = new Vector4(0.1f, 0.1f, 0.1f, 1.0f);
			lightBlock.lightAttenuation = 0.1f;
			lightBlock.maxIntensity = 0.5f;
			lightBlock.lights[0].cameraSpaceLightPos = new Vector4(0.5f, 0.0f, 0.0f, 1f);
			lightBlock.lights[0].lightIntensity = new Vector4(0.0f, 0.0f, 0.6f, 1.0f);
			lightBlock.lights[1].cameraSpaceLightPos = new Vector4(0.0f, 0.5f, 1.0f, 1f);
			lightBlock.lights[1].lightIntensity = new Vector4(0.4f, 0.0f, 0.0f, 1.0f);
			Programs.UpdateLightBlock(ballProgram, lightBlock);

			MaterialBlock materialBlock = new MaterialBlock();
			materialBlock.diffuseColor = new Vector4(1.0f, 0.673f, 0.043f, 1.0f);
			materialBlock.specularColor = new Vector4(1.0f, 0.673f, 0.043f, 1.0f) * 0.4f;
			materialBlock.specularShininess = 0.2f;

			Programs.SetUpMaterialBlock(ballProgram);
			Programs.UpdateMaterialBlock(ballProgram, materialBlock);

			Programs.SetNormalModelToCameraMatrix(ballProgram, Matrix3.Identity);
		}



		public override void display()
		{
			ClearDisplay();
			stool.Draw();
			for(int i = 0; i < cans.Count; i++)
			{
				cans[i].Draw();
			}
			ball.Draw();
			if (gravity)
			{
				ball.Accelerate(gravityVector);
			}
			for(int i = 0; i < cans.Count; i++)
			{
				cans[i].CheckCollisions(ball.GetOffset(), 0.2f);
			}
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
				case Keys.D:
					ball.SetOffset(new Vector3(-0.75f, 0.75f, 0f));
					break;
				case Keys.Z:
					ball.SetOffset(new Vector3(0f, 0f, 1f));
					break;
				case Keys.G:
					if (gravity)
					{ 
						gravity = false;
					}
					else
					{
						gravity = true;
					}
					break;
				case Keys.I:
					break;
				case Keys.S:
					ball.SetSpeed(new Vector3(0.0f, 0f, 0f));
					break;
				case Keys.T:
					ball.Accelerate(new Vector3(0.0f, 0f, 0.1f));
					break;
				}
			}
			return result.ToString();
		}
	}
}

