using System;
using OpenTK;

namespace GlslTutorials
{
	public class Cat : Animal
	{
		LitMatrixSphere2 s1;
		LitMatrixSphere2 s2;

		public Cat()
		{
			s1 = new LitMatrixSphere2(0.1f);
			s2 = new LitMatrixSphere2(0.3f);
			s2.SetOffset(new Vector3(0.15f, -0.20f, 0f));

		}

		public override void Move(Vector3 v)
		{
			s1.Move(v);
			s2.Move(v);
		}

		public override void Draw()
		{
			s1.Draw();
			s2.Draw();
		}  
	}
}

