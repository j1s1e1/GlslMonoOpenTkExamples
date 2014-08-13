using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_06_Translation : TutorialBase
	{
		public delegate Vector3 OffsetFunc(float x); 

	    public String ColorPassthrough_frag =
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_FragColor = theColor;" +
	    "}";
	
	    void InitializeProgram()
	    {
	        int vertex_shader = Shader.loadShader(ShaderType.VertexShader, VertexShaders.PosColorLocalTransform_vert);
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
	            Colors.BROWN_COLOR[0], Colors.BROWN_COLOR[1], Colors.BROWN_COLOR[2], Colors.BROWN_COLOR[3],
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
	
	    static Vector3 StationaryOffset(float fElapsedTime)
	    {
	        return new Vector3(0.0f, 0.0f, -20.0f);
	    }
	
	    Vector3 OvalOffset(float fElapsedTime)
	    {
	        float fLoopDuration = 3.0f;
	        float fScale = 3.14159f * 2.0f / fLoopDuration;
	
	        float fCurrTimeThroughLoop = fElapsedTime % fLoopDuration;
	
	        return new Vector3((float)Math.Cos(fCurrTimeThroughLoop * fScale) * 4f,
	                (float)Math.Sin(fCurrTimeThroughLoop * fScale) * 6f,
	                -20.0f);
	    }
	
	    Vector3 BottomCircleOffset(float fElapsedTime)
	    {
	        float fLoopDuration = 12.0f;
	        float fScale = 3.14159f * 2.0f / fLoopDuration;
	
	        float fCurrTimeThroughLoop = fElapsedTime % fLoopDuration;
	
	        return new Vector3((float)Math.Cos(fCurrTimeThroughLoop * fScale) * 5f,
	                -3.5f,
	                (float)Math.Sin(fCurrTimeThroughLoop * fScale) * 5f - 20.0f);
	    }
	
	    class Instance
	    {
	        OffsetFunc CalcOffset;
	
	        public Matrix4 ConstructMatrix(float fElapsedTime)
	        {
	            Matrix4 theMat = Matrix4.Identity;
				theMat.Row3 = new Vector4 (CalcOffset(fElapsedTime), 1.0f); 
	            return theMat;
	        }
	        public Instance(OffsetFunc CalcOffset_In)
	        {
	            CalcOffset = CalcOffset_In;
	        }
	    };
	
	    static Instance[] g_instanceList;
	
	    void SetupGInstanceList()
	    {
	        g_instanceList = new Instance[3];
	        g_instanceList[0] = new Instance(StationaryOffset);
	        g_instanceList[1] = new Instance(OvalOffset);
	        g_instanceList[2] = new Instance(BottomCircleOffset);
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
	                GL.DrawElements(PrimitiveType.Triangles, indexData.Length,  DrawElementsType.UnsignedShort, 0);
	            }
	
	            GL.DisableVertexAttribArray(positionAttribute);
	            GL.DisableVertexAttribArray(colorAttribute);
	            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	            GL.UseProgram(0);
	        }
		}
}

