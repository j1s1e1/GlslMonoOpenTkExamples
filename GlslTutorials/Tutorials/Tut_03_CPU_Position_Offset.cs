using System;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_03_CPU_Position_Offset : TutorialBase 
	{
	 	private string standard_vert =
	    "attribute vec4 position;"+
	
	    "void main()"+
	    "{"+
	        "gl_Position = position;"+
	    " }";

	    private string standard_frag =
	    "void main()"+
	    "{"+
	        "gl_FragColor = vec4(1.0, 1.0, 1.0, 1.0);"+
	    "}";
	
	    static float[] vertexData = new float[]{
	            0.25f, 0.25f, 0.0f, 1.0f,
	            0.25f, -0.25f, 0.0f, 1.0f,
	            -0.25f, -0.25f, 0.0f, 1.0f,
	    };

    	static short[] indexData = new short[]{ 0, 1, 2 };

    	private static int POSITION_STRIDE = POSITION_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;
    	private static int POSITION_START = 0;

	    void UpdateVertexBuffer(float[] data)
	    {
	        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
	        GL.BufferData(BufferTarget.ArrayBuffer,  (IntPtr)(data.Length * BYTES_PER_FLOAT), 
			              data, BufferUsageHint.StaticDraw);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	    }


	    private void InitializeProgram()
	    {
	        int vertexShader = Shader.loadShader(ShaderType.VertexShader, standard_vert);
	        int fragmentShader = Shader.loadShader(ShaderType.FragmentShader, standard_frag);
	        theProgram = Shader.createAndLinkProgram(vertexShader, fragmentShader);
	        positionAttribute = GL.GetAttribLocation(theProgram, "position");
	    }
	
	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected override void init()
	    {
	        InitializeProgram();
	        InitializeVertexBuffer(vertexData, indexData);
	    }
	
	    static float fXOffset;
	    static float fYOffset;
	
	    static void ComputePositionOffsets()
	    {
	        float fLoopDuration = 5.0f;
	        float fScale = 3.14159f * 2.0f / fLoopDuration;
	        float fElapsedTime = GetElapsedTime() / 1000f;
	        float fCurrTimeThroughLoop = fElapsedTime % fLoopDuration;
	        fXOffset = (float)Math.Cos(fCurrTimeThroughLoop * fScale) * 0.5f;
	        fYOffset = (float)Math.Sin(fCurrTimeThroughLoop * fScale) * 0.5f;
	    }
	
	    void AdjustVertexData(float fXOffset, float fYOffset)
	    {
	        float[] fNewData = new float[vertexData.Length];
	        Array.Copy(vertexData, 0, fNewData, 0, vertexData.Length);
	        for(int iVertex = 0; iVertex < vertexData.Length; iVertex += 4)
	        {
	            fNewData[iVertex] += fXOffset;
	            fNewData[iVertex + 1] += fYOffset;
	        }
	        UpdateVertexBuffer(fNewData);
	    }
	
	
	    //Called to update the display.
	    //You should call glutSwapBuffers after all of your rendering to display what you rendered.
	    //If you need continuous updates of the screen, call glutPostRedisplay() at the end of the function.
	    public override void display()
	    {
	        fXOffset = 0.0f;
	        fYOffset = 0.0f;
	        ComputePositionOffsets();
	        AdjustVertexData(fXOffset, fYOffset);
	
		    GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
		    GL.Clear(ClearBufferMask.ColorBufferBit);
		
		    GL.UseProgram(theProgram);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
	        GL.EnableVertexAttribArray(positionAttribute);
	        GL.VertexAttribPointer(positionAttribute, POSITION_DATA_SIZE_IN_ELEMENTS, VertexAttribPointerType.Float, 
				                       false, POSITION_STRIDE, 0);
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);
	        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
	
	        GL.DisableVertexAttribArray(positionAttribute);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	        GL.UseProgram(0);
	    }
	}
}

