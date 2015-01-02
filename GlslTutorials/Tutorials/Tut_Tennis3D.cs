using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Tennis3D : TutorialBase 
	{
		bool drawWalls = true;
		Random random = new Random();
		int playerNumber = 0;
		int lastPlayer = 5;
		Ball ball; 
		float ballRadius = 5f;
		float ballSpeedFactor = 25f;
		static float ballLimit = 50f;
		Vector3 ballLimitLow = new Vector3(-ballLimit, -ballLimit, -ballLimit);
		Vector3 ballLimitHigh = new Vector3(ballLimit, ballLimit, ballLimit);
		Vector3 ballSpeed;
		int ballProgram;
		Vector3[] playerRotations = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(90f, 0f, 0f),
			new Vector3(-90f, 0f, 0f),
			new Vector3(0f, 90f, 0f),
			new Vector3(0f, -90f, 0f),
			new Vector3(0f, 0f, 180f),

		};
		bool cull = true;
		bool rotateWorld = false;
		static float g_fzNear = 10.0f;
		static float g_fzFar = 1000.0f;

		float perspectiveAngle = 60f;
		float newPerspectiveAngle = 60f;

		static int NUMBER_OF_LIGHTS = 2;
		bool pause = false;
		Vector3 position = new Vector3(0f, 0f, 0f);
		Vector3 velocity = new Vector3(0.1f, 0.15f, 0.05f);
		Vector3 positionLimitLow =  new Vector3(-50f, -50f, -100f);
		Vector3 positionLimitHigh =  new Vector3(50f, 50f, 100f);
		Matrix4 ballModelMatrix = Matrix4.Identity;
		bool renderWithString = false;
		string renderString = "";
		PaintWall frontWall = new PaintWall();
		PaintWall backWall = new PaintWall();
		PaintWall leftWall = new PaintWall();
		PaintWall rightWall = new PaintWall();
		PaintWall topWall = new PaintWall();
		PaintWall bottomWall = new PaintWall();

	    class ProgramData
	    {
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
			public MaterialBlock materialBlock;

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
	
	    static ProgramData UniformColor;
	    static ProgramData ObjectColor;
	    static ProgramData UniformColorTint;
	
	    static ProgramData g_VertexDiffuseColor;
	    static ProgramData g_WhiteAmbDiffuseColor;
		static ProgramData g_Unlit;
		static ProgramData g_litShaderProg;
	
	
	    static ProgramData currentProgram;
		
		static Vector4 g_lightDirection = new Vector4(0.866f, 0.5f, 0.0f, 0.0f);
		Vector3 dirToLight = new Vector3(0.5f, 0.5f, 1f);
	
	    ProgramData LoadProgram(String strVertexShader, String strFragmentShader)
	    {
	        ProgramData data = new ProgramData();
	        int vertex_shader = Shader.compileShader(ShaderType.VertexShader, strVertexShader);
	        int fragment_shader = Shader.compileShader(ShaderType.FragmentShader, strFragmentShader);
	        data.theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
	
	        data.positionAttribute = GL.GetAttribLocation(data.theProgram, "position");
	        data.colorAttribute = GL.GetAttribLocation(data.theProgram, "color");
			if (data.positionAttribute != -1) 
			{
				if (data.positionAttribute != 0)
				{
					MessageBox.Show("These meshes only work with position at location 0 " + strVertexShader);
				}
			}
			if (data.colorAttribute != -1) 
			{
				if (data.colorAttribute != 1)
				{
					MessageBox.Show("These meshes only work with color at location 1" + strVertexShader);
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
	
	        return data;
	    }
	
	    void InitializeProgram()
	    {
	        UniformColor = LoadProgram(VertexShaders.PosOnlyWorldTransform_vert, FragmentShaders.ColorUniform_frag);
	        GL.UseProgram(UniformColor.theProgram);
	        GL.Uniform4(UniformColor.baseColorUnif, 0.694f, 0.4f, 0.106f, 1.0f);
	        GL.UseProgram(0);
	
	        UniformColorTint = LoadProgram(VertexShaders.PosColorWorldTransform_vert, FragmentShaders.ColorMultUniform_frag);
	        GL.UseProgram(UniformColorTint.theProgram);
	        GL.Uniform4(UniformColorTint.baseColorUnif, 0.5f, 0.5f, 0f, 1.0f);
	        GL.UseProgram(0);
			
	        g_WhiteAmbDiffuseColor = LoadProgram(VertexShaders.DirAmbVertexLighting_PN_vert, 
			                                     FragmentShaders.ColorPassthrough_frag);
	        
			g_VertexDiffuseColor = LoadProgram(VertexShaders.DirVertexLighting_PCN, 
			                                   FragmentShaders.ColorPassthrough_frag);
				
			g_Unlit = LoadProgram(VertexShaders.unlit, FragmentShaders.unlit);

			g_litShaderProg = LoadProgram(VertexShaders.BasicTexture_PN, FragmentShaders.ShaderGaussian);

			GL.UseProgram(g_VertexDiffuseColor.theProgram);
			Vector4 lightIntensity = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
			GL.Uniform3(g_VertexDiffuseColor.dirToLightUnif, ref dirToLight);
	        GL.Uniform4(g_VertexDiffuseColor.lightIntensityUnif, ref lightIntensity);
			GL.UseProgram(0);
			
			GL.UseProgram(g_WhiteAmbDiffuseColor.theProgram);
	        Vector3 light_direction = new Vector3(10f, 10f, 0f);
	        Vector4 light_intensity = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
	        Vector4  ambient_intensity = new Vector4(0.3f, 0.0f, 0.3f, 0.6f);
	        GL.Uniform3(g_WhiteAmbDiffuseColor.dirToLightUnif, ref light_direction);
	        GL.Uniform4(g_WhiteAmbDiffuseColor.lightIntensityUnif, ref light_intensity);
	        GL.Uniform4(g_WhiteAmbDiffuseColor.ambientIntensityUnif, ref ambient_intensity);
	        Matrix3 m = Matrix3.Identity;
	        GL.UniformMatrix3(g_WhiteAmbDiffuseColor.normalModelToCameraMatrixUnif, false, ref m);
	        GL.UseProgram(0);

			GL.UseProgram(g_Unlit.theProgram);
			GL.Uniform4(g_Unlit.baseColorUnif, 0.5f, 0.5f, 0f, 1.0f);
			Matrix4 test = Matrix4.Identity;
			GL.UniformMatrix4(g_Unlit.cameraToClipMatrixUnif, false, ref test);
			GL.UseProgram(0);

			// Test shader lights and materials
			GL.UseProgram(g_litShaderProg.theProgram);
			g_litShaderProg.lightBlock = new LightBlock(NUMBER_OF_LIGHTS);
			g_litShaderProg.lightBlock.SetUniforms(g_litShaderProg.theProgram);

			g_litShaderProg.lightBlock.ambientIntensity = new Vector4(0.1f, 0.1f, 0.1f, 1.0f);

			g_litShaderProg.lightBlock.lights[0].cameraSpaceLightPos = new Vector4(4.0f, 0.0f, 1.0f, 1.0f);
			g_litShaderProg.lightBlock.lights[0].lightIntensity = new Vector4(0.7f, 0.0f, 0.0f, 1.0f);

			g_litShaderProg.lightBlock.lights[1].cameraSpaceLightPos = new Vector4(4.0f, 0.0f, 1.0f, 1.0f);
			g_litShaderProg.lightBlock.lights[1].lightIntensity = new Vector4(0.0f, 0.0f, 0.7f, 1.0f);

			g_litShaderProg.lightBlock.UpdateInternal();

			g_litShaderProg.materialBlock = new MaterialBlock(new Vector4(0.0f, 0.3f, 0.0f, 1.0f),
				new Vector4(0.5f, 0.0f, 0.5f, 1.0f), 0.6f);
			g_litShaderProg.materialBlock.SetUniforms(g_litShaderProg.theProgram);
			g_litShaderProg.materialBlock.UpdateInternal();

			GL.UseProgram(0);

	        ObjectColor = LoadProgram(VertexShaders.PosColorWorldTransform_vert, FragmentShaders.ColorPassthrough_frag);
	        currentProgram = ObjectColor;

			ballProgram = Programs.AddProgram(VertexShaders.PosColorWorldTransform_vert, FragmentShaders.ColorPassthrough_frag);
	    }
	    static Mesh current_mesh;
		static Mesh g_unitSphereMesh;
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
			InitializeProgram();

			ball = new Ball(ballRadius);
			ball.SetLimits(ballLimitLow, ballLimitHigh);
			ballSpeed = new Vector3(
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble(), 
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble(), 
				ballSpeedFactor + ballSpeedFactor * (float)random.NextDouble());
			ball.SetSpeed(ballSpeed);
			//ball.SetProgram(ballProgram);

	        try 
	        {
				g_unitSphereMesh = new Mesh("unitsphere12.xml");
				
	        } catch (Exception ex) {
	            throw new Exception("Error " + ex.ToString());
	        }
	        
			SetupDepthAndCull();
			Textures.EnableTextures();

			Camera.Move(0f, 0f, 0f);
	        Camera.MoveTarget(0f, 0f, 0.0f);
	        reshape();
			current_mesh = g_unitSphereMesh;
			frontWall.Move(0f, 0f, 1f);
			frontWall.Scale(50f);

			backWall.Move(0f, 0f, -1f);
			backWall.Scale(50f);

			leftWall.Move(-1f, 0f, 0f);
			leftWall.Scale(50f);
			leftWall.RotateShape(Vector3.UnitY, 90f);
			rightWall.Move(1f, 0f, 0f);
			rightWall.Scale(50f);
			rightWall.RotateShape(Vector3.UnitY, -90f);

			topWall.Move(0f, 1f, 0f);
			topWall.Scale(50f);
			topWall.RotateShape(Vector3.UnitX, 90f);
			bottomWall.Move(0f, -1f, 0f);
			bottomWall.Scale(50f);
			bottomWall.RotateShape(Vector3.UnitX, -90f);
	    }
	
	    public override void display()
	    {
			GL.Disable(EnableCap.DepthTest);
	        ClearDisplay();
			if (drawWalls) backWall.Draw();
			if (drawWalls) leftWall.Draw();
			if (drawWalls) rightWall.Draw();
			if (drawWalls) topWall.Draw();
			if (drawWalls) bottomWall.Draw();

			ball.Draw();
	
	        if (current_mesh != null)
	        {
	            MatrixStack modelMatrix = new MatrixStack();
                using (PushStack pushstack = new PushStack(modelMatrix)) 
				{
					modelMatrix.Rotate(axis, angle);   // rotate last to leave in place
                    modelMatrix.Translate(position);
                    modelMatrix.Scale(15.0f, 15.0f, 15.0f);
					ballModelMatrix = modelMatrix.Top();

                    GL.UseProgram(currentProgram.theProgram);
                    Matrix4 mm = modelMatrix.Top();

                    if (noWorldMatrix) 
					{
						Matrix4 cm2 = Matrix4.Mult(mm, worldToCameraMatrix);
                        GL.UniformMatrix4(currentProgram.modelToCameraMatrixUnif, false, ref cm2);
						if (currentProgram.normalModelToCameraMatrixUnif != 0)
						{
							Matrix3 normalModelToCameraMatrix = Matrix3.Identity;
							Matrix4 applyMatrix = Matrix4.Mult(Matrix4.Identity,
							                                         Matrix4.CreateTranslation(dirToLight));
							normalModelToCameraMatrix = new Matrix3(applyMatrix);
							normalModelToCameraMatrix.Invert();
							GL.UniformMatrix3(currentProgram.normalModelToCameraMatrixUnif, false, 
							                  ref normalModelToCameraMatrix);
							//Matrix4 cameraToClipMatrix = Matrix4.Identity;
							//GL.UniformMatrix4(currentProgram.cameraToClipMatrixUnif, false, ref cameraToClipMatrix); 
                   
						}
						//Matrix4 cameraToClipMatrix = Matrix4.Identity;
						//GL.UniformMatrix4(currentProgram.cameraToClipMatrixUnif, false, ref cameraToClipMatrix); 
                    } 
					else 
					{
                        GL.UniformMatrix4(currentProgram.modelToWorldMatrixUnif, false, ref mm);
                    }
                }
				if (renderWithString)
				{
					current_mesh.Render(renderString);
				}
				else
				{
                	current_mesh.Render();
				}
                GL.UseProgram(0);
				if (perspectiveAngle != newPerspectiveAngle)
				{
					perspectiveAngle = newPerspectiveAngle;
					reshape();
				}
	        }
			if (drawWalls) frontWall.Draw();
			if (pause == false)
			{
				UpdatePosition();
				if (rotateWorld)
				{
					RotateWorldSub();
				}
			}
	    }

		private void RotateWorldSub()
		{
			Matrix4 rotX = Matrix4.CreateRotationX(0.05f * (float)random.NextDouble());
			Matrix4 rotY = Matrix4.CreateRotationY(0.05f * (float)random.NextDouble());
			Matrix4 rotZ = Matrix4.CreateRotationZ(0.05f * (float)random.NextDouble());
			//worldToCameraMatrix = Matrix4.Mult(worldToCameraMatrix, rot);
			worldToCameraMatrix = Matrix4.Mult(rotX, worldToCameraMatrix);
			worldToCameraMatrix = Matrix4.Mult(rotY, worldToCameraMatrix);
			worldToCameraMatrix = Matrix4.Mult(rotZ, worldToCameraMatrix);
			//cameraToClipMatrix = Matrix4.Mult(cameraToClipMatrix, rot);
			//cameraToClipMatrix = Matrix4.Mult(rot, cameraToClipMatrix);
			SetGlobalMatrices(currentProgram);
		}

		private void ChangePlayerView()
		{
			MatrixStack camMatrix = new MatrixStack();
			camMatrix.SetMatrix(Camera.GetLookAtMatrix());
			worldToCameraMatrix = camMatrix.Top();
			Matrix4 rotX = Matrix4.CreateRotationX(playerRotations[playerNumber].X * (float)Math.PI / 180f);
			Matrix4 rotY = Matrix4.CreateRotationY(playerRotations[playerNumber].Y * (float)Math.PI / 180f);
			Matrix4 rotZ = Matrix4.CreateRotationZ(playerRotations[playerNumber].Z * (float)Math.PI / 180f);
			//worldToCameraMatrix = Matrix4.Mult(worldToCameraMatrix, rot);
			worldToCameraMatrix = Matrix4.Mult(rotX, worldToCameraMatrix);
			worldToCameraMatrix = Matrix4.Mult(rotY, worldToCameraMatrix);
			worldToCameraMatrix = Matrix4.Mult(rotZ, worldToCameraMatrix);
			//cameraToClipMatrix = Matrix4.Mult(cameraToClipMatrix, rot);
			//cameraToClipMatrix = Matrix4.Mult(rot, cameraToClipMatrix);
			SetGlobalMatrices(currentProgram);
		}

		private void UpdatePosition()
		{
			if (ballModelMatrix.M41 < positionLimitLow.X)
			{
				leftWall.Paint(ballModelMatrix.M43/positionLimitHigh.X, ballModelMatrix.M42/positionLimitHigh.Y);
				if (velocity.X < 0) velocity.X *= -1;
			}
			if (ballModelMatrix.M41 > positionLimitHigh.X)
			{
				rightWall.Paint(ballModelMatrix.M43/positionLimitHigh.X, ballModelMatrix.M42/positionLimitHigh.Y);
				if (velocity.X > 0) velocity.X *= -1;
			}
			if (ballModelMatrix.M42 < positionLimitLow.Y)
			{
				bottomWall.Paint(ballModelMatrix.M41/positionLimitHigh.X, ballModelMatrix.M43/positionLimitHigh.Y);
				if (velocity.Y < 0) velocity.Y *= -1;
			}
			if (ballModelMatrix.M42 > positionLimitHigh.Y)
			{
				topWall.Paint(ballModelMatrix.M41/positionLimitHigh.X, ballModelMatrix.M43/positionLimitHigh.Y);
				if (velocity.Y > 0) velocity.Y *= -1;
			}
			if (ballModelMatrix.M43 < positionLimitLow.Z)
			{
				backWall.Paint(ballModelMatrix.M41/positionLimitHigh.X, ballModelMatrix.M42/positionLimitHigh.Y);
				if (velocity.Z < 0) velocity.Z *= -1;
			}
			if (ballModelMatrix.M43 > positionLimitHigh.Z)
			{
				frontWall.Paint(ballModelMatrix.M41/positionLimitHigh.X, ballModelMatrix.M42/positionLimitHigh.Y);
				if (velocity.Z > 0) velocity.Z *= -1;
			}
			position += velocity;
		}
	
	    static Vector3 axis = new Vector3(1f, 1f, 0);
	    static float angle = 0;
	
	    static private void SetGlobalMatrices(ProgramData program)
	    {
			Shape.worldToCamera = worldToCameraMatrix;
			Shape.cameraToClip = cameraToClipMatrix;

	        GL.UseProgram(program.theProgram);
			GL.UniformMatrix4(program.cameraToClipMatrixUnif, false, ref cameraToClipMatrix);  // this one is first
			GL.UniformMatrix4(program.worldToCameraMatrixUnif, false, ref worldToCameraMatrix); // this is the second one
	        GL.UseProgram(0);
	    }
	
	    public override void reshape()
	    {
	        MatrixStack camMatrix = new MatrixStack();
	        camMatrix.SetMatrix(Camera.GetLookAtMatrix());
	
			worldToCameraMatrix = camMatrix.Top();
	
	        MatrixStack persMatrix = new MatrixStack();
	        persMatrix.Perspective(perspectiveAngle, (width / (float)height), g_fzNear, g_fzFar);
			cameraToClipMatrix = persMatrix.Top();
			ChangePlayerView();
	        SetGlobalMatrices(currentProgram);
	
	        GL.Viewport(0, 0, width, height);

	    }
	
	    static bool noWorldMatrix = false;
	
	    public override String keyboard(Keys keyCode, int x, int y)
	    {
	        StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
	        switch (keyCode) {
			case Keys.NumPad6:
				Camera.MoveTarget(0.5f, 0f, 0.0f);
				result.AppendFormat(Camera.GetTargetString());
				break;
			case Keys.NumPad4:
				Camera.MoveTarget(-0.5f, 0f, 0.0f);
				result.AppendFormat(Camera.GetTargetString());
				break;
			case Keys.NumPad8:
				Camera.MoveTarget(0.0f, 0.5f, 0.0f);
				result.AppendFormat(Camera.GetTargetString());
				break;
			case Keys.NumPad2:
				Camera.MoveTarget(0f, -0.5f, 0.0f);
				result.AppendFormat(Camera.GetTargetString());
				break;
			case Keys.NumPad7:
				Camera.MoveTarget(0.0f, 0.0f, 0.5f);
				result.AppendFormat(Camera.GetTargetString());
				break;
			case Keys.NumPad3:
				Camera.MoveTarget(0f, 0.0f, -0.5f);
				result.AppendFormat(Camera.GetTargetString());
				break;
			case Keys.D1:
				axis = Vector3.UnitX;
				angle = angle + 1;
	            break;	
	        case Keys.D2:
				axis = Vector3.UnitY;
				angle = angle + 1;
	            break;		
	        case Keys.D3:
				axis = Vector3.UnitZ;
				angle = angle + 1;
	            break;				
            case Keys.D4:
                break;	
            case Keys.D5:
                break;			
			case Keys.D6:
				break;
			case Keys.C:
				if (cull)
				{
					cull = false;
					GL.Disable(EnableCap.CullFace);
					result.AppendLine("cull disabled");
				}
				else
				{
					cull = true;
					GL.Enable(EnableCap.CullFace);
					result.AppendLine("cull enabled");
				}
				break;
			case Keys.R:
				if (rotateWorld)
				{
					rotateWorld = false;
					result.AppendLine("rotateWorld disabled");
				}
				else
				{
					rotateWorld = true;
					result.AppendLine("rotateWorld enabled");
				}
				break;
            case Keys.Escape:
                //timer.Enabled = false;
                break;
            case Keys.Space:
				newPerspectiveAngle = perspectiveAngle + 5f;
				if (newPerspectiveAngle > 120f)
				{
					newPerspectiveAngle = 30f;
				}
                break;
			case Keys.Z:
				break;
			case Keys.X:
				noWorldMatrix = true;
				currentProgram = g_Unlit;
				reshape();
				break;
			case Keys.Y:
				noWorldMatrix = true;
				currentProgram = g_litShaderProg;
				reshape();
				break;
			case Keys.Q:
				result.AppendLine("currentProgram = " + currentProgram.ToString());
				break;
			case Keys.P:
				if (pause)
				{
					pause = false;
				}
				else
				{
					pause = true;
				}
				break;
			case Keys.V:
				playerNumber++;
				if (playerNumber > lastPlayer)
				{
					playerNumber = 0;
				}
				ChangePlayerView();
				result.AppendLine("Player number = " + playerNumber.ToString());
				break;
			case Keys.I:
				result.AppendLine("cameraToClipMatrix " + cameraToClipMatrix.ToString());
				result.AppendLine(AnalysisTools.CalculateMatrixEffects(cameraToClipMatrix));
				result.AppendLine("worldToCameraMatrix " + worldToCameraMatrix.ToString());
				result.AppendLine(AnalysisTools.CalculateMatrixEffects(worldToCameraMatrix));

				Matrix4 cameraToClipMatrixTimesWorldToCameraMatrix =
					Matrix4.Mult(cameraToClipMatrix, worldToCameraMatrix);
				result.AppendLine("cameraToClipMatrixTimesWorldToCameraMatrix " + 
					cameraToClipMatrixTimesWorldToCameraMatrix.ToString());
				result.AppendLine(AnalysisTools.CalculateMatrixEffects(cameraToClipMatrixTimesWorldToCameraMatrix));

				Matrix4 worldToCameraMatrixTimesCameraToClipMatrix =
					Matrix4.Mult(worldToCameraMatrix, cameraToClipMatrix);
				result.AppendLine("worldToCameraMatrixTimesCameraToClipMatrix " + 
					worldToCameraMatrixTimesCameraToClipMatrix.ToString());
				result.AppendLine(AnalysisTools.CalculateMatrixEffects(worldToCameraMatrixTimesCameraToClipMatrix));
				break;
			case Keys.S:
				ball.SetSocketControl();
				result.AppendLine("Socket Control");
				break;
			case Keys.E:
				ball.SetElasticControl();
				result.AppendLine("Socket Control");
				break;
			case Keys.W:
				if (drawWalls)
					drawWalls = false;
				else
					drawWalls = true;
				break;
			}
	        return result.ToString();
	    }
	
	}
}

