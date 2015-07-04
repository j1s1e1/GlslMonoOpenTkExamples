using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Triangle3d : Shape
	{
		private String vertexShaderCode =
			"attribute vec4 a_Position;" +
			"attribute vec3 a_Normal;" +		// Per-vertex normal information we will pass in.
			"varying vec3 v_Normal;" +		// This will be passed into the fragment shader.
			"varying vec3 v_Position;" +		// This will be passed into the fragment shader.
			"void main() {" +
			"v_Position = vec3(a_Position);" +
			"v_Normal = a_Normal;" +
			"gl_Position = a_Position;" +
			"}";

		private String fragmentShaderCode =
			"uniform vec4 u_Color;" +
			"uniform vec3 u_LightPos;" +       	// The position of the light in eye space.
			"varying vec3 v_Position;" +		// This will be passed into the fragment shader.
			"varying vec3 v_Normal;" +         	// Interpolated normal for this fragment.
			"void main() {" +

			// Will be used for attenuation.
			"float distance = length(u_LightPos - v_Position);" +

			// Get a lighting direction vector from the light to the vertex.
			"vec3 lightVector = normalize(u_LightPos - v_Position);" +

			// Calculate the dot product of the light vector and vertex normal. If the normal and light vector are
			// pointing in the same direction then it will get max illumination.
			"float diffuse = max(dot(v_Normal, lightVector), 0.0);" +

			// Add attenuation." +
			"diffuse = diffuse * (1.0 / distance);" +

			// Add ambient lighting"
			"diffuse = diffuse + 0.2;" +

			// Multiply the color by the diffuse illumination level and texture value to get final output color."
			"gl_FragColor = (diffuse * u_Color);" +
			"}";

		public static int mProgram = -1;
		private int mPositionHandle;
		private int mNormalHandle;
		private int mColorHandle;
		private int mLightPosHandle;

		Vector3[] vertices;
		bool drawable = false;

		private float[] getTriangleCoords()
		{
			float[] coords_with_normals = new float[18];
			for (int i = 0; i < coords_with_normals.Length; i++) {
				switch (i % 6) {
				case 0:	coords_with_normals[i] = vertices[i/6].X; break;
				case 1: coords_with_normals[i] = vertices[i/6].Y; break;
				case 2: coords_with_normals[i] = vertices[i/6].Z; break;
				case 3: coords_with_normals[i] = vertices[i/6].Normalized().X; break;
				case 4: coords_with_normals[i] = vertices[i / 6].Normalized().Y; break;
				case 5: coords_with_normals[i] = vertices[i / 6].Normalized().Z; break;

				}
			}
			return coords_with_normals;
		}
			
		public Triangle3d (Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, bool drawable_in = false)
		{
			drawable = drawable_in;
			vertices = new Vector3[3];
			vertices[0] = vertex1;
			vertices[1] = vertex2;
			vertices[2] = vertex3;
			vertexCount = 3;
			if (drawable)  // some triangle just used in other structures
			{
				vertexCoords = getTriangleCoords();
				vertexData = vertexCoords;
				SetupSimpleIndexBuffer();
				InitializeVertexBuffer();

				if (mProgram < 0)
				{
					int vertexShader = Shader.compileShader(ShaderType.VertexShader, vertexShaderCode);
					int fragmentShader = Shader.compileShader(ShaderType.FragmentShader, fragmentShaderCode);

					mProgram = Shader.createAndLinkProgram(vertexShader, fragmentShader,
						new String[] {"a_Position", "a_Normal"});


					// get handle to vertex shader's vPosition member
					mPositionHandle = GL.GetAttribLocation(mProgram, "a_Position");
					mNormalHandle = GL.GetAttribLocation(mProgram, "a_Normal");
					mLightPosHandle = GL.GetUniformLocation(mProgram, "u_LightPos");
				}
			}
		}

		public float[] getVertexFloats()
		{
			float[] vertex_floats = new float[9];
			Array.Copy(vertices[0].ToFloat(), 0, vertex_floats, 0, 3);
			Array.Copy(vertices[1].ToFloat(), 0, vertex_floats, 3, 3);
			Array.Copy(vertices[2].ToFloat(), 0, vertex_floats, 6, 3);
			return vertex_floats;
		}

		public Triangle3d[] create4Triangles()
		{
			Triangle3d[] four_triangles = new Triangle3d[4];
			Vector3[] new_vertexes = new Vector3[3];
			new_vertexes[0] = (vertices[0] + vertices[1]) /2;
			new_vertexes[1] = (vertices[1] + vertices[2]) / 2;
			new_vertexes[2] = (vertices[2] + vertices[0]) / 2;
			four_triangles[0] = new Triangle3d(vertices[0], new_vertexes[0], new_vertexes[2]);
			four_triangles[1] = new Triangle3d(new_vertexes[2], new_vertexes[0], new_vertexes[1]);
			four_triangles[2] = new Triangle3d(new_vertexes[0], vertices[1], new_vertexes[1]);
			four_triangles[3] = new Triangle3d(new_vertexes[2], new_vertexes[1], vertices[2]);
			return four_triangles;
		}

		public void NormalizeVertices()
		{
			vertices[0] = vertices[0].Normalized();
			vertices[1] = vertices[1].Normalized();
			vertices[2] = vertices[2].Normalized();
		}
			
		public void setOffset (float x_in, float y_in, float z_in)
		{
			move(x_in - x, y - y_in, z - z_in );
			x = x_in;
			y = y_in;
			z = z_in;
		}

		public void move(float x_move, float y_move, float z_move) {
			for (int i = 0; i < vertexCoords.Length; i++) {
				switch (i % 3) {
				case 0:
					vertexCoords[i] = vertexCoords[i] + x_move;
					break;
				case 1:
					vertexCoords[i] = vertexCoords[i] + y_move;
					break;
				case 2:
					vertexCoords[i] = vertexCoords[i] + z_move;
					break;
				}

			}
			SetupVertexBuffer();
		}

		public override void Draw() {
			if (drawable)
			{
				// Add program to OpenGL environment
				GL.UseProgram(mProgram);

				// get handle to vertex shader's vPosition member
				mPositionHandle = GL.GetAttribLocation(mProgram, "a_Position");
				mNormalHandle = GL.GetAttribLocation(mProgram, "a_Normal");
				mLightPosHandle = GL.GetUniformLocation(mProgram, "u_LightPos");

				// Enable a handle to the triangle vertices
				GL.EnableVertexAttribArray(mPositionHandle);

				// Prepare the triangle coordinate data
				GL.VertexAttribPointer(mPositionHandle, COORDS_PER_VERTEX, 
					VertexAttribPointerType.Float, false, vertexStride, (IntPtr)0);

				// Pass in the normal information
				GL.EnableVertexAttribArray(mNormalHandle);
				GL.VertexAttribPointer(mNormalHandle, COORDS_PER_VERTEX, 
					VertexAttribPointerType.Float, false, vertexStride, (IntPtr)3);

				// Pass in the light position in eye space.
				GL.Uniform3(mLightPosHandle, 0.75f, 0.75f, 0.75f);


				// get handle to fragment shader's vColor member
				mColorHandle = GL.GetUniformLocation(mProgram, "u_Color");

				// Set color for drawing the triangle
				GL.Uniform4(mColorHandle, 1, color);

				// Draw the triangle
				//GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
				GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);

				// Disable vertex array
				GL.DisableVertexAttribArray(mPositionHandle);
			}
		}
	}
}

