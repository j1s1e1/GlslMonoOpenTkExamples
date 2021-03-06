﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_12_Gamma_Correction : TutorialBase
	{
		bool renderSun = true;
		public Tut_12_Gamma_Correction ()
		{
		}

		struct UnlitProgData
		{
			public int theProgram;

			public int objectColorUnif;
			public int cameraToClipMatrixUnif;
			public int modelToCameraMatrixUnif;

			void SetWindowData(Matrix4 cameraToClip)
			{
				GL.UseProgram(theProgram);
				GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref cameraToClip);
				GL.UseProgram(0);
			}
		};

		SceneProgramData[] g_Programs = new SceneProgramData[(int)LightingProgramTypes.LP_MAX_LIGHTING_PROGRAM_TYPES];
		ShadersNames[] g_ShaderFiles = new ShadersNames[(int)LightingProgramTypes.LP_MAX_LIGHTING_PROGRAM_TYPES]
		{
			new ShadersNames(VertexShaders.HDR_PCN, FragmentShaders.DiffuseSpecularGamma),
			new ShadersNames(VertexShaders.HDR_PCN, FragmentShaders.DiffuseOnlyGamma),

			new ShadersNames(VertexShaders.HDR_PCN, FragmentShaders.DiffuseSpecularMtlGamma),
			new ShadersNames(VertexShaders.HDR_PCN, FragmentShaders.DiffuseOnlyMtlGamma),
		};

		UnlitProgData g_Unlit;

		const int g_materialBlockIndex = 0;
		const int g_lightBlockIndex = 1;
		const int g_projectionBlockIndex = 2;

		UnlitProgData LoadUnlitProgram(string vertexShader, string fragmentShader)
		{
			UnlitProgData data = new UnlitProgData();
			int vertexShaderInt = Shader.compileShader(ShaderType.VertexShader, vertexShader);
			int fragmentShaderInt = Shader.compileShader(ShaderType.FragmentShader, fragmentShader);

			data.theProgram  = Shader.createAndLinkProgram(vertexShaderInt, fragmentShaderInt);
			data.modelToCameraMatrixUnif =  GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");
			data.objectColorUnif =  GL.GetUniformLocation(data.theProgram, "baseColor");

			data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");

			return data;
		}

		void InitializePrograms()
		{
			for(int iProg = 0; iProg < (int)LightingProgramTypes.LP_MAX_LIGHTING_PROGRAM_TYPES; iProg++)
			{
				g_Programs[iProg] = SceneProgramData.LoadLitProgram(g_ShaderFiles[iProg]);
			}

			g_Unlit = LoadUnlitProgram(VertexShaders.PosTransform, FragmentShaders.ColorUniform_frag);
		}

		SceneProgramData GetProgram(LightingProgramTypes eType)
		{
			return g_Programs[(int)eType];
		}

		LightManager g_lights = new LightManager();

		///////////////////////////////////////////////
		// View/Object Setup
		static ViewData g_initialViewData = new ViewData
		(
			new Vector3(-59.5f, 44.0f, 95.0f),
				new Quaternion(0.3826834f, 0.0f, 0.0f, 0.92387953f), //  (1.0f, 0.0f, 0.0f, 0.0f), // no 45 degree angle
			50.0f,
			0.0f
		);

		static ViewScale g_viewScale = new ViewScale
		(
			3.0f, 80.0f,
			4.0f, 1.0f,
			5.0f, 1.0f,
			90.0f/250.0f
		);

		ViewPole g_viewPole = new ViewPole(g_initialViewData,
			g_viewScale, MouseButtons.MB_LEFT_BTN);

		public override void MouseMotion(int x, int y)
		{
			Framework.ForwardMouseMotion<ViewProvider>(g_viewPole, x, y);
			Framework.ForwardMouseMotion(g_viewPole, x, y);
		}

		public override void MouseButton(int button, int state, int x, int y)
		{
			Framework.ForwardMouseButton<ViewProvider>(g_viewPole, button, state, x, y);
			Framework.ForwardMouseButton(g_viewPole, button, state, x, y);
		}

		void MouseWheel(int wheel, int direction, int x, int y)
		{
			Framework.ForwardMouseWheel<ViewProvider>(g_viewPole, wheel, direction, x, y);
			Framework.ForwardMouseWheel(g_viewPole, wheel, direction, x, y);
		}

		Vector4 g_skyDaylightColor = new Vector4(0.65f, 0.65f, 1.0f, 1.0f);

		void SetupHDRLighting()
		{
			SunlightValue[] values = new SunlightValue[]
			{
				new SunlightValue(0.0f/24.0f, new Vector4(0.6f, 0.6f, 0.6f, 1.0f), new Vector4(1.8f, 1.8f, 1.8f, 1.0f), g_skyDaylightColor, 3.0f),
				new SunlightValue(4.5f/24.0f, new Vector4(0.6f, 0.6f, 0.6f, 1.0f), new Vector4(1.8f, 1.8f, 1.8f, 1.0f), g_skyDaylightColor, 3.0f),
				new SunlightValue(6.5f/24.0f, new Vector4(0.225f, 0.075f, 0.075f, 1.0f), new Vector4(0.45f, 0.15f, 0.15f, 1.0f), new Vector4(0.5f, 0.1f, 0.1f, 1.0f), 1.5f),
				new SunlightValue(8.0f/24.0f, new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f), 1.0f),
				new SunlightValue(18.0f/24.0f, new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f), 1.0f),
				new SunlightValue(19.5f/24.0f, new Vector4(0.225f, 0.075f, 0.075f, 1.0f), new Vector4(0.45f, 0.15f, 0.15f, 1.0f), new Vector4(0.5f, 0.1f, 0.1f, 1.0f), 1.5f),
				new SunlightValue(20.5f/24.0f, new Vector4(0.6f, 0.6f, 0.6f, 1.0f), new Vector4(1.8f, 1.8f, 1.8f, 1.0f), g_skyDaylightColor, 3.0f),
			};

			g_lights.SetSunlightValues(values);

			g_lights.SetPointLightIntensity(0, new Vector4(0.6f, 0.6f, 0.6f, 1.0f));
			g_lights.SetPointLightIntensity(1, new Vector4(0.0f, 0.0f, 0.7f, 1.0f));
			g_lights.SetPointLightIntensity(2, new Vector4(0.7f, 0.0f, 0.0f, 1.0f));
		}

		void SetupGammaLighting()
		{
			Vector4 sunlight = new Vector4(6.5f, 6.5f, 6.5f, 1.0f);
			Vector4 brightAmbient = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);

			SunlightValue[] values = new SunlightValue[]
			{
				new SunlightValue(0.0f/24.0f, brightAmbient, sunlight, new Vector4(0.65f, 0.65f, 1.0f, 1.0f), 10.0f),
				new SunlightValue(4.5f/24.0f, brightAmbient, sunlight, g_skyDaylightColor, 10.0f),
				new SunlightValue(6.5f/24.0f, new Vector4(0.01f, 0.025f, 0.025f, 1.0f), new Vector4(2.5f, 0.2f, 0.2f, 1.0f), new Vector4(0.5f, 0.1f, 0.1f, 1.0f), 5.0f),
				new SunlightValue(8.0f/24.0f, new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f), 3.0f),
				new SunlightValue(18.0f/24.0f, new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f), 3.0f),
				new SunlightValue(19.5f/24.0f, new Vector4(0.01f, 0.025f, 0.025f, 1.0f), new Vector4(2.5f, 0.2f, 0.2f, 1.0f), new Vector4(0.5f, 0.1f, 0.1f, 1.0f), 5.0f),
				new SunlightValue(20.5f/24.0f, brightAmbient, sunlight, g_skyDaylightColor, 10.0f)
			};

			g_lights.SetSunlightValues(values);

			g_lights.SetPointLightIntensity(0, new Vector4(0.6f, 0.6f, 0.6f, 1.0f));
			g_lights.SetPointLightIntensity(1, new Vector4(0.0f, 0.0f, 0.7f, 1.0f));
			g_lights.SetPointLightIntensity(2, new Vector4(0.7f, 0.0f, 0.0f, 1.0f));
		}

		Scene g_pScene;

		protected override void init()
		{
			g_fzNear = 1.0f;
			g_fzFar = 1000.0f;
			InitializePrograms();

			try
			{
				Scene.GetProgramFromTutorial = GetProgram;
				g_pScene = new Scene();
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error creating scene " + ex.ToString());
				throw;
			}

			SetupHDRLighting();

			g_lights.CreateTimer("tetra", FrameworkTimer.Type.TT_LOOP, 2.5f);

			reshape();
			SetupDepthAndCull();
			MatrixStack.rightMultiply = false;
		}

		bool g_bDrawCameraPos = false;
		bool g_bDrawLights = true;

		bool g_isGammaCorrect = false;
		float g_gammaValue = 2.2f;

		Vector4 GammaCorrect(Vector4 input, float gamma)
		{
			Vector4 ret = new Vector4();
			ret[0] = (float)Math.Pow(input[0], 1.0f / gamma);
			ret[1] = (float)Math.Pow(input[1], 1.0f / gamma);
			ret[2] = (float)Math.Pow(input[2], 1.0f / gamma);
			ret[3] = input[3];

			return ret;
		}

		public override void display()
		{
			if(g_pScene == null)
				return;

			g_lights.UpdateTime();

			float gamma = g_isGammaCorrect ? g_gammaValue : 1.0f;

			Vector4 bkg = g_lights.GetBackgroundColor();
			GL.ClearColor(bkg[0], bkg[1], bkg[2], bkg[3]);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			MatrixStack modelMatrix = new MatrixStack();
			modelMatrix.SetMatrix(g_viewPole.CalcMatrix());

			Matrix4 worldToCamMat = modelMatrix.Top();
			LightBlock lightData = g_lights.GetLightInformationGamma(worldToCamMat);

			lightData.gamma = gamma;

			foreach (SceneProgramData spd in g_Programs)
			{
				spd.lightBlock.Update(lightData);
			}

			if(g_pScene != null)
			{
				using ( PushStack pushstack = new PushStack(modelMatrix))
				{
					g_pScene.Draw(modelMatrix, g_materialBlockIndex, g_lights.GetTimerValue("tetra"));
				}
			}

			//Render the sun
			if (renderSun)
			{
				using ( PushStack pushstack = new PushStack(modelMatrix))
				{
					Vector3 sunlightDir = new Vector3(g_lights.GetSunlightDirection());
					modelMatrix.Translate(sunlightDir * 500.0f);
					modelMatrix.Scale(30.0f, 30.0f, 30.0f);

					GL.UseProgram(g_Unlit.theProgram);
					Matrix4 mm = modelMatrix.Top();
					GL.UniformMatrix4(g_Unlit.modelToCameraMatrixUnif, false, ref mm);

					Vector4 lightColor = GammaCorrect(g_lights.GetSunlightIntensity(), gamma);
					GL.Uniform4(g_Unlit.objectColorUnif, lightColor);
					g_pScene.GetSphereMesh().Render("flat");
				}
			}

			//Render the lights
			if(g_bDrawLights)
			{
				for(int light = 0; light < g_lights.GetNumberOfPointLights(); light++)
				{
					using (PushStack pushstack = new PushStack(modelMatrix))
					{
						modelMatrix.Translate(g_lights.GetWorldLightPosition(light));

						GL.UseProgram(g_Unlit.theProgram);
						Matrix4 mm = modelMatrix.Top();
						GL.UniformMatrix4(g_Unlit.modelToCameraMatrixUnif, false, ref mm);

						Vector4 lightColor = GammaCorrect(g_lights.GetPointLightIntensity(light), gamma);
						GL.Uniform4(g_Unlit.objectColorUnif, lightColor);
						g_pScene.GetCubeMesh().Render("flat");
					}
				}
			}

			if(g_bDrawCameraPos)
			{
				using ( PushStack pushstack = new PushStack(modelMatrix))
				{
					modelMatrix.SetIdentity();
					modelMatrix.Translate(new Vector3(0.0f, 0.0f, -g_viewPole.GetView().radius));

					GL.Disable(EnableCap.DepthTest);
					GL.DepthMask(false);
					GL.UseProgram(g_Unlit.theProgram);
					Matrix4 mm = modelMatrix.Top();
					GL.UniformMatrix4(g_Unlit.modelToCameraMatrixUnif, false, ref mm);
					GL.Uniform4(g_Unlit.objectColorUnif, 0.25f, 0.25f, 0.25f, 1.0f);
					g_pScene.GetCubeMesh().Render("flat");
					GL.DepthMask(true);
					GL.Enable(EnableCap.DepthTest);
					GL.Uniform4(g_Unlit.objectColorUnif, 1.0f, 1.0f, 1.0f, 1.0f);
					g_pScene.GetCubeMesh().Render("flat");
				}
			}
		}

		public override void reshape()
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(45.0f, (width / (float)height), g_fzNear, g_fzFar);
			// added
			persMatrix.Translate(-0.5f, 0.0f, -3f);
			persMatrix.Scale(0.01f);
			// end added
			ProjectionBlock projData = new ProjectionBlock();
			projData.cameraToClipMatrix = persMatrix.Top();

			foreach(SceneProgramData spd in g_Programs)
			{
				GL.UseProgram(spd.theProgram);
				GL.UniformMatrix4(spd.cameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
				GL.UseProgram(0);
			}

			GL.UseProgram(g_Unlit.theProgram);
			GL.UniformMatrix4(g_Unlit.cameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
			GL.UseProgram(0);

			GL.Viewport(0, 0, width, height);
		}


		TimerTypes g_eTimerMode = TimerTypes.TIMER_ALL;

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			switch (keyCode)
			{
			case Keys.Escape:
				g_pScene = null;
				break;

			case Keys.P: g_lights.TogglePause(g_eTimerMode); break;
			case Keys.Subtract: g_lights.RewindTime(g_eTimerMode, 1.0f); break;
			case Keys.Add: g_lights.FastForwardTime(g_eTimerMode, 1.0f); break;
			case Keys.T: g_bDrawCameraPos = !g_bDrawCameraPos; break;
			case Keys.D1: g_eTimerMode = TimerTypes.TIMER_ALL; result.AppendLine("All"); break;
			case Keys.D2: g_eTimerMode = TimerTypes.TIMER_SUN; result.AppendLine("Sun"); break;
			case Keys.D3: g_eTimerMode = TimerTypes.TIMER_LIGHTS; result.AppendLine("Lights"); break;

			case Keys.G: SetupGammaLighting(); break;
			case Keys.H: SetupHDRLighting(); break;

			case Keys.K:
				g_isGammaCorrect = !g_isGammaCorrect;
				if(g_isGammaCorrect)
					result.AppendLine("Gamma on!");
				else
					result.AppendLine("Gamma off!");
				break;

			case Keys.Y:
				g_gammaValue += 0.1f;
				result.AppendLine("Gamma: " + g_gammaValue.ToString());
				break;
			case Keys.U:
				g_gammaValue -= 0.1f;
				if(g_gammaValue < 1.0f)
					g_gammaValue = 1.0f;
				result.AppendLine("Gamma: " + g_gammaValue.ToString());
				break;

			case Keys.Space:
				{
					float sunAlpha = g_lights.GetSunTime();
					float sunTimeHours = sunAlpha * 24.0f + 12.0f;
					sunTimeHours = sunTimeHours > 24.0f ? sunTimeHours - 24.0f : sunTimeHours;
					int sunHours = (int)(sunTimeHours);
					float sunTimeMinutes = (sunTimeHours - sunHours) * 60.0f;
					int sunMinutes = (int)(sunTimeMinutes);
					MessageBox.Show("SunHours " + sunHours.ToString() + " SunMinutes " + sunMinutes.ToString());
				}
				break;
			case Keys.S:
				if (renderSun)
				{ 
					renderSun = false;
				}
				else
				{
					renderSun = true;
				}
				break;
			}

			g_viewPole.CharPress((char)keyCode);
			return result.ToString();
		}
	}
}

