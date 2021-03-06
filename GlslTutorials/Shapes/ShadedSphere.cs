using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class ShadedSphere : Shape
	{
		float radius;
		
		public ShadedSphere(float radius_in)
		{
			radius = radius_in;
        	vertexCoords = GetCircleCoords(radius);
        	vertexCount = vertexCoords.Length / COORDS_PER_VERTEX / 2;
		
			vertexData = vertexCoords;
			SetupSimpleIndexBuffer();
			
        	InitializeVertexBuffer();
			
			programNumber = Programs.AddProgram(VertexShaders.lms_vertexShaderCode, 
              	FragmentShaders.lms_fragmentShaderCode);
		}
		
		private float[] GetCircleCoords(float radius) 
		{
	        float[] coords = Icosahedron.GetDividedTriangles(4);
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
	        Matrix4 mm = Rotate(modelToWorld, axis, angle);	
			
			Programs.Draw(programNumber, vertexBufferObject[0], indexBufferObject[0], mm, indexData.Length, color);
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

