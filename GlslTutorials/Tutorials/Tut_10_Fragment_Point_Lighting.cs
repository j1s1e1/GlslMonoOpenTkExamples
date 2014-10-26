using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_10_Fragment_Point_Lighting : TutorialBase
	{
		public Tut_10_Fragment_Point_Lighting ()
		{
		}
		
		struct ProgramData
		{
			public int theProgram;
			public int modelSpaceLightPosUnif;
			public int lightIntensityUnif;
			public int ambientIntensityUnif;
			public int modelToCameraMatrixUnif;
			public int cameraToClipMatrixUnif;
		};

		struct UnlitProgData
		{
			public int theProgram;
			public int baseColorUnif;
			public int modelToCameraMatrixUnif;
			public int cameraToClipMatrixUnif;
		};
		
		float g_fzNear = 1.0f;
		float g_fzFar = 1000.0f;
		
		ProgramData g_WhiteDiffuseColor;
		ProgramData g_VertexDiffuseColor;
		ProgramData g_FragWhiteDiffuseColor;
		ProgramData g_FragVertexDiffuseColor;
		
		UnlitProgData g_Unlit;
		
		const int g_projectionBlockIndex = 2;
		
		Matrix4 coloredCylinderModelmatrix = Matrix4.Identity;
		
		UnlitProgData LoadUnlitProgram(string vertexShader, string fragmentShader)
		{
			UnlitProgData data = new UnlitProgData();
	        int vertexShaderInt = Shader.loadShader(ShaderType.VertexShader, vertexShader);
	        int fragmentShaderInt = Shader.loadShader(ShaderType.FragmentShader, fragmentShader);
			
			data.theProgram  = Shader.createAndLinkProgram(vertexShaderInt, fragmentShaderInt);
			data.modelToCameraMatrixUnif =  GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");
			data.baseColorUnif =  GL.GetUniformLocation(data.theProgram, "baseColor");
		
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
			data.modelSpaceLightPosUnif = GL.GetUniformLocation(data.theProgram, "modelSpaceLightPos");
			data.lightIntensityUnif = GL.GetUniformLocation(data.theProgram, "lightIntensity");
			data.ambientIntensityUnif = GL.GetUniformLocation(data.theProgram, "ambientIntensity");
		
			data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");
			return data;
		}
		
		void InitializePrograms()
		{
			g_WhiteDiffuseColor = LoadLitProgram(VertexShaders.ModelPosVertexLighting_PN, 
			                                     FragmentShaders.ColorPassthrough_frag);
			g_VertexDiffuseColor = LoadLitProgram(VertexShaders.ModelPosVertexLighting_PCN, 
			                                      FragmentShaders.ColorPassthrough_frag);
			g_FragWhiteDiffuseColor = LoadLitProgram(VertexShaders.FragmentLighting_PN, 
			                                         FragmentShaders.FragmentLighting);
			g_FragVertexDiffuseColor = LoadLitProgram(VertexShaders.FragmentLighting_PCN, 
			                                          FragmentShaders.FragmentLighting);
		
			g_Unlit = LoadUnlitProgram(VertexShaders.PosTransform, FragmentShaders.ColorUniform_frag);
		}
		
		static Mesh g_pCylinderMesh = null;
		static Mesh g_pPlaneMesh = null;
		static Mesh g_pCubeMesh = null;
		
		static ViewData g_initialViewData;
		
	    private static void InitializeGInitialViewData()
	    {
	        g_initialViewData = new ViewData(new Vector3(0.0f, 0.5f, 0.0f),
	                new Quaternion(0.92387953f, 0.3826834f, 0.0f, 0.0f),
	                5.0f,
	                0.0f);
	    }
		
		static ViewScale g_viewScale;
	
	    private static void InitializeGViewScale()
	    {
	        g_viewScale = new ViewScale(
	                3.0f, 20.0f,
	                1.5f, 0.5f,
	                0.0f, 0.0f,		//No camera movement.
	                90.0f/250.0f);
	    }
		
			
	    public static ObjectData g_initialObjectData = new ObjectData(new Vector3(0.0f, 0.5f, 0.0f),
	            new Quaternion(1.0f, 0.0f, 0.0f, 0.0f));
		
		public static  ViewProvider g_viewPole;
	
	    public static ObjectPole g_objtPole;
		
		protected override void init()
		{
			InitializeGInitialViewData();
	        InitializeGViewScale();
	        g_viewPole = new ViewProvider(g_initialViewData, g_viewScale, MouseButtons.MB_LEFT_BTN);
	        g_objtPole = new ObjectPole(g_initialObjectData, (float)(90.0f / 250.0f),
	                MouseButtons.MB_RIGHT_BTN, g_viewPole);
	
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
		
		Vector4 CalcLightPosition()
		{
			float fCurrTimeThroughLoop = GetElapsedTime();
		
			Vector4 ret = new Vector4(0.0f, g_fLightHeight, 0.0f, 1.0f);
		
			ret.X = (float)Math.Cos(fCurrTimeThroughLoop * (3.14159f * 2.0f)) * g_fLightRadius;
			ret.Z = (float)Math.Sin(fCurrTimeThroughLoop * (3.14159f * 2.0f)) * g_fLightRadius;
		
			return ret;
		}
		
		
		static bool g_bUseFragmentLighting = true;
		static bool g_bDrawColoredCyl = false;
		static bool g_bDrawLight = false;
		static bool g_bScaleCyl = false;
		
		public override void display()
		{
			ClearDisplay();
			if((g_pPlaneMesh != null) && (g_pCylinderMesh != null) && ( g_pCubeMesh != null))
			{
				MatrixStack modelMatrix = new MatrixStack();
				modelMatrix.SetMatrix(g_viewPole.CalcMatrix());
		
				Vector4 worldLightPos = CalcLightPosition();
		
				Vector4 lightPosCameraSpace = Vector4.Transform(worldLightPos, modelMatrix.Top());
		
				ProgramData pWhiteProgram;
				ProgramData pVertColorProgram;
		
				if(g_bUseFragmentLighting)
				{
					pWhiteProgram = g_FragWhiteDiffuseColor;
					pVertColorProgram = g_FragVertexDiffuseColor;
				}
				else
				{
					pWhiteProgram = g_WhiteDiffuseColor;
					pVertColorProgram = g_VertexDiffuseColor;
				}
		
				GL.UseProgram(pWhiteProgram.theProgram);
				GL.Uniform4(pWhiteProgram.lightIntensityUnif, 0.8f, 0.8f, 0.8f, 1.0f);
				GL.Uniform4(pWhiteProgram.ambientIntensityUnif, 0.2f, 0.2f, 0.2f, 1.0f);
			 	GL.UniformMatrix4(pWhiteProgram.cameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
				GL.UseProgram(pVertColorProgram.theProgram);
				GL.Uniform4(pVertColorProgram.lightIntensityUnif, 0.8f, 0.8f, 0.8f, 1.0f);
				GL.Uniform4(pVertColorProgram.ambientIntensityUnif, 0.2f, 0.2f, 0.2f, 1.0f);
				GL.UniformMatrix4(pVertColorProgram.cameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);

				GL.UseProgram(0);
		
				//Render the ground plane.
				using (PushStack pushstack = new PushStack(modelMatrix))
				{
					GL.UseProgram(pWhiteProgram.theProgram);
					Matrix4 mm = modelMatrix.Top();
					GL.UniformMatrix4(pWhiteProgram.modelToCameraMatrixUnif, false, ref mm);
		
					Matrix4 invTransform = modelMatrix.Top().Inverted();
					Vector4 lightPosModelSpace = Vector4.Transform(lightPosCameraSpace, invTransform);
					Vector3 lightPos = new Vector3(lightPosModelSpace.X, lightPosModelSpace.Y, lightPosModelSpace.Z);
					GL.Uniform3(pWhiteProgram.modelSpaceLightPosUnif, ref lightPos);
		
					g_pPlaneMesh.Render();
					GL.UseProgram(0);
				}
		
				//Render the Cylinder
				using (PushStack pushstack = new PushStack(modelMatrix))
				{
					modelMatrix.ApplyMatrix(g_objtPole.CalcMatrix());
					modelMatrix.Translate(new Vector3(0f, 0f, 10f));
					coloredCylinderModelmatrix = modelMatrix.Top();
					if(g_bScaleCyl)
						modelMatrix.Scale(1.0f, 1.0f, 0.2f);
					Matrix4 mm = modelMatrix.Top();
					Matrix4 invTransform = modelMatrix.Top().Inverted();
					Vector4 lightPosModelSpace = Vector4.Transform(lightPosCameraSpace,  invTransform);
		
					if(g_bDrawColoredCyl)
					{
						GL.UseProgram(pVertColorProgram.theProgram);

						GL.UniformMatrix4(pVertColorProgram.modelToCameraMatrixUnif, false, ref mm);
						Vector3 lightPos = new Vector3(lightPosModelSpace.X, lightPosModelSpace.Y, lightPosModelSpace.Z);
						GL.Uniform3(pVertColorProgram.modelSpaceLightPosUnif, ref lightPos);
	
						GL.UniformMatrix4(pVertColorProgram.cameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
						
						g_pCylinderMesh.Render("lit-color");
					}
					else
					{
						GL.UseProgram(pWhiteProgram.theProgram);
						GL.UniformMatrix4(pWhiteProgram.modelToCameraMatrixUnif, false, ref mm);
						Vector3 lightPos = new Vector3(lightPosModelSpace.X, lightPosModelSpace.Y, lightPosModelSpace.Z);
						GL.Uniform3(pWhiteProgram.modelSpaceLightPosUnif, ref lightPos);
						
						GL.UniformMatrix4(pWhiteProgram.cameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
	
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
						GL.UniformMatrix4(g_Unlit.modelToCameraMatrixUnif, false, ref mm);
						GL.Uniform4(g_Unlit.baseColorUnif, 0.8078f, 0.8706f, 0.9922f, 1.0f);
						
						GL.UniformMatrix4(g_Unlit.cameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
	
						g_pCubeMesh.Render("flat");
					}
				}
			}
		}
		
		static ProjectionBlock projData = new ProjectionBlock();
		
		public override void reshape ()
		{
			MatrixStack persMatrix = new MatrixStack();
	        persMatrix.Perspective(45.0f, (width / (float)height), g_fzNear, g_fzFar);
	
	        projData.cameraToClipMatrix = persMatrix.Top();

	        GL.Viewport(0, 0, width, height);
		}
		
		public override String keyboard(Keys keyCode, int x, int y)
		{
			float last_g_fLightHeight = g_fLightHeight;
			float last_g_fLightRadius = g_fLightRadius;
			
			StringBuilder result = new StringBuilder();
	        switch (keyCode)
			{
				case Keys.Escape:
					g_pPlaneMesh = null;
					g_pCylinderMesh = null;
					g_pCubeMesh = null;
					break;
				
				case Keys.Space:
					g_bDrawColoredCyl = !g_bDrawColoredCyl;
					result.AppendLine("Inverting color vs no color");
					break;
			
				case Keys.I: g_fLightHeight += 0.2f; break;
				case Keys.K: g_fLightHeight -= 0.2f; break;
				case Keys.L: g_fLightRadius += 0.2f; break;
				case Keys.J: g_fLightRadius -= 0.2f; break;
			
				case Keys.M: g_fLightHeight += 0.05f; break;
				case Keys.N: g_fLightHeight -= 0.05f; break;
				case Keys.O: g_fLightRadius += 0.05f; break;
				case Keys.P: g_fLightRadius -= 0.05f; break;
			
				case Keys.Y: 
					g_bDrawLight = !g_bDrawLight; 
					result.AppendLine("g_bDrawLight = " + g_bDrawLight.ToString());
					break;
				case Keys.T: 
					g_bScaleCyl = !g_bScaleCyl;
					result.AppendLine("g_bScaleCyl = " + g_bScaleCyl.ToString());
					break;
				case Keys.H: 
					g_bUseFragmentLighting = !g_bUseFragmentLighting; 
					result.AppendLine("g_bUseFragmentLighting = " + g_bUseFragmentLighting.ToString());
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
			
			if (last_g_fLightHeight != g_fLightHeight)
			{
				result.AppendLine("last_g_fLightHeight = " + last_g_fLightHeight.ToString());
			}
			if (last_g_fLightRadius != g_fLightRadius)
			{
				result.AppendLine("last_g_fLightRadius = " + last_g_fLightRadius.ToString());
			}
		
			if(g_fLightRadius < 0.2f)
				g_fLightRadius = 0.2f;
			result.Append(keyCode);
	        reshape();
	        display();
	        return result.ToString();			
		}
	}
}

