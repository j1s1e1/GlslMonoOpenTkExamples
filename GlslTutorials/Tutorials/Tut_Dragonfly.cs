using System;

namespace GlslTutorials
{
	public class Tut_Dragonfly : TutorialBase
	{
		Dragonfly dragonfly;
		protected override void init()
		{
			dragonfly = new Dragonfly(0, 0, 0);
		}

		public override void display()
		{
			ClearDisplay();
			dragonfly.Draw();
		}
	}
}

