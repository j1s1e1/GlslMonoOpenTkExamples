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
		
		string VertexShader = VertexShaders.PosOnlyWorldTransform_vert;
		string FragmentShader = FragmentShaders.ColorUniform_frag;
		int progarmNumber;
		
		public BlenderObject (string nameIn)
		{
			Name = nameIn;
			vertexes = new List<float>();
			indexes = new List<short>();
		}
		
		// v -1.458010 -3.046922 2.461986
		public void AddVertex(string vertexInfo)
		{
			List<float> newVertexes = vertexInfo.Substring(2).Split(' ').Select(s => Convert.ToSingle(s)).ToList();
			vertexes.AddRange(newVertexes);
		}
		
		public void AddTriangle(string triangleInfo, short offset)
		{
			if (triangleInfo.Contains("/"))
			{
				int debug = 0;
				List<short> newIndexes = new List<short>();
				string[] selections = triangleInfo.Substring(2).Split(' ');
				for (int i = 0; i < selections.Length; i++)
				{
					string[] selections_parts = selections[i].Split ('/');
					newIndexes.Add(Convert.ToInt16(selections_parts[0]));
					debug++;
				}
				indexes.AddRange(newIndexes);
			}
			else
			{
				List<short> newIndexes = 
					triangleInfo.Substring(2).Split(' ').Select(s => (short)(Convert.ToInt16(s) - offset)).ToList();
				indexes.AddRange(newIndexes);
			}
		}
		
		public void Setup()
		{
			progarmNumber = Programs.AddProgram(VertexShader, FragmentShader);
			
			vertexCount = indexes.Count;
			vertexStride = 3 * 4; // no color for now
			// fill in index data
			indexData = indexes.ToArray();
			
			// fill in vertex data
			vertexData = vertexes.ToArray();
			
			InitializeVertexBuffer();
		}
		
		public void Scale(Vector3 size)
		{
			modelToWorld.M11 = size.X;
			modelToWorld.M22 = size.Y;
			modelToWorld.M33 = size.Z;
		}
		
		public override void Draw()
		{
			Matrix4 mm = Rotate(modelToWorld, axis, angle);
			mm.M41 = offset.X;
			mm.M42 = offset.Y;
			mm.M43 = offset.Z;
			
			Programs.Draw(progarmNumber, vertexBufferObject, indexBufferObject, cameraToClip, worldToCamera, mm,
			              indexData.Length, color, COORDS_PER_VERTEX, vertexStride);
		}
	}
}

