using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_16_Gamma_Landscape : TutorialBase
	{
		static int NUMBER_OF_LIGHTS = 4;
		class ProgramData
		{
			public int theProgram;
			public int modelToCameraMatrixUnif;
			public int numberOfLightsUnif;
			public int cameraToClipMatrixUnif;
		};

		class UnlitProgData
		{
			public int theProgram;
			public int modelToCameraMatrixUnif;
			public int objectColorUnif;
			public int cameraToClipMatrixUnif;
		};

		static float  g_fzNear = 1.0f;
		static float  g_fzFar = 1000.0f;

		static ProgramData g_progStandard;
		static UnlitProgData g_progUnlit;

		static int g_projectionBlockIndex = 0;
		static int g_lightBlockIndex = 1;
		static int g_colorTexUnit = 0;

		ProgramData LoadProgram(String strVertexShader, String strFragmentShader)
		{
			ProgramData data = new ProgramData();
			int vertex_shader = Shader.loadShader(ShaderType.VertexShader, strVertexShader);
	        int fragment_shader = Shader.loadShader(ShaderType.FragmentShader, strFragmentShader);
	        data.theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
			
			data.modelToCameraMatrixUnif =  GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");
			data.numberOfLightsUnif = GL.GetUniformLocation(data.theProgram, "numberOfLights");
		
			int projectionBlock = GL.GetUniformBlockIndex(data.theProgram, "Projection");
			GL.UniformBlockBinding(data.theProgram, projectionBlock, g_projectionBlockIndex);
		
			int lightBlockIndex = GL.GetUniformBlockIndex(data.theProgram, "Light");
			GL.UniformBlockBinding(data.theProgram, lightBlockIndex, g_lightBlockIndex);
		
			int colorTextureUnif = GL.GetUniformLocation(data.theProgram, "diffuseColorTex");
			GL.UseProgram(data.theProgram);
			GL.Uniform1(colorTextureUnif, g_colorTexUnit);
			GL.UseProgram(0);
		
			return data;
		}

		UnlitProgData LoadUnlitProgram(String strVertexShader, String strFragmentShader)
		{
			UnlitProgData data = new UnlitProgData();
			int vertex_shader = Shader.loadShader(ShaderType.VertexShader, strVertexShader);
	        int fragment_shader = Shader.loadShader(ShaderType.FragmentShader, strFragmentShader);
	        data.theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
			
			data.modelToCameraMatrixUnif =  GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");
			data.objectColorUnif =  GL.GetUniformLocation(data.theProgram, "baseColor");
			data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");		
			return data;
		}
		
		void InitializePrograms()
		{
			g_progStandard = LoadProgram("PNT.vert", "litTexture.frag");
			g_progUnlit = LoadUnlitProgram(VertexShaders.PosTransform, FragmentShaders.ColorUniform_frag);
		}
		
		int g_projectionUniformBuffer = 0;
		int g_lightUniformBuffer = 0;
		int g_linearTexture = 0;
		// uint g_gammaTexture = 0;
		
		const int NUM_SAMPLERS = 2;
		int[] g_samplers = new int[NUM_SAMPLERS];
		
		void CreateSamplers()
		{
			GL.GenSamplers(NUM_SAMPLERS, out g_samplers[0]);
		
			for(int samplerIx = 0; samplerIx < NUM_SAMPLERS; samplerIx++)
			{
				GL.SamplerParameter(g_samplers[samplerIx], SamplerParameterName.TextureWrapS, 
				                    (int)TextureWrapMode.Repeat);
				GL.SamplerParameter(g_samplers[samplerIx], SamplerParameterName.TextureWrapT, 
				                    (int)TextureWrapMode.Repeat);
			}
		
			//Linear mipmap linear
			GL.SamplerParameter(g_samplers[0], SamplerParameterName.TextureMagFilter, 
			                    (int)TextureMagFilter.Linear);
			GL.SamplerParameter(g_samplers[0], SamplerParameterName.TextureMinFilter, 
			                    (int)TextureMinFilter.LinearMipmapLinear);
		
			//Max anisotropic
			float maxAniso = 0.0f;
			GL.GetFloat((GetPName)ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, out maxAniso);
		
			GL.SamplerParameter(g_samplers[1], SamplerParameterName.TextureMagFilter, 
			                    (int)TextureMagFilter.Linear);
			GL.SamplerParameter(g_samplers[1], SamplerParameterName.TextureMinFilter, 
			                    (int)TextureMinFilter.LinearMipmapLinear);
			GL.SamplerParameter(g_samplers[1], SamplerParameterName.TextureMaxAnisotropyExt, maxAniso); 
			// ExtTextureFilterAnisotropic.ExtTextureFilterAnisotropic
		}
		
		void LoadTextures()
		{
			try
			{
				g_linearTexture = Textures.Load("terrain_tex.png", 1);
				//std::string filename(Framework::FindFileOrThrow("terrain_tex.png"));
		
				//std::auto_ptr<glimg::ImageSet> pImageSet(glimg::loaders::dds::LoadFromFile(filename.c_str()));
		
				//glGenTextures(1, &g_linearTexture);
				//glBindTexture(GL_TEXTURE_2D, g_linearTexture);
		/*
				glimg::OpenGLPixelTransferParams xfer = glimg::GetUploadFormatType(pImageSet->GetFormat(), 0);
		
				for(int mipmapLevel = 0; mipmapLevel < pImageSet->GetMipmapCount(); mipmapLevel++)
				{
					glimg::SingleImage image = pImageSet->GetImage(mipmapLevel, 0, 0);
					glimg::Dimensions dims = image.GetDimensions();
		
					glTexImage2D(GL_TEXTURE_2D, mipmapLevel, GL_SRGB8_ALPHA8, dims.Width, dims.height, 0,
						xfer.format, xfer.type, image.GetImageData());
				}
		
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_BASE_LEVEL, 0);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAX_LEVEL, pImageSet->GetMipmapCount() - 1);
		
				glBindTexture(GL_TEXTURE_2D, 0);
				*/
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error loading textures " + ex.ToString());
			}
		}
		
	    static ViewData g_initialViewData;
	
	    private static void InitializeGInitialViewData()
	    {
	        g_initialViewData = new ViewData(new Vector3(-60.257084f, 10.947238f, 62.636356f),
	                new Quaternion(-0.972817f, -0.099283f, -0.211198f, -0.020028f),
	                30.0f,
	                0.0f);
	    }
		
  		static ViewScale g_initialViewScale;
	
	    private static void InitializeGViewScale()
	    {
	        g_initialViewScale = new ViewScale(
			5.0f, 90.0f,
			2.0f, 0.5f,
			4.0f, 1.0f,
			90.0f/250.0f);
	    }	
		
		public static  ViewProvider g_viewPole;
		
		public static ObjectPole g_objtPole;
		
		void MouseMotion(int x, int y)
	    {
	        Framework.ForwardMouseMotion(g_viewPole, x, y);
	        Framework.ForwardMouseMotion(g_objtPole, x, y);
	    }
	
	    void MouseButton(int button, int state, int x, int y)
	    {
	        Framework.ForwardMouseButton(g_viewPole, button, state, x, y);
	        Framework.ForwardMouseButton(g_objtPole, button, state, x, y);
	    }
	
	    void MouseWheel(int wheel, int direction, int x, int y)
	    {
	        Framework.ForwardMouseWheel(g_viewPole, wheel, direction, x, y);
	        Framework.ForwardMouseWheel(g_objtPole, wheel, direction, x, y);
	    }
		
		LightEnv g_pLightEnv;
		
		Mesh g_pTerrain;
		Mesh g_pSphere;
		
		//Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
		protected override void init()
		{
			InitializeGInitialViewData();
	        InitializeGViewScale();
	        g_viewPole = new ViewProvider(g_initialViewData, g_initialViewScale, MouseButtons.MB_LEFT_BTN);
			
			try
			{
				string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes";
				Stream lightEnv =  File.OpenRead(XmlFilesDirectory + @"/LightEnv.Xml");
				g_pLightEnv = new LightEnv(lightEnv);
		
				InitializePrograms();
				Stream terrain =  File.OpenRead(XmlFilesDirectory + @"/terrain.Xml");
				g_pTerrain = new Mesh(terrain);
				Stream UnitSphere =  File.OpenRead(XmlFilesDirectory + @"/UnitSphere.Xml");
				g_pSphere = new Mesh(UnitSphere);
			}
			catch(Exception ex)
			{
				MessageBox.Show ("resource load failed " + ex.ToString());
				return;
			}
		
			//glutMouseFunc(MouseButton);
			//glutMotionFunc(MouseMotion);
			//glutMouseWheelFunc(MouseWheel);
		
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
		
			//Setup our Uniform Buffers
			GL.GenBuffers(1, out g_projectionUniformBuffer);
			GL.BindBuffer(BufferTarget.UniformBuffer, g_projectionUniformBuffer);
			GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)ProjectionBlock.Size(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
		
			GL.BindBufferRange(BufferTarget.UniformBuffer, g_projectionBlockIndex, g_projectionUniformBuffer,
				(IntPtr)0, (IntPtr)ProjectionBlock.Size());
		
			GL.GenBuffers(1, out g_lightUniformBuffer);
			GL.BindBuffer(BufferTarget.UniformBuffer, g_lightUniformBuffer);
			GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)LightBlock.Size(NUMBER_OF_LIGHTS), IntPtr.Zero, BufferUsageHint.StreamDraw);
		
			GL.BindBufferRange(BufferTarget.UniformBuffer, g_lightBlockIndex, g_lightUniformBuffer,
				(IntPtr)0, (IntPtr)LightBlock.Size(NUMBER_OF_LIGHTS));
		
			GL.BindBuffer(BufferTarget.UniformBuffer, 0);
		
			LoadTextures();
			CreateSamplers();
		}
		
		int g_currSampler = 0;
		
		bool g_bDrawCameraPos = false;
		
		bool g_useGammaDisplay = true;
		
		//Called to update the display.
		//You should call glutSwapBuffers after all of your rendering to display what you rendered.
		//If you need continuous updates of the screen, call glutPostRedisplay() at the end of the function.
		 public override void display()
		{
		    if(g_pLightEnv == null)
		        return;
		
			if(g_useGammaDisplay)
				GL.Enable(EnableCap.FramebufferSrgb);
			else
				GL.Disable(EnableCap.FramebufferSrgb);
		
			g_pLightEnv.UpdateTime();
		
			Vector4 bgColor = g_pLightEnv.GetBackgroundColor();
			GL.ClearColor(bgColor.X, bgColor.Y, bgColor.Z, bgColor.W);
			GL.ClearDepth(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		
			MatrixStack modelMatrix = new MatrixStack();
			modelMatrix.ApplyMatrix(g_viewPole.CalcMatrix());
		
			LightBlock lightData = g_pLightEnv.GetLightBlock(g_viewPole.CalcMatrix());
		
			GL.BindBuffer(BufferTarget.UniformBuffer, g_lightUniformBuffer);
			GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)LightBlock.Size(NUMBER_OF_LIGHTS), 
			              lightData.ToFloat(), BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.UniformBuffer, 0);
		
			if((g_pSphere != null) && (g_pTerrain != null))
			{
				using (PushStack pushstack = new PushStack(modelMatrix))
				{
					modelMatrix.RotateX(-90.0f);
			
					GL.UseProgram(g_progStandard.theProgram);
					Matrix4 mm = modelMatrix.Top();
					GL.UniformMatrix4(g_progStandard.modelToCameraMatrixUnif, false,ref mm);
					GL.Uniform1(g_progStandard.numberOfLightsUnif, g_pLightEnv.GetNumLights());
			
					GL.ActiveTexture(TextureUnit.Texture0 + g_colorTexUnit);
					GL.BindTexture(TextureTarget.Texture2D, g_linearTexture);
					GL.BindSampler(g_colorTexUnit, g_samplers[g_currSampler]);
			
					g_pTerrain.Render("lit-tex");
			
					GL.BindSampler(g_colorTexUnit, 0);
					GL.BindTexture(TextureTarget.Texture2D, 0);
			
					GL.UseProgram(0);
				}
		
				//Render the sun
				{
					Vector3 sunlightDir = new Vector3(g_pLightEnv.GetSunlightDirection());
					modelMatrix.Translate(sunlightDir * 500.0f);
					modelMatrix.Scale(30.0f, 30.0f, 30.0f);
		
					GL.UseProgram(g_progUnlit.theProgram);
					Matrix4 mm = modelMatrix.Top();
					GL.UniformMatrix4(g_progUnlit.modelToCameraMatrixUnif, false, ref mm);
		
					Vector4 lightColor = g_pLightEnv.GetSunlightScaledIntensity(), gamma;
					GL.Uniform4(g_progUnlit.objectColorUnif, 1, lightColor.ToFloat());
					g_pSphere.Render("flat");
				}
		
				//Draw lights
				for(int light = 0; light < g_pLightEnv.GetNumPointLights(); light++)
				{
					using ( PushStack pushstack = new PushStack(modelMatrix))
					{
						modelMatrix.Translate(g_pLightEnv.GetPointLightWorldPos(light));
			
						GL.UseProgram(g_progUnlit.theProgram);
						Matrix4 mm = modelMatrix.Top();
						GL.UniformMatrix4(g_progUnlit.modelToCameraMatrixUnif, false, ref mm);
			
						Vector4 lightColor = g_pLightEnv.GetPointLightScaledIntensity(light);
						GL.Uniform4(g_progUnlit.objectColorUnif, 1, lightColor.ToFloat());
						g_pSphere.Render("flat");
					}
				}
		
				if(g_bDrawCameraPos)
				{
					//Draw lookat point.
					modelMatrix.SetIdentity();
					modelMatrix.Translate(new Vector3(0.0f, 0.0f, -g_viewPole.GetView().radius));
		
					GL.Disable(EnableCap.DepthTest);
					GL.DepthMask(false);
					GL.UseProgram(g_progUnlit.theProgram);
					Matrix4 mm = modelMatrix.Top();
					GL.UniformMatrix4(g_progUnlit.modelToCameraMatrixUnif, false, ref mm);
					GL.Uniform4(g_progUnlit.objectColorUnif, 0.25f, 0.25f, 0.25f, 1.0f);
					g_pSphere.Render("flat");
					GL.DepthMask(true);
					GL.Enable(EnableCap.DepthTest);
					GL.Uniform4(g_progUnlit.objectColorUnif, 1.0f, 1.0f, 1.0f, 1.0f);
					g_pSphere.Render("flat");
				}
		
			}
		
			//glutPostRedisplay();
			//glutSwapBuffers();
		}
		
		//Called whenever the window is resized. The new window size is given, in pixels.
		//This is an opportunity to call glViewport or glScissor to keep up with the change in size.
		void reshape (int w, int h)
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(60.0f, (w / (float)h), g_fzNear, g_fzFar);
		
			ProjectionBlock projData = new ProjectionBlock();
			projData.cameraToClipMatrix = persMatrix.Top();
		
			GL.BindBuffer(BufferTarget.UniformBuffer, g_projectionUniformBuffer);
			GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)0, (IntPtr)ProjectionBlock.Size(), projData.ToFloat());
			GL.BindBuffer(BufferTarget.UniformBuffer, 0);
		
			GL.Viewport(0, 0, w, h);
			//glutPostRedisplay();
		}
		
		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			switch (keyCode)
			{
				case Keys.Escape:
					g_pLightEnv = null;
					g_pSphere = null;
					g_pTerrain = null;
					g_pLightEnv = null;
					//glutLeaveMainLoop();
					break;
				case Keys.Space:
					g_useGammaDisplay = !g_useGammaDisplay;
					break;
				case Keys.Subtract: g_pLightEnv.RewindTime(1.0f); break;
				case Keys.Add: g_pLightEnv.FastForwardTime(1.0f); break;
				case Keys.T: g_bDrawCameraPos = !g_bDrawCameraPos; break;
				case Keys.P:g_pLightEnv.TogglePause(); break;
				case Keys.D1: g_currSampler = 0; break;
				case Keys.D2: g_currSampler = 1; break;
				case Keys.D3: g_currSampler = 2; break;
				case Keys.D4: g_currSampler = 3; break;
				case Keys.D5: g_currSampler = 4; break;
				case Keys.D6: g_currSampler = 5; break;
				case Keys.D7: g_currSampler = 6; break;
				case Keys.D8: g_currSampler = 7; break;
				case Keys.D9: g_currSampler = 8; break;
			}		
			//g_viewPole.CharPress(keyCode);
			return result.ToString();
		}

	}
}

