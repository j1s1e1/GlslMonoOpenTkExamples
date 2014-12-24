using System;
using System.IO;

namespace GlslTutorials
{
	public class SceneMesh
	{
		public SceneMesh(Stream filename)
		{
			m_pMesh = new Mesh(filename);
		}

		~SceneMesh()
		{
			m_pMesh = null;
		}

		public void Render()
		{
			m_pMesh.Render();
		}

		public Mesh GetMesh()
		{
			return m_pMesh;
		}

		private Mesh m_pMesh;
	};
}

