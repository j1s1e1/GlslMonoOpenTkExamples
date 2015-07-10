using System;

namespace GlslTutorials
{
	public class Tut_Dragonfly : TutorialBase
	{
		Dragonfly3d dragonfly;
		protected override void init()
		{
			dragonfly = new Dragonfly3d(0, 0, 0);
		}

		public override void display()
		{
			ClearDisplay();
			dragonfly.Draw();
		}
	}
}

