using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public class Tut_Stars : TutorialBase
	{
		List<Star> stars;
		int count = 100;
		protected override void init ()
		{
			stars = new List<Star>();
			for (int i = 0; i < count; i++)
			{
				stars.Add(new Star());
			}
			SetupDepthAndCull();
		}

		public override void display()
		{
			ClearDisplay();
			foreach (Star s in stars)
			{
				s.Draw();
			}
		}
	}
}

