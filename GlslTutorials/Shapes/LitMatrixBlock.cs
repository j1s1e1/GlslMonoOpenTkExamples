using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace GlslTutorials
{
	public class LitMatrixBlock : Shape
	{
		public static int mProgram = -1;
	    private static int positionAttribute;
	
	    private static int cameraToClipMatrixUnif;
	    private static int worldToCameraMatrixUnif;
	    private static int modelToWorldMatrixUnif;
	    private static int colorUnif;
		
		Vector3 size;
		
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
		
		static float[] lmbVertexData = new float[] { 	-0.5f, 0.5f, 0.5f,
													 	0.5f, 0.5f, 0.5f,
													 	-0.5f, 0.5f, -0.5f,
														0.5f, 0.5f, -0.5f,
														-0.5f, -0.5f, 0.5f,
														0.5f, -0.5f, 0.5f,
														-0.5f, -0.5f, -0.5f,
														0.5f, -0.5f, -0.5f,
													};
		
		public LitMatrixBlock (Vector3 sizeIn, float[] colorIn)
		{
			size = sizeIn;
			color = colorIn;
			cameraToClip.M11 = size.X;
			cameraToClip.M22 = size.Y;
			cameraToClip.M33 = size.Z;
			
			color = colorIn;
			if (mProgram < 0)
			{
				int vertexShader = Shader.compileShader(ShaderType.VertexShader, VertexShaders.PosOnlyWorldTransform_vert);
	            int fragmentShader = Shader.compileShader(ShaderType.FragmentShader, FragmentShaders.ColorUniform_frag);
	
	            mProgram = Shader.createAndLinkProgram(vertexShader, fragmentShader,
	                    new String[] {"a_Position", "a_Normal"});
	
	
	            // get handle to vertex shader's vPosition member
	            positionAttribute = GL.GetAttribLocation(mProgram, "position");
	
	            cameraToClipMatrixUnif = GL.GetUniformLocation(mProgram, "cameraToClipMatrix");
	            worldToCameraMatrixUnif = GL.GetUniformLocation(mProgram, "worldToCameraMatrix");
	            modelToWorldMatrixUnif = GL.GetUniformLocation(mProgram, "modelToWorldMatrix");
	            colorUnif = GL.GetUniformLocation(mProgram, "baseColor");
			}
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
	        // Add program to OpenGL environment
	        GL.UseProgram(mProgram);
	
	        Matrix4 mm = Rotate(modelToWorld, axis, angle);
			mm.M41 = offset.X;
			mm.M42 = offset.Y;
			mm.M43 = offset.Z;
	
	        GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref cameraToClip);
	        GL.UniformMatrix4(worldToCameraMatrixUnif, false, ref worldToCamera);
	        GL.UniformMatrix4(modelToWorldMatrixUnif, false, ref mm);
	        GL.Uniform4(colorUnif, 1, color);
	
	        // Enable a handle to the triangle vertices
	        
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);
			GL.EnableVertexAttribArray(positionAttribute);
	        // Prepare the triangle coordinate data
	        GL.VertexAttribPointer(positionAttribute, COORDS_PER_VERTEX, 
				VertexAttribPointerType.Float, false, vertexStride, (IntPtr)0);
			
	        // Draw the triangle
	 		GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
			
	        // Disable vertex array
	        GL.DisableVertexAttribArray(positionAttribute);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	        GL.UseProgram(0);
		}
	}
}

