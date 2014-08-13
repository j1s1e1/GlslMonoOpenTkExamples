using System;

namespace GlslTutorials
{
	public class Icosahedron
	{
 		static Icosahedron()
		{
			Create();
		}
   	 	static float[][] vertices;
    
    	public static float[] triangles;

	    static float[] CalculateVertex(float theta, float phi)
	    {
	        float[] result = new float[3];
	
	        result[0] = (float)(Math.Cos(theta) * Math.Cos(phi));
	        result[1] = (float)(Math.Sin(theta) * Math.Cos(phi));
	        result[2] = (float)(Math.Sin(phi));
	        return result;
	    }
	
	    static void CreateVertices()
	    {
	            /*
	        The locations of the vertices of a regular icosahedron can be described using spherical coordinates,
	         for instance as latitude and longitude. If two vertices are taken to be at the north and south poles
	          (latitude ±90°), then the other ten vertices are at latitude ±arctan(1/2) ≈ ±26.57°. These ten vertices
	           are at evenly spaced longitudes (36° apart), alternating between north and south latitudes.
	         */
	        float latitude_angle = (float)Math.Atan(0.5);
	        float longitude_angle = (float)(Math.PI / 5.0);
	        vertices = new float[12][];
	        vertices[0] = CalculateVertex(0, (float)(Math.PI/2.0f));
	        vertices[1] = CalculateVertex(0, (float)(-Math.PI/2.0f));
	        vertices[2] = CalculateVertex(longitude_angle * 0, latitude_angle);
	        vertices[3] = CalculateVertex(longitude_angle * 2, latitude_angle);
	        vertices[4] = CalculateVertex(longitude_angle * 4, latitude_angle);
	        vertices[5] = CalculateVertex(longitude_angle * 6, latitude_angle);
	        vertices[6] = CalculateVertex(longitude_angle * 8, latitude_angle);
	        vertices[7] = CalculateVertex(longitude_angle * 1, -latitude_angle);
	        vertices[8] = CalculateVertex(longitude_angle * 3, -latitude_angle);
	        vertices[9] = CalculateVertex(longitude_angle * 5, -latitude_angle);
	        vertices[10] = CalculateVertex(longitude_angle * 7, -latitude_angle);
	        vertices[11] = CalculateVertex(longitude_angle * 9, -latitude_angle);
	    }
	
	    static int triangle_count;
	
	    static void AddTriangle(float[] vertex1, float[] vertex2, float[] vertex3)
	    {
	        float[] vertex_temp = new float[3];
	        int point_count = triangle_count * 9;
	        for (int vertex = 0; vertex < 3; vertex++)
	        {
	            switch (vertex)
	            {
	                case 0: vertex_temp = vertex1; break;
	                case 1: vertex_temp = vertex2; break;
	                case 2: vertex_temp = vertex3; break;
	            }
	            for (int point = 0; point < 3; point++)
	            {
	                triangles[point_count++] = vertex_temp[point];
	            }
	        }
	        triangle_count++;
	    }
	
	    static void CreateTriangles()
	    {
	        triangle_count = 0;
	        triangles = new float[20 * 3 * 3];
	        AddTriangle(vertices[0], vertices[2], vertices[3]);
	        AddTriangle(vertices[0], vertices[3], vertices[4]);
	        AddTriangle(vertices[0], vertices[4], vertices[5]);
	        AddTriangle(vertices[0], vertices[5], vertices[6]);
	        AddTriangle(vertices[0], vertices[6], vertices[2]);
	
	        AddTriangle(vertices[2], vertices[7], vertices[3]);
	        AddTriangle(vertices[3], vertices[8], vertices[4]);
	        AddTriangle(vertices[4], vertices[9], vertices[5]);
	        AddTriangle(vertices[5], vertices[10], vertices[6]);
	        AddTriangle(vertices[6], vertices[11], vertices[2]);
	
	        AddTriangle(vertices[2], vertices[11], vertices[7]);
	        AddTriangle(vertices[3], vertices[7], vertices[8]);
	        AddTriangle(vertices[4], vertices[8], vertices[9]);
	        AddTriangle(vertices[5], vertices[9], vertices[10]);
	        AddTriangle(vertices[6], vertices[10], vertices[11]);
	
	        AddTriangle(vertices[7], vertices[1], vertices[8]);
	        AddTriangle(vertices[8], vertices[1], vertices[9]);
	        AddTriangle(vertices[9], vertices[1], vertices[10]);
	        AddTriangle(vertices[10], vertices[1], vertices[11]);
	        AddTriangle(vertices[11], vertices[1], vertices[7]);
	
	    }
	    static void Create()
	    {
	        CreateVertices();
	        CreateTriangles();
	    }
		
		public static float[] CloneTriangles()
		{
			float[] result = new float[triangles.Length];
			Array.Copy(triangles, 0, result, 0, result.Length);
			return result;
		}
	}
}

