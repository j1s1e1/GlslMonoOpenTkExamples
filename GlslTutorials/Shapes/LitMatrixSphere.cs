using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class LitMatrixSphere : Shape
	{
		public static int mProgram = -1;
    	private static int mPositionHandle;
    	private static int mNormalHandle;
    	private static int mColorHandle;
    	private static int mLightPosHandle;
	
	    private int cameraToClipMatrixUnif;
	    private static int worldToCameraMatrixUnif;
	    private int modelToWorldMatrixUnif;
	    private int colorUnif;
	
	    static Matrix4 cameraToClip = new Matrix4();
	    static Matrix4 worldToCamera = new Matrix4();
	    Matrix4 modelToWorld = new Matrix4();
		
		float radius;
		
		public LitMatrixSphere (float radius_in)
		{
			radius = radius_in;
        	vertexCoords = GetCircleCoords(radius);
        	vertexCount = vertexCoords.Length / COORDS_PER_VERTEX / 2;
		
			vertexData = vertexCoords;
			SetupSimpleIndexBuffer(COORDS_PER_VERTEX);
			
        	InitializeVertexBuffer();

	        if (mProgram < 0)
	        {
	            // prepare shaders and OpenGL program
	            int vertexShader = Shader.loadShader(ShaderType.VertexShader,
	                    VertexShaders.lms_vertexShaderCode);
	            int fragmentShader = Shader.loadShader(ShaderType.FragmentShader,
	                    FragmentShaders.lms_fragmentShaderCode);
	
	            mProgram = Shader.createAndLinkProgram(vertexShader, fragmentShader);
	
	
	            // get handle to vertex shader's vPosition member
	            mPositionHandle = GL.GetAttribLocation(mProgram, "a_Position");
	            mNormalHandle = GL.GetAttribLocation(mProgram, "a_Normal");
	            mLightPosHandle = GL.GetUniformLocation(mProgram, "u_LightPos");
	        }
		}
		
		private float[] GetCircleCoords(float radius) 
		{
	        float[] coords = Icosahedron.CloneTriangles();
	        float[] coords_with_normals = new float[2*coords.Length];
	        int j = 0;
	        for (int i = 0; i < coords.Length * 2; i++)
	        {
	            switch (i % 6)
	            {
	                case 0:
	                case 1:
	                case 2:
	                    coords_with_normals[i] = coords[j] * radius;
	                    j++;
	                    break;
	                case 3:  coords_with_normals[i] = coords[j-3]; break;
	                case 4:  coords_with_normals[i] = coords[j-2]; break;
	                case 5:  coords_with_normals[i] = coords[j-1]; break;
	
	            }
	
	        }
	        return coords_with_normals;
    	}
		
		public void SetOffset(Vector3 offset)
		{
		}
		
	    private void DrawSub(int first_triangle, int last_triangle)
	    {
	        int newVertexCount = (last_triangle - first_triangle + 1) * 3 * 3 / COORDS_PER_VERTEX;
	        // Add program to OpenGL environment
	        GL.UseProgram(mProgram);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);
	
	        // Enable a handle to the triangle vertices
	        GL.EnableVertexAttribArray(mPositionHandle);
	
	        // Prepare the triangle coordinate data
	        GL.VertexAttribPointer(mPositionHandle, COORDS_PER_VERTEX, VertexAttribPointerType.Float, false,
	                vertexStride, (IntPtr)0);
	
	        // Pass in the normal information
	        GL.EnableVertexAttribArray(mNormalHandle);
	        GL.VertexAttribPointer(mNormalHandle, COORDS_PER_VERTEX, VertexAttribPointerType.Float, false,
	                vertexStride, (IntPtr)0);
	
	        // Pass in the light position in eye space.
	        GL.Uniform3(mLightPosHandle, 0.75f, 0.75f, 0.75f);
	
	
	        // get handle to fragment shader's vColor member
	        mColorHandle = GL.GetUniformLocation(mProgram, "u_Color");
	
	        // Set color for drawing the triangle
	        GL.Uniform4(mColorHandle, 1, color);
	
	 		GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
			
	        GL.DisableVertexAttribArray(mPositionHandle);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	
	        GL.UseProgram(0);
	    }
	
	    public void Draw() {
	        DrawSub(0, 19);
	    }
	
	    public void DrawSemi(int first_triangle, int last_triangle) 
		{
	        DrawSub(first_triangle, last_triangle);
	    }
	}
}

