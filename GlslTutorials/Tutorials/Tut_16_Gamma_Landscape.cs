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
			public LightBlock lightBlock;
		};

		class UnlitProgData
		{
			public int theProgram;
			public int modelToCameraMatrixUnif;
			public int objectColorUnif;
			public int cameraToClipMatrixUnif;
		};

		static ProgramData g_progStandard;
		static UnlitProgData g_progUnlit;

		static int g_colorTexUnit = 0;

		ProgramData LoadProgram(String strVertexShader, String strFragmentShader)
		{
			ProgramData data = new ProgramData();
			int vertex_shader = Shader.compileShader(ShaderType.VertexShader, strVertexShader);
	        int fragment_shader = Shader.compileShader(ShaderType.FragmentShader, strFragmentShader);
	        data.theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
			
			data.modelToCameraMatrixUnif =  GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");
			data.numberOfLightsUnif = GL.GetUniformLocation(data.theProgram, "numberOfLights");
		
			//int projectionBlock = GL.GetUniformBlockIndex(data.theProgram, "Projection");
			//GL.UniformBlockBinding(data.theProgram, projectionBlock, g_projectionBlockIndex);
			data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");	
		
			data.lightBlock = new LightBlock(NUMBER_OF_LIGHTS);
			data.lightBlock.SetUniforms(data.theProgram);
		
			int colorTextureUnif = GL.GetUniformLocation(data.theProgram, "diffuseColorTex");
			GL.UseProgram(data.theProgram);
			GL.Uniform1(colorTextureUnif, g_colorTexUnit);
			GL.UseProgram(0);
		
			return data;
		}

		UnlitProgData LoadUnlitProgram(String strVertexShader, String strFragmentShader)
		{
			UnlitProgData data = new UnlitProgData();
			int vertex_shader = Shader.compileShader(ShaderType.VertexShader, strVertexShader);
	        int fragment_shader = Shader.compileShader(ShaderType.FragmentShader, strFragmentShader);
	        data.theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
			
			data.modelToCameraMatrixUnif =  GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");
			data.objectColorUnif =  GL.GetUniformLocation(data.theProgram, "baseColor");
			data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");		
			return data;
		}
		
		void InitializePrograms()
		{
			g_progStandard = LoadProgram(VertexShaders.PNT, FragmentShaders.litTexture);
			g_progUnlit = LoadUnlitProgram(VertexShaders.PosTransform, FragmentShaders.ColorUniform_frag);
		}

		int g_linearTexture = 0;
		
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
				g_linearTexture = Textures.CreateMipMapTexture("terrain_tex.png", 4);
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
		
		public static  ViewPole g_viewPole;
		
		public override void MouseMotion(int x, int y)
	    {
	        Framework.ForwardMouseMotion(g_viewPole, x, y);
	    }
	
		public override void MouseButton(int button, int state, int x, int y)
	    {
	        Framework.ForwardMouseButton(g_viewPole, button, state, x, y);
	    }
	
	    void MouseWheel(int wheel, int direction, int x, int y)
	    {
	        Framework.ForwardMouseWheel(g_viewPole, wheel, direction, x, y);
	    }
		
		LightEnv g_pLightEnv;
		
		Mesh g_pTerrain;
		Mesh g_pSphere;
		
		//Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
		protected override void init()
		{
			g_fzNear = 1.0f;
			g_fzFar = 1000.0f;
			InitializeGInitialViewData();
	        InitializeGViewScale();
			g_viewPole = new ViewPole(g_initialViewData, g_initialViewScale, MouseButtons.MB_LEFT_BTN);
			
			try
			{
				string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes";
				Stream lightEnv =  File.OpenRead(XmlFilesDirectory + @"/lightenv.xml");
				g_pLightEnv = new LightEnv(lightEnv);
		
				InitializePrograms();
				Stream terrain =  File.OpenRead(XmlFilesDirectory + @"/terrain.xml");
				g_pTerrain = new Mesh(terrain);
				Stream UnitSphere =  File.OpenRead(XmlFilesDirectory + @"/unitsphere.xml");
				g_pSphere = new Mesh(UnitSphere);
			}
			catch(Exception ex)
			{
				MessageBox.Show ("resource load failed " + ex.ToString());
				return;
			}
		
			SetupDepthAndCull();
		
			LoadTextures();
			CreateSamplers();
			MatrixStack.rightMultiply = false;
			reshape();
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
		
			g_progStandard.lightBlock.Update(lightData);
		
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
		
					Vector4 lightColor = g_pLightEnv.GetSunlightScaledIntensity();
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
		public override void reshape()
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(60.0f, (width / (float)height), g_fzNear, g_fzFar);

			ProjectionBlock projData = new ProjectionBlock();
			projData.cameraToClipMatrix = persMatrix.Top();
			
			Matrix4 cm = projData.cameraToClipMatrix;
			GL.UseProgram(g_progStandard.theProgram);
			GL.UniformMatrix4(g_progStandard.cameraToClipMatrixUnif, false, ref cm);
			GL.UseProgram(g_progUnlit.theProgram);
			GL.UniformMatrix4(g_progUnlit.cameraToClipMatrixUnif, false, ref cm);
			GL.UseProgram(0);
		
			GL.Viewport(0, 0, width, height);
		}
		
		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
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
					if (g_useGammaDisplay)
					{
						result.AppendLine("g_useGammaDisplay");
					}
					break;
				//case Keys.Subtract: g_pLightEnv.RewindTime(1.0f); break;
				//case Keys.Add: g_pLightEnv.FastForwardTime(1.0f); break;
				case Keys.T: g_bDrawCameraPos = !g_bDrawCameraPos; break;
				case Keys.P:g_pLightEnv.TogglePause(); break;
				case Keys.D1: g_currSampler = 0; break;
				case Keys.D2: g_currSampler = 1; break;
			case Keys.Subtract:
				MouseWheel(1, 0, 10, 10);
				break;
			case Keys.Add:
				MouseWheel(1, 1, 10, 10);
				break;
			}		
			g_viewPole.CharPress((char)keyCode);
			return result.ToString();
		}

	}
}

