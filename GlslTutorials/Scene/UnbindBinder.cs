using System;

namespace GlslTutorials
{
	public class UnbindBinder
	{
		public UnbindBinder(int prog)
		{
			m_prog = prog;
		}
		public void StateBinder(StateBinder pState) 
		{ 
			pState.UnbindState(m_prog);
		}
		int m_prog;
	}
}

