using System;

namespace GlslTutorials
{
	public class BindBinder
	{
		public BindBinder(int prog)
		{
			m_prog = prog;
		}
		public void StateBinder(StateBinder pState)
		{
			pState.BindState(m_prog);
		}
			
		private int m_prog;
	}
}

