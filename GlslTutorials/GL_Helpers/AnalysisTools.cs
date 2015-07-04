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
		
		
		public static string CalculateMatrixEffects(Matrix4 matrix)
		{
			// Check This
			StringBuilder result = new StringBuilder();
			result.AppendLine(matrix.ToString());
			result.AppendLine("");
			result.AppendLine("Translation by " + matrix.M41 + " " + matrix.M42 + " " + matrix.M43);
			Matrix3 normalizedMatrix = new Matrix3(matrix);
			normalizedMatrix.Normalize();
			float heading = 0f;
			float attitude = 0f;
			float bank = 0f;
			
			if (normalizedMatrix.M21 == 1)
			{
				result.AppendLine("North Pole");
				heading = (float)Math.Atan2(normalizedMatrix.M13,normalizedMatrix.M33);
				bank = 0f;
			}
			else
			{
				if (normalizedMatrix.M21 == -1)
				{
					result.AppendLine("South Pole");
					heading = (float)Math.Atan2(normalizedMatrix.M13,normalizedMatrix.M33);
					bank = 0;
				}
				else
				{
					heading = (float)Math.Atan2(-normalizedMatrix.M31, normalizedMatrix.M11);
					attitude = (float)Math.Asin(normalizedMatrix.M21);
					bank = (float)Math.Atan2(-normalizedMatrix.M23, normalizedMatrix.M22);
				}
			}
			heading = heading * 180f / (float)Math.PI;
			attitude = attitude * 180f / (float)Math.PI;
			bank = bank * 180f / (float)Math.PI;
			result.AppendLine("heading = " + heading.ToString());
			result.AppendLine("attitude = " + attitude.ToString());
			result.AppendLine("bank = " + bank.ToString());

			return result.ToString();
		}
		
		public static string TestRotations()
		{
			StringBuilder result = new StringBuilder();
			Matrix4 X45 = Matrix4.CreateRotationX(45f * (float)Math.PI / 180f);
			result.AppendLine("X45");
			result.Append(CalculateMatrixEffects(X45));
			Matrix4 Y45 = Matrix4.CreateRotationY(45f * (float)Math.PI / 180f);
			result.AppendLine("Y45");
			result.Append(CalculateMatrixEffects(Y45));
			Matrix4 Z45 = Matrix4.CreateRotationZ(45f * (float)Math.PI / 180f);
			result.AppendLine("Z45");
			result.Append(CalculateMatrixEffects(Z45));
			return result.ToString();
		}

		public static bool CompareMatrix4ToFloat(Matrix4 matrix, float[] floats)
		{
			Matrix4 floatToMatrix = new Matrix4(floats[0], floats[1], floats[2], floats[3],
												floats[4], floats[5], floats[6], floats[7],
												floats[8], floats[9], floats[10], floats[11],
												floats[12], floats[13], floats[14], floats[15]);
			Matrix4 result = matrix - floatToMatrix;
			if (result == Matrix4.Zero)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static Matrix4 CreateFromFloats(float[] floats)
		{
			return  new Matrix4(floats[0], floats[1], floats[2], floats[3],
				floats[4], floats[5], floats[6], floats[7],
				floats[8], floats[9], floats[10], floats[11],
				floats[12], floats[13], floats[14], floats[15]);
		}

		public static float[] CreateFromMatrix(Matrix4 m)
		{
			return  new float[]{m.M11, m.M12, m.M13, m.M14,
								m.M21, m.M22, m.M23, m.M24,
								m.M31, m.M32, m.M33, m.M34,
								m.M41, m.M42, m.M43, m.M44};
		}
	}
}

