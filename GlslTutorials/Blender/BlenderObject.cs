using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class BlenderObject : Shape
	{
		public string Name;
		List<float> vertexes;
		List<short> indexes;
		List<float> normals;
		List<string> vertexNormalIndexes;
		List<float> vertexNormals;
		
		public BlenderObject (string nameIn)
		{
			Name = nameIn;
			vertexes = new List<float>();
			indexes = new List<short>();
			normals = new List<float>();
			vertexNormalIndexes = new List<string>();
			vertexNormals = new List<float>();
		}
		
		// v -1.458010 -3.046922 2.461986
		public void AddVertex(string vertexInfo)
		{
			List<float> newVertexes = vertexInfo.Substring(2).Split(' ').Select(s => Convert.ToSingle(s)).ToList();
			vertexes.AddRange(newVertexes);
		}
		
		public void AddNormal(string normalInfo)
		{
			List<float> newNormal = normalInfo.Substring(3).Split(' ').Select(s => Convert.ToSingle(s)).ToList();
			normals.AddRange(newNormal);
		}
		
		public void AddTriangle(string triangleInfo, short vertexOffset, short normalOffset)
		{
			if (triangleInfo.Contains("/"))
			{
				List<short> newIndexes = new List<short>();
				string[] selections = triangleInfo.Substring(2).Split(' ');
				for (int i = 0; i < selections.Length; i++)
				{
					if (vertexNormalIndexes.Contains(selections[i]))
					{
						newIndexes.Add((short)vertexNormalIndexes.IndexOf(selections[i]));
					}
					else
					{
						vertexNormalIndexes.Add(selections[i]);
						string[] selections_parts = selections[i].Split ('/');
						List<float> newVertexNormal = new List<float>();
						int vertexIndex = Convert.ToInt32(selections_parts[0]) - vertexOffset;
						int normalIndex = Convert.ToInt32(selections_parts[2]) - normalOffset;
						newVertexNormal.AddRange (vertexes.GetRange(vertexIndex * 3, 3));
						newVertexNormal.AddRange (normals.GetRange(normalIndex * 3, 3));
						vertexNormals.AddRange(newVertexNormal);
						newIndexes.Add((short)vertexNormalIndexes.IndexOf(selections[i]));
					}
				}
				indexes.AddRange(newIndexes);
			}
			else
			{
				List<short> newIndexes = 
					triangleInfo.Substring(2).Split(' ').Select(s => (short)(Convert.ToInt16(s) - vertexOffset)).ToList();
				indexes.AddRange(newIndexes);
			}
		}
		
		public void Setup()
		{
			vertexCount = indexes.Count;
			// fill in index data
			indexData = indexes.ToArray();
			if (vertexNormals.Count > 0)
			{
				VertexShader = VertexShaders.lms_vertexShaderCode;
				FragmentShader = FragmentShaders.lms_fragmentShaderCode;
				vertexStride = (3 + 3) * 4; // position and normals
				vertexData = vertexNormals.ToArray();
			}
			else
			{
				vertexStride = 3 * 4; // position only
				// fill in vertex data
				vertexData = vertexes.ToArray();
			}
			InitializeVertexBuffer();
			programNumber = Programs.AddProgram(VertexShader, FragmentShader);
		}
		
		public override void Scale(Vector3 size)
		{
			modelToWorld.M11 = size.X;
			modelToWorld.M22 = size.Y;
			modelToWorld.M33 = size.Z;
		}
		
		public override void Draw()
		{
			Matrix4 mm = Rotate(modelToWorld, axis, angle);
			
			Programs.Draw(programNumber, vertexBufferObject[0], indexBufferObject[0], mm, indexData.Length, color);
		}
		
		public List<byte> GetBinaryBlenderObject()
		{
			List<byte> binaryBlenderObjectBytes = new List<byte>();
			int vertexBytes = vertexes.Count * 4;
			int indexBytes = indexes.Count * 2;
			int normalBytes = normals.Count * 4;
			int vertexNormalBytes = vertexNormals.Count * 4;
			
			binaryBlenderObjectBytes.AddRange(BitConverter.GetBytes(vertexBytes));
			binaryBlenderObjectBytes.AddRange(BitConverter.GetBytes(indexBytes));
			binaryBlenderObjectBytes.AddRange(BitConverter.GetBytes(normalBytes));
			binaryBlenderObjectBytes.AddRange(BitConverter.GetBytes(vertexNormalBytes));
			
			if (vertexBytes > 0) 
			{
				binaryBlenderObjectBytes.AddRange(vertexes.SelectMany(s => BitConverter.GetBytes(s)));
			}
			if (indexBytes > 0) 
			{
				binaryBlenderObjectBytes.AddRange(indexes.SelectMany(s => BitConverter.GetBytes(s)));
			}
			if (normalBytes > 0) 
			{
				binaryBlenderObjectBytes.AddRange(normals.SelectMany(s => BitConverter.GetBytes(s)));
			}
			if (vertexNormalBytes > 0) 
			{
				binaryBlenderObjectBytes.AddRange(vertexNormals.SelectMany(s => BitConverter.GetBytes(s)));
			}
			return binaryBlenderObjectBytes;
		}
		
		public int CreateFromBinaryData(byte[] binaryBlenderObjects, int offset)
		{
			int blenderHeaderBytes = 16;
			int vertexBytes = BitConverter.ToInt32 (binaryBlenderObjects, offset);
			offset = offset + 4;
			int indexBytes = BitConverter.ToInt32 (binaryBlenderObjects, offset);
			offset = offset + 4;
			int normalBytes = BitConverter.ToInt32 (binaryBlenderObjects, offset);
			offset = offset + 4;
			int vertexNormalBytes = BitConverter.ToInt32 (binaryBlenderObjects, offset);
			offset = offset + 4;
			if (vertexBytes > 0)
			{
				for (int i = 0; i < vertexBytes; i = i + 4)
				{
					vertexes.Add(BitConverter.ToSingle(binaryBlenderObjects, offset));
					offset = offset + 4;           
				}
			}
			if (indexBytes > 0)
			{
				for (int i = 0; i < indexBytes; i = i + 2)
				{
					indexes.Add(BitConverter.ToInt16(binaryBlenderObjects, offset));
					offset = offset + 2;           
				}
			}
			if (normalBytes > 0)
			{
				for (int i = 0; i < normalBytes; i = i + 4)
				{
					normals.Add(BitConverter.ToSingle(binaryBlenderObjects, offset));
					offset = offset + 4;           
				}
			}
			if (vertexNormalBytes > 0)
			{
				for (int i = 0; i < vertexNormalBytes; i = i + 4)
				{
					vertexNormals.Add(BitConverter.ToSingle(binaryBlenderObjects, offset));
					offset = offset + 4;           
				}
			}
			return blenderHeaderBytes + vertexBytes + indexBytes + normalBytes + vertexNormalBytes;
		}
	}
}

