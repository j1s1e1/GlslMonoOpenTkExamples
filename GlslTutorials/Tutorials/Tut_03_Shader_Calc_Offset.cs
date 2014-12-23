using System;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_03_Shader_Calc_Offset : TutorialBase 
	{
		static float[] vertexData = new float[]{
            0.25f, 0.25f, 0.0f, 1.0f,
            0.25f, -0.25f, 0.0f, 1.0f,
            -0.25f, -0.25f, 0.0f, 1.0f,
    	};
		
		static short[] indexData = new short[]{ 0, 1, 2 };

	    static int elapsedTimeUniform;

	    static String calcOffset_vert =
	    "attribute vec4 position;"+
	    "uniform float loopDuration;"+
	    "uniform float time;"+
	
	    "void main()"+
	    "{"+
	    "float timeScale = 3.14159 * 2.0 / loopDuration;" +
	
	    "float currTime = mod(time, loopDuration);" +
	            "vec4 totalOffset = vec4(" +
	            "cos(currTime * timeScale) * 0.5," +
	            "sin(currTime * timeScale) * 0.5," +
	            " 0.0," +
	            "0.0);" +
	
	            "gl_Position = position + totalOffset;" +
	    "}";
	
	    static String calcColor_frag =
	
	    "uniform float fragLoopDuration;"+
	    "uniform float time;"+
	
	    "const vec4 firstColor = vec4(1.0, 0.0, 1.0, 1.0);"+
	    "const vec4 secondColor = vec4(0.0, 1.0, 0.0, 1.0);"+
	
	    "void main()"+
	    "{"+
	        "float currTime = mod(time, fragLoopDuration);"+
	        "float currLerp = currTime / fragLoopDuration;"+
	
	        "gl_FragColor = mix(firstColor, secondColor, currLerp);"+
	    "}";
	
	    void InitializeProgram()
	    {
	        // prepare shaders and OpenGL program
	        int vertexShader = Shader.compileShader(ShaderType.VertexShader, calcOffset_vert);
	        int fragmentShader = Shader.compileShader(ShaderType.FragmentShader, calcColor_frag);
	
	        theProgram = Shader.createAndLinkProgram(vertexShader, fragmentShader);
	
	        positionAttribute = GL.GetAttribLocation(theProgram, "position");
	
	        elapsedTimeUniform = GL.GetUniformLocation(theProgram, "time");
	
	
	        int loopDurationUnf = GL.GetUniformLocation(theProgram, "loopDuration");
	        int fragLoopDurUnf = GL.GetUniformLocation(theProgram, "fragLoopDuration");
	        GL.UseProgram(theProgram);
	        GL.Uniform1(loopDurationUnf, 5.0f);
	        GL.Uniform1(fragLoopDurUnf, 2.0f);
	        GL.UseProgram(0);
	
	    }
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
	        InitializeProgram();
	        InitializeVertexBuffer(vertexData, indexData);
	    }
	
	    //Called to update the display.
	    //You should call glutSwapBuffers after all of your rendering to display what you rendered.
	    //If you need continuous updates of the screen, call glutPostRedisplay() at the end of the function.
	    public override void display()
	    {
		    GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
		    GL.Clear(ClearBufferMask.ColorBufferBit);
		
		    GL.UseProgram(theProgram);
	
	        GL.Uniform1(elapsedTimeUniform, GetElapsedTime() / 1000.0f);
	
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
	        GL.EnableVertexAttribArray(positionAttribute);
	        GL.VertexAttribPointer(positionAttribute, POSITION_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float, 
				                       false, POSITION_STRIDE, 0);
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);
	        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
	
	        GL.DisableVertexAttribArray(0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	        GL.UseProgram(0);
	    }
	}
}

