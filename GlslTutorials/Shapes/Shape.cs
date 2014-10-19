using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Shape
	{
		protected static int BYTES_PER_FLOAT = 4;
	    protected static int BYTES_PER_SHORT = 2;
		
 		public static float global_x_offset = 0;
        public static float global_y_offset = 0;
        public static float global_z_offset = 0;
        public static float global_x_rotate = 0;
        public static float global_y_rotate = 0;
        public static float global_z_rotate = 0;
		
		public static Matrix4 worldToCamera = Matrix4.Identity;
		protected Matrix4 modelToWorld = Matrix4.Identity;
		protected Matrix4 cameraToClip = Matrix4.Identity;

        protected float x = 0;
        protected float y = 0;
        protected float z = 0;

        protected bool global_move = true;

        protected float angle1 = 0;
        protected float angle2 = 0;
        protected float angle3 = 0;
        protected float rotation1 = 0;
        protected float rotation2 = 0;
        protected float rotation3 = 0;

        protected int vertexNormalBufferID;
        protected int indiciesBufferID;

        protected int[] indicesVboData;

        protected float[] color = new float[]{1f, 0f, 0f, 1f};
        protected int[] colorData = null;
        protected int colorBufferID;
        protected int bufferSize;

        public static bool allGreen = false;

        // From Working Sphere
        // number of coordinates per vertex in this array
        protected int vertexCount;
        protected int COORDS_PER_VERTEX = 3;
        protected int vertexStride = 3 * 4 * 2; // bytes per vertex
        protected float[] vertexCoords;
		

        protected void SetupVertexBuffer()
        {
			/*
            // initialize vertex byte buffer for shape coordinates
            ByteBuffer bb = ByteBuffer.allocateDirect(
                    // (number of coordinate values * 4 bytes per float)
                    vertexCoords.length * 4);
            // use the device hardware's native byte order
            bb.order(ByteOrder.nativeOrder());

            // create a floating point buffer from the ByteBuffer
            vertexBuffer = bb.asFloatBuffer();
            // add the coordinates to the FloatBuffer
            vertexBuffer.put(vertexCoords);
            // set the buffer to read the first coordinate
            vertexBuffer.position(0);
            */
        }
		
	    protected int[] vertexBufferObject = new int[1];
	    protected int[] indexBufferObject = new int[1];
		
		protected float[] vertexData;
		protected short[] indexData;
		
		public string CheckRotations(Vector3 center)
		{
			return AnalysisTools.CheckRotations(vertexData, indexData, center);
		}
		
		public string CheckExtents()
		{
			return AnalysisTools.CheckExtents(vertexData);
		}
		
		protected void SetupSimpleIndexBuffer()
		{
			indexData = new short[vertexData.Length/COORDS_PER_VERTEX];
			for (short i = 0; i < indexData.Length; i++)
			{
				indexData[i] = i;
			}
		}
	
		protected void InitializeVertexBuffer()
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

        public Shape()
        {
        }

        public virtual void Move (float x_add, float y_add, float z_add)
        {
            x = x + x_add;
            y = y + y_add;
            z = z + z_add;
        }
		
		protected Vector3 offset = new Vector3(0);

        public virtual void SetOffset (Vector3 offsetIn)
        {
			offset = offsetIn;
        }
		
        public virtual void SetXOffset(float x_in)
        {
            offset.X = x_in;
        }
        public virtual void SetYOffset(float y_in)
        {
             offset.Y = y_in;
        }
        public virtual void SetZOffset(float z_in)
        {
             offset.Z = z_in;
        }
		public Vector3 GetOffset()
		{
			return offset;
		}	
		
        protected void SetupIndexBuffer()
        {
            indicesVboData = new int[vertexCount];
            for (int i = 0; i < indicesVboData.Length; i++) {
                indicesVboData[i] = i;
            }
            // Element Array Buffer
            //indiciesBufferID = VBO_Tools.SetupIntBuffer(indicesVboData);
        }

        // Set color with red, green, blue and alpha (opacity) values
        public void SetColor(float red, float green, float blue) {
            color[0] = red;
            color[1] = green;
            color[2] = blue;
        }
		
		public void SetColor(float[] new_color)
		{
			color = new_color;
		}

        public void SolidColor(int color)
        {
			/*
            colorData = new int[vertexCount];
            for (int i = 0; i < colorData.Length; i++) {
                colorData[i] = VBO_Tools.ColorToRgba32(color);
            }
            // Color Array Buffer
            colorBufferID = VBO_Tools.SetupIntBuffer(colorData);
            */
        }

        public void MultiColor(int[] colors)
        {
			/*
            colorData = new int[vertexCount];
            for (int i = 0; i < colorData.length; i++) {
                colorData[i] = VBO_Tools.ColorToRgba32(colors[(i % colors.length)]);
            }
            // Color Array Buffer
            colorBufferID = VBO_Tools.SetupIntBuffer(colorData);
            */
        }

        public void SetRainbowColors()
        {
			/*
            // Color Data for the Verticies
            colorData = new int[vertexCount];
            for (int i = 0; i < colorData.Length; i++) {
                if (allGreen) {
                    switch (i % 8) {
                        case 0:
                            colorData[i] = 32 << 8;
                            break;
                        case 1:
                            colorData[i] = 64 << 8;
                            break;
                        case 2:
                            colorData[i] = 96 << 8;
                            break;
                        case 3:
                            colorData[i] = 128 << 8;
                            break;
                        case 4:
                            colorData[i] = 160 << 8;
                            break;
                        case 5:
                            colorData[i] = 192 << 8;
                            break;
                        case 6:
                            colorData[i] = 224 << 8;
                            ;
                            break;
                        case 7:
                            colorData[i] = 255 << 8;
                            break;
                    }
                } else {
                    switch (i % 8) {
                        case 0:
                            colorData[i] = VBO_Tools.ColorToRgba32(Colors.Red);
                            break;
                        case 1:
                            colorData[i] = VBO_Tools.ColorToRgba32(Colors.White);
                            break;
                        case 2:
                            colorData[i] = VBO_Tools.ColorToRgba32(Colors.Blue);
                            break;
                        case 3:
                            colorData[i] = VBO_Tools.ColorToRgba32(Colors.Green);
                            break;
                        case 4:
                            colorData[i] = VBO_Tools.ColorToRgba32(Colors.Yellow);
                            break;
                        case 5:
                            colorData[i] = VBO_Tools.ColorToRgba32(Colors.Orange);
                            break;
                        case 6:
                            colorData[i] = VBO_Tools.ColorToRgba32(Colors.Pink);
                            break;
                        case 7:
                            colorData[i] = VBO_Tools.ColorToRgba32(Colors.Purple);
                            break;
                    }
                }
            }
            // Color Array Buffer
            colorBufferID = VBO_Tools.SetupIntBuffer(colorData);
            */
        }

        public void SetAngles(float a1, float a2, float a3)
        {
            angle1 = a1;
            angle2 = a2;
            angle3 = a3;
        }

        public Vector3 GetAngles()
        {
            return new Vector3 (angle1, angle2, angle3);
        }

        public void SetRotations(float r1, float r2, float r3)
        {
            rotation1 = r1;
            rotation2 = r2;
            rotation3 = r3;
        }

	    public virtual void Draw()
	    {
	       
	    }
		
		protected Vector3 axis = new Vector3(0f, 0f, 1f);
		
		public void SetAxis(Vector3 axisIn)
		{
			axis = axisIn;
		}
		
	    protected float angle = 0;
		
		public void UpdateAngle(float degrees)
	    {
	        angle = degrees;
	    }
		
		///Applies a rotation matrix about the given axis, with the given angle in degrees.
	    public Matrix4 Rotate(Matrix4 input, Vector3 rotationAxis, float angleDeg)
	    {
	        Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angleDeg);
	        return Matrix4.Mult(rotation, input);
	    }
		
		public static void RotateWorld(Vector3 rotationAxis, float angleDeg)
		{
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angleDeg);
			worldToCamera = Matrix4.Mult(worldToCamera, rotation);
		}
		
		public void RotateShape(Vector3 rotationAxis, float angleDeg)
		{
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angleDeg);
			modelToWorld = Matrix4.Mult(modelToWorld, rotation);			
		}
	}
}

