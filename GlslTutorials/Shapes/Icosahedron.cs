using System;
using System.Collections.Generic;
using OpenTK;

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
		
		public static List<Vector3> vertexList;
		public static List<Triangle> triangleList;

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
			CreateVertexList();
			CreateTriangleList();
	    }
		
		public static float[] CloneTriangles()
		{
			float[] result = new float[triangles.Length];
			Array.Copy(triangles, 0, result, 0, result.Length);
			return result;
		}
		
		public static void CreateVertexList()
		{
			vertexList = new List<Vector3>();
			for (int i = 0; i < triangles.Length; i = i + 3)
			{
				vertexList.Add(new Vector3(triangles[i], triangles[i+1], triangles[i+2]));
			}
		}
		
		public static void CreateTriangleList()
		{
			triangleList = new List<Triangle>();
			for (int i = 0; i < vertexList.Count; i = i + 3)
			{
				triangleList.Add(new Triangle(vertexList[i], vertexList[i+1], vertexList[i+2]));
			}
		}
		
		public static List<Triangle> DivideTriangles(List<Triangle> triangles)
		{
			List<Triangle> result = new List<Triangle>();
			for (int i = 0; i < triangles.Count; i++)
			{
				result.AddRange(triangles[i].Divide());
			}
			return result;
		}
		
		public static List<Vector3> GetVertices(List<Triangle> triangles)
		{
			List<Vector3> result = new List<Vector3>();
			for (int i = 0; i < triangles.Count; i++)
			{
				result.AddRange(triangles[i].GetVertices());
			}
			return result;
		}
		
		public static List<Vector3> NormalizeVertices(List<Vector3> vertices)
		{
			List<Vector3> result = new List<Vector3>();
			foreach (Vector3 v in vertices)
			{
				result.Add(v.Normalized());
			}
			return result;
		}
		
		public static float[] GetFloats(List<Vector3> vertices)
		{
			float[] result = new float[vertices.Count * 3];
			for (int i = 0; i < vertices.Count; i++)
			{
				Array.Copy (vertices[i].ToFloat(), 0, result, i * 3, 3);
			}
			return result;
		}
		
		public static float[] GetFloats(List<Triangle> triangles)
		{
			float[] result = new float[triangles.Count * 9];
			for (int i = 0; i < triangles.Count; i++)
			{
				Array.Copy (triangles[i].GetFloats(), 0, result, i * 9, 9);
			}
			return result;
		}
		
		public static float[] GetDividedTriangles(int divideCount)
		{
			List<Triangle> result = new List<Triangle>();
			foreach (Triangle t in triangleList)
			{
				result.Add(t.Clone());
			}
			while (divideCount-- > 0)
			{
				result = DivideTriangles(result);
			}			
			List<Vector3> vertices = GetVertices(result);
			vertices = NormalizeVertices(vertices);
			return GetFloats(vertices);
		}
	}
}

