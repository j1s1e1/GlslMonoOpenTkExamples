using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_15_ManyImages : TutorialBase
	{
		public Tut_15_ManyImages ()
		{
		}

		struct ProgramData
		{
			public int theProgram;
			public int modelToCameraMatrixUnif;
			public int cameraToClipMatrixUnif;
		};

		float g_fzNear = 1.0f;
		float g_fzFar = 1000.0f;

		ProgramData g_program;

		const int g_projectionBlockIndex = 0;
		const int g_colorTexUnit = 0;

		ProgramData LoadProgram(string vertexShader, string fragmentShader)
		{
			ProgramData data = new ProgramData();
			int vertexShaderInt = Shader.compileShader(ShaderType.VertexShader, vertexShader);
			int fragmentShaderInt = Shader.compileShader(ShaderType.FragmentShader, fragmentShader);

			data.theProgram = Shader.createAndLinkProgram(vertexShaderInt, fragmentShaderInt);
			data.modelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");

			data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");

			int colorTextureUnif = GL.GetUniformLocation(data.theProgram, "colorTexture");
			GL.UseProgram(data.theProgram);
			GL.Uniform1(colorTextureUnif, g_colorTexUnit);
			GL.UseProgram(0);

			return data;
		}

		void InitializePrograms()
		{
			g_program = LoadProgram(VertexShaders.PT, FragmentShaders.Tex);
		}
			
		int g_checkerTexture = 0;
		int g_mipmapTestTexture = 0;

		const int NUM_SAMPLERS = 6;
		int[] g_samplers = new int[NUM_SAMPLERS];

		void CreateSamplers()
		{
			GL.GenSamplers(NUM_SAMPLERS, g_samplers);

			for(int samplerIx = 0; samplerIx < NUM_SAMPLERS; samplerIx++)
			{
				GL.SamplerParameter(g_samplers[samplerIx], SamplerParameterName.TextureWrapS, (int)All.Repeat);
				GL.SamplerParameter(g_samplers[samplerIx], SamplerParameterName.TextureWrapT, (int)All.Repeat);
			}

			//Nearest
			GL.SamplerParameter(g_samplers[0], SamplerParameterName.TextureMagFilter, (int)All.Nearest);
			GL.SamplerParameter(g_samplers[0], SamplerParameterName.TextureMinFilter, (int)All.Nearest);

			//Linear
			GL.SamplerParameter(g_samplers[1], SamplerParameterName.TextureMagFilter, (int)All.Linear);
			GL.SamplerParameter(g_samplers[1], SamplerParameterName.TextureMinFilter, (int)All.Linear);

			//Linear mipmap Nearest
			GL.SamplerParameter(g_samplers[2], SamplerParameterName.TextureMagFilter, (int)All.Linear);
			GL.SamplerParameter(g_samplers[2], SamplerParameterName.TextureMinFilter, (int)All.LinearMipmapNearest);

			//Linear mipmap linear
			GL.SamplerParameter(g_samplers[3], SamplerParameterName.TextureMagFilter, (int)All.Linear);
			GL.SamplerParameter(g_samplers[3], SamplerParameterName.TextureMinFilter, (int)All.LinearMipmapLinear);

			//Low anisotropic
			GL.SamplerParameter(g_samplers[4], SamplerParameterName.TextureMagFilter, (int)All.Linear);
			GL.SamplerParameter(g_samplers[4], SamplerParameterName.TextureMinFilter, (int)All.LinearMipmapLinear);
			GL.SamplerParameter(g_samplers[4], SamplerParameterName.TextureMaxAnisotropyExt, 4.0f);

			//Max anisotropic
			float maxAniso = 0.0f;
			GL.GetFloat((GetPName)ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, out maxAniso);

			GL.SamplerParameter(g_samplers[5], SamplerParameterName.TextureMagFilter, (int)All.Linear);
			GL.SamplerParameter(g_samplers[5], SamplerParameterName.TextureMinFilter, (int)All.LinearMipmapLinear);
			GL.SamplerParameter(g_samplers[5], SamplerParameterName.TextureMaxAnisotropyExt, maxAniso);
		}

		void FillWithColor(List<byte> buffer, byte red, byte green, byte blue, int width, int height)
		{
			int numTexels = width * height;
			buffer = new List<byte>();

			for (int i = 0; i < numTexels; i++)
			{
				buffer.Add(red);
				buffer.Add(green);
				buffer.Add(blue);
			}
		}

		byte[] mipmapColors = new byte[]
		{
			0xFF, 0xFF, 0x00,
			0xFF, 0x00, 0xFF,
			0x00, 0xFF, 0xFF,
			0xFF, 0x00, 0x00,
			0x00, 0xFF, 0x00,
			0x00, 0x00, 0xFF,
			0x00, 0x00, 0x00,
			0xFF, 0xFF, 0xFF,
		};

		void LoadMipmapTexture()
		{
			GL.GenTextures(1, out g_mipmapTestTexture);
			GL.BindTexture(TextureTarget.Texture2D, g_mipmapTestTexture);

			int oldAlign = 0;
			GL.GetInteger(GetPName.UnpackAlignment, out oldAlign);
			GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

			for(int mipmapLevel = 0; mipmapLevel < 8; mipmapLevel++)
			{
				int width = 128 >> mipmapLevel;
				int height = 128 >> mipmapLevel;
				List<byte> buffer = new List<byte>();

				byte[] pCurrColor = new byte[3];
				Array.Copy(mipmapColors, mipmapLevel * 3, pCurrColor, 0, 3);
				FillWithColor(buffer, pCurrColor[0], pCurrColor[1], pCurrColor[2], width, height);

				byte[] bufferArray = buffer.ToArray();
				GL.TexImage2D(TextureTarget.Texture2D, mipmapLevel, PixelInternalFormat.Rgb8, width, height, 0,
					PixelFormat.Rgb, PixelType.Byte, bufferArray);
			}

			GL.PixelStore(PixelStoreParameter.UnpackAlignment, oldAlign);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 7);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		void LoadCheckerTexture()
		{
			try
			{
				g_checkerTexture = Textures.CreateMipMapTexture("checker.png", 6);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error loading textures " + ex.ToString());
			}
		}

		Mesh g_pPlane = null;
		Mesh g_pCorridor = null;

		protected override void init ()
		{
			InitializePrograms();

			try
			{
				string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes";
				Stream corridor = File.OpenRead(XmlFilesDirectory + @"/corridor.xml");
				g_pCorridor = new Mesh(corridor);
				Stream plane = File.OpenRead(XmlFilesDirectory + @"/bigplane.xml");
				g_pPlane = new Mesh(plane);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error creating meshes " + ex.ToString());
			}

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.FrontFace(FrontFaceDirection.Cw);

			const float depthZNear = 0.0f;
			const float depthZFar = 1.0f;

			GL.Enable(EnableCap.DepthTest);
			GL.DepthMask(true);
			GL.DepthFunc(DepthFunction.Lequal);
			GL.DepthRange(depthZNear, depthZFar);
			GL.Enable(EnableCap.DepthClamp);
		
			LoadCheckerTexture();
			LoadMipmapTexture();
			CreateSamplers();
		}
			
		FrameworkTimer g_camTimer = new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, 5.0f);
		int g_currSampler = 0;

		bool g_useMipmapTexture = false;
		bool g_drawCorridor = false;

		public override void display()
		{
			GL.ClearColor(0.75f, 0.75f, 1.0f, 1.0f);
			GL.ClearDepth(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit |ClearBufferMask.DepthBufferBit);

			if(g_pPlane != null && g_pCorridor != null )
			{
				g_camTimer.Update();

				float cyclicAngle = g_camTimer.GetAlpha() * 6.28f;
				float hOffset = (float)Math.Cos(cyclicAngle) * 0.25f;
				float vOffset = (float)Math.Sin(cyclicAngle) * 0.25f;

				MatrixStack modelMatrix = new MatrixStack();

				Matrix4 worldToCamMat = Matrix4.LookAt(
					new Vector3(hOffset, 1.0f, -64.0f),
					new Vector3(hOffset, -5.0f + vOffset, -44.0f),
					new Vector3(0.0f, 1.0f, 0.0f));

				modelMatrix.ApplyMatrix(worldToCamMat);

				using ( PushStack pushstack = new PushStack(modelMatrix))
				{
					GL.UseProgram(g_program.theProgram);
					Matrix4 mm = modelMatrix.Top();
					GL.UniformMatrix4(g_program.modelToCameraMatrixUnif, false, ref mm);

					GL.ActiveTexture(TextureUnit.Texture0 + g_colorTexUnit);
					GL.BindTexture(TextureTarget.Texture2D,
						g_useMipmapTexture ? g_mipmapTestTexture : g_checkerTexture);
					GL.BindSampler(g_colorTexUnit, g_samplers[g_currSampler]);

					if(g_drawCorridor)
						g_pCorridor.Render("tex");
					else
						g_pPlane.Render("tex");

					GL.BindSampler(g_colorTexUnit, 0);
					GL.BindTexture(TextureTarget.Texture2D, 0);

					GL.UseProgram(0);
				}
			}
		}

		public override void reshape()
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(90.0f, (width / (float)height), g_fzNear, g_fzFar);

			ProjectionBlock projData = new ProjectionBlock();
			projData.cameraToClipMatrix = persMatrix.Top();

			GL.UseProgram(g_program.theProgram);
			GL.UniformMatrix4(g_program.cameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
			GL.UseProgram(0);

			GL.Viewport(0, 0, width, height);
		}

		string[] g_samplerNames = new string[NUM_SAMPLERS]
		{
			"Nearest",
			"Linear",
			"Linear with nearest mipmaps",
			"Linear with linear mipmaps",
			"Low anisotropic",
			"Max anisotropic",
		};
			
		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			switch (keyCode)
			{
			case Keys.Escape:
				g_pPlane = null;
				g_pCorridor = null;
				break;
			case Keys.Space:
				g_useMipmapTexture = !g_useMipmapTexture;
				break;
			case Keys.Y:
				g_drawCorridor = !g_drawCorridor;
				break;
			case Keys.P:
				g_camTimer.TogglePause();
				break;
			case Keys.D1:
				g_currSampler = 0;
				result.AppendLine("Sampler: " + g_samplerNames[g_currSampler].ToString());
				break;
			case Keys.D2:
				g_currSampler = 1;
				result.AppendLine("Sampler: " + g_samplerNames[g_currSampler].ToString());
				break;
			case Keys.D3:
				g_currSampler = 2;
				result.AppendLine("Sampler: " + g_samplerNames[g_currSampler].ToString());
				break;
			case Keys.D4:
				g_currSampler = 3;
				result.AppendLine("Sampler: " + g_samplerNames[g_currSampler].ToString());
				break;
			case Keys.D5:
				g_currSampler = 4;
				result.AppendLine("Sampler: " + g_samplerNames[g_currSampler].ToString());
				break;
			case Keys.D6:
				g_currSampler = 5;
				result.AppendLine("Sampler: " + g_samplerNames[g_currSampler].ToString());
				break;
			}
			return result.ToString();	
		}
	}
}

