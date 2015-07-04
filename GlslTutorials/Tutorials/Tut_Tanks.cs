using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tut_Tanks : TutorialBase
	{	
		Tank tank1;
		Tank tank2;
		
		protected override void init()
	    {
			tank1 = new Tank();	
			tank2 = new Tank();
			SetupDepthAndCull();
		}
		
		float angle = 0;

		private void Move()
		{
			angle += 0.02f;
		}
		
		public override void display()
	    {
	        ClearDisplay();
			tank1.Draw();
			tank2.Draw();
			Move();
		}
		
		public override String keyboard(Keys keyCode, int x, int y)
	    {
	        StringBuilder result = new StringBuilder();
	        switch (keyCode) 
			{
				case Keys.P:
					tank1.SetKeyboardControl();
					break;
				case Keys.R:
					tank1.SetRandomControl();
					break;
				case Keys.S:
					tank1.SetSocketControl();
					break;
				
	        }
			tank1.keyboard(keyCode);
	        result.AppendLine(keyCode.ToString());
	        reshape();
	        display();
	        return result.ToString();
	    }
	}
}

