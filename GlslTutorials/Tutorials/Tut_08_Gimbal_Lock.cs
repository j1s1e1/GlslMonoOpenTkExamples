using System;
using System.IO;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_08_Gimbal_Lock : TutorialBase 
	{
	    void InitializeProgram()
	    {
	        fFrustumScale = CalcFrustumScale(45.0f);
	        float fzNear = 1.0f;
	        float fzFar = 600.0f;
	        int vertex_shader = Shader.compileShader(ShaderType.VertexShader, VertexShaders.PosColorLocalTransform_vert);
	        int fragment_shader = Shader.compileShader(ShaderType.FragmentShader, FragmentShaders.ColorMultUniform_frag);
	        theProgram = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
	        positionAttribute = GL.GetAttribLocation(theProgram, "position");
	        colorAttribute = GL.GetAttribLocation(theProgram, "color");
	
	
	        modelToCameraMatrixUnif = GL.GetUniformLocation(theProgram, "modelToCameraMatrix");
	        cameraToClipMatrixUnif = GL.GetUniformLocation(theProgram, "cameraToClipMatrix");
	        baseColorUnif = GL.GetUniformLocation(theProgram, "baseColor");
	
	        cameraToClipMatrix.M11 = fFrustumScale;
	        cameraToClipMatrix.M22 = fFrustumScale;
	        cameraToClipMatrix.M33 = (fzFar + fzNear) / (fzNear - fzFar);
	        cameraToClipMatrix.M34 = -1.0f;
	        cameraToClipMatrix.M43 = (2 * fzFar * fzNear) / (fzNear - fzFar);
	
	        GL.UseProgram(theProgram);
	        GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref cameraToClipMatrix);
	        GL.UseProgram(0);
	    }
	
	    enum GimbalAxis
	    {
	        GIMBAL_X_AXIS,
	        GIMBAL_Y_AXIS,
	        GIMBAL_Z_AXIS,
	    };
	
	    static Mesh[] g_Gimbals = new Mesh[3];
	
	    static bool g_bDrawGimbals = true;
	
	    void DrawGimbal(MatrixStack currMatrix, GimbalAxis eAxis, Vector4 baseColor)
	    {
	        if(g_bDrawGimbals == false)
	            return;
	
	        {
	            switch(eAxis)
	            {
	                case GimbalAxis.GIMBAL_X_AXIS:
	                    break;
	                case GimbalAxis.GIMBAL_Y_AXIS:
	                    currMatrix.RotateZ(90.0f);
	                    currMatrix.RotateX(90.0f);
	                    break;
	                case GimbalAxis.GIMBAL_Z_AXIS:
	                    currMatrix.RotateY(90.0f);
	                    currMatrix.RotateX(90.0f);
	                    break;
	            }
	
	            GL.UseProgram(theProgram);
	            //Set the base color for this object.
	            GL.Uniform4(baseColorUnif, ref baseColor);
	            Matrix4 cm = currMatrix.Top();
	            GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref cm);
	
	            switch (eAxis)
	            {
	                case GimbalAxis.GIMBAL_X_AXIS: g_Gimbals[0].Render(); break;
	                case GimbalAxis.GIMBAL_Y_AXIS: g_Gimbals[1].Render(); break;
	                case GimbalAxis.GIMBAL_Z_AXIS: g_Gimbals[2].Render(); break;
	            }
	
	            GL.UseProgram(0);
	        }
	    }
	
	    static Mesh g_pObject = null;
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
	        InitializeProgram();
	
	        try
	        {
				string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes";
	            Stream smallgimbal = File.OpenRead(XmlFilesDirectory + @"/smallgimbal.xml");
	            g_Gimbals[0] = new Mesh(smallgimbal);
				Stream mediumgimbal = File.OpenRead(XmlFilesDirectory + @"/mediumgimbal.xml");
	            g_Gimbals[1] = new Mesh(mediumgimbal);
	            Stream largegimbal = File.OpenRead(XmlFilesDirectory + @"/largegimbal.xml");
				g_Gimbals[2] = new Mesh(largegimbal);
	            Stream ship = File.OpenRead(XmlFilesDirectory + @"/ship.xml");
	            g_pObject = new Mesh(ship);
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
			Camera.Move(0f, 0f, 0f);
	        Camera.MoveTarget(0f, 0f, 0.0f);
	        reshape();
			MatrixStack.rightMultiply = false;
	    }
	
	    class GimbalAngles
	    {
	        public float fAngleX;
	        public float fAngleY;
	        public float fAngleZ;
	    };
	
	    GimbalAngles g_angles = new GimbalAngles();
	
	    //Called to update the display.
	    //You should call glutSwapBuffers after all of your rendering to display what you rendered.
	    //If you need continuous updates of the screen, call glutPostRedisplay() at the end of the function.
	    public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
	
	        MatrixStack currMatrix = new MatrixStack();
	        currMatrix.Translate(new Vector3(0.0f, 0.0f, -200.0f));
	        currMatrix.RotateX(g_angles.fAngleX);
	        DrawGimbal(currMatrix, GimbalAxis.GIMBAL_X_AXIS, new Vector4(0.4f, 0.4f, 1.0f, 1.0f));
	        currMatrix.RotateY(g_angles.fAngleY);
	        DrawGimbal(currMatrix, GimbalAxis.GIMBAL_Y_AXIS, new Vector4(0.0f, 1.0f, 0.0f, 1.0f));
	        currMatrix.RotateZ(g_angles.fAngleZ);
	        DrawGimbal(currMatrix, GimbalAxis.GIMBAL_Z_AXIS, new Vector4(1.0f, 0.3f, 0.3f, 1.0f));
	
	        GL.UseProgram(theProgram);
	        currMatrix.Scale(3.0f, 3.0f, 3.0f);
	        currMatrix.RotateX(-90);
	        //Set the base color for this object.
	        GL.Uniform4(baseColorUnif, 1.0f, 1.0f, 1.0f, 1.0f);
	        Matrix4 cm = currMatrix.Top();
	        GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref cm);
	
	        g_pObject.Render("tint");
	
	        GL.UseProgram(0);
	
	    }
	
	    //Called whenever the window is resized. The new window size is given, in pixels.
	    //This is an opportunity to call glViewport or glScissor to keep up with the change in size.
	    public override void reshape ()
	    {
	        cameraToClipMatrix.M11 = fFrustumScale * (height / (float)width);
	        cameraToClipMatrix.M22 = fFrustumScale;
	
	        GL.UseProgram(theProgram);
	        GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref cameraToClipMatrix);
	        GL.UseProgram(0);
	
	        GL.Viewport(0, 0, width, height);
	
	    }
	
	    static float  SMALL_ANGLE_INCREMENT = 9.0f;
	
	    //Called whenever a key on the keyboard was pressed.
	    //The key is given by the ''key'' parameter, which is in ASCII.
	    //It's often a good idea to have the escape key (ASCII value 27) call glutLeaveMainLoop() to 
	    //exit the program.
	    public override string keyboard(Keys keyCode, int x, int y)
	    {
	        String result = keyCode.ToString();
	        switch (keyCode)
	        {
	            case Keys.Escape:
	                //timer.Enabled = false;
	                break;
	            case Keys.W: g_angles.fAngleX += SMALL_ANGLE_INCREMENT; break;
	            case Keys.S: g_angles.fAngleX -= SMALL_ANGLE_INCREMENT; break;
	
	            case Keys.A: g_angles.fAngleY += SMALL_ANGLE_INCREMENT; break;
	            case Keys.D: g_angles.fAngleY -= SMALL_ANGLE_INCREMENT; break;
	
	            case Keys.Q: g_angles.fAngleZ += SMALL_ANGLE_INCREMENT; break;
	            case Keys.E: g_angles.fAngleZ -= SMALL_ANGLE_INCREMENT; break;
	
	            case Keys.Space:
	                g_bDrawGimbals = !g_bDrawGimbals;
	                break;
	        }
	
	        display();
	        return result;
	    }
	}
}

