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
				short vertexcount = 1;
				short previousObjectVertexCount = 1;  // change from 1 to zero based
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
								bo.AddVertex(nextLine);
								vertexcount++;
							}
							if (nextLine[0] == 'f') bo.AddTriangle(nextLine, previousObjectVertexCount);
						}
						previousObjectVertexCount = vertexcount;
						bo.Setup();
						bo.Scale(new Vector3(0.1f, 0.1f, 0.1f));
						bo.SetOffset(new Vector3(0.0f, 0.0f, 0.0f));
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
	}
}

