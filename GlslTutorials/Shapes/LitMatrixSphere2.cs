using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class LitMatrixSphere2 : Shape
	{
	    Matrix4 cameraToClip = Matrix4.Identity;
	    Matrix4 modelToWorld = Matrix4.Identity;
		
		float radius;
		
		string VertexShader = VertexShaders.PosOnlyWorldTransform_vert;
		string FragmentShader = FragmentShaders.ColorUniform_frag;
		
		int progarmNumber;
		
		public LitMatrixSphere2 (float radius_in)
		{
			radius = radius_in;
        	vertexCoords = GetCircleCoords(radius);
        	vertexCount = vertexCoords.Length / COORDS_PER_VERTEX / 2;
		
			vertexData = vertexCoords;
			SetupSimpleIndexBuffer(COORDS_PER_VERTEX);
			
        	InitializeVertexBuffer();
			
			progarmNumber = Programs.AddProgram(VertexShader, FragmentShader);
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

		 	Matrix4 mm = Rotate(modelToWorld, axis, angle);
			mm.M41 = offset.X;
			mm.M42 = offset.Y;
			mm.M43 = offset.Z;	
			
			Programs.Draw(progarmNumber, vertexBufferObject, indexBufferObject, cameraToClip, worldToCamera, mm,
			              indexData.Length, color, COORDS_PER_VERTEX, vertexStride);
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
