using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Projected_Light_Test : TutorialBase
	{
		static int NUMBER_OF_LIGHTS = 2;
		bool renderWithString = false;
		List<string> renderStrings = new List<string>();
		int renderString = 0;
		Vector3 initialScale = new Vector3(50f, 50f, 50f);
		Vector3 scaleFactor = new Vector3(10f, 10f, 10f);
		Vector3 translateVector = new Vector3(0f, 0f, 0f);
		Vector3 cameraSpaceProjLightPos= new Vector3(0.0f, 0.0f, 1.0f);
		Vector4 ambientIntensity = new Vector4(0.1f, 0.1f, 0.1f, 1.0f);
		Vector4 redLight = new Vector4(0.3f, 0.0f, 0.0f, 1.0f);
		Vector4 greenLight = new Vector4(0.0f, 0.5f, 0.0f, 1.0f);

		Matrix4 modelToCameraMatrix = Matrix4.Identity;
		Matrix4 cameraToLightProjMatrix = Matrix4.Identity;

		MatrixStack cameraMatrixStack = new MatrixStack();

		class ProgramData
		{
			public string name = "";
			public int theProgram;
			public int positionAttribute;
			public int colorAttribute;
			public int modelToCameraMatrixUnif;
			public int modelToWorldMatrixUnif;
			public int worldToCameraMatrixUnif;
			public int cameraToClipMatrixUnif;
			public int baseColorUnif;

			public int normalModelToCameraMatrixUnif;
			public int dirToLightUnif;
			public int lightIntensityUnif;
			public int ambientIntensityUnif;
			public int normalAttribute;

			public LightBlock lightBlock;

			public int diffuseColorTexUnif;
			public int lightProjTexUnif;
			public int cameraToLightProjMatrixUnif;
			public int cameraSpaceProjLightPosUnif;

			// TEST		
			public override string ToString()
			{
				StringBuilder result = new StringBuilder();
				result.AppendLine("theProgram = " + theProgram.ToString());
				result.AppendLine("positionAttribute = " + positionAttribute.ToString());
				result.AppendLine("colorAttribute = " + colorAttribute.ToString());
				result.AppendLine("modelToCameraMatrixUnif = " + modelToCameraMatrixUnif.ToString());
				result.AppendLine("modelToWorldMatrixUnif = " + modelToWorldMatrixUnif.ToString());
				result.AppendLine("worldToCameraMatrixUnif = " + worldToCameraMatrixUnif.ToString());
				result.AppendLine("cameraToClipMatrixUnif = " + cameraToClipMatrixUnif.ToString());
				result.AppendLine("baseColorUnif = " + baseColorUnif.ToString());
				result.AppendLine("normalModelToCameraMatrixUnif = " + normalModelToCameraMatrixUnif.ToString());
				result.AppendLine("dirToLightUnif = " + dirToLightUnif.ToString());
				result.AppendLine("lightIntensityUnif = " + lightIntensityUnif.ToString());
				result.AppendLine("ambientIntensityUnif = " + ambientIntensityUnif.ToString());
				result.AppendLine("normalAttribute = " + normalAttribute.ToString());
				return result.ToString();
			}
		};

		static float g_fzNear = 10.0f;
		static float g_fzFar = 1000.0f;

		int currentProgram = 0;
		List<ProgramData> programs = new List<ProgramData>();

		Vector3 dirToLight = new Vector3(0.0f, 0.0f, 1f);

		float perspectiveAngle = 60f;
		float newPerspectiveAngle = 60f;

		ProgramData LoadProgram(string programSetString)
		{
			ProgramSet programSet = ProgramSets.Find(programSetString);
			ProgramData data = new ProgramData();
			data.name = programSet.name;
			int vertex_shader = Shader.compileShader(ShaderType.VertexShader, programSet.vertexShader);
			int fragment_shader = Shader.compileShader(ShaderType.FragmentShader, programSet.fragmentShader);
			data.theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);

			data.positionAttribute = GL.GetAttribLocation(data.theProgram, "position");
			data.colorAttribute = GL.GetAttribLocation(data.theProgram, "color");
			if (data.positionAttribute != -1) 
			{
				if (data.positionAttribute != 0)
				{
					MessageBox.Show("These meshes only work with position at location 0 " + programSet.vertexShader);
				}
			}
			if (data.colorAttribute != -1) 
			{
				if (data.colorAttribute != 1)
				{
					MessageBox.Show("These meshes only work with color at location 1" + programSet.vertexShader);
				}
			}

			data.modelToWorldMatrixUnif = GL.GetUniformLocation(data.theProgram, "modelToWorldMatrix");
			data.worldToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "worldToCameraMatrix");
			data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");
			if (data.cameraToClipMatrixUnif == -1)
			{
				data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "Projection.cameraToClipMatrix");
			}
			data.baseColorUnif = GL.GetUniformLocation(data.theProgram, "baseColor");
			if (data.baseColorUnif == -1)
			{
				data.baseColorUnif = GL.GetUniformLocation(data.theProgram, "objectColor");
			}

			data.modelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");

			data.normalModelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "normalModelToCameraMatrix");
			data.dirToLightUnif =  GL.GetUniformLocation(data.theProgram, "dirToLight");
			data.lightIntensityUnif = GL.GetUniformLocation(data.theProgram, "lightIntensity");
			data.ambientIntensityUnif = GL.GetUniformLocation(data.theProgram, "ambientIntensity");
			data.normalAttribute = GL.GetAttribLocation(data.theProgram, "normal");

			data.diffuseColorTexUnif = GL.GetUniformLocation(data.theProgram, "diffuseColorTex");
			data.lightProjTexUnif = GL.GetUniformLocation(data.theProgram, "lightProjTex");
			data.cameraToLightProjMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToLightProjMatrix");
			data.cameraSpaceProjLightPosUnif = GL.GetUniformLocation(data.theProgram, "cameraSpaceProjLightPos");

			return data;
		}

		private void UpdateLightBlock()
		{
			programs[currentProgram].lightBlock.ambientIntensity = ambientIntensity;

			programs[currentProgram].lightBlock.lights[0].cameraSpaceLightPos = new Vector4(1.0f, 0.0f, 1.0f, 1.0f);
			programs[currentProgram].lightBlock.lights[0].lightIntensity = redLight;

			programs[currentProgram].lightBlock.lights[1].cameraSpaceLightPos = new Vector4(0.0f, 1.0f, 1.0f, 1.0f);
			programs[currentProgram].lightBlock.lights[1].lightIntensity = greenLight;

			programs[currentProgram].lightBlock.maxIntensity = 0.5f;

			programs[currentProgram].lightBlock.UpdateInternal();
		}

		void InitializeProgram()
		{
			ProgramData projlight = LoadProgram("projlight");

			// Test shader lights and materials
			GL.UseProgram(projlight.theProgram);
			projlight.lightBlock = new LightBlock(NUMBER_OF_LIGHTS);
			projlight.lightBlock.SetUniforms(projlight.theProgram);
			programs.Add(projlight);
			UpdateLightBlock();
		}
		int currentMesh = 0;
		List<Mesh> meshes = new List<Mesh>();

		int currentTexture;
		int lightTexture;
		private void CreateTextures()
		{
			GL.Enable(EnableCap.Texture2D);
			currentTexture = Textures.CreateMipMapTexture("wood4_rotate.png", 6);
			lightTexture = Textures.Load("flashlight.png");
		}
			
		protected override void init()
		{
			InitializeProgram();
			GL.Enable(EnableCap.Texture2D);
			CreateTextures();
			MatrixStack.rightMultiply = false;
			try 
			{
				meshes.Add(new Mesh("SceneVersions/unitcube.xml"));
				meshes.Add(new Mesh("unitcubecolor.xml"));
				meshes.Add(new Mesh("unitcylinder.xml"));
				meshes.Add(new Mesh("unitplane.xml"));
				meshes.Add(new Mesh("infinity.xml"));
				meshes.Add(new Mesh("unitsphere12.xml"));
				meshes.Add(new Mesh("unitcylinder9.xml"));
				meshes.Add(new Mesh("unitdiorama.xml"));
				meshes.Add(new Mesh("ground.xml"));
				renderStrings.Add("lit");
				renderStrings.Add("lit-color");
				renderStrings.Add("color");
				renderStrings.Add("lit-tex");
				renderStrings.Add("lit-color-tex");
				renderStrings.Add("color-tex");
				renderStrings.Add("tex");
				renderStrings.Add("flat");
			} catch (Exception ex) {
				throw new Exception("Error " + ex.ToString());
			}

			SetupDepthAndCull();

			cameraMatrixStack.Reset();
			cameraMatrixStack.Translate(0.5f, 0.5f, 0.0f);
		}

		static ViewData g_initialView = new ViewData
			(
				new Vector3(0.0f, 0.0f, 1.0f),
				Quaternion.FromAxisAngle(new Vector3(1f, 0f, 0f), 0f),
				25.0f,
				180.0f
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
				new Quaternion(1.0f, 0.0f, 0.0f, 0.0f),
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

		Matrix4 cameraMatrix;

		public override void display()
		{
			ClearDisplay();

			cameraMatrix = g_viewPole.CalcMatrix();
			Matrix4 lightView = g_lightViewPole.CalcMatrix();

			if (meshes[currentMesh] != null)
			{
				MatrixStack modelMatrix = new MatrixStack();
				using (PushStack pushstack = new PushStack(modelMatrix)) 
				{
					modelMatrix.Rotate(axis, angle);   // rotate last to leave in place
					//modelMatrix.Translate(Camera.g_camTarget);
					//modelMatrix.Scale(initialScale.X / scaleFactor.X, 
					//	initialScale.Y / scaleFactor.Y,
					//	initialScale.Z / scaleFactor.Z);

					GL.UseProgram(programs[currentProgram].theProgram);

					GL.UniformMatrix4(programs[currentProgram].cameraToLightProjMatrixUnif, false, ref cameraToLightProjMatrix);
									
					GL.Uniform3(programs[currentProgram].cameraSpaceProjLightPosUnif, cameraSpaceProjLightPos);


					GL.Uniform1(programs[currentProgram].diffuseColorTexUnif, 0);
					GL.Uniform1(programs[currentProgram].lightProjTexUnif, 1);

					GL.ActiveTexture(TextureUnit.Texture0);
					GL.BindTexture(TextureTarget.Texture2D, currentTexture);

					GL.ActiveTexture(TextureUnit.Texture1);
					GL.BindTexture(TextureTarget.Texture2D, lightTexture);

					cameraMatrix = cameraMatrixStack.Top();

					modelToCameraMatrix = Matrix4.Mult(Matrix4.Identity, cameraMatrix);
					GL.UniformMatrix4(programs[currentProgram].modelToCameraMatrixUnif, false, ref modelToCameraMatrix);
					if (programs[currentProgram].normalModelToCameraMatrixUnif != 0)
					{
						Matrix3 normalModelToCameraMatrix = Matrix3.Identity;
						Matrix4 applyMatrix = Matrix4.Mult(Matrix4.Identity,
							Matrix4.CreateTranslation(dirToLight));
						normalModelToCameraMatrix = new Matrix3(applyMatrix);
						normalModelToCameraMatrix.Invert();
						GL.UniformMatrix3(programs[currentProgram].normalModelToCameraMatrixUnif, false, 
							ref normalModelToCameraMatrix);
						Matrix4 cameraToClipMatrix = Matrix4.Identity;
						GL.UniformMatrix4(programs[currentProgram].cameraToClipMatrixUnif, false, ref cameraToClipMatrix); 

					}
				}
				if (renderWithString)
				{
					try
					{
						meshes[currentMesh].Render(renderStrings[renderString]);
					}
					catch (Exception ex)
					{
						renderWithString = false;
						MessageBox.Show("Error displaying mesh wih render string " + renderStrings[renderString] + " " + ex.ToString());
					}
				}
				else
				{
					meshes[currentMesh].Render();
				}
				GL.UseProgram(0);
				if (perspectiveAngle != newPerspectiveAngle)
				{
					perspectiveAngle = newPerspectiveAngle;
					reshape();
				}
			}
		}

		static Vector3 axis = new Vector3(1f, 1f, 0);
		static float angle = 0;

		static Matrix4 pm;
		static Matrix4 cm;

		static private void SetGlobalMatrices(ProgramData program)
		{
			GL.UseProgram(program.theProgram);
			GL.UniformMatrix4(program.cameraToClipMatrixUnif, false, ref pm);  // this one is first
			GL.UniformMatrix4(program.worldToCameraMatrixUnif, false, ref cm); // this is the second one
			GL.UseProgram(0);
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			switch (keyCode) {
			case Keys.NumPad6:
				cameraMatrixStack.Translate(new Vector3(0.1f, 0.0f, 0.0f));
				break;
			case Keys.NumPad4:
				cameraMatrixStack.Translate(new Vector3(-0.1f, 0.0f, 0.0f));
				break;
			case Keys.NumPad8:
				cameraMatrixStack.Translate(new Vector3(0.0f, 0.1f, 0.0f));
				break;
			case Keys.NumPad2:
				cameraMatrixStack.Translate(new Vector3(0.0f, -0.1f, 0.0f));
				break;
			case Keys.NumPad7:
				cameraMatrixStack.Translate(new Vector3(0.0f, 0.0f, 0.1f));
				break;
			case Keys.NumPad3:
				cameraMatrixStack.Translate(new Vector3(0.0f, 0.0f, -0.1f));
				break;
			case Keys.D1:
				cameraMatrixStack.RotateX(5f);
				break;	
			case Keys.D2:
				cameraMatrixStack.RotateY(5f);
				break;		
			case Keys.D3:
				cameraMatrixStack.RotateZ(5f);
				break;
			case Keys.D8:
				ambientIntensity.W = 1f;
				ambientIntensity += new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
				if (ambientIntensity.X > 0.8f)
				{
					ambientIntensity = new Vector4(0f, 0f, 0f, 1.0f);
				}
				result.AppendLine("ambientIntensity = " + ambientIntensity.ToString());
				UpdateLightBlock();
				break;	
			case Keys.D9:
				redLight.W = 1f;
				redLight += new Vector4(0.1f, 0.0f, 0.0f, 0.0f);
				if (redLight.X > 0.8f)
				{
					redLight = new Vector4(0f, 0f, 0f, 1.0f);
				}
				result.AppendLine("redLight = " + redLight.ToString());
				UpdateLightBlock();
				break;			
			case Keys.D0:
				greenLight.W = 1f;
				greenLight += new Vector4(0.0f, 0.1f, 0.0f, 0.0f);
				if (greenLight.Y > 0.8f)
				{
					greenLight = new Vector4(0f, 0f, 0f, 1.0f);
				}
				result.AppendLine("greenLight = " + greenLight.ToString());
				UpdateLightBlock();
				break;

			case Keys.V:
				newPerspectiveAngle = perspectiveAngle + 5f;
				if (newPerspectiveAngle > 120f)
				{
					newPerspectiveAngle = 30f;
				}
				break;

			case Keys.S:
				renderWithString = true;
				renderString++;
				if (renderString > renderStrings.Count - 1) renderString = 0;
				result.AppendLine("Render String = " + renderStrings[renderString]);
				break;
			case Keys.M:
				renderWithString = false;
				currentMesh++;
				if (currentMesh > meshes.Count - 1) currentMesh = 0;
				result.AppendLine("Mesh = " + meshes[currentMesh].fileName);
				break;
			case Keys.P:
				currentProgram++;
				if (currentProgram > programs.Count - 1) currentProgram = 0;
				result.AppendLine("Program = " + programs[currentProgram].name);
				break;
			case Keys.O:
				scaleFactor = meshes[currentMesh].GetUnitScaleFactor();
				result.Append(scaleFactor.ToString());
				break;
			case Keys.X:
				cameraSpaceProjLightPos.X += 1f;
				break;
			case Keys.Y:
				cameraSpaceProjLightPos.Y += 1f;
				break;
			case Keys.Z:
				cameraSpaceProjLightPos.Z += 1f;
				break;
			case Keys.I:
				result.AppendLine("CameraMatrix " + cameraMatrix.ToString());
				result.AppendLine(AnalysisTools.CalculateMatrixEffects(cameraMatrix));
				break;
			case Keys.C:
				GL.FrontFace(FrontFaceDirection.Cw);
				result.AppendLine("FrontFaceDirection.Cw");
				break;
			case Keys.D:
				GL.FrontFace(FrontFaceDirection.Ccw);
				result.AppendLine("FrontFaceDirection.Ccw");
				break;
			}

			reshape();
			display();
			return result.ToString();
		}
	}
}

