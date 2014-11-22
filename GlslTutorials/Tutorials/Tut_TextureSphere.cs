using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_TextureSphere : TutorialBase
	{
		public Tut_TextureSphere()
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
		
		static float[] vertexData;
		static float[] textureCoordinates;
		static float[] vertexDataWithTextureCoordinates;
		
		private static int vertexCount;
		private static int texCoordOffset;
		
		static ProgramData LoadProgram(String strVertexShader, String strFragmentShader)
		{
			ProgramData data = new ProgramData();
	        int vertex_shader = Shader.loadShader(ShaderType.VertexShader, strVertexShader);
	        int fragment_shader = Shader.loadShader(ShaderType.FragmentShader, strFragmentShader);
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
		
		private void ScaleCoordinates(float scale, float zOffset)
		{
			for (int i = 0; i < vertexData.Length; i++)
			{
				vertexData[i] = vertexData[i] * scale;
				if (i % 3 == 2) vertexData[i] = vertexData[i]  + zOffset;
			}
		}
		
		private void AddFourthCoordinate()
		{
			float[] newVertexData = new float[vertexData.Length * 4/3];
			for (int i = 0; i < vertexData.Length / 3; i++)
			{
				Array.Copy(vertexData, i*3, newVertexData, i * 4, 3);
				newVertexData[i*4+3] = 1f;
			}
			vertexData = newVertexData;
		}
		
		private void CalculateTextureCoordinates()
		{
			float minx = 0f;
			float miny = 0f;
			float maxx = 1f;
			float maxy = 1f;
			textureCoordinates = new float[vertexCount * TEXTURE_DATA_SIZE_IN_ELEMENTS];
			for (int vertex = 0; vertex < vertexCount; vertex++)
			{
				float x = vertexData[vertex * 3];
				float y = vertexData[vertex * 3 + 1];
				float z = vertexData[vertex * 3 + 2];
				float longitude = (float)Math.Atan2(y, x);
				float lattitude = (float)Math.Asin(z);	
				//lattitude = lattitude + (float)Math.PI/4;
				//longitude = longitude + 3 * (float)Math.PI/4;
				
				textureCoordinates[vertex * 2] = (float)((longitude + Math.PI) / (Math.PI * 2));
				textureCoordinates[vertex * 2 + 1] = (float)((lattitude + Math.PI/2) / Math.PI);
				if (textureCoordinates[vertex * 2] < minx) minx = textureCoordinates[vertex * 2];
				if (textureCoordinates[vertex * 2] > maxx) maxx = textureCoordinates[vertex * 2];
				if (textureCoordinates[vertex * 2 + 1] < miny) miny = textureCoordinates[vertex * 2];
				if (textureCoordinates[vertex * 2 + 1] > maxy) maxy = textureCoordinates[vertex * 2];
				if (textureCoordinates[vertex * 2] < 0) textureCoordinates[vertex * 2] = 0f;
				if (textureCoordinates[vertex * 2] > 1) textureCoordinates[vertex * 2] = 1f;
				if (textureCoordinates[vertex * 2 + 1] < 0) textureCoordinates[vertex * 2] = 0f;
				if (textureCoordinates[vertex * 2 + 1] > 1) textureCoordinates[vertex * 2] = 1f;
			}
		}
		
		private void AddTextureCoordinates()
		{
			vertexDataWithTextureCoordinates = new float[vertexData.Length + textureCoordinates.Length];
			Array.Copy(vertexData, 0, vertexDataWithTextureCoordinates, 0, vertexData.Length);
			Array.Copy(textureCoordinates, 0, vertexDataWithTextureCoordinates, vertexData.Length, 
			           textureCoordinates.Length);
		}
		
		protected override void init ()
		{
			vertexData = Icosahedron.GetDividedTriangles(4);
			vertexCount = vertexData.Length / 3;  // Icosahedron class only uses 3 floats per vertex
			CalculateTextureCoordinates();
			ScaleCoordinates(0.8f, 0.5f);
			AddFourthCoordinate();
			vertexCount = vertexData.Length / COORDS_PER_VERTEX;
			texCoordOffset = 4 * 4 * vertexCount;
			AddTextureCoordinates();
			InitializePrograms();
			InitializeVertexBuffer(vertexDataWithTextureCoordinates);
			CreateSampler();
		    GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
		    //current_texture = Textures.Load("Mars_MGS_colorhillshade_mola_1024.jpg", 1);
			current_texture = Textures.Load("Venus_Magellan_C3-MDIR_ClrTopo_Global_Mosaic_1024.jpg", 1);
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

