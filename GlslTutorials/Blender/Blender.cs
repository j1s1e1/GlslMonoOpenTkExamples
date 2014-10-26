using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Blender
	{
		public Blender ()
		{
		}
		
		List<BlenderObject> blenderObjects;
		
		public void ReadFile(string filename)
		{
			string nextLine;			
			blenderObjects = new List<BlenderObject>();
			using (StreamReader sr = new StreamReader(new FileStream(filename, FileMode.Open)))
			{
				short vertexCount = 1;
				short normalCount = 1;
				short previousObjectVertexCount = 1;  // change from 1 to zero based
				short previousObjectNormalCount = 1;  // change from 1 to zero based
				nextLine = sr.ReadLine();
				while (!sr.EndOfStream)
				{
					if (nextLine[0] == 'o')
					{
						BlenderObject bo = new BlenderObject(nextLine);
						while (!sr.EndOfStream)
						{
							nextLine = sr.ReadLine();
							if (nextLine[0] == 'o') break;
							if (nextLine[0] == 'v')
							{
								if (nextLine[1] == ' ')
								{
									bo.AddVertex(nextLine);
									vertexCount++;
								}
								if (nextLine[1] == 'n')
								{
									bo.AddNormal(nextLine);
									normalCount++;
								}
							}
							if (nextLine[0] == 'f')
							{
								bo.AddTriangle(nextLine, previousObjectVertexCount, previousObjectNormalCount);
							}
						}
						previousObjectVertexCount = vertexCount;
						previousObjectNormalCount = normalCount;
						bo.Setup();
						blenderObjects.Add(bo);
					}
					else
					{
						if (!sr.EndOfStream) nextLine = sr.ReadLine();
					}
				}
			}
		}
		
		public int ObjectCount()
		{
			return blenderObjects.Count;
		}
		
		public void Draw()
		{
			foreach (BlenderObject bo in blenderObjects)
			{
				bo.Draw();
			}
		}
		
		public void Scale(Vector3 scale)
		{
			foreach (BlenderObject bo in blenderObjects)
			{
				bo.Scale(scale);
			}
		}
		
		public void SetOffset(Vector3 offset)
		{
			foreach (BlenderObject bo in blenderObjects)
			{
				bo.SetOffset(offset);
			}
		}
		
		public void SetColor(float[] color)
		{
			foreach (BlenderObject bo in blenderObjects)
			{
				bo.SetColor(color);
			}
		}
		
		public void RotateShapes(Vector3 rotationAxis, float angle)
		{
			foreach (BlenderObject bo in blenderObjects)
			{
				bo.RotateShape(rotationAxis, angle);
			}
		}
		
	 	public void SetProgram(int program)
	    {
	        foreach (BlenderObject bo in blenderObjects)
	        {
	            // Need to add normals before using other programs
	            bo.SetProgram(program);
	        }
	    }
	}
}

