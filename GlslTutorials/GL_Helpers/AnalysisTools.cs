using System;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class AnalysisTools
	{
		public AnalysisTools ()
		{
		}
		
		private static string CheckRotation(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 objectCenter)
		{
			Vector3 triangleCenter = new Vector3((vertex1.X + vertex2.X + vertex3.X)/3,
			                                     (vertex1.Y + vertex2.Y + vertex3.Y)/3,
			                                     (vertex1.Z + vertex2.Z + vertex3.Z)/3);
			Vector3 Vertex2minusVertex1 = vertex2 - vertex1;
			Vector3 Vertex3minusVertex2 = vertex3 - vertex2;
			Vector3 outVector = Vector3.Cross(Vertex2minusVertex1, Vertex3minusVertex2);
			
			Vector3 triangleCenterToCenter = triangleCenter - objectCenter;
			Vector3 triangelCenterPlusOutVector = triangleCenter + outVector;
			Vector3 triangelCenterPlusOutVectorToCenter = triangelCenterPlusOutVector - objectCenter;
			
			float distanceA = triangleCenterToCenter.Length;
			float distanceB = (triangleCenterToCenter + triangelCenterPlusOutVectorToCenter).Length;
			                                     
			if (distanceA > distanceB)
			{
				return "CCW";
			}
			else
			{
				return "CW";
			}
		}
		
		public static string CheckRotations(float[] vertexes, short[] indexes,  Vector3 center)
		{
			StringBuilder result = new StringBuilder();
			int triangleCount = 0;
			for (int i = 0; i < indexes.Length; i = i + 3)
			{
				int a = 3 * indexes[i];
				int b = 3 * indexes[i+1];
				int c = 3 * indexes[i+2];
				result.AppendLine(CheckRotation(
					new Vector3(vertexes[a], vertexes[a+1], vertexes[a+2]),
					new Vector3(vertexes[b], vertexes[b+1], vertexes[b+2]),
					new Vector3(vertexes[c], vertexes[c+1], vertexes[c+2]),
					center
					));
			triangleCount++;
			}
			result.AppendLine("Triangle Count = " + triangleCount.ToString());
			return result.ToString();
		}
		
		public static string CheckExtents(float[] vertexes)
		{
			StringBuilder result = new StringBuilder();
			float minX = vertexes[0];
			float maxX = minX;
			float minY = vertexes[1];
			float maxY = minY;
			float minZ = vertexes[2];
			float maxZ = minZ;
			for (int i = 3; i < vertexes.Length; i = i + 3)
			{
				if (vertexes[i] < minX) minX = vertexes[i];
				if (vertexes[i] > maxX) maxX = vertexes[i];
				if (vertexes[i+1] < minY) minY = vertexes[i+1];
				if (vertexes[i+1] > maxY) maxY = vertexes[i+1];
				if (vertexes[i+2] < minZ) minZ = vertexes[i+2];
				if (vertexes[i+2] > maxZ) maxZ = vertexes[i+2];
				
			}
			result.AppendLine("minX = " + minX.ToString());
			result.AppendLine("maxX = " + maxX.ToString());
			result.AppendLine("minY = " + minY.ToString());
			result.AppendLine("maxY = " + maxY.ToString());
			result.AppendLine("minZ = " + minZ.ToString());
			result.AppendLine("maxZ = " + maxZ.ToString());
			return result.ToString();
		}
	}
}

