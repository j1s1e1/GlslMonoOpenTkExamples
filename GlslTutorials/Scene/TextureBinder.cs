using System;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class TextureBinder : UniformBinderBase
	{
		public TextureBinder()
		{
			m_texUnit = 0;
			m_texType = TextureTarget.Texture2D;
			m_texObj = 0;
			m_samplerObj = 0;
		}

		void SetTexture(int texUnit, TextureTarget texType, int texObj, int samplerObj)
		{
			m_texUnit = texUnit;
			m_texType = texType;
			m_texObj = texObj;
			m_samplerObj = samplerObj;
		}

		public override void BindState(int prog)
		{
			GL.ActiveTexture(TextureUnit.Texture0 + m_texUnit);
			GL.BindTexture(m_texType, m_texObj);
			GL.BindSampler(m_texUnit, m_samplerObj);
		}

		public override void UnbindState(int prog)
		{
			GL.ActiveTexture(TextureUnit.Texture0 + m_texUnit);
			GL.BindTexture(m_texType, 0);
			GL.BindSampler(m_texUnit, 0);
		}
				
		private int m_texUnit;
		private TextureTarget m_texType;
		private int m_texObj;
		private int m_samplerObj;
	}; 
}

