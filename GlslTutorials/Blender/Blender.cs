using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Blender : Shape
	{
		public Blender ()
		{
		}
		
		List<BlenderObject> blenderObjects;
		
		Vector3 currentOffset = new Vector3();
		
		// These files can be created using binary blender objects
		public string ReadBinaryFile(string filename)
		{
			string BlenderFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/Blender/";
			blenderObjects = new List<BlenderObject>();
			int offset = 0;
			StringBuilder result = new StringBuilder();
			byte[] binaryBlenderObjects = File.ReadAllBytes(BlenderFilesDirectory + filename);
			int objectCount = BitConverter.ToInt32 (binaryBlenderObjects, 0);
			result.AppendLine("Found " + objectCount.ToString() + " Blender Objects");
			offset = offset + 4;
			for (int i = 0; i < objectCount; i++)
			{
				BlenderObject bo = new BlenderObject("Object" + i.ToString());
				int blenderObjectSize = bo.CreateFromBinaryData(binaryBlenderObjects, offset);
				offset = offset + blenderObjectSize;
				result.AppendLine("Object " + i.ToString() + " size = " + blenderObjectSize.ToString());
				bo.Setup();
				blenderObjects.Add(bo);
			}
			return result.ToString();
		}
		
		public void ReadFile(string filename)
		{
			string BlenderFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/Blender/";
			string nextLine;			
			blenderObjects = new List<BlenderObject>();
			using (StreamReader sr = new StreamReader(new FileStream(BlenderFilesDirectory + filename, FileMode.Open)))
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
		
		public override void Draw()
		{
			foreach (BlenderObject bo in blenderObjects)
			{
				bo.Draw();
			}
		}
		
		public override void Scale(Vector3 scale)
		{
			foreach (BlenderObject bo in blenderObjects)
			{
				bo.Scale(scale);
			}
		}
		
		public override void SetOffset(Vector3 offset)
		{
			currentOffset = offset;
			foreach (BlenderObject bo in blenderObjects)
			{
				bo.SetOffset(offset);
			}
		}
		
		public override Vector3 GetOffset()
		{
			return currentOffset;
		}
		
		public override void SetColor(float[] color)
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
		
		public override void SetProgram(int program)
	    {
	        foreach (BlenderObject bo in blenderObjects)
	        {
	            bo.SetProgram(program);
	        }
	    }
		
		public void SaveBinaryBlenderObjects(string fileName)
		{
			string BlenderFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/Blender/";
			List<byte> binaryBlenderObjects = new List<byte>();
			int objectCount = blenderObjects.Count;
			binaryBlenderObjects.AddRange(BitConverter.GetBytes(objectCount));
			foreach (BlenderObject bo in blenderObjects)
			{
				binaryBlenderObjects.AddRange(bo.GetBinaryBlenderObject());
			}
			File.WriteAllBytes(BlenderFilesDirectory + fileName, binaryBlenderObjects.ToArray());
		}
		
		public void Face(Vector3 direction)
		{
			Vector3 axis = new Vector3();
			float angle = 0;
			RotateShapes(direction, 10f);
		}
	}
}

