using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

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
		protected static int TEXTURE_DATA_SIZE_IN_ELEMENTS = 2;
		protected static int TEXTURE_STRIDE = TEXTURE_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;
    	protected Timer timer;

	    // For single program tutorials
	    protected int 		theProgram;
	    protected int 		positionAttribute;
	    protected int 		colorAttribute;
	    protected int  		modelToCameraMatrixUnif;
	    protected int		cameraToClipMatrixUnif;
	    protected int  		baseColorUnif;
	    protected static Matrix4 cameraToClipMatrix = new Matrix4();
		protected static Matrix4 worldToCameraMatrix = new Matrix4();

	    protected short[] elementSB;
	    protected float[] vertexDataFB;
	
	    protected static int BYTES_PER_FLOAT = 4;
	    protected static int BYTES_PER_SHORT = 2;
	
	    protected int width = 512;
	    protected int height = 512;
		
		protected int fzFar;

		protected float g_fzNear = 1.0f;
		protected float g_fzFar = 10f;
		
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
	            MessageBox.Show("Error " + ex.ToString());
	        }
	        reshape();
	        messages.AppendLine("Tutorial Setup Complete");
	        return messages;
	    }

	    public static float GetElapsedTime()
	    {
	    	return elapsedTime;
	    }

	    protected int[] vertexBufferObject = new int[1];
	    protected int[] indexBufferObject = new int[1];
	
	    protected void InitializeVertexBuffer(float[] vertexData)
	    {
			
	        GL.GenBuffers(1, vertexBufferObject);
	        GL.GenBuffers(1, indexBufferObject);
	
	        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
	        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * BYTES_PER_FLOAT), 
			              vertexData, BufferUsageHint.StaticDraw);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);  
	    }
		
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
	    public virtual void reshape()
	    {
	        //GL.Viewport(0, 0, width, height);
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

		public virtual void MouseButton(int button, int state, int x, int y)
		{
		}

		public virtual void MouseMotion(int x, int y)
		{
		}
		
		protected void SetupDepthAndCull()
		{
			GL.Enable(EnableCap.CullFace);
	        GL.CullFace(CullFaceMode.Back);
	        GL.FrontFace(FrontFaceDirection.Cw);
	
	        GL.Enable(EnableCap.DepthTest);
	        GL.DepthMask(true);
	        GL.DepthFunc(DepthFunction.Lequal);
	        GL.DepthRange(0.0f, 1.0f); 
		}
		
		protected void ClearDisplay()
		{
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	        GL.ClearDepth(1.0f);
	        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		bool info = false;
		protected bool displayOptions = false;
		bool updateCull = false;
		bool updateDepth = false;
		bool updateDepthMask = false;
		bool updateAlpha = false;
		bool updateCcw = false;
		bool updateBlend = false;
		bool blend = false;
		bool ccw = false;
		bool updateCullFace = false;
		int cullFaceSelection = 0;
		bool cull = true;
		bool depth = true;
		bool depthMask = true;
		bool alpha = false;

		bool callReshape = false;

		string logDisplayState()
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine("alpha " +  alpha.ToString());
			result.AppendLine("cull " +  cull.ToString());
			result.AppendLine("cullFaceSelection " +  cullFaceSelection.ToString());
			result.AppendLine("depth " + depth.ToString());
			result.AppendLine("blend " + blend.ToString());
			return result.ToString();
		}

		protected void SetDisplayOptions(Keys keyCode)
		{
			switch (keyCode) {
			case Keys.Enter:
				displayOptions = false;
				break;
			case Keys.A:
				updateAlpha = true;
				break;
			case Keys.C:
				updateCull = true;
				break;
			case Keys.D:
				updateDepth = true;
				break;
			case Keys.M:
				updateDepthMask = true;
				break;
			case Keys.D1:
				g_fzNear = 1f;
				g_fzFar = 10;
				callReshape = true;
				break;
			case Keys.D2:
				g_fzNear = 1f;
				g_fzFar = 100;
				callReshape = true;
				break;
			case Keys.D3:
				g_fzNear = 10f;
				g_fzFar = 100;
				callReshape = true;
				break;
			case Keys.D4:
				g_fzNear = 10f;
				g_fzFar = 1000;
				callReshape = true;
				break;
			case Keys.D5:
				g_fzNear = 0.1f;
				g_fzFar = 2f;
				callReshape = true;
				break;

			case Keys.D6:
				g_fzNear = 0.05f;
				g_fzFar = 2f;
				callReshape = true;
				break;
			case Keys.D7:
				g_fzNear = 0.25f;
				g_fzFar = 2f;
				callReshape = true;
				break;

			case Keys.D8:
				g_fzNear = 0.5f;
				g_fzFar = 2f;
				callReshape = true;
				break;
			case Keys.D9:
				g_fzNear = 1f;
				g_fzFar = 2f;
				callReshape = true;
				break;
			}
		}

		protected string UpdateDisplayOptions() {
			StringBuilder result = new StringBuilder();
			if (updateAlpha) {
				updateAlpha = false;
				if (alpha) {
					alpha = false;
					GL.BlendEquation(BlendEquationMode.FuncAdd);
					GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
					result.AppendLine("alpha disabled");
				} else {
					alpha = true;
					GL.BlendEquation(BlendEquationMode.FuncAdd);
					GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrc1Color);
					result.AppendLine("alpha enabled");
				}
			}
			if (updateBlend) {
				updateBlend = false;
				if (blend) {
					blend = false;
					GL.Disable(EnableCap.Blend);
					result.AppendLine("blend disabled");
				} else {
					blend = true;
					GL.Enable(EnableCap.Blend);
					result.AppendLine("blend enabled");
				}
			}
			if (updateCull) {
				updateCull = false;
				if (cull) {
					cull = false;
					GL.Disable(EnableCap.CullFace);
					result.AppendLine("cull disabled");
				} else {
					cull = true;
					GL.Enable(EnableCap.CullFace);
					result.AppendLine("cull enabled");
				}
			}
			if (updateDepth)
			{
				updateDepth = false;
				if (depth)
				{
					depth = false;
					GL.Disable(EnableCap.DepthTest);
					GL.DepthMask(false);
					result.AppendLine("depth disabled");
				}
				else
				{
					depth = true;
					GL.Enable(EnableCap.DepthTest);
					GL.DepthMask(true);
					result.AppendLine("depth enabled");
				}
			}
			if (updateCullFace)
			{
				updateCullFace = false;
				cullFaceSelection++;
				if (cullFaceSelection > 2) cullFaceSelection = 0;
				switch (cullFaceSelection) {
				case 0:
					GL.CullFace(CullFaceMode.FrontAndBack);
					result.AppendLine("cull face GL_FRONT_AND_BACK");
					break;
				case 1:
					GL.CullFace(CullFaceMode.Front);
					result.AppendLine("cull face GL_FRONT");
					break;
				case 2:
					GL.CullFace(CullFaceMode.Back);
					result.AppendLine("cull face GL_BACK");
					break;
				}
			}
			if (callReshape)
			{
				callReshape = false;
				reshape();
			}
			if (info) result.Append(logDisplayState());
			return result.ToString();
		}
	}
}

