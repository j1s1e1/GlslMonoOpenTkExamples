using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_06_Rotations : TutorialBase
	{
		public delegate Matrix3 RotationFunc(float x); 
		
		public String PosColorLocalTransform_vert =
	    "attribute vec4 color;" +
	    "attribute vec4 position;" +
	
	    "uniform mat4 cameraToClipMatrix;" +
	    "uniform mat4 modelToCameraMatrix;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "vec4 cameraPos = modelToCameraMatrix * position;" +
	        "gl_Position = cameraToClipMatrix * cameraPos;" +
	        "theColor = color;" +
	    "}";
	
	    public String ColorPassthrough_frag =
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_FragColor = theColor;" +
	    "}";
	
	    void InitializeProgram()
	    {
	        int vertex_shader = Shader.loadShader(ShaderType.VertexShader, PosColorLocalTransform_vert);
	        int fragment_shader = Shader.loadShader(ShaderType.FragmentShader, ColorPassthrough_frag);
	        theProgram = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
	        positionAttribute = GL.GetAttribLocation(theProgram, "position");
	        colorAttribute = GL.GetAttribLocation(theProgram, "color");
	
	        modelToCameraMatrixUnif = GL.GetUniformLocation(theProgram, "modelToCameraMatrix");
	        cameraToClipMatrixUnif = GL.GetUniformLocation(theProgram, "cameraToClipMatrix");
	
	        fFrustumScale = CalcFrustumScale(45.0f);
	        float fzNear = 1.0f;
	        float fzFar = 61.0f;
	
	        cameraToClipMatrix.M11 = fFrustumScale;
	        cameraToClipMatrix.M22 = fFrustumScale;
	        cameraToClipMatrix.M33 = (fzFar + fzNear) / (fzNear - fzFar);
	        cameraToClipMatrix.M34 = -1.0f;
	        cameraToClipMatrix.M43 = (2 * fzFar * fzNear) / (fzNear - fzFar);
	
	        GL.UseProgram(theProgram);
	        GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref cameraToClipMatrix);
	        GL.UseProgram(0);
	    }
	
	    static int numberOfVertices = 8;
	    private int COLOR_START = numberOfVertices * POSITION_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;
	
	    static float[] vertexData =
	    {
	            +1.0f, +1.0f, +1.0f, 1.0f,
	            -1.0f, -1.0f, +1.0f, 1.0f,
	            -1.0f, +1.0f, -1.0f, 1.0f,
	            +1.0f, -1.0f, -1.0f, 1.0f,
	
	            -1.0f, -1.0f, -1.0f, 1.0f,
	            +1.0f, +1.0f, -1.0f, 1.0f,
	            +1.0f, -1.0f, +1.0f, 1.0f,
	            -1.0f, +1.0f, +1.0f, 1.0f,
	
	            Colors.GREEN_COLOR[0], Colors.GREEN_COLOR[1], Colors.GREEN_COLOR[2], Colors.GREEN_COLOR[3],
	            Colors.BLUE_COLOR[0], Colors.BLUE_COLOR[1], Colors.BLUE_COLOR[2], Colors.BLUE_COLOR[3],
	            Colors.RED_COLOR[0], Colors.RED_COLOR[1], Colors.RED_COLOR[2], Colors.RED_COLOR[3],
	            Colors.BROWN_COLOR[0], Colors.BROWN_COLOR[1], Colors.BROWN_COLOR[2], Colors.BROWN_COLOR[3],
	
	            Colors.GREEN_COLOR[0], Colors.GREEN_COLOR[1], Colors.GREEN_COLOR[2], Colors.GREEN_COLOR[3],
	            Colors.BLUE_COLOR[0], Colors.BLUE_COLOR[1], Colors.BLUE_COLOR[2], Colors.BLUE_COLOR[3],
	            Colors.RED_COLOR[0], Colors.RED_COLOR[1], Colors.RED_COLOR[2], Colors.RED_COLOR[3],
	            Colors.BROWN_COLOR[0], Colors.BROWN_COLOR[1], Colors.BROWN_COLOR[2], Colors.BROWN_COLOR[3]
	    };
	
	    static short[] indexData =
	    {
	            0, 1, 2,
	            1, 0, 3,
	            2, 3, 0,
	            3, 2, 1,
	
	            5, 4, 6,
	            4, 5, 7,
	            7, 6, 4,
	            6, 7, 5,
	    };
	
	    float CalcLerpFactor(float fElapsedTime, float fLoopDuration)
	    {
	        float fValue = fElapsedTime % fLoopDuration / fLoopDuration;
	        if(fValue > 0.5f)
	            fValue = 1.0f - fValue;
	
	        return fValue * 2.0f;
	    }
	
	    Matrix3 NullRotation(float fElapsedTime)
	    {
	        return Matrix3.Identity;
	    }
	
	    float ComputeAngleRad(float fElapsedTime, float fLoopDuration)
	    {
	        float fScale = 3.14159f * 2.0f / fLoopDuration;
	        float fCurrTimeThroughLoop = fElapsedTime % fLoopDuration;
	        return fCurrTimeThroughLoop * fScale;
	    }
	
	    Matrix3 RotateX(float fElapsedTime)
	    {
	        float fAngRad = ComputeAngleRad(fElapsedTime, 3.0f);
	        float fCos = (float)Math.Cos(fAngRad);
	        float fSin = (float)Math.Sin(fAngRad);
	
	        Matrix3 theMat = Matrix3.Identity;
	        theMat.M11 = fCos; theMat.M21 = -fSin;
	        theMat.M12 = fSin; theMat.M22 = fCos;
	        return theMat;
	    }
	
	    Matrix3 RotateY(float fElapsedTime)
	    {
	        float fAngRad = ComputeAngleRad(fElapsedTime, 2.0f);
	        float fCos = (float)Math.Cos(fAngRad);
	        float fSin = (float)Math.Sin(fAngRad);
	
	        Matrix3 theMat = Matrix3.Identity;
	        theMat.M11 = fCos; theMat.M31 = fSin;
	        theMat.M13 = -fSin; theMat.M33 = fCos;
	        return theMat;
	    }
	
	    Matrix3 RotateZ(float fElapsedTime)
	    {
	        float fAngRad = ComputeAngleRad(fElapsedTime, 2.0f);
	        float fCos = (float)Math.Cos(fAngRad);
	        float fSin = (float)Math.Sin(fAngRad);
	
	        Matrix3 theMat = Matrix3.Identity;
	        theMat.M11 = fCos; theMat.M21 = -fSin;
	        theMat.M12 = fSin; theMat.M22 = fCos;
	        return theMat;
	    }
	
	    Matrix3 RotateAxis(float fElapsedTime)
	    {
	        float fAngRad = ComputeAngleRad(fElapsedTime, 2.0f);
	        float fCos = (float)Math.Cos(fAngRad);
	        float fInvCos = 1.0f - fCos;
	        float fSin = (float)Math.Sin(fAngRad);
	
	        Vector3 axis = new Vector3(1.0f, 1.0f, 1.0f);
	        axis.Normalize();
	
	        Matrix3 theMat = Matrix3.Identity;
	        theMat.M11 = (axis.X * axis.X) + ((1 - axis.X * axis.X) * fCos);
	        theMat.M21 = axis.X * axis.Y * (fInvCos) - (axis.Z * fSin);
	        theMat.M31 = axis.X * axis.Z * (fInvCos) + (axis.Y * fSin);
	
	        theMat.M21 = axis.X * axis.Y * (fInvCos) + (axis.Z * fSin);
	        theMat.M22 = (axis.Y * axis.Y) + ((1 - axis.Y * axis.Y) * fCos);
	        theMat.M32 = axis.Y * axis.Z * (fInvCos) - (axis.X * fSin);
	
	        theMat.M13 = axis.X * axis.Z * (fInvCos) - (axis.Y * fSin);
	        theMat.M23 = axis.Y * axis.Z * (fInvCos) + (axis.X * fSin);
	        theMat.M33 = (axis.Z * axis.Z) + ((1 - axis.Z * axis.Z) * fCos);
	        return theMat;
	    }
	
	
	    class Instance
	    {
	        RotationFunc CalcRotation;
	        Vector3 offset;
	
	        public Matrix4 ConstructMatrix(float fElapsedTime)
	        {
	            Matrix3 rotMatrix = new Matrix3();
	            rotMatrix = CalcRotation(fElapsedTime);
	            Matrix4 theMat =  Matrix4.Identity;
	            theMat.Row0 = new Vector4(rotMatrix.Row0, 0.0f);
	            theMat.Row1 = new Vector4(rotMatrix.Row1, 0.0f);
	            theMat.Row2 = new Vector4(rotMatrix.Row2, 0.0f);
	            theMat.Row3 = new Vector4(offset, 1.0f);
	            return theMat;
	        }
	
	        public Instance(RotationFunc CalcRotation_in, Vector3 offset_in)
	        {
	            CalcRotation = CalcRotation_in;
	            offset = offset_in;
	        }
	    };
	
	    Instance[] g_instanceList;
	
	    void SetupGInstanceList()
	    {
	        g_instanceList = new Instance[5];
	        g_instanceList[0] = new Instance(NullRotation, new Vector3(0.0f, 0.0f, -25.0f));
	        g_instanceList[1] = new Instance(RotateX, new Vector3(-5.0f, -5.0f, -25.0f));
	        g_instanceList[2] = new Instance(RotateY, new Vector3(-5.0f, 5.0f, -25.0f));
	        g_instanceList[3] = new Instance(RotateZ, new Vector3(5.0f, 5.0f, -25.0f));
	        g_instanceList[4] = new Instance(RotateAxis, new Vector3(5.0f, -5.0f, -25.0f));
	    }
	
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
	        SetupGInstanceList();
	        InitializeProgram();
	        InitializeVertexBuffer(vertexData, indexData);
	
			GL.Enable(EnableCap.CullFace);
	        GL.CullFace(CullFaceMode.Back);
	        GL.FrontFace(FrontFaceDirection.Cw);
	
	        GL.Enable(EnableCap.DepthTest);
	        GL.DepthMask(true);
	        GL.DepthFunc(DepthFunction.Lequal);
	        GL.DepthRange(0.0f, 1.0f);
	    }
	
	    //Called to update the display.
	    //You should call glutSwapBuffers after all of your rendering to display what you rendered.
	    //If you need continuous updates of the screen, call glutPostRedisplay() at the end of the function.
	    public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
	
	        GL.UseProgram(theProgram);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
	        // Bind Attributes
	        GL.EnableVertexAttribArray(positionAttribute);
	        GL.EnableVertexAttribArray(colorAttribute);
	        GL.VertexAttribPointer(positionAttribute, POSITION_DATA_SIZE_IN_ELEMENTS, 
			                       VertexAttribPointerType.Float, false, POSITION_STRIDE, 0);
	        GL.VertexAttribPointer(colorAttribute, COLOR_DATA_SIZE_IN_ELEMENTS,
			                       VertexAttribPointerType.Float, false, COLOR_STRIDE, COLOR_START);
	
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);
	
	        float fElapsedTime = GetElapsedTime() / 1000.0f;
	        for(int iLoop = 0; iLoop < g_instanceList.Length; iLoop++)
	        {
	            Instance currInst = g_instanceList[iLoop];
	            Matrix4 transformMatrix = currInst.ConstructMatrix(fElapsedTime);
	
	            GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref transformMatrix);
	            GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
	        }
	
	        GL.DisableVertexAttribArray(positionAttribute);
	        GL.DisableVertexAttribArray(colorAttribute);
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
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
	}
}

