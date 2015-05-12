using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_PaintBoxSlave : TutorialBase
	{
		static PaintBoxState paintBoxState;
		static PaintBox paintBox;
		List<Paddle2> paddles;
		List<TextClass> scores;
		Random random = new Random();
		Ball ball;
		float ballRadius = 0.25f;
		float ballSpeedFactor = 1f;
		static float ballLimit = 0.75f;
		static Vector3 ballOffset = new Vector3(0f, 0f, -1f);
		Vector3 ballLimitLow = ballOffset + new Vector3(-ballLimit, -ballLimit, -ballLimit);
		Vector3 ballLimitHigh = ballOffset + new Vector3(ballLimit, ballLimit, ballLimit);
		Vector3 boxLimitLow;
		Vector3 boxLimitHigh;

		Vector3 ballSpeed;

		float perspectiveAngle = 90f;
		float newPerspectiveAngle = 90f;

		float textureRotation = -90f;
		float epsilon = 0.251f;

		bool rotateWorld = false;
		int ballProgram;
		int numberOfLights = 2;

		bool updatePositions = false;

		TextClass mousePostion;
		bool staticText = true;

		bool displayScores = false;

		int[] scoreInts = new int[6];

		bool blockTest = false;
		bool sphereTest = false;
		bool blenderTest = false;

		const int FRONT_PADDLE = 0;
		const int BACK_PADDLE = 1;
		const int LEFT_PADDLE = 2;
		const int RIGHT_PADDLE = 3;
		const int TOP_PADDLE = 4;
		const int BOTTOM_PADDLE = 5;

		static int FRONT_SCORE = 5;
		static int BACK_SCORE = 4;
		static int LEFT_SCORE = 0;
		static int RIGHT_SCORE = 1;
		static int TOP_SCORE = 3;
		static int BOTTOM_SCORE = 2;

		static int HIT_VALUE = 100;

		public static int[] walls = new int[]{FRONT_PADDLE, BACK_PADDLE, LEFT_PADDLE, RIGHT_PADDLE, TOP_PADDLE, BOTTOM_PADDLE};

		public static void PaddleHitBall(int paddleNumber)
		{
			switch (paddleNumber)
			{
			case FRONT_PADDLE: paintBox.AddToScore(FRONT_SCORE, HIT_VALUE); break;
			case BACK_PADDLE: paintBox.AddToScore(BACK_SCORE, HIT_VALUE); break;
			case LEFT_PADDLE: paintBox.AddToScore(LEFT_SCORE, HIT_VALUE); break;
			case RIGHT_PADDLE: paintBox.AddToScore(RIGHT_SCORE, HIT_VALUE); break;
			case TOP_PADDLE: paintBox.AddToScore(TOP_SCORE, HIT_VALUE); break;
			case BOTTOM_PADDLE: paintBox.AddToScore(BOTTOM_SCORE, HIT_VALUE); break;
			}
		}

		private void AddScore(Vector3 offset, Vector3 rotationAxis, float rotationAngle)
		{
			TextClass score = new TextClass("1000", 0.4f, 0.04f, staticText);
			score.RotateShape(rotationAxis, rotationAngle);
			score.SetOffset(offset);
			scores.Add(score);
		}

		private void AddPaddle(Vector3 limitLow, Vector3 limitHigh)
		{
			Paddle2 paddle = new Paddle2();
			paddle.SetLimits(limitLow, limitHigh);
			paddle.SetRemoteControl();
			paddles.Add(paddle);
			ball.AddPaddle(paddle);
		}

		protected override void init ()
		{
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

			boxLimitLow = new Vector3();
			boxLimitLow = ballLimitLow - new Vector3(ballRadius, ballRadius, ballRadius);
			boxLimitHigh = new Vector3();
			boxLimitHigh = ballLimitHigh + new Vector3(ballRadius, ballRadius, ballRadius);

			paintBox = new PaintBox();
			ball = new Ball(ballRadius);
			ball.SetLimits(ballLimitLow, ballLimitHigh);
			ballSpeed = new Vector3(
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble(),
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble(),
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble());
			ball.SetSpeed(ballSpeed);
			ball.SetLightPosition(new Vector3(0f, 0f, -1f));

			paintBox.SetLimits(boxLimitLow, boxLimitHigh, new Vector3(epsilon, epsilon, epsilon));
			paintBox.Move(new Vector3(0f, 0f, -1f));
			ball.MoveLimits(new Vector3(0f, 0f, -1f));

			SetupDepthAndCull();
			GL.Disable(EnableCap.CullFace);
			Textures.EnableTextures();
			g_fzNear = 0.5f;
			g_fzFar = 100f;
			reshape();
			paddles = new List<Paddle2>();

			AddPaddle(new Vector3(-1f, -1f, -1f), new Vector3(1f, 1f, -1f));
			paddles[0].SetMouseControl();

			AddPaddle(new Vector3(-1f, -1f, -3f), new Vector3(1f, 1f, -3f));
			paddles[BACK_PADDLE].SetNormal(new Vector3(0f, 0f, 1f));
			paddles[BACK_PADDLE].UseBlock(new Vector3(0.3f, 0.3f, 0.05f));

			AddPaddle(new Vector3(-1f, -1f, -3f), new Vector3(-1f, 1f, -1f));
			paddles[LEFT_PADDLE].SetNormal(new Vector3(1f, 0f, 0f));
			paddles[LEFT_PADDLE].UseBlock(new Vector3(0.05f, 0.3f, 0.3f));

			AddPaddle(new Vector3(1f, -1f, -3f), new Vector3(1f, 1f, -1f));
			paddles[RIGHT_PADDLE].SetNormal(new Vector3(-1f, 0f, 0f));
			paddles[RIGHT_PADDLE].UseBlock(new Vector3(0.05f, 0.3f, 0.3f));

			AddPaddle(new Vector3(-1f, 1f, -3f), new Vector3(1f, 1f, -1f));
			paddles[TOP_PADDLE].SetNormal(new Vector3(0f, -1f, 0f));
			paddles[TOP_PADDLE].UseBlock(new Vector3(0.3f, 0.05f, 0.3f));

			AddPaddle(new Vector3(-1f, -1f, -3f), new Vector3(1f, -1f, -1f));
			paddles[BOTTOM_PADDLE].SetNormal(new Vector3(0f, 1f, 0f));
			paddles[BOTTOM_PADDLE].UseBlock(new Vector3(0.3f, 0.05f, 0.3f));

			mousePostion = new TextClass("MousePosition", 0.4f, 0.04f, staticText);
			mousePostion.SetOffset(new Vector3(-0.75f, -0.75f, -0.5f));
			updateProgram();
			scores = new List<TextClass>();
			AddScore(new Vector3(-0.6f, 0.0f, -1f), new Vector3(0f, 1f, 0f), 45f); 
			AddScore(new Vector3(0.6f, 0.0f, -1f), new Vector3(0f, 1f, 0f), -45f); 
	
			AddScore(new Vector3(0.0f, -0.45f, -1f), new Vector3(1.0f, -0f, 0f), 0f);
			AddScore(new Vector3(0.0f, 0.45f, -1f), new Vector3(1.0f, -0f, 0f), 0f);

			AddScore(new Vector3(0.0f, 0.25f, -1f), new Vector3(1.0f, -0f, 0f), 0f);
			AddScore(new Vector3(0.0f, -0.85f, -1f), new Vector3(1.0f, -0f, 0f), 0f);
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
			foreach(Paddle2 paddle in paddles)
			{
				paddle.Draw();
			}
			if (displayScores)
			{
				foreach(TextClass t in scores)
				{
					t.Draw();
				}
				int[] newScores = paintBox.GetScores();
				for (int i = 0; i < scoreInts.Length; i++)
				{
					if (scoreInts[i] != newScores[i])
					{
						scoreInts[i] = newScores[i];
						if (scores.Count > i)
						{
							scores[i].UpdateText(scoreInts[i].ToString());
						}
					}
				}
			}
			UpdateDisplayOptions();
			if (rotateWorld)
			{
				RotateWorldSub();
			}
			if (updatePositions)
			{
				mousePostion.UpdateText("MousePosition " + paddles[0].GetOffset());
				mousePostion.Draw();
			}
			Vector3 offset;
			if (blenderTest)
			{
				blenderTest = false;
				offset = ball.GetOffset();
				ball.UseBlender(new Vector3(0.1f, 0.1f, 0.1f), "X_Wing3.obj");
				ball.SetOffset(offset);
				ball.SetProgram(ballProgram);
			}
			if (blockTest) 
			{
				blockTest = false;
				offset = ball.GetOffset();
				ball.UseBlock(new Vector3(0.2f, 0.2f, 0.2f));
				ball.SetOffset(offset);
				ball.SetProgram(ballProgram);
				offset = paddles[0].GetOffset();
				paddles[0].UseBlock(new Vector3(0.2f, 0.2f, 0.2f));
				paddles[0].SetProgram(ballProgram);
				paddles[0].SetOffset(offset);
			}
			if (sphereTest)
			{
				sphereTest = false;
				offset = ball.GetOffset();
				ball.UseSphere(ballRadius);
				ball.SetOffset(offset);
				ball.SetProgram(ballProgram);
			}
		}

		private void updateProgram()
		{
			ballProgram = Programs.AddProgram(VertexShaders.HDR_PCN2, 
				FragmentShaders.DiffuseSpecularHDR);
			ball.SetProgram(ballProgram);
//			foreach (Paddle2 p in paddles)
//			{
//				//p.SetProgram(ballProgram);
//			}
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
					ball.MoveLimits(new Vector3(0f, 0f, 0.1f));
					break;
				case Keys.D2:
					paintBox.Move(new Vector3(0f, 0f, -0.1f));
					ball.MoveLimits(new Vector3(0f, 0f, -0.1f));
					break;
				case Keys.D3:
					paintBox.MoveFront(new Vector3(0f, 0f, 0.1f));
					break;
				case Keys.D4:
					paintBox.MoveFront(new Vector3(0f, 0f, -0.1f));
					break;
				case Keys.D5:
					paintBox.RotateShapeOffset(Vector3.UnitX, 5f);
					result.AppendLine("RotateShapeOffset 5X");
					break;
				case Keys.D6:
					paintBox.RotateShapeOffset(Vector3.UnitY, 5f);
					result.AppendLine("RotateShapeOffset 5Y");
					break;
				case Keys.D7:
					paintBox.RotateShapeOffset(Vector3.UnitZ, 5f);
					result.AppendLine("RotateShapeOffset 5Z");
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
					paintBox.Clear();
					break;
				case Keys.D:
					if (updatePositions)
					{
						updatePositions = false;
					}
					else
					{
						updatePositions = true;
					}
					break;
				case Keys.F:
					blenderTest = true;
					break;
				case Keys.G:
					blockTest = true;
					break;
				case Keys.H:
					sphereTest = true;
					break;
				case Keys.I:
					result.AppendLine("g_fzNear = " + g_fzNear.ToString());
					result.AppendLine("g_fzFar = " + g_fzFar.ToString());
					result.AppendLine("perspectiveAngle = " + perspectiveAngle.ToString());
					result.AppendLine("textureRotation = " + textureRotation.ToString());
					result.AppendLine("BallPosition = " + ball.GetOffset().ToString());
					result.AppendLine("BallLimits = " + ball.GetLimits());
					result.AppendLine("BoxLimits = " + paintBox.GetLimits());
					break;
				case Keys.K:
					paddles[0].SetKeyboardControl();
					break;
				case Keys.M:
					paddles[0].SetMouseControl();
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
				case Keys.S:
					if (displayScores)
					{
						displayScores = false;
					}
					else
					{
						displayScores = true;
					}
					break;
				}
			}
			paddles[0].keyboard(keyCode);
			return result.ToString();
		}

		public override void MouseMotion (int x, int y)
		{
			paddles[0].MouseMotion(x, y);
		}
	}
}

