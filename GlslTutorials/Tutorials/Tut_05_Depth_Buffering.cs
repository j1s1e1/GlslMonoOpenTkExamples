using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_05_Depth_Buffering : TutorialBase
	{
	 public String standard_vert5 =
	    "attribute vec4 position;" +
	    "attribute vec4 color;" +
	
	    "uniform vec3 offset;" +
	    "uniform mat4 perspectiveMatrix;" +
	
	    "varying vec4 theColor;" +
	
	    "void main(void)" +
	    "{" +
	        "vec4 cameraPos = position + vec4(offset.x, offset.y, offset.z, 0.0);" +
	
	        "gl_Position = perspectiveMatrix * cameraPos;" +
	        "theColor = color;" +
	    "}";
	
	    public String StandardColors_frag =
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_FragColor = theColor;" +
	    "}";
	
	    static int numberOfVertices = 36;
	    private static int COLOR_START = numberOfVertices * POSITION_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;
	    static float RIGHT_EXTENT = 0.8f;
	    static float LEFT_EXTENT = -RIGHT_EXTENT;
	    static float TOP_EXTENT = 0.20f;
	    static float MIDDLE_EXTENT = 0.0f;
	    static float BOTTOM_EXTENT = -TOP_EXTENT;
	    static float FRONT_EXTENT = -1.25f;
	    static float REAR_EXTENT = -1.75f;
	    static float[] GREEN_COLOR = { 0.0f, 1.0f, 0.0f, 1.0f};
	    static float[] BLUE_COLOR = { 0.0f, 0.0f, 1.0f, 1.0f};
	    static float[] RED_COLOR = { 1.0f, 0.0f, 0.0f, 1.0f};
	    static float[] GREY_COLOR = { 0.8f, 0.8f, 0.8f, 1.0f};
	    static float[] BROWN_COLOR = { 0.5f, 0.5f, 0.0f, 1.0f};
	    float[] vertexData = {
	            //Object 1 positions
	            LEFT_EXTENT,	TOP_EXTENT,		REAR_EXTENT, 1.0f,
	            LEFT_EXTENT,	MIDDLE_EXTENT,	FRONT_EXTENT, 1.0f,
	            RIGHT_EXTENT,	MIDDLE_EXTENT,	FRONT_EXTENT, 1.0f,
	
	            RIGHT_EXTENT,	TOP_EXTENT,		REAR_EXTENT, 1.0f,
	            LEFT_EXTENT,	BOTTOM_EXTENT,	REAR_EXTENT, 1.0f,
	            LEFT_EXTENT,	MIDDLE_EXTENT,	FRONT_EXTENT, 1.0f,
	
	            RIGHT_EXTENT,	MIDDLE_EXTENT,	FRONT_EXTENT, 1.0f,
	            RIGHT_EXTENT,	BOTTOM_EXTENT,	REAR_EXTENT, 1.0f,
	            LEFT_EXTENT,	TOP_EXTENT,		REAR_EXTENT, 1.0f,
	
	            LEFT_EXTENT,	MIDDLE_EXTENT,	FRONT_EXTENT, 1.0f,
	            LEFT_EXTENT,	BOTTOM_EXTENT,	REAR_EXTENT, 1.0f,
	            RIGHT_EXTENT,	TOP_EXTENT,		REAR_EXTENT, 1.0f,
	
	            RIGHT_EXTENT,	MIDDLE_EXTENT,	FRONT_EXTENT, 1.0f,
	            RIGHT_EXTENT,	BOTTOM_EXTENT,	REAR_EXTENT, 1.0f,
	            LEFT_EXTENT,	BOTTOM_EXTENT,	REAR_EXTENT, 1.0f,
	
	            LEFT_EXTENT,	TOP_EXTENT,		REAR_EXTENT, 1.0f,
	            RIGHT_EXTENT,	TOP_EXTENT,		REAR_EXTENT, 1.0f,
	            RIGHT_EXTENT,	BOTTOM_EXTENT,	REAR_EXTENT, 1.0f,
	
	            //Object 2 positions
	            TOP_EXTENT,		RIGHT_EXTENT,	REAR_EXTENT, 1.0f,
	            MIDDLE_EXTENT,	RIGHT_EXTENT,	FRONT_EXTENT, 1.0f,
	            MIDDLE_EXTENT,	LEFT_EXTENT,	FRONT_EXTENT, 1.0f,
	            TOP_EXTENT,		LEFT_EXTENT,	REAR_EXTENT, 1.0f,
	
	            BOTTOM_EXTENT,	RIGHT_EXTENT,	REAR_EXTENT, 1.0f,
	            MIDDLE_EXTENT,	RIGHT_EXTENT,	FRONT_EXTENT, 1.0f,
	            MIDDLE_EXTENT,	LEFT_EXTENT,	FRONT_EXTENT, 1.0f,
	            BOTTOM_EXTENT,	LEFT_EXTENT,	REAR_EXTENT, 1.0f,
	
	            TOP_EXTENT,		RIGHT_EXTENT,	REAR_EXTENT, 1.0f,
	            MIDDLE_EXTENT,	RIGHT_EXTENT,	FRONT_EXTENT, 1.0f,
	            BOTTOM_EXTENT,	RIGHT_EXTENT,	REAR_EXTENT, 1.0f,
	
	            TOP_EXTENT,		LEFT_EXTENT,	REAR_EXTENT, 1.0f,
	            MIDDLE_EXTENT,	LEFT_EXTENT,	FRONT_EXTENT, 1.0f,
	            BOTTOM_EXTENT,	LEFT_EXTENT,	REAR_EXTENT, 1.0f,
	
	            BOTTOM_EXTENT,	RIGHT_EXTENT,	REAR_EXTENT, 1.0f,
	            TOP_EXTENT,		RIGHT_EXTENT,	REAR_EXTENT, 1.0f,
	            TOP_EXTENT,		LEFT_EXTENT,	REAR_EXTENT, 1.0f,
	            BOTTOM_EXTENT,	LEFT_EXTENT,	REAR_EXTENT, 1.0f,
	
	            //Object 1 colors
	            GREEN_COLOR[0], GREEN_COLOR[1], GREEN_COLOR[2], GREEN_COLOR[3],
	            GREEN_COLOR[0], GREEN_COLOR[1], GREEN_COLOR[2], GREEN_COLOR[3],
	            GREEN_COLOR[0], GREEN_COLOR[1], GREEN_COLOR[2], GREEN_COLOR[3],
	            GREEN_COLOR[0], GREEN_COLOR[1], GREEN_COLOR[2], GREEN_COLOR[3],
	
	            BLUE_COLOR[0], BLUE_COLOR[1], BLUE_COLOR[2], BLUE_COLOR[3],
	            BLUE_COLOR[0], BLUE_COLOR[1], BLUE_COLOR[2], BLUE_COLOR[3],
	            BLUE_COLOR[0], BLUE_COLOR[1], BLUE_COLOR[2], BLUE_COLOR[3],
	            BLUE_COLOR[0], BLUE_COLOR[1], BLUE_COLOR[2], BLUE_COLOR[3],
	
	            RED_COLOR[0], RED_COLOR[1], RED_COLOR[2], RED_COLOR[3],
	            RED_COLOR[0], RED_COLOR[1], RED_COLOR[2], RED_COLOR[3],
	            RED_COLOR[0], RED_COLOR[1], RED_COLOR[2], RED_COLOR[3],
	
	            GREY_COLOR[0], GREY_COLOR[1], GREY_COLOR[2], GREY_COLOR[3],
	            GREY_COLOR[0], GREY_COLOR[1], GREY_COLOR[2], GREY_COLOR[3],
	            GREY_COLOR[0], GREY_COLOR[1], GREY_COLOR[2], GREY_COLOR[3],
	
	            BROWN_COLOR[0], BROWN_COLOR[1], BROWN_COLOR[2], BROWN_COLOR[3],
	            BROWN_COLOR[0], BROWN_COLOR[1], BROWN_COLOR[2], BROWN_COLOR[3],
	            BROWN_COLOR[0], BROWN_COLOR[1], BROWN_COLOR[2], BROWN_COLOR[3],
	            BROWN_COLOR[0], BROWN_COLOR[1], BROWN_COLOR[2], BROWN_COLOR[3],
	
	            RED_COLOR[0], RED_COLOR[1], RED_COLOR[2], RED_COLOR[3],
	            RED_COLOR[0], RED_COLOR[1], RED_COLOR[2], RED_COLOR[3],
	            RED_COLOR[0], RED_COLOR[1], RED_COLOR[2], RED_COLOR[3],
	            RED_COLOR[0], RED_COLOR[1], RED_COLOR[2], RED_COLOR[3],
	
	            BROWN_COLOR[0], BROWN_COLOR[1], BROWN_COLOR[2], BROWN_COLOR[3],
	            BROWN_COLOR[0], BROWN_COLOR[1], BROWN_COLOR[2], BROWN_COLOR[3],
	            BROWN_COLOR[0], BROWN_COLOR[1], BROWN_COLOR[2], BROWN_COLOR[3],
	            BROWN_COLOR[0], BROWN_COLOR[1], BROWN_COLOR[2], BROWN_COLOR[3],
	
	            BLUE_COLOR[0], BLUE_COLOR[1], BLUE_COLOR[2], BLUE_COLOR[3],
	            BLUE_COLOR[0], BLUE_COLOR[1], BLUE_COLOR[2], BLUE_COLOR[3],
	            BLUE_COLOR[0], BLUE_COLOR[1], BLUE_COLOR[2], BLUE_COLOR[3],
	
	            GREEN_COLOR[0], GREEN_COLOR[1], GREEN_COLOR[2], GREEN_COLOR[3],
	            GREEN_COLOR[0], GREEN_COLOR[1], GREEN_COLOR[2], GREEN_COLOR[3],
	            GREEN_COLOR[0], GREEN_COLOR[1], GREEN_COLOR[2], GREEN_COLOR[3],
	
	            GREY_COLOR[0], GREY_COLOR[1], GREY_COLOR[2], GREY_COLOR[3],
	            GREY_COLOR[0], GREY_COLOR[1], GREY_COLOR[2], GREY_COLOR[3],
	            GREY_COLOR[0], GREY_COLOR[1], GREY_COLOR[2], GREY_COLOR[3],
	            GREY_COLOR[0], GREY_COLOR[1], GREY_COLOR[2], GREY_COLOR[3],
	    };
	    static short[] indexData =
	            {
	                    0, 2, 1,
	                    3, 2, 0,
	
	                    4, 5, 6,
	                    6, 7, 4,
	
	                    8, 9, 10,
	                    11, 13, 12,
	
	                    14, 16, 15,
	                    17, 16, 14,
	
	                    18 + 0, 18 + 2, 18 + 1,
	                    18 + 3, 18 + 2, 18 + 0,
	
	                    18 + 4, 18 + 5, 18 + 6,
	                    18 + 6, 18 + 7, 18 + 4,
	
	                    18 + 8, 18 + 9, 18 + 10,
	                    18 + 11, 18 + 13, 18 + 12,
	
	                    18 + 14, 18 + 16, 18 + 15,
	                    18 + 17, 18 + 16, 18 + 14,
	            };
	    static int offsetUniform;
	    static int perspectiveMatrixUnif;
	
	    void InitializeProgram()
	    {
	        int vertex_shader = Shader.compileShader(ShaderType.VertexShader, standard_vert5);
	        int fragment_shader = Shader.compileShader(ShaderType.FragmentShader, StandardColors_frag);
	        theProgram = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
	        positionAttribute = GL.GetAttribLocation(theProgram, "position");
	        colorAttribute = GL.GetAttribLocation(theProgram, "color");
	
	        offsetUniform = GL.GetUniformLocation(theProgram, "offset");
	
	        perspectiveMatrixUnif = GL.GetUniformLocation(theProgram, "perspectiveMatrix");
	        fFrustumScale = 1.0f;
	        float fzNear = 0.5f;
	        float fzFar = 3.0f;
	
	        Matrix4 theMatrix = new Matrix4();
	
	        theMatrix.M11 = fFrustumScale;
	        theMatrix.M22 = fFrustumScale;
	        theMatrix.M33 = (fzFar + fzNear) / (fzNear - fzFar);
	        theMatrix.M43 = (2 * fzFar * fzNear) / (fzNear - fzFar);
	        theMatrix.M34 = -1.0f;
	
	        GL.UseProgram(theProgram);
	        GL.UniformMatrix4(perspectiveMatrixUnif, false, ref theMatrix);
	        GL.UseProgram(0);
	    }
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
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
	        GL.EnableVertexAttribArray(positionAttribute);
	        GL.EnableVertexAttribArray(colorAttribute);
	        GL.VertexAttribPointer(positionAttribute, POSITION_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float, 
			                       false, POSITION_STRIDE, 0);
	        GL.VertexAttribPointer(colorAttribute, COLOR_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float, 
			                       false, COLOR_STRIDE, COLOR_START);
	
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);
	
	        GL.Uniform3(offsetUniform, 0.0f, 0.0f, -0.75f);
	        GL.DrawElements(PrimitiveType.Triangles, indexData.Length/2, DrawElementsType.UnsignedShort, 0);
	
	        GL.Uniform3(offsetUniform, 0.0f, 0.0f, -1f);
	        GL.DrawElements(PrimitiveType.Triangles, indexData.Length/2, DrawElementsType.UnsignedShort, indexData.Length/2);
	
	        GL.DisableVertexAttribArray(positionAttribute);
	        GL.DisableVertexAttribArray(colorAttribute);
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	
	        GL.UseProgram(0);
	    }
	}
}

