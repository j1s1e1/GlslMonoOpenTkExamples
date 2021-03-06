using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class LitMatrixSphere2 : Shape
	{
		float radius;
		private int divideCount;

		static new float[] vertexData;
		static new short[] indexData;
		static new int[] vertexBufferObject = new int[]{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
		static new int[] indexBufferObject = new int[10];
		static new int COORDS_PER_VERTEX = 3;

		static void InitializeVertexBuffer(int divideCount)
		{
			GL.GenBuffers(1, out vertexBufferObject[divideCount]);
			GL.GenBuffers(1, out indexBufferObject[divideCount]);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[divideCount]);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexData.Length * BYTES_PER_SHORT), 
				indexData, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[divideCount]);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * BYTES_PER_FLOAT), 
				vertexData, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);  
		}

		static new void SetupSimpleIndexBuffer()
		{
			indexData = new short[vertexData.Length/COORDS_PER_VERTEX];
			for (short i = 0; i < indexData.Length; i++)
			{
				indexData[i] = i;
			}
		}
		
		public LitMatrixSphere2 (float radius_in, int divideCountIn = 1)
		{
			divideCount = divideCountIn;
			radius = radius_in;
			Scale(new Vector3(radius, radius, radius));

			if (vertexBufferObject[divideCount] == -1)
			{
				vertexCoords = GetCircleCoords(1.0f);
				COORDS_PER_VERTEX = 6;
	        	vertexCount = vertexCoords.Length / COORDS_PER_VERTEX;
			
				vertexData = vertexCoords;
				SetupSimpleIndexBuffer();
				
				InitializeVertexBuffer(divideCount);
			}
			
			programNumber = Programs.AddProgram(VertexShaders.lms_vertexShaderCode, 
              	FragmentShaders.lms_fragmentShaderCode);
		}
		
		~LitMatrixSphere2()
		{
			GL.DeleteBuffers(1, ref vertexBufferObject[0]);
			GL.DeleteBuffers(1, ref indexBufferObject[0]);
		}
		
		private float[] GetCircleCoords(float radius) 
		{
			float[] coords = Icosahedron.GetDividedTriangles(divideCount);
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
			int newVertexCount = indexData.Length / 20 * (last_triangle - first_triangle + 1);

			Programs.Draw(programNumber, vertexBufferObject[divideCount], indexBufferObject[divideCount], modelToWorld,
				newVertexCount, color);
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

