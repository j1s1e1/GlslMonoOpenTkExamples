using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_09_Ambient_Lighting : TutorialBase 
	{	
	    static float g_fzNear = 10.0f;
	    static float g_fzFar = 1000.0f;
	
	    class ProgramData
	    {
	        public int theProgram;
	
	        public int positionAttribute;
	        public int colorAttribute;
	        public int normalAttribute;
	
	        public int dirToLightUnif;
	        public int lightIntensityUnif;
	        public int ambientIntensityUnif;
	
	        public int modelToCameraMatrixUnif;
	        public int normalModelToCameraMatrixUnif;
	
	        public int cameraToClipMatrixUnif;	// to avoid uniform buffer block
	    };
	
	    static ProgramData g_WhiteDiffuseColor;
	    static ProgramData g_VertexDiffuseColor;
	    static ProgramData g_WhiteAmbDiffuseColor;
	    static ProgramData g_VertexAmbDiffuseColor;
	
	    static int g_projectionBlockIndex = 2;
	
	    ProgramData LoadProgram(String vertexShader, String fragmentShader)
	    {
	        ProgramData data = new ProgramData();
	        int vertexShaderInt = Shader.loadShader(ShaderType.VertexShader, vertexShader);
	        int fragmentShaderInt = Shader.loadShader(ShaderType.FragmentShader, fragmentShader);
	        data.theProgram = Shader.createAndLinkProgram(vertexShaderInt, fragmentShaderInt);
	
	        data.positionAttribute = GL.GetAttribLocation(data.theProgram, "position");
	        data.colorAttribute = GL.GetAttribLocation(data.theProgram, "color");
	        data.normalAttribute = GL.GetAttribLocation(data.theProgram, "normal");
	
			if (data.positionAttribute != -1) 
			{
				if (data.positionAttribute != 0)
				{
					MessageBox.Show("These meshes only work with position at location 0 " + vertexShader);
				}
			}
			if (data.colorAttribute != -1) 
			{
				if (data.colorAttribute != 1)
				{
					MessageBox.Show("These meshes only work with color at location 1" + vertexShader);
				}
			}
	
	        data.modelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");
	        data.normalModelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "normalModelToCameraMatrix");
	        data.dirToLightUnif = GL.GetUniformLocation(data.theProgram, "dirToLight");
	        data.lightIntensityUnif = GL.GetUniformLocation(data.theProgram, "lightIntensity");
	        data.ambientIntensityUnif = GL.GetUniformLocation(data.theProgram, "ambientIntensity");
	
	        //FIX_THIS  int projectionBlock = GL.GetUniformBlockIndex(data.theProgram, "Projection");
	        //FIX_THIS  GL.UniformBlockBinding(data.theProgram, projectionBlock, g_projectionBlockIndex);
	        // to avoid uniform block
	        data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "Projection.cameraToClipMatrix");
	        return data;
	    }
	
	    void InitializeProgram()
	    {
	        g_WhiteDiffuseColor = LoadProgram(VertexShaders.DirVertexLighting_PN_vert, FragmentShaders.ColorPassthrough_frag);
	        g_VertexDiffuseColor = LoadProgram(VertexShaders.DirVertexLighting_PCN_vert, FragmentShaders.ColorPassthrough_frag);
	        g_WhiteAmbDiffuseColor = LoadProgram(VertexShaders.DirAmbVertexLighting_PN_vert, FragmentShaders.ColorPassthrough_frag);
	        g_VertexAmbDiffuseColor = LoadProgram(VertexShaders.DirAmbVertexLighting_PCN_vert, FragmentShaders.ColorPassthrough_frag);
	    }
	
	    static Mesh g_pCylinderMesh = null;
	    static Mesh g_pPlaneMesh = null;
	
	    ///////////////////////////////////////////////
	    // View/Object Setup
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
	
	    static int[] g_projectionUniformBuffer = new int[]{0};
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
	        InitializeGInitialViewData();
	        InitializeGViewScale();
	        g_viewPole = new ViewProvider(g_initialViewData, g_viewScale, MouseButtons.MB_LEFT_BTN);
	        g_objtPole = new ObjectPole(g_initialObjectData, (float)(90.0f / 250.0f),
	                MouseButtons.MB_RIGHT_BTN, g_viewPole);
	
	        InitializeProgram();
	
	        try
	        {
				string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes";
				Stream unitcylinder = File.OpenRead(XmlFilesDirectory + @"/unitcylinder.xml");
	            g_pCylinderMesh = new Mesh(unitcylinder);
				Stream unitplane = File.OpenRead(XmlFilesDirectory + @"/unitplane.xml");
	            g_pPlaneMesh = new Mesh(unitplane);
	        }
	        catch(Exception ex)
	        {
	            throw new Exception("Error:" + ex.ToString());
	        }
						
			GL.Enable(EnableCap.CullFace);
	        GL.CullFace(CullFaceMode.Back);
	        GL.FrontFace(FrontFaceDirection.Cw);
	
	        GL.Enable(EnableCap.DepthTest);
	        GL.DepthMask(true);
	        GL.DepthFunc(DepthFunction.Lequal);
	        GL.DepthRange(0.0f, 1.0f);
	        reshape();
			
	        GL.GenBuffers(1, g_projectionUniformBuffer);
	        GL.BindBuffer(BufferTarget.UniformBuffer, g_projectionUniformBuffer[0]);
	        GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)ProjectionBlock.byteLength(), (IntPtr)0, BufferUsageHint.StaticDraw);

	        reshape();
	    }
	
	    static Vector4 g_lightDirection = new Vector4(0.866f, 0.5f, 0.0f, 0.0f);
	
	    static bool g_bDrawColoredCyl = true;
	    static bool g_bShowAmbient = true;
	
	    public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			
	        if((g_pPlaneMesh != null) && (g_pCylinderMesh != null))
	        {
	            MatrixStack modelMatrix = new MatrixStack();
	            modelMatrix.SetMatrix(g_viewPole.CalcMatrix());
	
	            Vector4 lightDirCameraSpace = Vector4.Transform(g_lightDirection, modelMatrix.Top());
	            ProgramData whiteDiffuse = g_bShowAmbient ? g_WhiteAmbDiffuseColor : g_WhiteDiffuseColor;
	            ProgramData vertexDiffuse = g_bShowAmbient ? g_VertexAmbDiffuseColor : g_VertexDiffuseColor;
	
	            if(g_bShowAmbient)
	            {
	                GL.UseProgram(whiteDiffuse.theProgram);
	                GL.Uniform4(whiteDiffuse.lightIntensityUnif, 0.8f, 0.8f, 0.8f, 1.0f);
	                GL.Uniform4(whiteDiffuse.ambientIntensityUnif, 0.2f, 0.2f, 0.2f, 1.0f);
					GL.UseProgram(0);
	                GL.UseProgram(vertexDiffuse.theProgram);
	                GL.Uniform4(vertexDiffuse.lightIntensityUnif, 0.8f, 0.8f, 0.8f, 1.0f);
	                GL.Uniform4(vertexDiffuse.ambientIntensityUnif, 0.2f, 0.2f, 0.2f, 1.0f);
					GL.UseProgram(0);
	            }
	            else
	            {
	                GL.UseProgram(whiteDiffuse.theProgram);
	                GL.Uniform4(whiteDiffuse.lightIntensityUnif, 0.5f, 0.5f, 0.5f, 0.5f);
					GL.UseProgram(0);
	                GL.UseProgram(vertexDiffuse.theProgram);
	                GL.Uniform4(vertexDiffuse.lightIntensityUnif, 0.5f, 0.5f, 0.5f, 0.5f);
					GL.UseProgram(0);
	            }
				
				//TEST
				//lightDirCameraSpace = new Vector4(1f, 1f, -1f, 0f);
	
	            GL.UseProgram(whiteDiffuse.theProgram);
				Vector3 dirToLight = new Vector3(lightDirCameraSpace.X, lightDirCameraSpace.Y, lightDirCameraSpace.Z);
				// test 
				dirToLight = new Vector3(10f, 10f, 0f);
	            GL.Uniform3(whiteDiffuse.dirToLightUnif, dirToLight);
				GL.UseProgram(0);
	            GL.UseProgram(vertexDiffuse.theProgram);
	            GL.Uniform3(vertexDiffuse.dirToLightUnif, dirToLight);
	            GL.UseProgram(0);
				
                //Render the ground plane.
                using ( PushStack pushstack = new PushStack(modelMatrix))
                {		
                    GL.UseProgram(whiteDiffuse.theProgram);
					
                    Matrix4 mm =  modelMatrix.Top();
					mm = Matrix4.Identity; // TEST
                    GL.UniformMatrix4(whiteDiffuse.modelToCameraMatrixUnif, false, ref mm);
					projData.cameraToClipMatrix = Matrix4.Identity; // Test
                    GL.UniformMatrix4(whiteDiffuse.cameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
                    Matrix3 normMatrix = new Matrix3(modelMatrix.Top());
                    normMatrix.Normalize();
					normMatrix = Matrix3.Identity; // TEST
                    GL.UniformMatrix3(whiteDiffuse.normalModelToCameraMatrixUnif, false, ref normMatrix);
                    g_pPlaneMesh.Render();
                    GL.UseProgram(0);
                }
                	
				
                //Render the Cylinder
                using (PushStack pushstack = new PushStack(modelMatrix))
                {
                    modelMatrix.ApplyMatrix(g_objtPole.CalcMatrix());

                    if(g_bDrawColoredCyl)
                    {					
                        GL.UseProgram(vertexDiffuse.theProgram);
                        Matrix4 mm = modelMatrix.Top();
                        mm = Matrix4.Identity; // TEST
                        GL.UniformMatrix4(vertexDiffuse.modelToCameraMatrixUnif, false, ref mm);
						projData.cameraToClipMatrix = Matrix4.Identity; // TEST
                        GL.UniformMatrix4(vertexDiffuse.cameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
                        Matrix3 normMatrix = new Matrix3(modelMatrix.Top());
                        normMatrix = Matrix3.Identity; // TEST
                        GL.UniformMatrix3(vertexDiffuse.normalModelToCameraMatrixUnif, false, ref normMatrix);
						g_pCylinderMesh.Render("lit-color");
                    }
                    else
                    {
                        GL.UseProgram(whiteDiffuse.theProgram);
                        Matrix4 mm = modelMatrix.Top();
						mm = Matrix4.Identity; // TEST
                        GL.UniformMatrix4(whiteDiffuse.modelToCameraMatrixUnif, false, ref mm);
						projData.cameraToClipMatrix = Matrix4.Identity; // TEST
                        GL.UniformMatrix4(whiteDiffuse.cameraToClipMatrixUnif, false, ref projData.cameraToClipMatrix);
                        Matrix3  normMatrix = new Matrix3(modelMatrix.Top());
						normMatrix = Matrix3.Identity; // TEST
                        GL.UniformMatrix3(whiteDiffuse.normalModelToCameraMatrixUnif, false, ref normMatrix);
                        g_pCylinderMesh.Render("lit");
                    }
                    
                    GL.UseProgram(0);
				}
				
	        }
	    }
	
	    static ProjectionBlock projData = new ProjectionBlock();
		
	    //Called whenever the window is resized. The new window size is given, in pixels.
	    //This is an opportunity to call glViewport or glScissor to keep up with the change in size.
	    public override void reshape ()
	    {
	        MatrixStack persMatrix = new MatrixStack();
	        persMatrix.Perspective(45.0f, (width / (float)height), g_fzNear, g_fzFar);
	
	        projData.cameraToClipMatrix = persMatrix.Top();
	
			
			MatrixStack camMatrix = new MatrixStack();
	        camMatrix.SetMatrix(Camera.GetLookAtMatrix());
	
	        GL.Viewport(0, 0, width, height);
	    }
	
	    //Called whenever a key on the keyboard was pressed.
	    //The key is given by the ''key'' parameter, which is in ASCII.
	    //It's often a good idea to have the escape key (ASCII value 27) call glutLeaveMainLoop() to 
	    //exit the program.
	    public override String keyboard(Keys keyCode, int x, int y)
	    {
	        StringBuilder result = new StringBuilder();
	        switch (keyCode)
	        {
				case Keys.A:
					g_bDrawColoredCyl = true;
					break;
				case Keys.B:
					g_bDrawColoredCyl = false;
					break;				
	            case Keys.Escape:
	                //timer.Enabled = false;
	                break;
	            case Keys.Space:
	                g_bDrawColoredCyl = !g_bDrawColoredCyl;
	                if (g_bDrawColoredCyl)
	                    result.Append("Colored Cylinder On.\n");
	                else
	                    result.Append("Colored Cylinder Off.\n");
	                break;
	
	            case Keys.T:
	                g_bShowAmbient = !g_bShowAmbient;
	                if(g_bShowAmbient)
	                    result.Append("Ambient Lighting On.\n");
	                else
	                    result.Append("Ambient Lighting Off.\n");
	
	                break;
	        }
	        result.Append(keyCode);
	        reshape();
	        display();
	        return result.ToString();
	    }
	}
}

