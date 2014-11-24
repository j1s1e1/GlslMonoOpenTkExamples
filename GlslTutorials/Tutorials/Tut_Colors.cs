using System;
using OpenTK;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Colors : TutorialBase
	{
	    public static string VertexColor_vert =
	
	    "attribute vec4 position;" +
	    "attribute vec4 color;" +
		"uniform float xScale;" +
		"uniform float xOffset;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_Position = vec4(position.x * xScale + xOffset, position.y, position.z, position.w);" +
	        "theColor = color;" +
	    "}";
	
	    public static string VertexColor_frag =
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_FragColor = theColor;" +
	    "}";
		
		/*
		 * 		1 Red		2 Orange		4 Yellow	6 chartreuse
		 * 	 
		 * 
		 * 
		 * 
		 * 		0 Red		3 Orange		5 Yellow	7 chartreuse
		 * */
		
		static float triangleWidth = 2f / 12f  ;
		
		static float[] vertexData = new float[] {
			    // Position
	            -1f + triangleWidth * 0f,    -1f, 0f, 1f,
	            -1f + triangleWidth * 0f,  	  1f, 0f, 1f,
				-1f + triangleWidth * 1f,    -1f, 0f, 1f,
	            -1f + triangleWidth * 1f,  	  1f, 0f, 1f,
				-1f + triangleWidth * 2f,    -1f, 0f, 1f,
	            -1f + triangleWidth * 2f,  	  1f, 0f, 1f,
				-1f + triangleWidth * 3f,    -1f, 0f, 1f,
	            -1f + triangleWidth * 3f,  	  1f, 0f, 1f,
				-1f + triangleWidth * 4f,    -1f, 0f, 1f,
	            -1f + triangleWidth * 4f,  	  1f, 0f, 1f,
				-1f + triangleWidth * 5f,    -1f, 0f, 1f,
	            -1f + triangleWidth * 5f,  	  1f, 0f, 1f,
				-1f + triangleWidth * 6f,    -1f, 0f, 1f,
	            -1f + triangleWidth * 6f,  	  1f, 0f, 1f,
				-1f + triangleWidth * 7f,    -1f, 0f, 1f,
	            -1f + triangleWidth * 7f,  	  1f, 0f, 1f,
				-1f + triangleWidth * 8f,    -1f, 0f, 1f,
	            -1f + triangleWidth * 8f,  	  1f, 0f, 1f,
				-1f + triangleWidth * 9f,    -1f, 0f, 1f,
	            -1f + triangleWidth * 9f,  	  1f, 0f, 1f,
				-1f + triangleWidth * 10f,   -1f, 0f, 1f,
	            -1f + triangleWidth * 10f,    1f, 0f, 1f,
				-1f + triangleWidth * 11f,   -1f, 0f, 1f,
	            -1f + triangleWidth * 11f,    1f, 0f, 1f,
				-1f + triangleWidth * 12f,   -1f, 0f, 1f,
	            -1f + triangleWidth * 12f,    1f, 0f, 1f,
				-1f + triangleWidth * 13f,   -1f, 0f, 1f,
	            -1f + triangleWidth * 13f,    1f, 0f, 1f,
				-1f + triangleWidth * 14f,   -1f, 0f, 1f,
	            -1f + triangleWidth * 14f,    1f, 0f, 1f,
				-1f + triangleWidth * 15f,   -1f, 0f, 1f,
	            -1f + triangleWidth * 15f,    1f, 0f, 1f,
				-1f + triangleWidth * 16f,   -1f, 0f, 1f,
	            -1f + triangleWidth * 16f,    1f, 0f, 1f,			
				// Color
	            1.0f,    0.0f, 0.0f, 1.0f,
				1.0f,    0.0f, 0.0f, 1.0f,
	            1.0f,    0.5f, 0.0f, 1.0f,
				1.0f,    0.5f, 0.0f, 1.0f,
	            1.0f,    1.0f, 0.0f, 1.0f,
				1.0f,    1.0f, 0.0f, 1.0f,
				0.5f,    1.0f, 0.0f, 1.0f,
				0.5f,    1.0f, 0.0f, 1.0f,
				0.0f,    1.0f, 0.0f, 1.0f,
				0.0f,    1.0f, 0.0f, 1.0f,
				0.0f,    1.0f, 0.5f, 1.0f,
				0.0f,    1.0f, 0.5f, 1.0f,
				0.0f,    1.0f, 1.0f, 1.0f,
				0.0f,    1.0f, 1.0f, 1.0f,
				0.0f,    0.5f, 1.0f, 1.0f,
				0.0f,    0.5f, 1.0f, 1.0f,
				0.0f,    0.0f, 1.0f, 1.0f,
				0.0f,    0.0f, 1.0f, 1.0f,
				0.5f,    0.0f, 1.0f, 1.0f,
				0.5f,    0.0f, 1.0f, 1.0f,
				1.0f,    0.0f, 1.0f, 1.0f,
				1.0f,    0.0f, 1.0f, 1.0f,
				1.0f,    0.0f, 0.5f, 1.0f,
				1.0f,    0.0f, 0.5f, 1.0f,
				1.0f,    0.0f, 0.0f, 1.0f,
				1.0f,    0.0f, 0.0f, 1.0f,
				0.5f,    0.5f, 0.0f, 1.0f,
				0.5f,    0.5f, 0.0f, 1.0f,
				0.0f,    0.5f, 0.5f, 1.0f,
				0.0f,    0.5f, 0.5f, 1.0f,
				0.5f,    0.0f, 0.5f, 1.0f,
				0.5f,    0.0f, 0.5f, 1.0f,
				0.5f,    0.5f, 0.5f, 1.0f,
				0.5f,    0.5f, 0.5f, 1.0f,
	    };
		
		private static int vertexCount = vertexData.Length / 2 / COORDS_PER_VERTEX;
		private static int COLOR_START = vertexCount * POSITION_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;
		
		private static float xOffset = 0f;
		private static float xScale = 1f;
		private static int xOffsetUnif;
		private static int xScaleUnif;
		
	    void InitializeProgram()
	    {
	        int vertex_shader = Shader.loadShader(ShaderType.VertexShader, VertexColor_vert);
	        int fragment_shader = Shader.loadShader(ShaderType.FragmentShader, VertexColor_frag);
	        theProgram = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
	        positionAttribute = GL.GetAttribLocation(theProgram, "position");
	        colorAttribute = GL.GetAttribLocation(theProgram, "color");
			xOffsetUnif = GL.GetUniformLocation(theProgram, "xOffset");
			xScaleUnif = GL.GetUniformLocation(theProgram, "xScale");
	    }
		
	    protected override void init()
	    {
	        InitializeProgram();
	        InitializeVertexBuffer(vertexData);
			SetupDepthAndCull();
	    }
		
	    public override void display()
	    {
	        ClearDisplay();
			
	        GL.UseProgram(theProgram);
			
			GL.Uniform1(xOffsetUnif, xOffset);
			GL.Uniform1(xScaleUnif, xScale);
	
	        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
	        GL.EnableVertexAttribArray(positionAttribute);
	        GL.EnableVertexAttribArray(colorAttribute);
	        GL.VertexAttribPointer(positionAttribute, POSITION_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float, 
				                       false, POSITION_STRIDE, 0);
	        GL.VertexAttribPointer(colorAttribute, COLOR_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float, 
				                       false, COLOR_STRIDE, COLOR_START);
	        GL.DrawArrays(PrimitiveType.TriangleStrip, 0, vertexCount);
	
	        GL.DisableVertexAttribArray(positionAttribute);
	        GL.DisableVertexAttribArray(colorAttribute);
	        GL.UseProgram(0);
	    }
		
		public override String keyboard(Keys keyCode, int x, int y)
	    {
	        StringBuilder result = new StringBuilder();
	        switch (keyCode) {

	            case Keys.Add:
					xScale = xScale * 1.05f;
	                break;	
	            case Keys.Subtract:
	                xScale = xScale * 1f / 1.05f;
	                break;					
	            case Keys.NumPad4:
	                xOffset = xOffset - 0.05f;
	                break;
				case Keys.NumPad5:
					xScale = 1f;
	                xOffset = 0f;
	                break;	
	            case Keys.NumPad6:
	                xOffset = xOffset + 0.05f;
	                break;			
	        }
	        result.AppendLine(keyCode.ToString());
	        reshape();
	        display();
	        return result.ToString();
	    }
	}
}

