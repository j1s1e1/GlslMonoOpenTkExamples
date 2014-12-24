using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class SceneTexture
	{
		public SceneTexture(string fileName, uint creationFlags)
		{
			m_texObj = Textures.Load(fileName, 1);  //FIXME add options
			m_texType = TextureTarget.Texture2D;
		}

		~SceneTexture()
		{
			GL.DeleteTextures(1, ref m_texObj);
		}

		public int GetTexture() 
		{
			return m_texObj;
		}
		public TextureTarget GetTextureType() 
		{
			return m_texType;
		}

		private int m_texObj;
		private TextureTarget m_texType;
	};
}

