using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace GlslTutorials
{
	public class TutorialBase : IDisposable
	{
		public void Dispose()
		{
			timer.Enabled = false;
			timer.Stop ();
			timer.Dispose();
		}
		public static GLControl GlControl;
		protected static int COORDS_PER_VERTEX = 4;
	    protected static int POSITION_DATA_SIZE_IN_ELEMENTS = 4;
	    protected static int COLOR_DATA_SIZE_IN_ELEMENTS = 4;
	    protected static int POSITION_STRIDE = POSITION_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;
	    protected static int COLOR_STRIDE = COLOR_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;

    	static long startTime;

    	protected Timer timer;

	    // For single program tutorials
	    protected int 		theProgram;
	    protected int 		positionAttribute;
	    protected int 		colorAttribute;
	    protected int  		modelToCameraMatrixUnif;
	    protected int		cameraToClipMatrixUnif;
	    protected int  		baseColorUnif;
	    protected Matrix4 cameraToClipMatrix = new Matrix4();

	    protected short[] elementSB;
	    protected float[] vertexDataFB;
	
	    protected static int BYTES_PER_FLOAT = 4;
	    protected static int BYTES_PER_SHORT = 2;
	
	    protected int width = 0;
	    protected int height = 0;
		
		protected int fzFar;
		
	    public TutorialBase()
	    {
			TimerSetup();
	    }
		
		static long elapsedTime = 0;
		
		private void TimerTick(Object sender, EventArgs e)
		{
			elapsedTime = elapsedTime + 10;
			if (timer.Enabled == true)
			{
				display();
				GlControl.SwapBuffers();
			}
		}
		
		private void TimerSetup()
		{
			elapsedTime = 0;
			timer = new Timer();
			timer.Tick += TimerTick;
			timer.Start();
			timer.Interval = 10;
		}

	    public StringBuilder Setup()
	    {
			
	        StringBuilder messages = new StringBuilder();
	        TimerSetup ();
	
	        try {
	            init();
	        }
	        catch (Exception ex)
	        {
	            int debug = 0;
	            debug++;
	        }
	        reshape();
	        messages.AppendLine("Tutorial Setup Complete");
	        return messages;
	    }

	    protected static float GetElapsedTime()
	    {
	    	return elapsedTime;
	    }

	    protected int[] vertexBufferObject = new int[1];
	    protected int[] indexBufferObject = new int[1];
	
	    protected void InitializeVertexBuffer(float[] vertexData, short[] indexData)
	    {
			
	        GL.GenBuffers(1, vertexBufferObject);
	        GL.GenBuffers(1, indexBufferObject);
	
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);
	        GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexData.Length * BYTES_PER_SHORT), 
			              indexData, BufferUsageHint.StaticDraw);
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	
	        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
	        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * BYTES_PER_FLOAT), 
			              vertexData, BufferUsageHint.StaticDraw);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	        
	    }

	    //Called after the window and OpenGL are initialized. Called exactly once, before the main loop.
	    protected virtual void init()
	    {
	    }

	    //Called whenever the window is resized. The new window size is given, in pixels.
	    //This is an opportunity to call glViewport or glScissor to keep up with the change in size.
	    public void reshape()
	    {
	        //GLES20.glViewport(0, 0, width, height);
	    }
	
	    public virtual void display()
	    {
	    }
		
		public virtual string keypress(System.Windows.Forms.Keys key, int x, int y)
		{
			return keyboard(key, x, y);
		}

	    //Called whenever a key on the keyboard was pressed.
	    //The key is given by the ''key'' parameter, which is in ASCII.
	    //It's often a good idea to have the escape key (ASCII value 27) call glutLeaveMainLoop() to
	    //exit the program.
	    public virtual String keyboard(System.Windows.Forms.Keys key, int x, int y)
	    {
	        switch (key) {
	            case Keys.Escape:
	                timer.Enabled = false;
	                break;
	        }
	        return "Default keyboard hanlder, only escape enabled.";
	    }

	    protected static int defaults(int displayMode, int width, int height)
	    {
	        return displayMode;
	    }
	
	    protected static float DegToRad(float fAngDeg)
	    {
	        float fDegToRad = 3.14159f * 2.0f / 360.0f;
	        return fAngDeg * fDegToRad;
	    }
	
	    protected static float CalcFrustumScale(float fFovDeg)
	    {
	        float degToRad = 3.14159f * 2.0f / 360.0f;
	        float fFovRad = fFovDeg * degToRad;
	        return 1.0f / (float)Math.Tan(fFovRad / 2.0f);
	    }
	
	    protected static float fFrustumScale;
	
	    protected static float Mix(float in1, float in2, float mix_factor)
	    {
	        return in1 * 1 - mix_factor + in2 * mix_factor;
	    }
	
	    public virtual void TouchEvent(int x_position, int y_position)
	    {
	
	    }
	}
}

