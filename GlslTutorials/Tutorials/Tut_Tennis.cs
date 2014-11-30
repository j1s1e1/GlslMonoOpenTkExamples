using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tut_Tennis : TutorialBase
	{
		public Tut_Tennis ()
		{
		}
		
		Paddle topPaddle;
		Paddle bottomPaddle;
		Ball ball;
		TextClass ScoreTop;
		TextClass ScoreBottom;
		int scoreTopInt = 0;
		int scoreBottomInt = 0;
		
		protected override void init()
	    {
			Programs.reset();
			Shape.resetWorldToCameraMatrix();
			ball = new Ball();
			ball.SetLimits(new Vector3(-1f, -1f, 0.5f), new Vector3(1f, 1f, 0.5f));
			topPaddle = new Paddle();	
			topPaddle.SetLimits(new Vector3(-1f, 0.9f, 0.5f), new Vector3(1f, 0.9f, 0.5f));
			bottomPaddle = new Paddle();
			bottomPaddle.SetLimits(new Vector3(-1f, -0.9f, 0.5f), new Vector3(1f, -0.9f, 0.5f));
			ball.AddPaddle(topPaddle);
			ball.AddPaddle(bottomPaddle);
			ScoreTop = new TextClass("000", 1f, 0.1f, true, true);
			ScoreTop.SetOffset(new Vector3(-0.8f, 0.7f, 0.5f));
			ScoreBottom = new TextClass("000", 1f, 0.1f, true, true);
			ScoreBottom.SetOffset(new Vector3(0.7f, -0.7f, 0.5f));
			SetupDepthAndCull();
		}
		
		public override void display()
	    {
	        ClearDisplay();
			ball.Draw();
			topPaddle.Draw();
			bottomPaddle.Draw();
			ScoreTop.Draw();
			ScoreBottom.Draw();
			UpdateScores();
		}
		
		bool goal = false;
		
		public void UpdateScores()
		{
			float ballHeight = ball.GetOffset().Y;
			if (goal == false)
			{
				if (ballHeight > 0.95f)
				{
					goal = true;
					scoreBottomInt++;
					ScoreBottom.UpdateText(scoreBottomInt.ToString("000"));
					
				}
				if (ballHeight < -0.95f)
				{
					goal = true;
					scoreTopInt++;
					ScoreTop.UpdateText(scoreTopInt.ToString("000"));
				}
			}
			else
			{
				if ((ballHeight > -0.85f) && (ballHeight < 0.85f))
				{
					goal = false;
				}
			}
		}
		
		public override String keyboard(Keys keyCode, int x, int y)
	    {
	        StringBuilder result = new StringBuilder();
	        switch (keyCode) 
			{
				case Keys.P:
					topPaddle.SetKeyboardControl();
					break;
				case Keys.R:
					topPaddle.SetRandomControl();
					break;
				case Keys.S:
					topPaddle.SetSocketControl();
					break;
				
	        }
			topPaddle.keyboard(keyCode);
	        result.AppendLine(keyCode.ToString());
	        reshape();
	        display();
	        return result.ToString();
	    }
	}
}

