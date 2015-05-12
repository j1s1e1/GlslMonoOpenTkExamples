using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Cylinder : Shape
	{
		List<Vector3> vertices = new List<Vector3>();
		public Cylinder ()
		{
			vertices.AddRange(Circle.GetCircleCoords(0.05f, 12, 0.05f));
			vertices.AddRange(GetPrism(0.05f, 12, 0.05f, -0.05f));
			vertices.AddRange(Circle.GetCircleCoords(0.05f, 12, -0.05f));
			vertexData = new float[vertices.Count * 3];
			// ADD NORMALS?
			for (int i = 0; i < vertexData.Length; i++)
			{
				switch (i % 3)
				{
					case 0: vertexData[i] = vertices[i/3].X; break;
					case 1: vertexData[i] = vertices[i/3].Y; break;
					case 2: vertexData[i] = vertices[i/3].Z; break;
				}
			}
			SetupSimpleIndexBuffer();

			InitializeVertexBuffer();
			programNumber = Programs.AddProgram(VertexShaders.PosOnlyWorldTransform_vert, 
				FragmentShaders.ColorUniform_frag);
		}

		private List<Vector3> GetPrism(float radius, int sliceCount, float zTop, float zBottom)
		{
			float xCurrent;
			float xPrevious;
			float yCurrent;
			float yPrevious;
			List<Vector3> v = new List<Vector3>();
			float angleStep = (float)(Math.PI * 2 / sliceCount);
			for (int i = 0; i < sliceCount; i++)
			{
				xCurrent = (float)(radius * Math.Cos(angleStep * i));
				yCurrent = (float)(radius * Math.Sin(angleStep * i));
				if (i == 0)
				{
					xPrevious = (float)(radius * Math.Cos(angleStep * (sliceCount - 1)));
					yPrevious = (float)(radius * Math.Sin(angleStep * (sliceCount - 1)));
				}
				else
				{
					xPrevious = (float)(radius * Math.Cos(angleStep * (i - 1)));
					yPrevious = (float)(radius * Math.Sin(angleStep * (i - 1)));
				}
				v.Add(new Vector3(xCurrent, yCurrent, zTop));
				v.Add(new Vector3(xCurrent, yCurrent, zBottom));
				v.Add(new Vector3(xPrevious, yPrevious, zTop));
				v.Add(new Vector3(xPrevious, yPrevious, zTop));
				v.Add(new Vector3(xCurrent, yCurrent, zBottom));
				v.Add(new Vector3(xPrevious, yPrevious, zBottom));
			}
			return v;
		}

		public void CheckCollisions(Vector3 center, float radius)
		{
			float distance = (GetOffset() - center).Length;
			if (distance < Math.Abs(radius))
			{
				color = Colors.RED_COLOR;
			}
		}

		public override void Draw()
		{
			Programs.Draw(programNumber, vertexBufferObject, indexBufferObject, modelToWorld,
				indexData.Length, color);
		}
	}
}

