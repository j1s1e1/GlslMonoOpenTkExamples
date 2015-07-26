﻿using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Octahedron : Shape
	{
		//				  0
		//	
		//		    4------------3
		//		   /            /
		//		  /			   /
		//		 / 	 		  /
		//	    1------------2
		//
		//
		//			  5

		Vector3 size;

		static short[] lmbIndexData = new short[] 
		{	0, 1, 2,
			0, 2, 3,
			0, 3, 4,
			0, 4, 1,
			1, 5, 2,
			2, 5, 3,
			3, 5, 4,
			4, 5, 1};

		static float RIGHT_EXTENT = 0.5f;
		static float LEFT_EXTENT = -RIGHT_EXTENT;
		static float TOP_EXTENT = 0.5f;
		static float BOTTOM_EXTENT = -TOP_EXTENT;
		static float FRONT_EXTENT = 0.5f;
		static float REAR_EXTENT = -FRONT_EXTENT;

		static float[] lmbVertexData = new float[] 
		{ 	0f, TOP_EXTENT, 0f,
			LEFT_EXTENT, 0f, FRONT_EXTENT,
			RIGHT_EXTENT, 0f, FRONT_EXTENT,
			RIGHT_EXTENT, 0f, REAR_EXTENT,
			LEFT_EXTENT, 0f, REAR_EXTENT,
			0f, BOTTOM_EXTENT, 0f,
		};

		public Octahedron (Vector3 sizeIn, float[] colorIn)
		{
			size = sizeIn;
			color = colorIn;
			modelToWorld.M11 = size.X;
			modelToWorld.M22 = size.Y;
			modelToWorld.M33 = size.Z;

			programNumber = Programs.AddProgram(VertexShader, FragmentShader);

			vertexCount = 3 * 8;
			vertexStride = 3 * 4; // no color for now
			// fill in index data
			indexData = lmbIndexData;

			// fill in vertex data
			vertexData = lmbVertexData; //  GetVertexData();

			InitializeVertexBuffer();
		}

		~Octahedron()
		{
			GL.DeleteBuffers(1, ref vertexBufferObject[0]);
			GL.DeleteBuffers(1, ref indexBufferObject[0]);
		}

		private float[] GetVertexData()
		{
			float[] vertexData = new float[lmbVertexData.Length];
			for (int i = 0; i < vertexData.Length; i = i + 3)
			{
				vertexData[i] = lmbVertexData[i] * size.X;
				vertexData[i+1] = lmbVertexData[i+1] * size.Y;
				vertexData[i+2] = lmbVertexData[i+2] * size.Z;
			}
			return vertexData;
		}

		public override void Draw()
		{
			Programs.Draw(programNumber, vertexBufferObject, indexBufferObject, modelToWorld, indexData.Length, color);
		}
	}
}

