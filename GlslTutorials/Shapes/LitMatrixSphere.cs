using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class LitMatrixSphere : Shape
	{
		public static int mProgram = -1;
		private static int positionAttribute;
	
	    private int cameraToClipMatrixUnif;
	    private static int worldToCameraMatrixUnif;
	    private int modelToWorldMatrixUnif;
	    private int colorUnif;
	
		float radius;
		
		string VertexShader = VertexShaders.PosOnlyWorldTransform_vert;
		string FragmentShader = FragmentShaders.ColorUniform_frag;
		
		public LitMatrixSphere (float radius_in)
		{
			radius = radius_in;
        	vertexCoords = GetCircleCoords(radius);
        	vertexCount = vertexCoords.Length / COORDS_PER_VERTEX / 2;
		
			vertexData = vertexCoords;
			SetupSimpleIndexBuffer();
			
        	InitializeVertexBuffer();

	        if (mProgram < 0)
	        {
	            // prepare shaders and OpenGL program
	            int vertexShader = Shader.loadShader(ShaderType.VertexShader,
	                    VertexShader);
	            int fragmentShader = Shader.loadShader(ShaderType.FragmentShader,
	                    FragmentShader);
	
	            mProgram = Shader.createAndLinkProgram(vertexShader, fragmentShader,
	                    new String[] {"a_Position", "a_Normal"});
	
	            // get handle to vertex shader's vPosition member
	            positionAttribute = GL.GetAttribLocation(mProgram, "position");
	
	            cameraToClipMatrixUnif = GL.GetUniformLocation(mProgram, "cameraToClipMatrix");
	            worldToCameraMatrixUnif = GL.GetUniformLocation(mProgram, "worldToCameraMatrix");
	            modelToWorldMatrixUnif = GL.GetUniformLocation(mProgram, "modelToWorldMatrix");
	            colorUnif = GL.GetUniformLocation(mProgram, "baseColor");
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
		
	    private void DrawSub(int first_triangle, int last_triangle)
	    {
	        int newVertexCount = (last_triangle - first_triangle + 1) * 3 * 3 / COORDS_PER_VERTEX;
	        // Add program to OpenGL environment
	        GL.UseProgram(mProgram);
			
		 	Matrix4 mm = Rotate(modelToWorld, axis, angle);
			mm.M41 = offset.X;
			mm.M42 = offset.Y;
			mm.M43 = offset.Z;
	
	        GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref cameraToClip);
	        GL.UniformMatrix4(worldToCameraMatrixUnif, false, ref worldToCamera);
	        GL.UniformMatrix4(modelToWorldMatrixUnif, false, ref mm);
	        GL.Uniform4(colorUnif, 1, color);
			
			 // Enable a handle to the triangle vertices
	        
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);
			GL.EnableVertexAttribArray(positionAttribute);
	        // Prepare the triangle coordinate data
	        GL.VertexAttribPointer(positionAttribute, COORDS_PER_VERTEX, 
				VertexAttribPointerType.Float, false, vertexStride, (IntPtr)0);
			
	        // Draw the triangle
	 		GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
			
	        // Disable vertex array
	        GL.DisableVertexAttribArray(positionAttribute);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	        GL.UseProgram(0);				
	    }
	
	    public override void Draw() {
	        DrawSub(0, 19);
	    }
	
	    public void DrawSemi(int first_triangle, int last_triangle) 
		{
	        DrawSub(first_triangle, last_triangle);
	    }
	}
}

