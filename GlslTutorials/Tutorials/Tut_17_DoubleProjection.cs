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
		Vector3 translateVector = new Vector3(0f, 0f, -10f);
		float scaleFactor = 0.02f;
		float reduceSpeed = 10f;
		static int NUMBER_OF_LIGHTS = 2;
		bool rightMultiply;
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

		static ViewData g_initPersView = new ViewData
		(
			new Vector3(0.0f, 0.0f, 0.0f),
			new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
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

		public override void MouseMotion(int x, int y)
		{
			Framework.ForwardMouseMotion(g_viewPole, x, y);
			Framework.ForwardMouseMotion(g_persViewPole, x, y);
		}

		public override void MouseButton(int button, int state, int x, int y)
		{
			Framework.ForwardMouseButton(g_viewPole, button, state, x, y);
			Framework.ForwardMouseButton(g_persViewPole, button, state, x, y);
		}

		void MouseWheel(int wheel, int direction, int x, int y)
		{
			Framework.ForwardMouseWheel(g_viewPole, wheel, direction, x, y);
		}

		FrameworkScene g_pScene = null;

		List<NodeRef> g_nodes = new List<NodeRef>();
		FrameworkTimer g_timer = new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, 10.0f);

		UniformIntBinder g_lightNumBinder;

		int g_unlitModelToCameraMatrixUnif;
		int g_unlitCameraToClipMatrixUnif;
		int g_unlitObjectColorUnif;
		int g_unlitProg;

		int g_litCameraToClipMatrixUnif;
		int g_litProg;

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
			g_lightNumBinder = new UniformIntBinder();
			AssociateUniformWithNodes(nodes, g_lightNumBinder, "numberOfLights");
			SetStateBinderWithNodes(nodes, g_lightNumBinder);

 			int unlit = pScene.FindProgram("p_unlit");
			int lit = pScene.FindProgram("p_lit");
			Mesh pSphereMesh = pScene.FindMesh("m_sphere");

			//No more things that can throw.
			g_spinBarOrient = nodes[3].NodeGetOrient();

			g_unlitProg = unlit;
			g_unlitModelToCameraMatrixUnif = GL.GetUniformLocation(unlit, "modelToCameraMatrix");
			g_unlitCameraToClipMatrixUnif  = GL.GetUniformLocation(unlit, "cameraToClipMatrix");
			g_unlitObjectColorUnif = GL.GetUniformLocation(unlit, "objectColor");

			g_litProg = lit;
			g_litCameraToClipMatrixUnif  = GL.GetUniformLocation(lit, "cameraToClipMatrix");

			g_nodes = nodes;
			g_pSphereMesh = pSphereMesh;

			g_pScene = pScene;
		}

		protected override void init()
		{
			SetupDepthAndCull();
			GL.Enable(EnableCap.FramebufferSrgb);
			GL.Enable(EnableCap.Texture2D); // must be before scene??
			try
			{
				LoadAndSetupScene();
			}
			catch(Exception ex)
			{
				MessageBox.Show("Failed to load scene: " + ex.ToString());
			}
			//reshape();
			MatrixStack.rightMultiply = false;
			rightMultiply = true;
		}

		bool g_bDrawCameraPos = true;
		bool g_bDepthClampProj = true;

		void BuildLights(Matrix4 camMatrix)
		{
			LightBlock lightData = new LightBlock(NUMBER_OF_LIGHTS);
			lightData.ambientIntensity = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
			lightData.lightAttenuation = 1.0f / (5.0f * 5.0f);
			lightData.maxIntensity = 3.0f;
			lightData.lights[0].lightIntensity = new Vector4(2.0f, 2.0f, 2.5f, 1.0f);
			lightData.lights[0].cameraSpaceLightPos = 
				Vector4.Transform(new Vector4(-0.2f, 0.5f, 0.5f, 0.0f), camMatrix);
			lightData.lights[1].lightIntensity = new Vector4(3.5f, 6.5f, 3.0f, 1.0f) * 1.2f;
			lightData.lights[1].cameraSpaceLightPos = 
				Vector4.Transform(new Vector4(5.0f, 6.0f, 0.5f, 1.0f), camMatrix);

			g_lightNumBinder.SetValue(2);

			lightData.SetUniforms(g_unlitProg);
			lightData.UpdateInternal();

			lightData.SetUniforms(g_litProg);
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

			MatrixStack modelMatrix = new MatrixStack();
			modelMatrix.ApplyMatrix(g_viewPole.CalcMatrix());


			BuildLights(modelMatrix.Top());

			if (rightMultiply)
			{
				g_nodes[0].NodeSetOrient(Quaternion.FromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), 
					360.0f *  g_timer.GetAlpha()/reduceSpeed));
				g_nodes[3].NodeSetOrient(Quaternion.Multiply(g_spinBarOrient,
					Quaternion.FromAxisAngle(new Vector3(0.0f, 0.0f, 1.0f), 360.0f * g_timer.GetAlpha()/reduceSpeed)));
			}
			else
			{
				g_nodes[0].NodeSetOrient(Quaternion.FromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), 
					360.0f *  g_timer.GetAlpha()/reduceSpeed));
				g_nodes[3].NodeSetOrient(Quaternion.Multiply(
					Quaternion.FromAxisAngle(new Vector3(0.0f, 0.0f, 1.0f), 360.0f * g_timer.GetAlpha()/reduceSpeed), 
					g_spinBarOrient));
			}

			{
				MatrixStack persMatrix = new MatrixStack();
				persMatrix.Perspective(60.0f, (width/2f / height), g_fzNear, g_fzFar);

				// added
				persMatrix.Translate(translateVector);
				persMatrix.Scale(scaleFactor);
				// end added

				ProjectionBlock projData = new ProjectionBlock();
				projData.cameraToClipMatrix = persMatrix.Top();

				GL.UseProgram(g_unlitProg);
				GL.UniformMatrix4(g_unlitCameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
				GL.UseProgram(0);

				GL.UseProgram(g_pScene.FindProgram("p_lit"));
				GL.UniformMatrix4(g_litCameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
				GL.UseProgram(0);
			}

			GL.Viewport(0, 0, width/2, height);
			g_pScene.Render(modelMatrix.Top());

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
				applyMatrix.M44 = 1f;
				persMatrix.ApplyMatrix(applyMatrix);
				persMatrix.Perspective(60.0f, (width/2f / height), g_fzNear, g_fzFar);

				// added
				persMatrix.Translate(translateVector);
				persMatrix.Scale(scaleFactor);
				// end added

				ProjectionBlock projData = new ProjectionBlock();
				projData.cameraToClipMatrix = persMatrix.Top();

				GL.UseProgram(g_unlitProg);
				GL.UniformMatrix4(g_unlitCameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
				GL.UseProgram(0);

				GL.UseProgram(g_pScene.FindProgram("p_lit"));
				GL.UniformMatrix4(g_litCameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
				GL.UseProgram(0);

			}

			if(!g_bDepthClampProj)
				GL.Disable(EnableCap.DepthClamp);
			GL.Viewport(width/2, 0, width/2, height);
			g_pScene.Render(modelMatrix.Top());
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

		void AssociateUniformWithNodes(List<NodeRef> nodes, UniformIntBinder binder, string unifName)
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

