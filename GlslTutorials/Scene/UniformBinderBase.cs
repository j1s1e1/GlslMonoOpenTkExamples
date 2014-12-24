using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class UniformBinderBase : StateBinder
	{
		public UniformBinderBase()
		{

		}

		public void AssociateWithProgram(int prog, string unifName)
		{
			m_progUnifLoc[prog] = GL.GetUniformLocation(prog, unifName);
		}

		protected int GetUniformLoc(int prog)
		{
			return m_progUnifLoc[prog];
		}

		private Dictionary<int, int> m_progUnifLoc = new Dictionary<int, int>();
	};
}

