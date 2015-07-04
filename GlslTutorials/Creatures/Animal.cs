using System;
using OpenTK;

namespace GlslTutorials
{
	public class Animal
	{
		LitMatrixSphere2 s;

		public Animal ()
		{
			s = new LitMatrixSphere2(0.1f);
		}

		public virtual void Move(Vector3 v)
		{
			s.Move(v);
		}


		public virtual void Draw()
		{
			s.Draw();
		}           
	}
}

