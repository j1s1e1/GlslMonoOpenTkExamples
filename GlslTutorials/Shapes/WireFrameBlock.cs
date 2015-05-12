using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace GlslTutorials
{
	public class WireFrameBlock : Shape
	{
		//		  2-------3
		//		 /|		 /|
		//		0-|-----1 |
		//		| 6-----|-7
		//		|/ 		|/ 
		//	    4-------5
		//

		static short[] lmbIndexData = new short[] {	0, 1, 2, 1, 3, 2,
			4, 6, 5, 5, 6, 7,
			3, 1, 5, 3, 5, 7,
			4, 0, 6, 6, 0, 2,
			0, 4, 1, 1, 4, 5,
			6, 2, 3, 6, 3, 7,};
		
		static float RIGHT_EXTENT = 0.5f;
	    static float LEFT_EXTENT = -RIGHT_EXTENT;
	    static float TOP_EXTENT = 0.5f;
	    static float BOTTOM_EXTENT = -TOP_EXTENT;
	    static float FRONT_EXTENT = 0.5f;
	    static float REAR_EXTENT = -FRONT_EXTENT;
		
		static float[] lmbVertexData = new float[] { 	LEFT_EXTENT, TOP_EXTENT, FRONT_EXTENT,
														RIGHT_EXTENT, TOP_EXTENT, FRONT_EXTENT,
														LEFT_EXTENT, TOP_EXTENT, REAR_EXTENT,
														RIGHT_EXTENT, TOP_EXTENT, REAR_EXTENT,
														LEFT_EXTENT, BOTTOM_EXTENT, FRONT_EXTENT,
														RIGHT_EXTENT, BOTTOM_EXTENT, FRONT_EXTENT,
														LEFT_EXTENT, BOTTOM_EXTENT, REAR_EXTENT,
														RIGHT_EXTENT, BOTTOM_EXTENT, REAR_EXTENT,
													};

		public WireFrameBlock (Vector3 sizeIn, float[] colorIn)
		{
			Scale(sizeIn);
			color = colorIn;
			programNumber = Programs.AddProgram(VertexShader, FragmentShader);

			vertexCount = 2 * 3 * 6;
			vertexStride = 3 * 4; // no color for now
			// fill in index data
			indexData = lmbIndexData;
			
			// fill in vertex data
			vertexData = lmbVertexData;
			
			InitializeVertexBuffer();
		}
		
		public override void Draw()
		{
	        Matrix4 mm = Rotate(modelToWorld, axis, angle);

			Programs.DrawWireFrame(programNumber, vertexBufferObject, indexBufferObject, mm, indexData.Length, color);
		}
	}
}

