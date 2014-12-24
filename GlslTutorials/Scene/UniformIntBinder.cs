using System;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class UniformIntBinder :  UniformBinderBase
	{
		public UniformIntBinder()
		{
			m_val = 0;
		}

		public void SetValue(int val)
		{ 
			m_val = val;
		}

		public override void BindState(int prog)
		{
			GL.Uniform1(GetUniformLoc(prog), m_val);
		}

		public override void UnbindState(int prog) 
		{

		}

		private int m_val;
	};
}

