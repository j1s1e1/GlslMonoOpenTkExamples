using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tut_Collisions : TutorialBase
	{
		List<Ball> balls;

		float ballRadius = 0.05f;
		float ballSpeedFactor = 1f;
		static float ballLimit = 0.75f;
		static Vector3 ballOffset = new Vector3(0f, 0f, 0.0f);
		Vector3 ballLimitLow = ballOffset + new Vector3(-ballLimit, -ballLimit, -0.5f);
		Vector3 ballLimitHigh = ballOffset + new Vector3(ballLimit, ballLimit, -0.5f);

		Vector3 ballSpeed;

		Random random = new Random();

		private void AddBall()
		{
			Ball ball = new Ball(ballRadius);
			ball.SetLimits(ballLimitLow, ballLimitHigh);
			ballSpeed = new Vector3(
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble(),
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble(),
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble());
			ball.SetSpeed(ballSpeed);
			ball.SetLightPosition(new Vector3(0f, 0f, -1f));
			balls.Add(ball);
		}

		protected override void init ()
		{
			SetupDepthAndCull();
			balls = new List<Ball>();
			for (int i = 0; i < 7; i++)
			{
				AddBall();
			}

			foreach (Ball b in balls)
			{
				foreach (Ball c in balls)
				{
					if (b.Equals(c))
					{
					}
					else
					{
						b.AddPaddle(c);
					}
				}
			}
		}

		public override void display()
		{
			ClearDisplay();
			foreach(Ball b in balls)
			{
				b.Draw();
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
					break;
				case Keys.D2:
					break;
				case Keys.D3:
					break;
				case Keys.D4:
					break;
				case Keys.D5:
					break;
				case Keys.D6:
					break;
				case Keys.D7:
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
				case Keys.F:
					break;
				case Keys.I:
					break;
				case Keys.K:
					break;
				case Keys.M:
					break;
				case Keys.P:
					break;
				case Keys.R:
					break;
				case Keys.D:
					foreach (Ball b in balls)
					{
						b.ScaleSpeed(0.9f);
					}
					break;
				case Keys.U:
					foreach (Ball b in balls)
					{
						b.ScaleSpeed(1.1f);
					}
					break;
				}
			}
			return result.ToString();
		}
	}
}

