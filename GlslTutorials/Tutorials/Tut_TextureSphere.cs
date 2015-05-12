using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_TextureSphere : TutorialBase
	{
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

		bool rotatePlanet = false;
		float rotateSpeed = 1f;
		Vector3 axis = new Vector3(0f, 1f, 0f);
		
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
			GL.SamplerParameter(sampler, SamplerParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
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
			textureCoordinates = new float[vertexCount * TEXTURE_DATA_SIZE_IN_ELEMENTS];
			for (int vertex = 0; vertex < vertexCount; vertex++)
			{
				float x = vertexData[vertex * 3];
				float y = vertexData[vertex * 3 + 1];
				float z = vertexData[vertex * 3 + 2];
				float longitude = (float)Math.Atan2(y, x);
				float latitude = (float)Math.Asin(z);	
				
				textureCoordinates[vertex * 2] = (float)((longitude + Math.PI) / (Math.PI * 2));
				textureCoordinates[vertex * 2 + 1] = (float)((latitude + Math.PI/2) / Math.PI);
				if (textureCoordinates[vertex * 2] < 0) textureCoordinates[vertex * 2] = 0f;
				if (textureCoordinates[vertex * 2] > 1) textureCoordinates[vertex * 2] = 1f;
				if (textureCoordinates[vertex * 2 + 1] < 0) textureCoordinates[vertex * 2] = 0f;
				if (textureCoordinates[vertex * 2 + 1] > 1) textureCoordinates[vertex * 2] = 1f;
			}
			// center all x coordinates in original 100%
			for (int vertex = 0; vertex < vertexCount; vertex++)
			{
				textureCoordinates[vertex * 2] = 1f/12f + 10f/12f * textureCoordinates[vertex * 2];
			}
			// Check each set of 3 coordinates for crossing edges.  Move some if necessary
			for (int vertex = 0; vertex < vertexCount; vertex = vertex + 3)
			{
				if (textureCoordinates[vertex * 2] < 0.35f)
				{
					if (textureCoordinates[(vertex + 1) * 2] > 0.65f)
					{
						textureCoordinates[(vertex + 1) * 2] = textureCoordinates[(vertex + 1) * 2] - 10f/12f;
					}
					if (textureCoordinates[(vertex + 2) * 2] > 0.65f)
					{
						textureCoordinates[(vertex + 2) * 2] = textureCoordinates[(vertex + 2) * 2] - 10f/12f;
					}
				}
				if (textureCoordinates[vertex * 2] > 0.65f)
				{
					if (textureCoordinates[(vertex + 1) * 2] < 0.35f)
					{
						textureCoordinates[(vertex + 1) * 2] = textureCoordinates[(vertex + 1) * 2] + 10f/12f;
					}
					if (textureCoordinates[(vertex + 2) * 2] < 0.35f)
					{
						textureCoordinates[(vertex + 2) * 2] = textureCoordinates[(vertex + 2) * 2] + 10f/12f;
					}				
				}
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
			vertexData = Icosahedron.GetDividedTriangles(2);
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
			current_texture = Textures.Load("Venus_Magellan_C3-MDIR_ClrTopo_Global_Mosaic_1024.jpg", 1, false, false, true);
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

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			if (displayOptions)
			{
				SetDisplayOptions(keyCode);
			}
			else {
				switch (keyCode) {
				case Keys.Enter:
					displayOptions = true;
					break;
				case Keys.D1:
					Shape.MoveWorld(new Vector3(0f, 0f, 1f));
					break;
				case Keys.D2:
					Shape.MoveWorld(new Vector3(0f, 0f, -1f));
					break;
				case Keys.D3:
					Shape.MoveWorld(new Vector3(0f, 0f, 10f));
					break;
				case Keys.D4:
					Shape.MoveWorld(new Vector3(0f, 0f, -10f));
					result.AppendLine("RotateShape 5X");
					break;
				case Keys.D5:
					//planet.RotateAboutCenter(Vector3.UnitY, 5f);
					result.AppendLine("RotateShape 5Y");
					break;
				case Keys.D6:
					//planet.RotateAboutCenter(Vector3.UnitZ, 5f);
					result.AppendLine("RotateShape 5Z");
					break;
				case Keys.NumPad6:
					Shape.MoveWorld(new Vector3(10f, 0.0f, 0.0f));
					break;
				case Keys.NumPad4:
					Shape.MoveWorld(new Vector3(-10f, 0.0f, 0.0f));
					break;
				case Keys.NumPad8:
					Shape.MoveWorld(new Vector3(0.0f, 10f, 0.0f));
					break;
				case Keys.NumPad2:
					Shape.MoveWorld(new Vector3(0.0f, -10f, 0.0f));
					break;
				case Keys.NumPad7:
					Shape.MoveWorld(new Vector3(0.0f, 0.0f, 10f));
					break;
				case Keys.NumPad3:
					Shape.MoveWorld(new Vector3(0.0f, 0.0f, -10f));
					break;
				case Keys.A:
					break;
				case Keys.B:
					break;
				case Keys.C:
					break;
				case Keys.D:
					break;
				case Keys.I:
					result.AppendLine("worldToCamera");
					result.AppendLine(Shape.worldToCamera.ToString());
					result.AppendLine("modelToWorld");
					break;
				case Keys.N:
					break;
				case Keys.P:
					break;
				case Keys.R:
					break;
				case Keys.V:
					break;
				case Keys.F:
					break;
				case Keys.Z:
					break;
				}
			}
			return result.ToString();
		}

	}
}

