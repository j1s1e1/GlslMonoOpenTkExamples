using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_04_MatrixPerspective : TutorialBase
	{ 
  
		static String MatrixPerspective_vert =
	    "attribute vec4 position;" +
	    "attribute vec4 color;" +
	
	    "uniform vec2 offset;" +
	    "uniform mat4 perspectiveMatrix;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "vec4 cameraPos = position + vec4(offset.x, offset.y, 0.0, 0.0);" +
	        "gl_Position = perspectiveMatrix * cameraPos;" +
	        "theColor = color;" +
	    "}";

	    static String StandardColors_frag =
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_FragColor = theColor;" +
	    "}";
	
	    static short[] indexData = new short[]{
	            0,1,2,
	            3,4,5,
	            6,7,8,
	            9,10,11,
	            12,13,14,
	            15,16,17,
	            18,19,20,
	            21,22,23,
	            24,25,26,
	            27,28,29,
	            30,31,32,
	            33,34,35
	    };
	
	    static float[] vertexData = new float[]{
	
	            0.25f,  0.25f, 0.75f, 1.0f,
	            0.25f, -0.25f, 0.75f, 1.0f,
	            -0.25f,  0.25f, 0.75f, 1.0f,
	
	            0.25f, -0.25f, 0.75f, 1.0f,
	            -0.25f, -0.25f, 0.75f, 1.0f,
	            -0.25f,  0.25f, 0.75f, 1.0f,
	
	            0.25f,  0.25f, -0.75f, 1.0f,
	            -0.25f,  0.25f, -0.75f, 1.0f,
	            0.25f, -0.25f, -0.75f, 1.0f,
	
	            0.25f, -0.25f, -0.75f, 1.0f,
	            -0.25f,  0.25f, -0.75f, 1.0f,
	            -0.25f, -0.25f, -0.75f, 1.0f,
	
	            -0.25f,  0.25f,  0.75f, 1.0f,
	            -0.25f, -0.25f,  0.75f, 1.0f,
	            -0.25f, -0.25f, -0.75f, 1.0f,
	
	            -0.25f,  0.25f,  0.75f, 1.0f,
	            -0.25f, -0.25f, -0.75f, 1.0f,
	            -0.25f,  0.25f, -0.75f, 1.0f,
	
	            0.25f,  0.25f,  0.75f, 1.0f,
	            0.25f, -0.25f, -0.75f, 1.0f,
	            0.25f, -0.25f,  0.75f, 1.0f,
	
	            0.25f,  0.25f,  0.75f, 1.0f,
	            0.25f,  0.25f, -0.75f, 1.0f,
	            0.25f, -0.25f, -0.75f, 1.0f,
	
	            0.25f,  0.25f, -0.75f, 1.0f,
	            0.25f,  0.25f,  0.75f, 1.0f,
	            -0.25f,  0.25f,  0.75f, 1.0f,
	
	            0.25f,  0.25f, -0.75f, 1.0f,
	            -0.25f,  0.25f,  0.75f, 1.0f,
	            -0.25f,  0.25f, -0.75f, 1.0f,
	
	            0.25f, -0.25f, -0.75f, 1.0f,
	            -0.25f, -0.25f,  0.75f, 1.0f,
	            0.25f, -0.25f,  0.75f, 1.0f,
	
	            0.25f, -0.25f, -0.75f, 1.0f,
	            -0.25f, -0.25f, -0.75f, 1.0f,
	            -0.25f, -0.25f,  0.75f, 1.0f,
	
	            0.0f, 0.0f, 1.0f, 1.0f,
	            0.0f, 0.0f, 1.0f, 1.0f,
	            0.0f, 0.0f, 1.0f, 1.0f,
	
	            0.0f, 0.0f, 1.0f, 1.0f,
	            0.0f, 0.0f, 1.0f, 1.0f,
	            0.0f, 0.0f, 1.0f, 1.0f,
	
	            0.8f, 0.8f, 0.8f, 1.0f,
	            0.8f, 0.8f, 0.8f, 1.0f,
	            0.8f, 0.8f, 0.8f, 1.0f,
	
	            0.8f, 0.8f, 0.8f, 1.0f,
	            0.8f, 0.8f, 0.8f, 1.0f,
	            0.8f, 0.8f, 0.8f, 1.0f,
	
	            0.0f, 1.0f, 0.0f, 1.0f,
	            0.0f, 1.0f, 0.0f, 1.0f,
	            0.0f, 1.0f, 0.0f, 1.0f,
	
	            0.0f, 1.0f, 0.0f, 1.0f,
	            0.0f, 1.0f, 0.0f, 1.0f,
	            0.0f, 1.0f, 0.0f, 1.0f,
	
	            0.5f, 0.5f, 0.0f, 1.0f,
	            0.5f, 0.5f, 0.0f, 1.0f,
	            0.5f, 0.5f, 0.0f, 1.0f,
	
	            0.5f, 0.5f, 0.0f, 1.0f,
	            0.5f, 0.5f, 0.0f, 1.0f,
	            0.5f, 0.5f, 0.0f, 1.0f,
	
	            1.0f, 0.0f, 0.0f, 1.0f,
	            1.0f, 0.0f, 0.0f, 1.0f,
	            1.0f, 0.0f, 0.0f, 1.0f,
	
	            1.0f, 0.0f, 0.0f, 1.0f,
	            1.0f, 0.0f, 0.0f, 1.0f,
	            1.0f, 0.0f, 0.0f, 1.0f,
	
	            0.0f, 1.0f, 1.0f, 1.0f,
	            0.0f, 1.0f, 1.0f, 1.0f,
	            0.0f, 1.0f, 1.0f, 1.0f,
	
	            0.0f, 1.0f, 1.0f, 1.0f,
	            0.0f, 1.0f, 1.0f, 1.0f,
	            0.0f, 1.0f, 1.0f, 1.0f,
	
	    };
	
	    private static int vertexCount = vertexData.Length / 2 / COORDS_PER_VERTEX;
	
	    private static int COLOR_START = vertexCount * POSITION_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;
	
	
	    static int offsetUniform;
	    static int perspectiveMatrixUnif;
	
	    void InitializeProgram()
	    {
	        int vertexShader = Shader.loadShader(ShaderType.VertexShader, MatrixPerspective_vert);
	        int fragmentShader = Shader.loadShader(ShaderType.FragmentShader, StandardColors_frag);
	        theProgram = Shader.createAndLinkProgram(vertexShader, fragmentShader);
	
	        offsetUniform = GL.GetUniformLocation(theProgram, "offset");
	        perspectiveMatrixUnif = GL.GetUniformLocation(theProgram, "perspectiveMatrix");
	
	        float fFrustumScale = 0.5f;
	        float fzNear = 0.5f;
	        float fzFar = 3.0f;
	
	        Matrix4 theMatrix = new Matrix4();
	
	        theMatrix.M11 = fFrustumScale;
	        theMatrix.M22 = fFrustumScale;
	        theMatrix.M33 = (fzFar + fzNear) / (fzNear - fzFar);
	        theMatrix.M43 = (2 * fzFar * fzNear) / (fzNear - fzFar);
	        theMatrix.M34 = -1.0f;
	
	        //theMatrix = Matrix4.Identity;
	
	        GL.UseProgram(theProgram);
	        GL.UniformMatrix4(perspectiveMatrixUnif, false, ref theMatrix);
	        positionAttribute = GL.GetAttribLocation(theProgram, "position");
	        colorAttribute = GL.GetAttribLocation(theProgram, "color");
	        GL.UseProgram(0);
	    }
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
	        InitializeProgram();
	        InitializeVertexBuffer(vertexData, indexData);
	        GL.Enable(EnableCap.CullFace);
	        GL.CullFace(CullFaceMode.Front);
	        GL.FrontFace(FrontFaceDirection.Cw);
	    } 
	
	    //Called to update the display.
	    //You should call glutSwapBuffers after all of your rendering to display what you rendered.
	    //If you need continuous updates of the screen, call glutPostRedisplay() at the end of the function.
	    public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit);
	
	        GL.UseProgram(theProgram);
	
	        GL.Uniform2(offsetUniform, 0.5f, 0.5f);
	
	        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
	        // Bind Attributes
	        GL.EnableVertexAttribArray(positionAttribute);
	        GL.EnableVertexAttribArray(colorAttribute);
	        GL.VertexAttribPointer(positionAttribute, POSITION_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float,
	                false, POSITION_STRIDE, 0);
	        GL.VertexAttribPointer(colorAttribute, COLOR_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float,
	                false, COLOR_STRIDE,  COLOR_START);
	
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);
	
	        GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
	
	        GL.DisableVertexAttribArray(positionAttribute);
	        GL.DisableVertexAttribArray(colorAttribute);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	        GL.UseProgram(0);
	
	    }

	}
}

