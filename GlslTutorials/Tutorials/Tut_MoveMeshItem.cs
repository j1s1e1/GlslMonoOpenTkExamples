using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_MoveMeshItem : TutorialBase 
	{
		bool renderWithString = false;
		string renderString = "";
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
	
	    static float g_fzNear = 10.0f;
	    static float g_fzFar = 1000.0f;
	
	    static ProgramData UniformColor;
	    static ProgramData ObjectColor;
	    static ProgramData UniformColorTint;
	
	    static ProgramData g_WhiteDiffuseColor;
	    static ProgramData g_VertexDiffuseColor;
	    static ProgramData g_WhiteAmbDiffuseColor;
	    static ProgramData g_VertexAmbDiffuseColor;
		static ProgramData g_Unlit;
		static ProgramData g_litShaderProg;
	
	
	    static ProgramData currentProgram;
		
		static Vector4 g_lightDirection = new Vector4(0.866f, 0.5f, 0.0f, 0.0f);
		Vector3 dirToLight = new Vector3(0.5f, 0.5f, 1f);

		float perspectiveAngle = 60f;
		float newPerspectiveAngle = 60f;
	
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
	
	        g_WhiteDiffuseColor = LoadProgram(VertexShaders.PosColorLocalTransform_vert, 
			                                  FragmentShaders.ColorPassthrough_frag);
			
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
			g_litShaderProg.lightBlock = new LightBlock();
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
	    }
	    static Mesh current_mesh;
	    static Mesh g_pCubeColorMesh;
	    static Mesh g_pCylinderMesh;
		static Mesh g_pPlaneMesh;
		static Mesh g_pInfinityMesh;
		static Mesh g_unitSphereMesh;
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
	        InitializeProgram();
	
	        try 
	        {
				string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes";
	            Stream UnitCubeColor =  File.OpenRead(XmlFilesDirectory + @"/unitcubecolor.xml");
				g_pCubeColorMesh = new Mesh(UnitCubeColor);
	            Stream UnitCylinder = File.OpenRead(XmlFilesDirectory + @"/unitcylinder.xml");
	            g_pCylinderMesh = new Mesh(UnitCylinder);
				
				Stream unitplane = File.OpenRead(XmlFilesDirectory + @"/unitplane.xml");
	            g_pPlaneMesh = new Mesh(unitplane);
				
				Stream infinity = File.OpenRead(XmlFilesDirectory + @"/infinity.xml");
				g_pInfinityMesh = new Mesh(infinity);

				Stream unitSphere = File.OpenRead(XmlFilesDirectory + @"/unitsphere12.xml");
				g_unitSphereMesh = new Mesh(unitSphere);
				
	        } catch (Exception ex) {
	            throw new Exception("Error " + ex.ToString());
	        }
	        
			SetupDepthAndCull();
			
			Camera.Move(0f, 0f, 0f);
	        Camera.MoveTarget(0f, 0f, 0.0f);
	        reshape();
	        current_mesh = g_pCubeColorMesh;
	    }
	
	    public override void display()
	    {
	        ClearDisplay();
	
	        if (current_mesh != null)
	        {
	            MatrixStack modelMatrix = new MatrixStack();
                using (PushStack pushstack = new PushStack(modelMatrix)) 
				{
					modelMatrix.Rotate(axis, angle);   // rotate last to leave in place
                    modelMatrix.Translate(Camera.g_camTarget);
                    modelMatrix.Scale(15.0f, 15.0f, 15.0f);
                   

                    GL.UseProgram(currentProgram.theProgram);
                    Matrix4 mm = modelMatrix.Top();

                    if (noWorldMatrix) 
					{
                        Matrix4 cm2 = Matrix4.Mult(mm, cm);
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
	
	    //Called whenever the window is resized. The new window size is given, in pixels.
	    //This is an opportunity to call glViewport or glScissor to keep up with the change in size.
	    public override void reshape()
	    {
	        MatrixStack camMatrix = new MatrixStack();
	        camMatrix.SetMatrix(Camera.GetLookAtMatrix());
	
	        cm = camMatrix.Top();
	
	        MatrixStack persMatrix = new MatrixStack();
	        persMatrix.Perspective(perspectiveAngle, (width / (float)height), g_fzNear, g_fzFar);
	        pm = persMatrix.Top();

	        SetGlobalMatrices(currentProgram);
	
	        GL.Viewport(0, 0, width, height);

	    }
	
	    static bool noWorldMatrix = false;
	
	    //Called whenever a key on the keyboard was pressed.
	    //The key is given by the ''key'' parameter, which is in ASCII.
	    //It's often a good idea to have the escape key (ASCII value 27) call glutLeaveMainLoop() to 
	    //exit the program.
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
			case Keys.P:
				newPerspectiveAngle = perspectiveAngle + 5f;
				if (newPerspectiveAngle > 120f)
				{
					newPerspectiveAngle = 30f;
				}
				break;
            case Keys.A:
				renderWithString = false;
                current_mesh = g_pCylinderMesh;
                break;
            case Keys.B:
				renderWithString = false;
                current_mesh = g_pCubeColorMesh;
                break;
			case Keys.C:
				renderWithString = false;
				current_mesh = g_pPlaneMesh;
				break;
			case Keys.D:
				renderWithString = false;
				current_mesh = g_pInfinityMesh;
				break;
			case Keys.E:
				renderWithString = false;
				current_mesh = g_unitSphereMesh;
				break;
			case Keys.F:
				renderWithString = true;
				renderString = "flat";
				current_mesh = g_unitSphereMesh;
				break;
			case Keys.I:
                result.AppendLine("I Decrease g_camTarget.X");
                Camera.MoveTarget(-4.0f, 0, 0);
                break;
            case Keys.M:
                result.AppendLine("M Increase g_camTarget.X");
                Camera.MoveTarget(4.0f, 0, 0);
                break;
            case Keys.J:				
				result.AppendLine("J Increase g_camTarget.Z");
                Camera.MoveTarget(0, 0, 4.0f);
				break;				
            case Keys.K:				
				result.AppendLine("K Decrease g_camTarget.Z");
                Camera.MoveTarget(0, 0, -4.0f);
				break;
            case Keys.Escape:
                //timer.Enabled = false;
                break;
            case Keys.Space:
                break;
			case Keys.Z:
				break;

			case Keys.W:
				currentProgram = ObjectColor;
				reshape();
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
	        }
			
	        reshape();
	        display();
	        return result.ToString();
	    }
	
	}
}

