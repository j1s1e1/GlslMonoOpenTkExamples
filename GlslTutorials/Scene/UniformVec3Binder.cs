using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class UniformVec3Binder : UniformBinderBase
	{
		public UniformVec3Binder()
		{
			m_val = new Vector3();	
		}

		public void SetValue(Vector3 val)	
		{ 
			m_val = val; 
		}

		public override void BindState(int prog)
		{
			GL.Uniform3(GetUniformLoc(prog), m_val);
		}

		public override void UnbindState(int prog)
		{

		}

		private Vector3 m_val;
	};
}

