using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_14_Basic_Textures : TutorialBase
	{
		public Tut_14_Basic_Textures ()
		{
		}
		
		class ProgramData
		{
			public int theProgram;
			public int modelToCameraMatrixUnif;
			public int normalModelToCameraMatrixUnif;
			public int cameraToClipMatrixUnif;
		};

		class UnlitProgData
		{
			public int theProgram;
			public int objectColorUnif;
			public int modelToCameraMatrixUnif;
			public int cameraToClipMatrixUnif;
		};

		static float g_fzNear = 1.0f;
		static float g_fzFar = 1000.0f;

		static ProgramData g_litShaderProg;
		static ProgramData g_litTextureProg;

		static UnlitProgData g_Unlit;

		static int g_materialBlockIndex = 0;
		static int g_lightBlockIndex = 1;
		static int g_projectionBlockIndex = 2;
		
		static int g_gaussTexUnit = 0;
		
		int NUMBER_OF_LIGHTS = 2;
		
		static UnlitProgData LoadUnlitProgram(String strVertexShader, String strFragmentShader)
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

		static ProgramData LoadStandardProgram(String strVertexShader, String strFragmentShader)
		{
		    ProgramData data = new ProgramData();
	        int vertex_shader = Shader.loadShader(ShaderType.VertexShader, strVertexShader);
	        int fragment_shader = Shader.loadShader(ShaderType.FragmentShader, strFragmentShader);
	        data.theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
			
			data.modelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");
			data.normalModelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "normalModelToCameraMatrix");
		
			int materialBlock = GL.GetUniformBlockIndex(data.theProgram, "Material");
			int lightBlock = GL.GetUniformBlockIndex(data.theProgram, "Light");
			
			data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");	
		
			GL.UniformBlockBinding(data.theProgram, materialBlock, g_materialBlockIndex);
			GL.UniformBlockBinding(data.theProgram, lightBlock, g_lightBlockIndex);
		
			int gaussianTextureUnif = GL.GetUniformLocation(data.theProgram, "gaussianTexture");
			GL.UseProgram(data.theProgram);
			GL.Uniform1(gaussianTextureUnif, g_gaussTexUnit);
			GL.UseProgram(0);
		
			return data;
		}

		static void InitializePrograms()
		{
			Shader.compileShader(ShaderType.VertexShader, VertexShaders.BasicTexture_PN);
			Shader.compileShader(ShaderType.FragmentShader, FragmentShaders.ShaderGaussian);
			
			g_litShaderProg = LoadStandardProgram(VertexShaders.BasicTexture_PN, FragmentShaders.ShaderGaussian);
			g_litTextureProg = LoadStandardProgram(VertexShaders.BasicTexture_PN, FragmentShaders.TextureGaussian);
		
			g_Unlit = LoadUnlitProgram(VertexShaders.PosTransform, FragmentShaders.ColorUniform_frag);
		}
		
	 	public static ObjectData g_initialObjectData = new ObjectData(new Vector3(0.0f, 0.5f, 0.0f),
	            new Quaternion(1.0f, 0.0f, 0.0f, 0.0f));

	    static ViewData g_initialViewData;
	
	    private static void InitializeGInitialViewData()
	    {
	        g_initialViewData = new ViewData(g_initialObjectData.position,
	                new Quaternion(0.92387953f, 0.3826834f, 0.0f, 0.0f),
	                10.0f,
	                0.0f);
	    }
		
  		static ViewScale g_viewScale;
	
	    private static void InitializeGViewScale()
	    {
	        g_viewScale = new ViewScale(
	               	1.5f, 70.0f,
					1.5f, 0.5f,
					0.0f, 0.0f,		//No camera movement.
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
		
		static ProjectionBlock projData = new ProjectionBlock();

		class MaterialBlock
		{
			public Vector4 diffuseColor;
			public Vector4 specularColor;
			public float specularShininess;
			public float[] padding = new float[3];
			
			public static int Size()
			{
				int size = 0;
				size += 2 * Vector4.SizeInBytes;
				size += sizeof(float) * 4;
				return size;
			}
			
			public float[] ToFloat()
		    {
				float[] result = new float[Size()/4];
				int position = 0;
				Array.Copy(diffuseColor.ToFloat(), 0, result, position, 4);
				position += 4;
				Array.Copy(specularColor.ToFloat(), 0, result, position, 4);
				position += 4;
				result[position] = specularShininess;
				return result;
			}
		};

		static Mesh g_pObjectMesh;
		static Mesh g_pCubeMesh;

		static int g_lightUniformBuffer = 0;
		static int g_projectionUniformBuffer = 0;
		static int g_materialUniformBuffer = 0;
		
		static int NUM_GAUSS_TEXTURES = 4;
		static uint[] g_gaussTextures = new uint[NUM_GAUSS_TEXTURES];
		
		static int g_gaussSampler = 0;
		
		static int g_imposterVAO;
		static int g_imposterVBO;
		
		float g_specularShininess = 0.2f;

		void BuildGaussianData(out byte[] textureData, int cosAngleResolution)
		{
			textureData = new byte[cosAngleResolution];
		
			for(int iCosAng = 0; iCosAng < cosAngleResolution; iCosAng++)
			{
				float cosAng = iCosAng / (float)(cosAngleResolution - 1);
				float angle = (float) Math.Acos(cosAng);
				float exponent = angle / g_specularShininess;
				exponent = -(exponent * exponent);
				float gaussianTerm = (float)Math.Exp(exponent);
		
				textureData[iCosAng] = (byte)(gaussianTerm * 255.0f);
			}
		}
		
		uint CreateGaussianTexture(int cosAngleResolution)
		{
			byte[] textureData;
			BuildGaussianData(out textureData, cosAngleResolution);
		
			uint gaussTexture;
			GL.GenTextures(1, out gaussTexture);
			GL.BindTexture(TextureTarget.Texture1D, gaussTexture);
			GL.TexImage1D(TextureTarget.Texture1D, 0, PixelInternalFormat.R8, cosAngleResolution, 0, PixelFormat.Red, PixelType.Byte, textureData);
			GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureBaseLevel, 0);
			GL.TexParameter(TextureTarget.Texture1D,TextureParameterName.TextureMaxLevel, 0);
			GL.BindTexture(TextureTarget.Texture1D, 0);
		
			return gaussTexture;
		}
		
		int CalcCosAngResolution(int level)
		{
			const int cosAngleStart = 64;
			return cosAngleStart * ((int)Math.Pow(2.0f, level));
		}
		
		void CreateGaussianTextures()
		{
			for(int loop = 0; loop < NUM_GAUSS_TEXTURES; loop++)
			{
				int cosAngleResolution = CalcCosAngResolution(loop);
				g_gaussTextures[loop] = CreateGaussianTexture(cosAngleResolution);
			}
		
			GL.GenSamplers(1, out g_gaussSampler);
			GL.SamplerParameter(g_gaussSampler, SamplerParameterName.TextureMagFilter,  (int)TextureMagFilter.Nearest);
			GL.SamplerParameter(g_gaussSampler, SamplerParameterName.TextureMinFilter,  (int)TextureMinFilter.Nearest);
			GL.SamplerParameter(g_gaussSampler, SamplerParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
		}
		
		
		//Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
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
	            Stream Infinity =  File.OpenRead(XmlFilesDirectory + @"/Infinity.xml");
				g_pObjectMesh = new Mesh(Infinity);
				Stream UnitCube =  File.OpenRead(XmlFilesDirectory + @"/UnitCube.xml");
				g_pCubeMesh = new Mesh(UnitCube);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error creating meshes " + ex.ToString());
				throw;
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
	        reshape();
		
			//Setup our Uniform Buffers
			MaterialBlock mtl = new MaterialBlock();
			mtl.diffuseColor = new Vector4(1.0f, 0.673f, 0.043f, 1.0f);
			mtl.specularColor = new Vector4(1.0f, 0.673f, 0.043f, 1.0f) * 0.4f;
			mtl.specularShininess = g_specularShininess;
		
			GL.GenBuffers(1, out g_materialUniformBuffer);
			GL.BindBuffer(BufferTarget.UniformBuffer, g_materialUniformBuffer);
			GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)MaterialBlock.Size(), mtl.ToFloat(), BufferUsageHint.StaticDraw);
		
			GL.GenBuffers(1, out g_lightUniformBuffer);
			GL.BindBuffer(BufferTarget.UniformBuffer, g_lightUniformBuffer);
			GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)LightBlock.Size(NUMBER_OF_LIGHTS), 
			              IntPtr.Zero, BufferUsageHint.DynamicDraw);
		
			GL.GenBuffers(1, out g_projectionUniformBuffer);
			GL.BindBuffer(BufferTarget.UniformBuffer, g_projectionUniformBuffer);
			GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)ProjectionBlock.Size(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
		
			//Bind the static buffers.
			GL.BindBufferRange(BufferTarget.UniformBuffer, g_lightBlockIndex, g_lightUniformBuffer, 
			                   IntPtr.Zero, (IntPtr)LightBlock.Size(NUMBER_OF_LIGHTS));
		
			GL.BindBufferRange(BufferTarget.UniformBuffer, g_projectionBlockIndex, g_projectionUniformBuffer, IntPtr.Zero, (IntPtr)ProjectionBlock.Size());
		
			GL.BindBufferRange(BufferTarget.UniformBuffer, g_materialBlockIndex, g_materialUniformBuffer, IntPtr.Zero, (IntPtr)MaterialBlock.Size());
		
			GL.BindBuffer(BufferTarget.UniformBuffer, 0);
		
			CreateGaussianTextures();
		}
	
		bool g_bDrawCameraPos = false;
		bool g_bDrawLights = true;
		bool g_bUseTexture = false;
		int g_currTexture = 0;
		
		float g_lightHeight = 1.0f;
		float g_lightRadius = 3.0f;
		
		Vector4 CalcLightPosition()
		{
			const float fLoopDuration = 5.0f;
			const float fScale = 3.14159f * 2.0f;
		
	        float fElapsedTime = GetElapsedTime() / 1000f;
	        float timeThroughLoop = fElapsedTime % fLoopDuration;
		
			Vector4 ret = new Vector4(0.0f, g_lightHeight, 0.0f, 1.0f);
		
			ret.X = (float)Math.Cos(timeThroughLoop * fScale) * g_lightRadius;
			ret.Z = (float)Math.Sin(timeThroughLoop * fScale) * g_lightRadius;
		
			return ret;
		}
		
		const float g_fHalfLightDistance = 25.0f;
		const float g_fLightAttenuation = 1.0f / (g_fHalfLightDistance * g_fHalfLightDistance);
		
	    public override void display()
		{
			//g_lightTimer.Update();
		
		    GL.ClearColor(0.75f, 0.75f, 1.0f, 1.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		
			if((g_pObjectMesh != null) && (g_pCubeMesh != null))
			{
				MatrixStack modelMatrix = new MatrixStack();
				modelMatrix.SetMatrix(g_viewPole.CalcMatrix());
				Matrix4 worldToCamMat = modelMatrix.Top();
		
				LightBlock lightData = new LightBlock();
		
				lightData.ambientIntensity = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
				lightData.lightAttenuation = g_fLightAttenuation;
		
				Vector3 globalLightDirection = new Vector3(0.707f, 0.707f, 0.0f);
		
				lightData.lights[0].cameraSpaceLightPos = 
					Vector4.Transform(new Vector4(globalLightDirection, 0.0f), worldToCamMat);
				lightData.lights[0].lightIntensity = new Vector4(0.6f, 0.6f, 0.6f, 1.0f);
		
				lightData.lights[1].cameraSpaceLightPos = Vector4.Transform(CalcLightPosition(), worldToCamMat);
				lightData.lights[1].lightIntensity = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);
		
				GL.BindBuffer(BufferTarget.UniformBuffer, g_lightUniformBuffer);
				GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)0, (IntPtr)LightBlock.Size(NUMBER_OF_LIGHTS), 
				                 lightData.ToFloat());
				GL.BindBuffer(BufferTarget.UniformBuffer, 0);
		
				using ( PushStack pushstack = new PushStack(modelMatrix))
				{
					GL.BindBufferRange(BufferTarget.UniformBuffer, g_materialBlockIndex, g_materialUniformBuffer,
						IntPtr.Zero, (IntPtr)MaterialBlock.Size());
		
					modelMatrix.ApplyMatrix(g_objtPole.CalcMatrix());
					modelMatrix.Scale(2.0f);
		
					Matrix3 normMatrix = new Matrix3(modelMatrix.Top());
					//normMatrix = glm::transpose(glm::inverse(normMatrix));
		
					ProgramData prog = g_bUseTexture ? g_litTextureProg : g_litShaderProg;
		
					GL.UseProgram(prog.theProgram);
					Matrix4 mm = modelMatrix.Top();
					GL.UniformMatrix4(prog.modelToCameraMatrixUnif, false, ref mm);
					GL.UniformMatrix3(prog.normalModelToCameraMatrixUnif, false, ref normMatrix);
		
					GL.ActiveTexture(TextureUnit.Texture0 + g_gaussTexUnit);
					GL.BindTexture(TextureTarget.Texture1D, g_gaussTextures[g_currTexture]);
					GL.BindSampler(g_gaussTexUnit, g_gaussSampler);
		
					g_pObjectMesh.Render("lit");
		
					GL.BindSampler(g_gaussTexUnit, 0);
					GL.BindTexture(TextureTarget.Texture1D, 0);
		
					GL.UseProgram(0);
					GL.BindBufferBase(BufferTarget.UniformBuffer, g_materialBlockIndex, 0);
				}
		
				if(g_bDrawLights)
				{
					using (PushStack pushstack = new PushStack(modelMatrix))
					{
						modelMatrix.Translate(new Vector3(CalcLightPosition()));
						modelMatrix.Scale(0.25f);
			
						GL.UseProgram(g_Unlit.theProgram);
						Matrix4 mm = modelMatrix.Top();
						GL.UniformMatrix4(g_Unlit.modelToCameraMatrixUnif, false, ref mm);
			
						Vector4 lightColor = new Vector4(1f, 1f, 1f, 1f);
						GL.Uniform4(g_Unlit.objectColorUnif, ref lightColor);
						g_pCubeMesh.Render("flat");	
					}
		
					modelMatrix.Translate(globalLightDirection * 100.0f);
					modelMatrix.Scale(5.0f);
		
					Matrix4 mm2 = modelMatrix.Top();
					GL.UniformMatrix4(g_Unlit.modelToCameraMatrixUnif, false, ref mm2);
					g_pCubeMesh.Render("flat");
		
					GL.UseProgram(0);
				}
		
				if(g_bDrawCameraPos)
				{
					using (PushStack pushstack = new PushStack(modelMatrix))
					{
						modelMatrix.SetIdentity();
						modelMatrix.Translate(new Vector3(0.0f, 0.0f, -g_viewPole.GetView().radius));
						modelMatrix.Scale(0.25f);
		
						GL.Disable(EnableCap.DepthTest);
						GL.DepthMask(false);
						GL.UseProgram(g_Unlit.theProgram);
						Matrix4 mm = modelMatrix.Top();
						GL.UniformMatrix4(g_Unlit.modelToCameraMatrixUnif, false, ref mm);
						GL.Uniform4(g_Unlit.objectColorUnif, 0.25f, 0.25f, 0.25f, 1.0f);
						g_pCubeMesh.Render("flat");
						GL.DepthMask(true);
						GL.Enable(EnableCap.DepthTest);
						GL.Uniform4(g_Unlit.objectColorUnif, 1.0f, 1.0f, 1.0f, 1.0f);
						g_pCubeMesh.Render("flat");
					}
				}
			}
		
			//glutPostRedisplay();
			//glutSwapBuffers();
		}

		void reshape (int w, int h)
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(45.0f, (w / (float)h), g_fzNear, g_fzFar);
		
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
					g_pObjectMesh = null;
					g_pCubeMesh = null;
					break;
			
				//case Keys.P: g_lightTimer.TogglePause(); break;
				//case Keys.Subtract: g_lightTimer.Rewind(0.5f); break;
				//case Keys.Add: g_lightTimer.Fastforward(0.5f); break;
				case Keys.T: g_bDrawCameraPos = !g_bDrawCameraPos; break;
				case Keys.G: g_bDrawLights = !g_bDrawLights; break;
				case Keys.Space:
					g_bUseTexture = !g_bUseTexture;
					if(g_bUseTexture)
						result.AppendLine("Texture\n");
					else
						result.AppendLine("Shader\n");
					break;
				case Keys.D1: g_currTexture = 0; break;
				case Keys.D2: g_currTexture = 1; break;
				case Keys.D3: g_currTexture = 2; break;
				case Keys.D4: g_currTexture = 3; break;
				case Keys.D5: g_currTexture = 4; break;
				case Keys.D6: g_currTexture = 5; break;
				case Keys.D7: g_currTexture = 6; break;
				case Keys.D8: g_currTexture = 7; break;
				case Keys.D9: g_currTexture = 8; break;
			}
			result.AppendLine("Angle Resolution:  " + CalcCosAngResolution(g_currTexture).ToString());
			return result.ToString();
		}
	}
		
}
