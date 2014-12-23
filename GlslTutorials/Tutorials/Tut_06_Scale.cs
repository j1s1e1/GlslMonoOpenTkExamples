using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_06_Scale : TutorialBase
	{
		public delegate Vector3 ScaleFunction(float x);
		
			
	 	static String PosColorLocalTransform_vert =
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
	
	    static String ColorPassthrough_frag =
	    "varying vec4 theColor;" +
	    "void main()" +
	    "{" +
	        "gl_FragColor = theColor;" +
	    "}";
	
	    private static int numberOfVertices = 8;
	    private static int COLOR_START = numberOfVertices * POSITION_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;
	
	    void InitializeProgram()
	    {
	        int vertexShader = Shader.compileShader(ShaderType.VertexShader, PosColorLocalTransform_vert);
	        int fragmentShader = Shader.compileShader(ShaderType.FragmentShader, ColorPassthrough_frag);
	        theProgram = Shader.createAndLinkProgram(vertexShader, fragmentShader);
	
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
	
	    Vector3 NullScale(float fElapsedTime)
	    {
	        return new Vector3(1.0f, 1.0f, 1.0f);
	    }
	
	    Vector3 StaticUniformScale(float fElapsedTime)
	    {
	        return new Vector3(4.0f, 4.0f, 4.0f);
	    }
	
	    Vector3 StaticNonUniformScale(float fElapsedTime)
	    {
	        return new Vector3(0.5f, 1.0f, 10.0f);
	    }
	
	    Vector3 DynamicUniformScale(float fElapsedTime)
	    {
	        float fLoopDuration = 3.0f;
	
	        return new Vector3(Mix(1.0f, 4.0f, CalcLerpFactor(fElapsedTime, fLoopDuration)));
	    }
	
	    Vector3 DynamicNonUniformScale(float fElapsedTime)
	    {
	        float fXLoopDuration = 3.0f;
	        float fZLoopDuration = 5.0f;
	
	        return new Vector3(Mix(1.0f, 0.5f, CalcLerpFactor(fElapsedTime, fXLoopDuration)),
	                1.0f,
	                Mix(1.0f, 10.0f, CalcLerpFactor(fElapsedTime, fZLoopDuration)));
	    }
	
	    class Instance
	    {
	        ScaleFunction CalcScale;
	        Vector3 offset;
	
	        public Instance(ScaleFunction sf, Vector3 o)
	        {
	            CalcScale = sf;
	            offset = o;
	        }
	
	        public Matrix4 ConstructMatrix(float fElapsedTime)
	        {
	            Vector3 theScale = new Vector3(0f);
				theScale = CalcScale(fElapsedTime); 
	            Matrix4 theMat = new Matrix4();
	            theMat.M11 = theScale.X;
	            theMat.M22 = theScale.Y;
	            theMat.M33 = theScale.Z;
	            theMat.Row3 = new Vector4(offset, 1.0f);
	
	            return theMat;
	        }
	    };
	
	    Instance[] g_instanceList = new Instance[5];
	
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
	        g_instanceList[0]  = new Instance(NullScale, new Vector3(0.0f, 0.0f, -45.0f));
	        g_instanceList[1]  = new Instance(StaticUniformScale, new Vector3(-10.0f, -10.0f, -45.0f));
	        g_instanceList[2]  = new Instance(StaticNonUniformScale, new Vector3(-10.0f, 10.0f, -45.0f));
	        g_instanceList[3]  = new Instance(DynamicUniformScale, new Vector3(10.0f, 10.0f, -45.0f));
	        g_instanceList[4]  = new Instance(DynamicNonUniformScale, new Vector3(10.0f, -10.0f, -45.0f));
	
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

