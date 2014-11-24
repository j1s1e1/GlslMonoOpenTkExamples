using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class ShadedSphere : Shape
	{
		float radius;
		int TEXTURE_DATA_SIZE_IN_ELEMENTS = 2;
		float[] textureCoordinates;
		float[] vertexDataWithTextureCoordinates;
		static int g_colorTexUnit = 0;
		
		public ShadedSphere(float radius_in)
		{
			radius = radius_in;
        	vertexCoords = GetCircleCoords(1f);
        	vertexCount = vertexCoords.Length / COORDS_PER_VERTEX / 2;
		
			vertexData = vertexCoords;
			SetupSimpleIndexBuffer();
			CalculateTextureCoordinates();
			ScaleCoordinates(radius_in);
			AddTextureCoordinates();
			
        	InitializeVertexBuffer();
			
			programNumber = Programs.AddProgram(VertexShaders.MatrixTexture, 
              	FragmentShaders.MatrixTexture);
			Programs.SetUniformTexture(programNumber, g_colorTexUnit);
			Programs.LoadTexture(programNumber, "Venus_Magellan_C3-MDIR_ClrTopo_Global_Mosaic_1024.jpg", true);
			
		}
		
		private void ScaleCoordinates(float scale)
		{
			for (int i = 0; i < vertexCount; i++)
			{
				vertexData[6 * i + 0] = vertexData[6 * i + 0] * scale;
				vertexData[6 * i + 1] = vertexData[6 * i + 1] * scale;
				vertexData[6 * i + 2] = vertexData[6 * i + 2] * scale;
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
		
		private void CalculateTextureCoordinates()
		{
			textureCoordinates = new float[vertexCount * TEXTURE_DATA_SIZE_IN_ELEMENTS];
			for (int vertex = 0; vertex < vertexCount; vertex++)
			{
				float x = vertexData[vertex * 6];
				float y = vertexData[vertex * 6 + 1];
				float z = vertexData[vertex * 6 + 2];
				float longitude = (float)Math.Atan2(y, x);
				float lattitude = (float)Math.Asin(z);	
				//lattitude = lattitude + (float)Math.PI/4;
				//longitude = longitude + 3 * (float)Math.PI/4;
				
				textureCoordinates[vertex * 2] = (float)((longitude + Math.PI) / (Math.PI * 2));
				textureCoordinates[vertex * 2 + 1] = (float)((lattitude + Math.PI/2) / Math.PI);
				if (textureCoordinates[vertex * 2] < 0) textureCoordinates[vertex * 2] = 0f;
				if (textureCoordinates[vertex * 2] > 1) textureCoordinates[vertex * 2] = 1f;
				if (textureCoordinates[vertex * 2 + 1] < 0) textureCoordinates[vertex * 2] = 0f;
				if (textureCoordinates[vertex * 2 + 1] > 1) textureCoordinates[vertex * 2] = 1f;
			}
			// center all x coordinates in original 100%
			for (int vertex = 0; vertex < vertexCount; vertex++)
			{
				textureCoordinates[vertex * 2] = 1f/12f + 10f/12f * textureCoordinates[vertex * 2];
			}
			// Check each set of 3 coordinates for crossing edges.  Move some if necessary
			for (int vertex = 0; vertex < vertexCount; vertex = vertex + 3)
			{
				if (textureCoordinates[vertex * 2] < 0.35f)
				{
					if (textureCoordinates[(vertex + 1) * 2] > 0.65f)
					{
						textureCoordinates[(vertex + 1) * 2] = textureCoordinates[(vertex + 1) * 2] - 10f/12f;
					}
					if (textureCoordinates[(vertex + 2) * 2] > 0.65f)
					{
						textureCoordinates[(vertex + 2) * 2] = textureCoordinates[(vertex + 2) * 2] - 10f/12f;
					}
				}
				if (textureCoordinates[vertex * 2] > 0.65f)
				{
					if (textureCoordinates[(vertex + 1) * 2] < 0.35f)
					{
						textureCoordinates[(vertex + 1) * 2] = textureCoordinates[(vertex + 1) * 2] + 10f/12f;
					}
					if (textureCoordinates[(vertex + 2) * 2] < 0.35f)
					{
						textureCoordinates[(vertex + 2) * 2] = textureCoordinates[(vertex + 2) * 2] + 10f/12f;
					}				
				}
			}			
		}
		
		private void AddTextureCoordinates()
		{
			vertexDataWithTextureCoordinates = new float[vertexData.Length + textureCoordinates.Length];
			for (int i = 0; i < vertexCount; i++)
			{
				Array.Copy(vertexData, 6 * i, vertexDataWithTextureCoordinates, 8 * i, 6);
				Array.Copy(textureCoordinates, 2 * i, vertexDataWithTextureCoordinates, 8 * i + 6, 
			           2);
			}
			vertexData = vertexDataWithTextureCoordinates;
		}
		
	    private void DrawSub(int first_triangle, int last_triangle)
	    {
	        int newVertexCount = (last_triangle - first_triangle + 1) * 3 * 3 / COORDS_PER_VERTEX;
	        // Add program to OpenGL environment

		 	Matrix4 mm = Rotate(modelToWorld, axis, angle);
			mm.M41 = offset.X;
			mm.M42 = offset.Y;
			mm.M43 = offset.Z;	
			
			Programs.Draw(programNumber, vertexBufferObject, indexBufferObject, cameraToClip, worldToCamera, mm,
			              indexData.Length, color);
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

