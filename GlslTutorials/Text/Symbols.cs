using System;
using OpenTK;

namespace GlslTutorials
{
	public class Symbols
	{
		public static float[] Dash;
		public static float[] Space;
		public static float[] Period;
		
		static Symbols()
		{
			AddDash();
			AddSpace();
			AddPeriod();
		}

		private static float[] MoveX(float[] input, float distance)
		{
			for (int i = 0; i < input.Length; i = i + 3)
			{
				input[i] = input[i] + distance;
			}
			return input;
		}

		private static float[] MoveY(float[] input, float distance)
		{
			for (int i = 1; i < input.Length; i = i + 3)
			{
				input[i] = input[i] + distance;
			}
			return input;
		}		
		
		private static void AddVertex(float[] symbol, int vertexNumber, Vector3 vertex)
		{
			symbol[vertexNumber * 3 + 0] = vertex.X;
			symbol[vertexNumber * 3 + 1] = vertex.Y;
			symbol[vertexNumber * 3 + 2] = vertex.Z;
		}
		
		public static float[] Rectangle(float width, float height)
	    {
			float X0 = -width/2;
			float X1 = width/2;
			float Y0 = height/2;
			float Y1 = -height/2;
			
			Vector3 V0 = new Vector3(X0, Y0, 0f);
			Vector3 V1 = new Vector3(X0, Y1, 0f);
			Vector3 V2 = new Vector3(X1, Y0, 0f);
			Vector3 V3 = new Vector3(X1, Y1, 0f);

	        float[] rectangle = new float [18];
			AddVertex(rectangle, 0, V0);
			AddVertex(rectangle, 1, V1);
			AddVertex(rectangle, 2, V2);
			AddVertex(rectangle, 3, V2);
			AddVertex(rectangle, 4, V1);
			AddVertex(rectangle, 5, V3);
	        return rectangle;
	    }
		
		private static void AddDash()
	    {
	        Dash = Rectangle(6f, 2f);
	    }
		
		private static void AddSpace()
		{
			Space = new float[9]; // Zero Size Triangle
		}
	
		private static void AddPeriod()
		{
			Period = Rectangle(2f, 2f);
			Period = MoveY(Period, -5f);
		}
	}
}

