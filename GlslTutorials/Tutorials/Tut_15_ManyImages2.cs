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
	public class Tut_15_ManyImages2 : TutorialBase
	{
		const int g_colorTexUnit = 0;

		TextureSphere ts = new TextureSphere(0.2f);
		TextureElement te;

		bool useManyImageProgram = false;
		int manyImageProgram;
		int textureSphereProgram;
		int textureElementProgram;
		bool drawTextureSphere = false;

		int textureElementTexture = 0;

		void LoadProgram(string vertexShader, string fragmentShader)
		{
			manyImageProgram = Programs.AddProgram(vertexShader, fragmentShader);

			Programs.SetUniformTexture(manyImageProgram, g_colorTexUnit);
		}

		void InitializePrograms()
		{
			LoadProgram(VertexShaders.PT, FragmentShaders.Tex);
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

		void LoadMipmapTexture()
		{
			try
			{
				g_mipmapTestTexture = Textures.CreateMipMapTexture("checker.png", 6);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error loading textures " + ex.ToString());
			}
		}

		void LoadCheckerTexture()
		{
			try
			{
				g_checkerTexture = Textures.Load("checker.png");
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
			g_fzNear = 1.0f;
			g_fzFar = 1000.0f;
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

			SetupDepthAndCull();
		
			LoadCheckerTexture();
			LoadMipmapTexture();
			CreateSamplers();

			textureSphereProgram = ts.GetProgram();
			ts.SetProgram(manyImageProgram);

			te = new TextureElement("wood4_rotate.png");
			textureElementProgram = te.GetProgram();
			te.SetProgram(manyImageProgram);
			textureElementTexture = te.GetTexture();
			te.Scale(new Vector3(0.4f, 0.4f, 10f));
			te.RotateShape(Vector3.UnitX, 45f);

		}
			
		FrameworkTimer g_camTimer = new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, 5.0f);
		int g_currSampler = 0;

		bool g_useMipmapTexture = true;
		bool g_drawCorridor = true;

		public override void display()
		{
			reshape();
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
					GL.UseProgram(Programs.GetProgram(manyImageProgram));
					Matrix4 mm = modelMatrix.Top();
					GL.UniformMatrix4(Programs.GetModelToCameraMatrixUnif(manyImageProgram), false, ref mm);

					GL.ActiveTexture(TextureUnit.Texture0 + g_colorTexUnit);
					GL.BindTexture(TextureTarget.Texture2D,
						g_useMipmapTexture ? g_mipmapTestTexture : g_checkerTexture);
					GL.BindSampler(g_colorTexUnit, g_samplers[g_currSampler]);

					if(g_drawCorridor)
						g_pCorridor.Render("tex");
					else
						g_pPlane.Render("tex");

					if (drawTextureSphere) ts.Draw();
					ts.RotateShapeAboutAxis(1f);
					te.Draw();

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

			GL.UseProgram(Programs.GetProgram(manyImageProgram));
			GL.UniformMatrix4(Programs.GetCameraToClipMatrixUniform(manyImageProgram), false, ref projData.cameraToClipMatrix);
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
				if (g_useMipmapTexture)
					result.AppendLine("Using mipmap texture");
				else
					result.AppendLine("Not using mipmap texture");
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
			case Keys.T:
				if (useManyImageProgram)
				{
					useManyImageProgram = false;
					ts.SetProgram(textureSphereProgram);
					ts.SetTexture(1);
					ts.SetLightScale(0.1f);

					te.SetProgram(textureElementProgram);
					te.SetTexture(textureElementTexture);

					result.AppendLine("useManyImageProgram = false");
				}
				else
				{
					useManyImageProgram = true;
					ts.SetProgram(manyImageProgram);
					ts.SetTexture(2);
					te.SetTexture(2);
					result.AppendLine("useManyImageProgram = true");
				}
				break;
			case Keys.D:
				if (drawTextureSphere)
				{
					drawTextureSphere = false;
				}
				else
				{
					drawTextureSphere = true;
				}
				break;
			}
			return result.ToString();	
		}
	}
}

