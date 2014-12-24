using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class SceneTexture
	{
		public SceneTexture(Stream filename, uint creationFlags)
		{
			/* FIXME
			string pathname(Framework::FindFileOrThrow(filename));

			std::auto_ptr<glimg::ImageSet> pImageSet;
			string ext = GetExtension(pathname);
			if(ext == "dds")
			{
				pImageSet.reset(glimg::loaders::dds::LoadFromFile(pathname.c_str()));
			}
			else
			{
				pImageSet.reset(glimg::loaders::stb::LoadFromFile(pathname.c_str()));
			}

			m_texObj = glimg::CreateTexture(pImageSet.get(), creationFlags);
			m_texType = glimg::GetTextureType(pImageSet.get(), creationFlags);
			*/
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

