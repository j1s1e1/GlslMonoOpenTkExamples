using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_SingleMeshItem : TutorialBase 
	{
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
	
	
	    static ProgramData currentProgram;
	
	    ProgramData LoadProgram(String strVertexShader, String strFragmentShader)
	    {
	        ProgramData data = new ProgramData();
	        int vertex_shader = Shader.loadShader(ShaderType.VertexShader, strVertexShader);
	        int fragment_shader = Shader.loadShader(ShaderType.FragmentShader, strFragmentShader);
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
	
	        g_WhiteDiffuseColor = LoadProgram(VertexShaders.PosColorLocalTransform_vert, FragmentShaders.ColorPassthrough_frag);
	        g_WhiteAmbDiffuseColor = LoadProgram(VertexShaders.DirAmbVertexLighting_PN_vert, FragmentShaders.ColorPassthrough_frag);
	        
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
	
	        ObjectColor = LoadProgram(VertexShaders.PosColorWorldTransform_vert, FragmentShaders.ColorPassthrough_frag);
	        currentProgram = ObjectColor;
	    }
	    static Mesh current_mesh;
	    static Mesh g_pCubeColorMesh;
	    static Mesh g_pCylinderMesh;
		static Mesh g_pPlaneMesh;
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
	        InitializeProgram();
	
	        try 
	        {
				string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes";
	            Stream UnitCubeColor =  File.OpenRead(XmlFilesDirectory + @"/unitcubecolor.xml");
				g_pCubeColorMesh = new Mesh(UnitCubeColor);
	            Stream UnitCylinder =File.OpenRead(XmlFilesDirectory + @"/unitcylinder.xml");
	            g_pCylinderMesh = new Mesh(UnitCylinder);
				
				Stream unitplane = File.OpenRead(XmlFilesDirectory + @"/unitplane.xml");
	            g_pPlaneMesh = new Mesh(unitplane);
				
	        } catch (Exception ex) {
	            throw new Exception("Error " + ex.ToString());
	        }
	        GL.Enable(EnableCap.CullFace);
	        GL.CullFace(CullFaceMode.Back);
	        GL.FrontFace(FrontFaceDirection.Cw);
	
	        GL.Enable(EnableCap.DepthTest);
	        GL.DepthMask(true);
	        GL.DepthFunc(DepthFunction.Lequal);
	        GL.DepthRange(0.0f, 1.0f);
			Camera.Move(0f, 0f, 0f);
	        Camera.MoveTarget(0f, 0f, 0.0f);
	        reshape();
	        current_mesh = g_pCubeColorMesh;
	    }
	
	    public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
	
	        if (current_mesh != null)
	        {
	
	            MatrixStack modelMatrix = new MatrixStack();
                using (PushStack pushstack = new PushStack(modelMatrix)) 
				{
                    modelMatrix.Translate(Camera.g_camTarget);
                    modelMatrix.Translate(0f, 0f, 0f);
                    modelMatrix.Scale(15.0f, 15.0f, 15.0f);
                    modelMatrix.Rotate(axis, angle);
                    angle = angle + 1f;

                    GL.UseProgram(currentProgram.theProgram);
                    Matrix4 mm = modelMatrix.Top();

                    if (noWorldMatrix) 
					{
                        Matrix4 cm2 = Matrix4.Mult(mm, cm);
                        GL.UniformMatrix4(currentProgram.modelToCameraMatrixUnif, false, ref cm2);
                    } 
					else 
					{
                        GL.UniformMatrix4(currentProgram.modelToWorldMatrixUnif, false, ref mm);
                    }
                }
                current_mesh.Render();
                GL.UseProgram(0);
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
	        persMatrix.Perspective(60.0f, (width / (float)height), g_fzNear, g_fzFar);
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
	        switch (keyCode) {

	            case Keys.D1:
	                currentProgram = ObjectColor;
	                noWorldMatrix = false;
	                break;	
	            case Keys.D2:
	                currentProgram = UniformColor;
	                noWorldMatrix = false;
	                break;		
	            case Keys.D3:
	                currentProgram = UniformColorTint;
	                noWorldMatrix = false;
	                break;				
	            case Keys.D4:
	                currentProgram = g_WhiteDiffuseColor;
	                noWorldMatrix = true;
	                break;	
	            case Keys.D5:
	                currentProgram = g_WhiteAmbDiffuseColor;
	                noWorldMatrix = true;
	                break;				
				
	            case Keys.A:
	                current_mesh = g_pCylinderMesh;
	                break;
	            case Keys.B:
	                current_mesh = g_pCubeColorMesh;
	                break;
				case Keys.C:
					current_mesh = g_pPlaneMesh;
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
	        }
	        result.AppendLine(keyCode.ToString());
			result.AppendLine("currentProgram = " + currentProgram.ToString());
			
	        reshape();
	        display();
	        return result.ToString();
	    }
	
	}
}

