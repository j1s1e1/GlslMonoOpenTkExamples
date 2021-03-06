﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_17_Projected_Light : TutorialBase
	{
		Vector3 translateVector = new Vector3(0f, 0f, 0f);
		float scaleFactor = 0.02f;
		static int NUMBER_OF_LIGHTS = 2;
		const int g_projectionBlockIndex = 0;
		const int g_lightBlockIndex = 1;
		const int g_lightProjTexUnit = 3;


		const int NUM_SAMPLERS = 2;
		int[] g_samplers = new int[NUM_SAMPLERS];

		void CreateSamplers()
		{
			GL.GenSamplers(NUM_SAMPLERS, out g_samplers[0]);

			GL.GenSamplers(NUM_SAMPLERS, out g_samplers[0]);

			for(int samplerIx = 0; samplerIx < NUM_SAMPLERS; samplerIx++)
			{
				GL.SamplerParameter(g_samplers[samplerIx], SamplerParameterName.TextureMagFilter, (int)All.Linear);
				GL.SamplerParameter(g_samplers[samplerIx], SamplerParameterName.TextureMinFilter, (int)All.Linear);
			}

			GL.SamplerParameter(g_samplers[0], SamplerParameterName.TextureWrapS, (int)All.ClampToEdge);
			GL.SamplerParameter(g_samplers[0], SamplerParameterName.TextureWrapT, (int)All.ClampToEdge);

			GL.SamplerParameter(g_samplers[1], SamplerParameterName.TextureWrapS, (int)All.ClampToBorder);
			GL.SamplerParameter(g_samplers[1], SamplerParameterName.TextureWrapT, (int)All.ClampToBorder);


			//float[] color = new float[]{0.0f, 0.0f, 0.0f, 1.0f};
			// FIXME GL.SamplerParameter(g_samplers[1], SamplerParameterName.bor  GL_TEXTURE_BORDER_COLOR, color);
		}

		struct TexDef 
		{ 
			public string fileName; 
			public string name; 
			public TexDef(string fn, string n)
			{
				fileName = fn;
				name = n;
			}

		};

		static TexDef[] g_texDefs = new TexDef[]
		{
			new TexDef("flashlight.png", "Flashlight"),
			new TexDef("pointsoflight.png", "Multiple Point Lights"),
			new TexDef("bands.png", "Light Bands")
		};


		int[] g_lightTextures = new int[g_texDefs.Length];
		int NUM_LIGHT_TEXTURES = g_texDefs.Length;
		int g_currTextureIndex = 0;

		void LoadTextures()
		{
			try
			{
				for(int tex = 0; tex < NUM_LIGHT_TEXTURES; ++tex)
				{
					g_lightTextures[tex] = Textures.Load(g_texDefs[tex].fileName, 1);
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error loading textures " + ex.ToString());
			}
		}

		static ViewData g_initialView = new ViewData
		(
			new Vector3(0.0f, 0.0f, 10.0f),
			new Quaternion(0.16043f, -0.376867f, -0.0664516f, 0.909845f),
			25.0f,
			0.0f
		);

		static ViewScale g_initialViewScale = new ViewScale
		(
			5.0f, 70.0f,
			2.0f, 0.5f,
			2.0f, 0.5f,
			90.0f/250.0f
		);


		static ViewData g_initLightView = new ViewData
		(
			new Vector3(0.0f, 0.0f, 20.0f),
			new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
			5.0f,
			0.0f
		);

		static ViewScale g_initLightViewScale = new ViewScale
		(
				0.05f, 10.0f,
				0.1f, 0.05f,
				4.0f, 1.0f,
				90.0f/250.0f
		);

		ViewPole g_viewPole = new ViewPole(g_initialView, g_initialViewScale, MouseButtons.MB_LEFT_BTN);
		ViewPole g_lightViewPole = new ViewPole(g_initLightView, g_initLightViewScale, MouseButtons.MB_RIGHT_BTN, true);

		public override void MouseMotion(int x, int y)
		{
			Framework.ForwardMouseMotion(g_viewPole, x, y);
			Framework.ForwardMouseMotion(g_lightViewPole, x, y);
		}

		public override void MouseButton(int button, int state, int x, int y)
		{
			Framework.ForwardMouseButton(g_viewPole, button, state, x, y);
			Framework.ForwardMouseButton(g_lightViewPole, button, state, x, y);
		}

		void MouseWheel(int wheel, int direction, int x, int y)
		{
			Framework.ForwardMouseWheel(g_viewPole, wheel, direction, x, y);
		}

		FrameworkScene g_pScene;
		List<NodeRef> g_nodes;
		FrameworkTimer g_timer = new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, 10.0f);

		UniformIntBinder g_lightNumBinder;
		UniformMat4Binder g_lightProjMatBinder;
		UniformVec3Binder g_camLightPosBinder;

		Quaternion g_spinBarOrient;

		int g_unlitModelToCameraMatrixUnif;
		int g_unlitCameraToClipMatrixUnif;
		int g_unlitObjectColorUnif;
		int g_unlitProg;
		Mesh g_pSphereMesh;

		int g_coloredModelToCameraMatrixUnif;
		int g_coloredCameraToClipMatrixUnif;
		int g_colroedProg;

		int g_projLightCameraToClipMatrixUnif;
		int g_projLightProg;

		Mesh g_pAxesMesh;

		void LoadAndSetupScene()
		{
			FrameworkScene pScene = new FrameworkScene("proj2d_scene.xml");
			List<NodeRef> nodes = new List<NodeRef>();
			nodes.Add(pScene.FindNode("cube"));
			nodes.Add(pScene.FindNode("rightBar"));
			nodes.Add(pScene.FindNode("leaningBar"));
			nodes.Add(pScene.FindNode("spinBar"));
			nodes.Add(pScene.FindNode("diorama"));
			nodes.Add(pScene.FindNode("floor"));

			g_lightNumBinder = new UniformIntBinder();
			AssociateUniformWithNodes(nodes, g_lightNumBinder, "numberOfLights");
			SetStateBinderWithNodes(nodes, g_lightNumBinder);
			g_lightProjMatBinder = new UniformMat4Binder();
			AssociateUniformWithNodes(nodes, g_lightProjMatBinder, "cameraToLightProjMatrix");
			SetStateBinderWithNodes(nodes, g_lightProjMatBinder);
			g_camLightPosBinder = new UniformVec3Binder();
			AssociateUniformWithNodes(nodes, g_camLightPosBinder, "cameraSpaceProjLightPos");
			SetStateBinderWithNodes(nodes, g_camLightPosBinder);

			int unlit = pScene.FindProgram("p_unlit");
			Mesh pSphereMesh = pScene.FindMesh("m_sphere");

			int colored = pScene.FindProgram("p_colored");

			int projLight = pScene.FindProgram("p_proj");

			Mesh pAxesMesh = pScene.FindMesh("m_axes");

			//No more things that can throw.
			g_spinBarOrient = nodes[3].NodeGetOrient();

			g_unlitProg = unlit;
			GL.UseProgram(unlit);
			g_unlitModelToCameraMatrixUnif = GL.GetUniformLocation(unlit, "modelToCameraMatrix");
			g_unlitCameraToClipMatrixUnif  = GL.GetUniformLocation(unlit, "cameraToClipMatrix");
			g_unlitObjectColorUnif = GL.GetUniformLocation(unlit, "objectColor");
			GL.UseProgram(0);

			g_colroedProg = colored;
			GL.UseProgram(colored);
			g_coloredCameraToClipMatrixUnif= GL.GetUniformLocation(colored, "cameraToClipMatrix");
			g_coloredModelToCameraMatrixUnif = GL.GetUniformLocation(colored, "modelToCameraMatrix");
			GL.UseProgram(0);

			g_projLightProg = projLight;
			GL.UseProgram(projLight);
			g_projLightCameraToClipMatrixUnif= GL.GetUniformLocation(projLight, "cameraToClipMatrix");
			GL.UseProgram(0);

			g_nodes = nodes;

			g_pSphereMesh = pSphereMesh;

			g_pScene = pScene;

			g_pAxesMesh = pAxesMesh;
		}

		const int MAX_NUMBER_OF_LIGHTS = 4;

		protected override void init()
		{
			g_fzNear = 1.0f;
			g_fzFar = 1000.0f;
			SetupDepthAndCull();

			GL.Enable(EnableCap.FramebufferSrgb);
			GL.Enable(EnableCap.Texture2D);
			MatrixStack.rightMultiply = false;

			CreateSamplers();
			LoadTextures();

			try
			{
				LoadAndSetupScene();
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error loading scene " + ex.ToString());
			}
		}
			
		int g_currSampler = 0;

		float[] g_lightFOVs = new float[]{ 10.0f, 20.0f, 45.0f, 75.0f, 90.0f, 120.0f, 150.0f, 170.0f };
		int g_currFOVIndex = 3;

		bool g_bDrawCameraPos = true;
		bool g_bShowOtherLights = true;

		int g_displayWidth = 500;
		int g_displayHeight = 500;

		void BuildLights(Matrix4 camMatrix )
		{
			LightBlock lightData = new LightBlock(NUMBER_OF_LIGHTS);
			lightData.ambientIntensity = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
			lightData.lightAttenuation = 1.0f / (30.0f * 30.0f);
			lightData.maxIntensity = 2.0f;
			lightData.lights[0].lightIntensity = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
			lightData.lights[0].cameraSpaceLightPos = 
				Vector4.Transform(Vector4.Normalize(new Vector4(-0.2f, 0.5f, 0.5f, 0.0f)), camMatrix);
			lightData.lights[1].lightIntensity = new Vector4(3.5f, 6.5f, 3.0f, 1.0f) * 0.5f;
			lightData.lights[1].cameraSpaceLightPos = 
				Vector4.Transform(new Vector4(5.0f, 6.0f, 0.5f, 1.0f), camMatrix);

			// Update in used programs
			lightData.SetUniforms(g_unlitProg);
			lightData.UpdateInternal();

			lightData.SetUniforms(g_colroedProg);
			lightData.UpdateInternal();

			lightData.SetUniforms(g_projLightProg);
			lightData.UpdateInternal();
		}

		public override void display()
		{
			if(g_pScene == null)
				return;

			g_timer.Update();

			GL.ClearColor(0.8f, 0.8f, 0.8f, 1.0f);
			GL.ClearDepth(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit |ClearBufferMask.DepthBufferBit);

			Matrix4 cameraMatrix = g_viewPole.CalcMatrix();
			Matrix4 lightView = g_lightViewPole.CalcMatrix();

			MatrixStack modelMatrix = new MatrixStack();
			modelMatrix.ApplyMatrix(cameraMatrix);

			BuildLights(cameraMatrix);

			g_nodes[0].NodeSetOrient(Quaternion.FromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), 
				360.0f *  g_timer.GetAlpha()));
			g_nodes[3].NodeSetOrient(g_spinBarOrient * Quaternion.FromAxisAngle(new Vector3(0.0f, 0.0f, 1.0f), 
				360.0f * g_timer.GetAlpha()));
				
			{
				MatrixStack persMatrix = new MatrixStack();
				persMatrix.Perspective(60.0f, (g_displayWidth / (float)g_displayHeight), g_fzNear, g_fzFar);
				// added
				//persMatrix.Scale(scaleFactor);
				//persMatrix.Translate(translateVector);
				// end added

				ProjectionBlock projData = new ProjectionBlock();
				projData.cameraToClipMatrix = persMatrix.Top();

				GL.UseProgram(g_colroedProg);
				GL.UniformMatrix4(g_coloredCameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
				GL.UseProgram(0);

				GL.UseProgram(g_unlitProg);
				GL.UniformMatrix4(g_unlitCameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
				GL.UseProgram(0);

				GL.UseProgram(g_projLightProg);
				GL.UniformMatrix4(g_projLightCameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
				GL.UseProgram(0);

			}

			GL.ActiveTexture(TextureUnit.Texture0 + g_lightProjTexUnit);
			GL.BindTexture(TextureTarget.Texture2D, g_lightTextures[g_currTextureIndex]);
			GL.BindSampler(g_lightProjTexUnit, g_samplers[g_currSampler]);

			{
				MatrixStack lightProjStack = new MatrixStack();
				//Texture-space transform
				lightProjStack.Translate(0.5f, 0.5f, 0.0f);
				lightProjStack.Scale(0.5f, 0.5f, 1.0f);
				//Project. Z-range is irrelevant.
				lightProjStack.Perspective(g_lightFOVs[g_currFOVIndex], 1.0f, 1.0f, 100.0f);
				//Transform from main camera space to light camera space.
				lightProjStack.ApplyMatrix(lightView);
				lightProjStack.ApplyMatrix(Matrix4.Invert(cameraMatrix));

				g_lightProjMatBinder.SetValue(lightProjStack.Top());

				// Row or Column??
				Vector4 worldLightPos = Matrix4.Invert(lightView).Row3;
				Vector3 lightPos = new Vector3(Vector4.Transform(worldLightPos, cameraMatrix));

				g_camLightPosBinder.SetValue(lightPos);
			}

			GL.Viewport(0, 0, width, height);
			g_pScene.Render(modelMatrix.Top());

			{
				//Draw axes
				using (PushStack pushstack = new PushStack(modelMatrix))
				{
					modelMatrix.ApplyMatrix(Matrix4.Invert(lightView));
					modelMatrix.Scale(15.0f);
					modelMatrix.Scale(1.0f, 1.0f, -1.0f); //Invert the Z-axis so that it points in the right direction.

					GL.UseProgram(g_colroedProg);
					Matrix4 mm = modelMatrix.Top();
					GL.UniformMatrix4(g_coloredModelToCameraMatrixUnif, false, ref mm);
					g_pAxesMesh.Render();
				}
			}

			if(g_bDrawCameraPos)
			{
				//Draw lookat point.
				using (PushStack pushstack = new PushStack(modelMatrix))
				{
					modelMatrix.SetIdentity();
					modelMatrix.Translate(new Vector3(0.0f, 0.0f, -g_viewPole.GetView().radius));
					modelMatrix.Scale(0.5f);

					GL.Disable(EnableCap.DepthTest);
					GL.DepthMask(false);
					GL.UseProgram(g_unlitProg);
					Matrix4 mm = modelMatrix.Top();
					GL.UniformMatrix4(g_unlitModelToCameraMatrixUnif, false, ref mm);
					GL.Uniform4(g_unlitObjectColorUnif, 0.25f, 0.25f, 0.25f, 1.0f);
					g_pSphereMesh.Render("flat");
					GL.DepthMask(true);
					GL.Enable(EnableCap.DepthTest);
					GL.Uniform4(g_unlitObjectColorUnif, 1.0f, 1.0f, 1.0f, 1.0f);
					g_pSphereMesh.Render("flat");
				}
			}

			GL.ActiveTexture(TextureUnit.Texture0 + g_lightProjTexUnit);
			GL.BindTexture(TextureTarget.Texture2D, 0);
			GL.BindSampler(g_lightProjTexUnit, 0);
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			switch (keyCode)
			{
			case Keys.Escape:
				g_pScene = null;
				break;
			case Keys.Space:
				g_lightViewPole.Reset();
				break;
			case Keys.T:
				g_bDrawCameraPos = !g_bDrawCameraPos;
				break;
			case Keys.G:
				g_bShowOtherLights = !g_bShowOtherLights;
				break;
			case Keys.H:
				g_currSampler = (g_currSampler + 1) % NUM_SAMPLERS;
				break;
			case Keys.P:
				g_timer.TogglePause();
				break;
			case Keys.Enter: //Enter key.
				{
					try
					{
						LoadAndSetupScene();
					}
					catch(Exception ex)
					{
						MessageBox.Show("Failed to reload, due to: " + ex.ToString());
					}
				}
				break;
			case Keys.Y:
				g_currFOVIndex = Math.Min(g_currFOVIndex + 1, (int)(g_lightFOVs.Length - 1));
				result.AppendLine("Curr FOV: " + g_lightFOVs[g_currFOVIndex].ToString());
				break;
			case Keys.N:
				g_currFOVIndex = Math.Max(g_currFOVIndex - 1, 0);
				result.AppendLine("Curr FOV: " + g_lightFOVs[g_currFOVIndex].ToString());
				break;
			case Keys.D1:
				g_currTextureIndex = 0;
				result.AppendLine("Current Texture = " + g_texDefs[g_currTextureIndex].name);
				break;
			case Keys.D2:
				g_currTextureIndex = 1;
				result.AppendLine("Current Texture = " + g_texDefs[g_currTextureIndex].name);
				break;
			case Keys.D3:
				g_currTextureIndex = 2;
				result.AppendLine("Current Texture = " + g_texDefs[g_currTextureIndex].name);
				break;
			case Keys.Subtract:
				MouseWheel(1, 0, 10, 10);
				break;
			case Keys.Add:
				MouseWheel(1, 1, 10, 10);
				break;
			}

			g_viewPole.CharPress((char)keyCode);
			g_lightViewPole.CharPress((char)keyCode);
			return result.ToString();
		}

		void AssociateUniformWithNodes(List<NodeRef> nodes, UniformBinderBase binder, string unifName)
		{
			foreach (NodeRef nr in nodes)
			{
				binder.AssociateWithProgram(nr.GetProgram(), unifName);
			}
		}

		void SetStateBinderWithNodes(List<NodeRef> nodes, StateBinder binder)
		{
			foreach (NodeRef nr in nodes)
			{
				nr.SetStateBinder(binder);
			}
		}
	}
}

