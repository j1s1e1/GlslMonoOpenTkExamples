using System;

namespace GlslTutorials
{
	public class StateBinder
	{
		//The current program will be in use when this is called.
		public virtual void BindState(int prog)
		{
		}

		//The current program will be in use when this is called.
		public virtual void UnbindState(int prog)
		{
		}
	}
}

