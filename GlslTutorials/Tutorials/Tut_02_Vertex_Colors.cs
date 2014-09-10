using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace GlslTutorials
{

	public class Tut_02_Vertex_Colors : TutorialBase 
	{
	    public static string VertexColor_vert =
	
	    "attribute vec4 position;" +
	    "attribute vec4 color;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_Position = position;" +
	        "theColor = color;" +
	    "}";
	
	    public static string VertexColor_frag =
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_FragColor = theColor;" +
	    "}";
		
		static float[] vertexData = new float[] {
	            0.0f,    0.5f, 0.0f, 1.0f,
	            0.5f, -0.366f, 0.0f, 1.0f,
	            -0.5f, -0.366f, 0.0f, 1.0f,
	            1.0f,    0.0f, 0.0f, 1.0f,
	            0.0f,    1.0f, 0.0f, 1.0f,
	            0.0f,    0.0f, 1.0f, 1.0f,
	    };
		
		private static int vertexCount = vertexData.Length / 2 / COORDS_PER_VERTEX;
		private static int COLOR_START = vertexCount * POSITION_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;
		
		
	    void InitializeProgram()
	    {
	        int vertex_shader = Shader.loadShader(ShaderType.VertexShader, VertexColor_vert);
	        int fragment_shader = Shader.loadShader(ShaderType.FragmentShader, VertexColor_frag);
	        theProgram = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
	        positionAttribute = GL.GetAttribLocation(theProgram, "position");
	        colorAttribute = GL.GetAttribLocation(theProgram, "color");
	    }
		
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
	        InitializeProgram();
	        InitializeVertexBuffer(vertexData);
	    }
		
	    //Called to update the display.
	    //You should call glutSwapBuffers after all of your rendering to display what you rendered.
	    //If you need continuous updates of the screen, call glutPostRedisplay() at the end of the function.
	    public override void display()
	    {
	        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit);
	
	        GL.UseProgram(theProgram);
	
	        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
	        GL.EnableVertexAttribArray(positionAttribute);
	        GL.EnableVertexAttribArray(colorAttribute);
	        GL.VertexAttribPointer(positionAttribute, POSITION_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float, 
				                       false, POSITION_STRIDE, 0);
	        GL.VertexAttribPointer(colorAttribute, COLOR_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float, 
				                       false, COLOR_STRIDE, COLOR_START);
	        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
	
	        GL.DisableVertexAttribArray(positionAttribute);
	        GL.DisableVertexAttribArray(colorAttribute);
	        GL.UseProgram(0);
	    }
	}

}

