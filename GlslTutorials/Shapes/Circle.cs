using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Circle
	{
		public Circle ()
		{
		}

		public static List<Vector3> GetCircleCoords(float radius, int vertexCount, float z)
		{
			List<Vector3> circleCoords = new List<Vector3>();
			float angleStep = (float)(Math.PI * 2 / vertexCount);
			for (int i = 0; i < vertexCount; i++)
			{
				circleCoords.Add(new Vector3((float)Math.Cos(-angleStep * i) * radius, 
					(float)Math.Sin(angleStep * i) * radius, z));
				if (i == 0)
				{
					circleCoords.Add(new Vector3((float)Math.Cos(-angleStep * (vertexCount - 1)) * radius, 
						(float)Math.Sin(angleStep * (vertexCount - 1)) * radius, z));
				}
				else
				{
					circleCoords.Add(new Vector3((float)Math.Cos(-angleStep * (i - 1)) * radius, 
						(float)Math.Sin(angleStep * (i - 1)) * radius, z));
				}
				circleCoords.Add(new Vector3(0f, 0f, z));
			}
			return circleCoords;
		}
	}
}

