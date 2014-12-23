using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_ShaderTexture : TutorialBase
	{
		public Tut_ShaderTexture()
		{
		}
		
		private int current_texture;
		
		class ProgramData
		{
			public int theProgram;
			public int position;
			public int texCoord;
		};
		
		static ProgramData simpleTextureProgram;
		static int sampler = 0;
		static int texUnit = 0;
		static int g_colorTexUnit = 0;
		
		static float leftX = 0.2f;
		static float rightX = 0.6f;
		static float bottomY = 0.2f;
		static float topY = 0.6f;
		
		static float[] vertexData = new float[] {
	            -1.0f,  -1.0f, 0.0f, 1.0f,
	            -1.0f, 	 1.0f, 0.0f, 1.0f,
	             1.0f, 	 1.0f, 0.0f, 1.0f,
	            -1.0f,  -1.0f, 0.0f, 1.0f,
	             1.0f, 	 1.0f, 0.0f, 1.0f,
	             1.0f,  -1.0f, 0.0f, 1.0f,
			     leftX, bottomY,
			     leftX, topY,
			     rightX, topY,
			     leftX, bottomY,
			     rightX, topY,
			     rightX, bottomY,				
	    };
		
		private static int vertexCount = 6;
		private static int texCoordOffset = 4 * 4 * vertexCount;
		
		static ProgramData LoadProgram(String strVertexShader, String strFragmentShader)
		{
			ProgramData data = new ProgramData();
	        int vertex_shader = Shader.compileShader(ShaderType.VertexShader, strVertexShader);
	        int fragment_shader = Shader.compileShader(ShaderType.FragmentShader, strFragmentShader);
	        data.theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);

			data.position =  GL.GetAttribLocation(data.theProgram, "position");
			data.texCoord =  GL.GetAttribLocation(data.theProgram, "texCoord");
			
			int colorTextureUnif = GL.GetUniformLocation(data.theProgram, "diffuseColorTex");
			GL.UseProgram(data.theProgram);
			GL.Uniform1(colorTextureUnif, g_colorTexUnit);
			GL.UseProgram(0);
			
			return data;
		}
		
		static void InitializePrograms()
		{
			simpleTextureProgram = LoadProgram(VertexShaders.SimpleTexture, FragmentShaders.SimpleTexture);
		}
		
		void CreateSampler()
		{
			GL.GenSamplers(1, out sampler);
			GL.SamplerParameter(sampler, SamplerParameterName.TextureMagFilter,  (int)TextureMagFilter.Nearest);
			GL.SamplerParameter(sampler, SamplerParameterName.TextureMinFilter,  (int)TextureMinFilter.Nearest);
			GL.SamplerParameter(sampler, SamplerParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdgeSgis);
			
		}
		
		protected override void init ()
		{
			InitializePrograms();
			InitializeVertexBuffer(vertexData);
			CreateSampler();
		    GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
		    current_texture = Textures.Load("Mars_MGS_colorhillshade_mola_1024.jpg", 1);
		    GL.Enable(EnableCap.Texture2D);
			
			SetupDepthAndCull();
			
		}
		
	 	public override void display()
		{
			
			ClearDisplay();		
			
			GL.UseProgram(simpleTextureProgram.theProgram);
			
		    GL.BindTexture(TextureTarget.Texture2D, current_texture);
			GL.BindSampler(texUnit, sampler);
			
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
	        GL.EnableVertexAttribArray(simpleTextureProgram.position);
	        GL.EnableVertexAttribArray(simpleTextureProgram.texCoord);
	        GL.VertexAttribPointer(simpleTextureProgram.position, POSITION_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float, 
				                       false, POSITION_STRIDE, 0);
	        GL.VertexAttribPointer(simpleTextureProgram.texCoord, TEXTURE_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float, 
				                       false, TEXTURE_STRIDE, texCoordOffset);
	        GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
	
	        GL.DisableVertexAttribArray(simpleTextureProgram.position);
	        GL.DisableVertexAttribArray(simpleTextureProgram.texCoord);
			
			GL.BindSampler(texUnit, 0);
		    GL.BindTexture(TextureTarget.Texture2D, 0);
			
			GL.UseProgram(0);
		}
	}
}

