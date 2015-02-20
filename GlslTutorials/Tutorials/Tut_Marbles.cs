using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GlslTutorials
{
	public class Tut_Marbles : TutorialBase
	{
		List<Marble> marbles = new List<Marble>();

		protected override void init()
		{
			for (int i = 0; i < 1; i++)
			{ 
				marbles.Add(new Marble());
			}

			for(int i = 0; i < marbles.Count; i++)
			{
				for(int j = 0; j < marbles.Count; j++)
				{
					if (j != i)
					{
						marbles[i].AddOtherObject(marbles[j]);
					}
				}
			}
			SetupDepthAndCull();
		}


		public override void display()
		{
			ClearDisplay();
			for(int i = 0; i < marbles.Count; i++)
			{
				marbles[i].Draw();
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
				case Keys.M:
					marbles.Add(new Marble());
					break;
				case Keys.I:
					break;
				}
			}
			return result.ToString();
		}
	}
}

