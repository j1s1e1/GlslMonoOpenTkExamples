using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_17_DoubleProjection  : TutorialBase
	{
		public Tut_17_DoubleProjection ()
		{
		}
		const float g_fzNear = 1.0f;
		const float g_fzFar = 1000.0f;

		const int g_colorTexUnit = 0;

		////////////////////////////////
		//View setup.
		static ViewData g_initialView = new ViewData
		(
			new Vector3(0.0f, 0.0f, 0.0f),
			new Quaternion(0.909845f, 0.16043f, -0.376867f, -0.0664516f),
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


		static ViewData g_initPersView = new ViewData
		(
			new Vector3(0.0f, 0.0f, 0.0f),
			new Quaternion(1.0f, 0.0f, 0.0f, 0.0f),
			5.0f,
			0.0f
		);

		static ViewScale g_initPersViewScale = new ViewScale
		(
			0.05f, 10.0f,
			0.1f, 0.05f,
			4.0f, 1.0f,
			90.0f/250.0f
		);

		ViewPole g_viewPole = new ViewPole(g_initialView, g_initialViewScale, MouseButtons.MB_LEFT_BTN);
		ViewPole g_persViewPole = new ViewPole(g_initPersView, g_initPersViewScale, MouseButtons.MB_RIGHT_BTN);


		Scene g_pScene = null;

		List<NodeRef> g_nodes = new List<NodeRef>();
		FrameworkTimer g_timer = new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, 10.0f);

		// FIXME Framework::UniformIntBinder g_lightNumBinder;
		// FIXME Framework::TextureBinder g_stoneTexBinder;

		int g_unlitModelToCameraMatrixUnif;
		int g_unlitCameraToClipMatrixUnif;
		int g_unlitObjectColorUnif;
		int g_unlitProg;
		Mesh g_pSphereMesh;
		Quaternion g_spinBarOrient;

		void LoadAndSetupScene()
		{

			FrameworkScene pScene = new FrameworkScene("dp_scene.xml");

			List<NodeRef> nodes = new List<NodeRef>();
			nodes.Add(pScene.FindNode("cube"));
			nodes.Add(pScene.FindNode("rightBar"));
			nodes.Add(pScene.FindNode("leaningBar"));
			nodes.Add(pScene.FindNode("spinBar"));
/* FIXME
			AssociateUniformWithNodes(nodes, g_lightNumBinder, "numberOfLights");
			SetStateBinderWithNodes(nodes, g_lightNumBinder);

			GLuint unlit = pScene->FindProgram("p_unlit");
			Framework::Mesh *pSphereMesh = pScene->FindMesh("m_sphere");

			//No more things that can throw.
			g_spinBarOrient = nodes[3].NodeGetOrient();
			g_unlitProg = unlit;
			g_unlitModelToCameraMatrixUnif = glGetUniformLocation(unlit, "modelToCameraMatrix");
			g_unlitObjectColorUnif = glGetUniformLocation(unlit, "objectColor");

			std::swap(nodes, g_nodes);
			nodes.clear();	//If something was there already, delete it.

			std::swap(pSphereMesh, g_pSphereMesh);

			Framework::Scene *pOldScene = g_pScene;
			g_pScene = pScene.release();
			pScene.reset(pOldScene);	//If something was there already, delete it.
			*/
		}

		struct PerLight
		{
			Vector4 cameraSpaceLightPos;
			Vector4 lightIntensity;
		};

		const int MAX_NUMBER_OF_LIGHTS = 4;

		protected override void init()
		{
			SetupDepthAndCull();
			GL.Enable(EnableCap.FramebufferSrgb);

			try
			{
				LoadAndSetupScene();
			}
			catch(Exception ex)
			{
				MessageBox.Show("Failed to load scene: " + ex.ToString());
			}
			reshape();
		}
			
		int g_currSampler = 0;

		bool g_bDrawCameraPos = false;
		bool g_bDepthClampProj = true;

		int g_displayWidth = 700;
		int g_displayHeight = 350;

		void BuildLights(Matrix4 camMatrix )
		{
			LightBlock lightData = new LightBlock();
			lightData.ambientIntensity = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
			lightData.lightAttenuation = 1.0f / (5.0f * 5.0f);
			lightData.maxIntensity = 3.0f;
			lightData.lights[0].lightIntensity = new Vector4(2.0f, 2.0f, 2.5f, 1.0f);
			lightData.lights[0].cameraSpaceLightPos = 
				Vector4.Transform(new Vector4(-0.2f, 0.5f, 0.5f, 0.0f), camMatrix);
			lightData.lights[1].lightIntensity = new Vector4(3.5f, 6.5f, 3.0f, 1.0f) * 1.2f;
			lightData.lights[1].cameraSpaceLightPos = 
				Vector4.Transform(new Vector4(5.0f, 6.0f, 0.5f, 1.0f), camMatrix);

			// FIXME g_lightNumBinder.SetValue(2);

			// FIXME glBindBuffer(GL_UNIFORM_BUFFER, g_lightUniformBuffer);
			// FIXME glBufferData(GL_UNIFORM_BUFFER, sizeof(LightBlock), &lightData, GL_STREAM_DRAW);
		}

		public override void display()
		{
			if(g_pScene == null)
				return;

			g_timer.Update();

			GL.ClearColor(0.8f, 0.8f, 0.8f, 1.0f);
			GL.ClearDepth(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit |ClearBufferMask.DepthBufferBit);

			MatrixStack modelMatrix = new MatrixStack();
			modelMatrix.ApplyMatrix(g_viewPole.CalcMatrix());

			BuildLights(modelMatrix.Top());

			/* FIXME
			g_nodes[0].NodeSetOrient(glm::rotate(glm::fquat(),
				360.0f * g_timer.GetAlpha(), glm::vec3(0.0f, 1.0f, 0.0f)));

			g_nodes[3].NodeSetOrient(g_spinBarOrient * glm::rotate(glm::fquat(),
				360.0f * g_timer.GetAlpha(), glm::vec3(0.0f, 0.0f, 1.0f)));
				*/
			{
				MatrixStack persMatrix = new MatrixStack();
				persMatrix.Perspective(60.0f, (width / height), g_fzNear, g_fzFar);

				ProjectionBlock projData = new ProjectionBlock();
				projData.cameraToClipMatrix = persMatrix.Top();

				GL.UseProgram(g_unlitProg);
				GL.UniformMatrix4(g_unlitCameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
				GL.UseProgram(0);
			}

			GL.Viewport(0, 0, width, height);
			// FIXME g_pScene.Render(modelMatrix.Top());

			if(g_bDrawCameraPos)
			{
				using (PushStack pushstack = new PushStack(modelMatrix))//Draw lookat point.
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

			{
				MatrixStack persMatrix = new MatrixStack();
				Matrix4 applyMatrix = g_persViewPole.CalcMatrix();
				applyMatrix.Row3 = Vector4.Zero;
				applyMatrix.Column3 = Vector4.Zero;
				persMatrix.ApplyMatrix(applyMatrix);
				persMatrix.Perspective(60.0f, (width / height), g_fzNear, g_fzFar);

				ProjectionBlock projData = new ProjectionBlock();
				projData.cameraToClipMatrix = persMatrix.Top();

				GL.UseProgram(g_unlitProg);
				GL.UniformMatrix4(g_unlitCameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
				GL.UseProgram(0);

			}

			if(!g_bDepthClampProj)
				GL.Disable(EnableCap.DepthClamp);
			GL.Viewport(width + (width % 2), 0, width, height);
			// FIXME g_pScene.Render(modelMatrix.Top());
			GL.Enable(EnableCap.DepthClamp);
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
				g_persViewPole.Reset();
				break;
			case Keys.T:
				g_bDrawCameraPos = !g_bDrawCameraPos;
				break;
			case Keys.Y:
				g_bDepthClampProj = !g_bDepthClampProj;
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
			}

			g_viewPole.CharPress((char)keyCode);
			return result.ToString();
		}
	}
}

