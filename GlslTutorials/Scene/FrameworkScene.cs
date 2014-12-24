using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	class FrameworkScene
	{
		public FrameworkScene(string filename)
		{
			m_pImpl = new SceneImpl(filename);
		}

		public void Render(Matrix4 cameraMatrix)
		{
			m_pImpl.Render(cameraMatrix);
		}

		public NodeRef FindNode(string nodeName)
		{
			return m_pImpl.FindNode(nodeName);
		}

		public int FindProgram(string progName)
		{
			return m_pImpl.FindProgram(progName);
		}

		public Mesh FindMesh(string meshName)
		{
			return m_pImpl.FindMesh(meshName);
		}

		private SceneImpl m_pImpl;
	};
}

