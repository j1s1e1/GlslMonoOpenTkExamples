using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class UniformMat4Binder : UniformBinderBase
	{
		public UniformMat4Binder()
		{
			m_val = Matrix4.Identity;
		}

		public void SetValue(Matrix4 val)	
		{ 
			m_val = val; 
		}

		public override void BindState(int prog)
		{
			GL.UniformMatrix4(GetUniformLoc(prog), false, ref m_val);
		}

		public override void UnbindState(int prog) 
		{

		}

		private Matrix4  m_val;
	};

}

