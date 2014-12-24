using System;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class SceneProgram
	{
		public SceneProgram(int programObj, int matrixLoc, int normalMatLoc)
		{
			m_programObj = programObj;
			m_matrixLoc = matrixLoc;
			m_normalMatLoc = normalMatLoc;
		}

		~SceneProgram()
		{
			GL.DeleteProgram(m_programObj);
		}

		public int GetMatrixLoc()
		{
			return m_matrixLoc;
		}

		public int GetNormalMatLoc() 
		{
			return m_normalMatLoc;
		}

		public void UseProgram()
		{
			GL.UseProgram(m_programObj);
		}

		public int GetProgram() 
		{
			return m_programObj;
		}
			
		private int m_programObj;
		private int m_matrixLoc;
		private int m_normalMatLoc;
	};
}

