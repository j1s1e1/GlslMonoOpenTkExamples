using System;
using System.Drawing;
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
		public Matrix4 modelToWorld = Matrix4.Identity;
		public static Matrix4 cameraToClip = Matrix4.Identity;

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
		
		protected int[] vertexBufferObject = new int[1];
	    protected int[] indexBufferObject = new int[1];
		
		protected float[] vertexData;
		protected short[] indexData;
		
		protected string VertexShader = VertexShaders.PosOnlyWorldTransform_vert;
		protected string FragmentShader = FragmentShaders.ColorUniform_frag;
		protected int programNumber;

		protected static Random random = new Random();
		
		public virtual void SetProgram(int newProgram)
		{
			programNumber = newProgram;
		}

		public int GetProgram()
		{
			return programNumber;
		}
		
		public static void ResetWorldToCameraMatrix()
	    {
	        worldToCamera = Matrix4.Identity;
	    }

		public static void ScaleWorldToCameraMatrix(float scaleFactor)
		{
			worldToCamera = Matrix4.Mult(Matrix4.Scale(new Vector3(scaleFactor, scaleFactor, scaleFactor)), worldToCamera);
		}

		public static void ResetCameraToClipMatrix()
		{
			cameraToClip = Matrix4.Identity;
		}


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
			Move(new Vector3(x_add, y_add, z_add));
        }

		public virtual void Move (Vector3 v)
		{
			modelToWorld.Row3 += new Vector4(v, 0f); 
		}

		public virtual void SetOffset(float x, float y, float z)
		{
			SetOffset(new Vector3(x, y, z));
		}

        public virtual void SetOffset (Vector3 offsetIn)
        {
			modelToWorld.Row3 = new Vector4(offsetIn, 1.0f);
        }
		
        public virtual void SetXOffset(float x_in)
        {
			modelToWorld.M41 = x_in;
        }
        public virtual void SetYOffset(float y_in)
        {
			modelToWorld.M42 = y_in;
        }
        public virtual void SetZOffset(float z_in)
        {
			modelToWorld.M43 = z_in;
        }
		public virtual Vector3 GetOffset()
		{
			return new Vector3(modelToWorld.M41, modelToWorld.M42, modelToWorld.M43);
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
		public virtual void SetColor(float red, float green, float blue) {
            color[0] = red;
            color[1] = green;
            color[2] = blue;
        }

		public virtual void SetColor(Color colorIn)
		{
			color[0] = colorIn.R / 256f;
			color[1] = colorIn.G / 256f;
			color[2] = colorIn.B / 256f;
			
		}
		
		public virtual void SetColor(float[] new_color)
		{
			color = new_color;
		}

		public void LighterColor(float multiple)
		{
			color[0]  *= multiple;
			color[1]  *= multiple;
			color[2]  *= multiple;
		}

		public void DarkerColor(float multiple)
		{
			color[0]  *= multiple;
			color[1]  *= multiple;
			color[2]  *= multiple;
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

		public static void RotateWorld(Vector3 offset, Vector3 rotationAxis, float angleDeg)
		{
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angleDeg);
			worldToCamera.Row3 = worldToCamera.Row3 + new Vector4(offset, 0);
			worldToCamera = Matrix4.Mult(worldToCamera, rotation);
			worldToCamera.Row3 = worldToCamera.Row3 - new Vector4(offset, 0);
		}

		public static void SetCameraOffset(Vector3 v)
		{
			worldToCamera.M41 = v.X;
			worldToCamera.M42 = v.Y;
			worldToCamera.M43 = v.Z;
		}

		public static void MoveWorld(Vector3 v)
		{
			worldToCamera.M41 += v.X;
			worldToCamera.M42 += v.Y;
			worldToCamera.M43 += v.Z;
		}

		public static void RotateCamera(Vector3 focalPoint, Vector3 axis, float angleDeg)
		{
			//Vector3 currentCameraPosition = new Vector3(worldToCamera.M41, worldToCamera.M42, worldToCamera.M43);
		}

		public static void SetCameraRotation(Quaternion q)
		{
		}

		public static void SetCameraRotation(float xRotation, float yRotation, float zRotation)
		{

		}

		public static void SetWorldToCameraRotation(float xRotation, float yRotation, float zRotation)
		{
			ResetWorldToCameraMatrix();
			RotateWorld(Vector3.UnitX, xRotation);
			RotateWorld(Vector3.UnitY, yRotation);
			RotateWorld(Vector3.UnitZ, zRotation);
		}

		public void RotateShapeAboutAxis(float angleDeg)
		{
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(axis, (float)Math.PI / 180.0f * angleDeg);
			modelToWorld = Matrix4.Mult(modelToWorld, rotation);			
		}
		
		public void RotateShape(Vector3 rotationAxis, float angleDeg)
		{
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angleDeg);
			modelToWorld = Matrix4.Mult(modelToWorld, rotation);			
		}

		public void RotateShape(Vector3 offset, Vector3 rotationAxis, float angleDeg)
		{
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angleDeg);
			//rotation.Row3 = rotation.Row3 - new Vector4(offset, 0);
			modelToWorld.Row3 = modelToWorld.Row3 - new Vector4(offset, 0);
			modelToWorld = Matrix4.Mult(modelToWorld, rotation);
			modelToWorld.Row3 = modelToWorld.Row3 + new Vector4(offset, 0);
		}

		public virtual void Scale(Vector3 scale)
		{
			modelToWorld = Matrix4.Mult(modelToWorld, Matrix4.CreateScale(scale));
		}

		public static void SetScale(float scale)
		{
			worldToCamera.M11 = scale;
			worldToCamera.M22 = scale;
			worldToCamera.M33 = scale;
		}

		public static void SetScale(Vector3 scale)
		{
			worldToCamera.M11 = scale.X;
			worldToCamera.M22 = scale.Y;
			worldToCamera.M33 = scale.Z;
		}

		public void SetRotation(Matrix3 rotation)
		{
			modelToWorld.Row0 = new Vector4(rotation.Row0, modelToWorld.M14);
			modelToWorld.Row1 = new Vector4(rotation.Row1, modelToWorld.M24);
			modelToWorld.Row2 = new Vector4(rotation.Row2, modelToWorld.M34);
		}

		public static void SetCameraToClipMatrix(Matrix4 m)
		{
			cameraToClip = m;
		}

		public static void SetWorldToCameraMatrix(Matrix4 m)
		{
			worldToCamera = m;
		}

		public virtual void SetLightPosition(Vector3 lightPosition)
		{
			Programs.SetLightPosition(programNumber, lightPosition);
		}
	}
}

