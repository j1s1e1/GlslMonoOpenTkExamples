using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_10_Fragment_Attenuation : TutorialBase
	{
		public Tut_10_Fragment_Attenuation ()
		{
		}

		struct ProgramData
		{
			public int theProgram;
			public int modelToCameraMatrixUnif;
			public int lightIntensityUnif;
			public int ambientIntensityUnif;

			public int normalModelToCameraMatrixUnif;
			public int cameraSpaceLightPosUnif;
			public int lightAttenuationUnif;
			public int bUseRSquareUnif;

			public int cameraToClipMatrixUnif;

			public int clipToCameraMatrixUnif;
			public int windowSizeUnif;
		};

		struct UnlitProgData
		{
			public int theProgram;
			public int objectColorUnif;
			public int modelToCameraMatrixUnif;
			public int cameraToClipMatrixUnif;
		};

		float g_fzNear = 1.0f;
		float g_fzFar = 1000.0f;

		ProgramData g_FragWhiteDiffuseColor;
		ProgramData g_FragVertexDiffuseColor;

		UnlitProgData g_Unlit;

		const int g_projectionBlockIndex = 2;
		const int g_unprojectionBlockIndex = 1;

		Matrix4 coloredCylinderModelmatrix = Matrix4.Identity;


		UnlitProgData LoadUnlitProgram(string vertexShader, string fragmentShader)
		{
			UnlitProgData data = new UnlitProgData();
			int vertexShaderInt = Shader.loadShader(ShaderType.VertexShader, vertexShader);
			int fragmentShaderInt = Shader.loadShader(ShaderType.FragmentShader, fragmentShader);

			data.theProgram  = Shader.createAndLinkProgram(vertexShaderInt, fragmentShaderInt);
			data.modelToCameraMatrixUnif =  GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");
			data.objectColorUnif =  GL.GetUniformLocation(data.theProgram, "baseColor");

			data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");

			return data;
		}

		ProgramData LoadLitProgram(string vertexShader, string fragmentShader)
		{
			ProgramData data = new ProgramData();
			int vertexShaderInt = Shader.loadShader(ShaderType.VertexShader, vertexShader);
			int fragmentShaderInt = Shader.loadShader(ShaderType.FragmentShader, fragmentShader);

			data.theProgram = Shader.createAndLinkProgram(vertexShaderInt, fragmentShaderInt);
			data.modelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");
			data.lightIntensityUnif = GL.GetUniformLocation(data.theProgram, "lightIntensity");
			data.ambientIntensityUnif = GL.GetUniformLocation(data.theProgram, "ambientIntensity");

			data.normalModelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "normalModelToCameraMatrix");
			data.cameraSpaceLightPosUnif = GL.GetUniformLocation(data.theProgram, "cameraSpaceLightPos");
			data.windowSizeUnif = GL.GetUniformLocation(data.theProgram, "windowSize");
			data.lightAttenuationUnif = GL.GetUniformLocation(data.theProgram, "lightAttenuation");
			data.bUseRSquareUnif = GL.GetUniformLocation(data.theProgram, "bUseRSquare");

			// GLuint projectionBlock = glGetUniformBlockIndex(data.theProgram, "Projection");
			// glUniformBlockBinding(data.theProgram, projectionBlock, g_projectionBlockIndex);
			// GLuint unprojectionBlock = glGetUniformBlockIndex(data.theProgram, "UnProjection");
			// glUniformBlockBinding(data.theProgram, unprojectionBlock, g_unprojectionBlockIndex);

			data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");
			data.clipToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "clipToCameraMatrix");
			data.windowSizeUnif = GL.GetUniformLocation(data.theProgram, "windowSize");

			return data;
		}

		void InitializePrograms()
		{
			Shader.compileShader(ShaderType.VertexShader, VertexShaders.FragLightAtten_PN);
			Shader.compileShader(ShaderType.VertexShader, VertexShaders.FragLightAtten_PCN);
			Shader.compileShader(ShaderType.FragmentShader, FragmentShaders.FragLightAtten);

			g_FragWhiteDiffuseColor = LoadLitProgram(VertexShaders.FragLightAtten_PN, 
				FragmentShaders.FragLightAtten);
			g_FragVertexDiffuseColor = LoadLitProgram(VertexShaders.FragLightAtten_PCN, 
				FragmentShaders.FragLightAtten);

			g_Unlit = LoadUnlitProgram(VertexShaders.PosTransform, FragmentShaders.ColorUniform_frag);
		}

		Mesh g_pCylinderMesh;
		Mesh g_pPlaneMesh;
		Mesh g_pCubeMesh;

		///////////////////////////////////////////////
		// View/Object Setup
		static ViewData g_initialViewData = new ViewData
		(
			new Vector3(0.0f, 0.5f, 0.0f),
			new Quaternion(0.92387953f, 0.3826834f, 0.0f, 0.0f),
			5.0f,
			0.0f
		);

		static ViewScale g_viewScale = new ViewScale
		(
			3.0f, 20.0f,
			1.5f, 0.5f,
			0.0f, 0.0f,		//No camera movement.
			90.0f/250.0f
		);

		static ObjectData g_initialObjectData = new ObjectData
		(
				new Vector3(0.0f, 0.5f, 0.0f),
				new Quaternion(1.0f, 0.0f, 0.0f, 0.0f)
		);

		static ViewProvider g_viewPole = new ViewProvider(g_initialViewData,
			g_viewScale, MouseButtons.MB_LEFT_BTN);
		static ObjectPole g_objtPole = new ObjectPole(g_initialObjectData,
			90.0f/250.0f, MouseButtons.MB_RIGHT_BTN, g_viewPole);

		class UnProjectionBlock
		{
			public Matrix4 clipToCameraMatrix;
			public Vector2 windowSize;
		};

		//Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
		protected override void init()
		{
			InitializePrograms();

			try
			{
				string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes";
				Stream unitcylinder = File.OpenRead(XmlFilesDirectory + @"/unitcylinderfragmentpointlighting.xml");
				g_pCylinderMesh = new Mesh(unitcylinder);
				Stream largeplane = File.OpenRead(XmlFilesDirectory + @"/largeplane.xml");
				g_pPlaneMesh = new Mesh(largeplane);
				Stream unitcube = File.OpenRead(XmlFilesDirectory + @"/unitcube.xml");
				g_pCubeMesh = new Mesh(unitcube);
			}
			catch(Exception ex)
			{
				throw new Exception("Error:" + ex.ToString());
			}

			SetupDepthAndCull();
			reshape();
		}

		static float g_fLightHeight = 1.5f;
		static float g_fLightRadius = 1.0f;

		FrameworkTimer g_LightTimer = new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, 5.0f);

		Vector4 CalcLightPosition()
		{
			float fCurrTimeThroughLoop = g_LightTimer.GetAlpha();

			Vector4 ret = new Vector4(0.0f, g_fLightHeight, 0.0f, 1.0f);

			ret.X = (float) Math.Cos(fCurrTimeThroughLoop * (3.14159f * 2.0f)) * g_fLightRadius;
			ret.Z = 10f + (float) Math.Sin(fCurrTimeThroughLoop * (3.14159f * 2.0f)) * g_fLightRadius;

			return ret;
		}
			
		static bool g_bDrawColoredCyl = false;
		static bool g_bDrawLight = false;
		static bool g_bScaleCyl = false;
		static bool g_bUseRSquare = false;

		static float g_fLightAttenuation = 1.0f;

		//Called to update the display.
		//You should call glutSwapBuffers after all of your rendering to display what you rendered.
		//If you need continuous updates of the screen, call glutPostRedisplay() at the end of the function.

		public override void display()
		{
			g_LightTimer.Update();
			ClearDisplay();

			if(g_pPlaneMesh != null && g_pCylinderMesh != null && g_pCubeMesh != null)
			{
				MatrixStack modelMatrix = new MatrixStack();
				modelMatrix.SetMatrix(g_viewPole.CalcMatrix());

				Vector4 worldLightPos = CalcLightPosition();
				Vector3 lightPosCameraSpace = new Vector3(Vector4.Transform(worldLightPos, modelMatrix.Top()));

				GL.UseProgram(g_FragWhiteDiffuseColor.theProgram);
				GL.Uniform4(g_FragWhiteDiffuseColor.lightIntensityUnif, 0.8f, 0.8f, 0.8f, 1.0f);
				GL.Uniform4(g_FragWhiteDiffuseColor.ambientIntensityUnif, 0.2f, 0.2f, 0.2f, 1.0f);

				GL.Uniform3(g_FragWhiteDiffuseColor.cameraSpaceLightPosUnif, ref lightPosCameraSpace);
				GL.Uniform1(g_FragWhiteDiffuseColor.lightAttenuationUnif, g_fLightAttenuation);
				GL.Uniform1(g_FragWhiteDiffuseColor.bUseRSquareUnif, g_bUseRSquare ? 1 : 0);

				GL.UseProgram(g_FragVertexDiffuseColor.theProgram);
				GL.Uniform4(g_FragVertexDiffuseColor.lightIntensityUnif, 0.8f, 0.8f, 0.8f, 1.0f);
				GL.Uniform4(g_FragVertexDiffuseColor.ambientIntensityUnif, 0.2f, 0.2f, 0.2f, 1.0f);
				GL.Uniform3(g_FragVertexDiffuseColor.cameraSpaceLightPosUnif, ref lightPosCameraSpace);
				GL.Uniform1(g_FragVertexDiffuseColor.lightAttenuationUnif, g_fLightAttenuation);
				GL.Uniform1(g_FragVertexDiffuseColor.bUseRSquareUnif, g_bUseRSquare ? 1 : 0);
				GL.UseProgram(0);

				{
					//Render the ground plane.
					using (PushStack pushstack = new PushStack(modelMatrix))
					{
						Matrix3 normMatrix = new Matrix3(modelMatrix.Top());
						normMatrix = Matrix3.Transpose(normMatrix.Inverted());

						GL.UseProgram(g_FragWhiteDiffuseColor.theProgram);
						Matrix4 mm = modelMatrix.Top();
						GL.UniformMatrix4(g_FragWhiteDiffuseColor.modelToCameraMatrixUnif, false, ref mm);

						GL.UniformMatrix3(g_FragWhiteDiffuseColor.normalModelToCameraMatrixUnif, false,
							ref normMatrix);
						g_pPlaneMesh.Render();
						GL.UseProgram(0);
					}

					//Render the Cylinder
					using (PushStack pushstack = new PushStack(modelMatrix))
					{
						modelMatrix.ApplyMatrix(g_objtPole.CalcMatrix());
						modelMatrix.Translate(new Vector3(0f, 0f, 10f));
						if(g_bScaleCyl)
							modelMatrix.Scale(1.0f, 1.0f, 0.2f);
						coloredCylinderModelmatrix = modelMatrix.Top();

						Matrix3 normMatrix = new Matrix3(modelMatrix.Top());
						normMatrix = Matrix3.Transpose(normMatrix.Inverted());

						if(g_bDrawColoredCyl)
						{
							GL.UseProgram(g_FragVertexDiffuseColor.theProgram);
							Matrix4 mm = modelMatrix.Top();
							GL.UniformMatrix4(g_FragVertexDiffuseColor.modelToCameraMatrixUnif, false,
								ref mm);

							GL.UniformMatrix3(g_FragVertexDiffuseColor.normalModelToCameraMatrixUnif, false,
								ref normMatrix);
							g_pCylinderMesh.Render("lit-color");
						}
						else
						{
							GL.UseProgram(g_FragWhiteDiffuseColor.theProgram);
							Matrix4 mm = modelMatrix.Top();
							GL.UniformMatrix4(g_FragWhiteDiffuseColor.modelToCameraMatrixUnif, false,
								ref mm);

							GL.UniformMatrix3(g_FragWhiteDiffuseColor.normalModelToCameraMatrixUnif, false,
								ref normMatrix);
							g_pCylinderMesh.Render("lit");
						}
						GL.UseProgram(0);
					}

					//Render the light
					if(g_bDrawLight)
					{
						using (PushStack pushstack = new PushStack(modelMatrix))
						{
							modelMatrix.Translate(new Vector3(worldLightPos));
							modelMatrix.Scale(0.1f, 0.1f, 0.1f);

							GL.UseProgram(g_Unlit.theProgram);
							Matrix4 mm = modelMatrix.Top();
							GL.UniformMatrix4(g_Unlit.modelToCameraMatrixUnif, false,
								ref mm);
							GL.Uniform4(g_Unlit.objectColorUnif, 0.8078f, 0.8706f, 0.9922f, 1.0f);
							g_pCubeMesh.Render("flat");
						}
					}
				}
			}
		}

		ProjectionBlock projData = new ProjectionBlock();
			
		public override void reshape ()
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(45.0f, (width / (float)height), g_fzNear, g_fzFar);

			projData.cameraToClipMatrix = persMatrix.Top();

			UnProjectionBlock unprojData = new UnProjectionBlock();
			unprojData.clipToCameraMatrix = Matrix4.Invert(persMatrix.Top());
			unprojData.windowSize = new Vector2(width, height);

			GL.UseProgram(g_FragWhiteDiffuseColor.theProgram);
			GL.UniformMatrix4(g_FragWhiteDiffuseColor.cameraToClipMatrixUnif, 
				false, ref projData.cameraToClipMatrix);
			GL.UniformMatrix4(g_FragWhiteDiffuseColor.clipToCameraMatrixUnif, false, ref unprojData.clipToCameraMatrix );
			GL.Uniform2(g_FragWhiteDiffuseColor.windowSizeUnif, unprojData.windowSize);
			GL.UseProgram(0);

			GL.UseProgram(g_FragVertexDiffuseColor.theProgram);
			GL.UniformMatrix4(g_FragVertexDiffuseColor.cameraToClipMatrixUnif, 
				false, ref projData.cameraToClipMatrix);
			GL.UniformMatrix4(g_FragVertexDiffuseColor.clipToCameraMatrixUnif, false, ref unprojData.clipToCameraMatrix );
			GL.Uniform2(g_FragVertexDiffuseColor.windowSizeUnif, unprojData.windowSize);
			GL.UseProgram(0);

			GL.UseProgram(g_Unlit.theProgram);
			GL.UniformMatrix4(g_Unlit.cameraToClipMatrixUnif, 
				false, ref projData.cameraToClipMatrix);
			GL.UseProgram(0);

			GL.Viewport(0, 0, width, height);
		}
			
		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			bool bChangedAtten = false;
			switch (keyCode)
			{
			case Keys.Escape:
				g_pPlaneMesh = null;
				g_pCylinderMesh = null;
				g_pCubeMesh = null;
				break;

			case  Keys.Space:
				g_bDrawColoredCyl = !g_bDrawColoredCyl;
				break;

			case  Keys.I: g_fLightHeight += 0.2f; break;
			case Keys.K: g_fLightHeight -= 0.2f; break;
			case Keys.L: g_fLightRadius += 0.2f; break;
			case Keys.J: g_fLightRadius -= 0.2f; break;

			case Keys.O: g_fLightAttenuation *= 1.5f; bChangedAtten = true; break;
			case Keys.U: g_fLightAttenuation /= 1.5f; bChangedAtten = true; break;

			case Keys.Y: g_bDrawLight = !g_bDrawLight; break;
			case Keys.T: g_bScaleCyl = !g_bScaleCyl; break;
			case Keys.B: g_LightTimer.TogglePause(); break;

			case Keys.H:
				g_bUseRSquare = !g_bUseRSquare;
				if(g_bUseRSquare)
					result.AppendLine("Inverse Squared Attenuation\n");
				else
					result.AppendLine("Plain Inverse Attenuation\n");
				break;
			case Keys.Z:
				result.AppendLine("cameraToClipMatrix = " + projData.cameraToClipMatrix.ToString());
				result.AppendLine("");
				result.AppendLine("coloredCylinderModelmatrix = " + coloredCylinderModelmatrix.ToString());
				result.AppendLine("");
				Matrix4 multiply = Matrix4.Mult(projData.cameraToClipMatrix, coloredCylinderModelmatrix);
				result.Append(AnalysisTools.CalculateMatrixEffects(multiply));
				break;
			}

			if(g_fLightRadius < 0.2f)
				g_fLightRadius = 0.2f;

			if(g_fLightAttenuation < 0.1f)
				g_fLightAttenuation = 0.1f;

			if(bChangedAtten)
				result.AppendLine("Atten: " + g_fLightAttenuation.ToString()); 
			return result.ToString();	
		}

	}
}

