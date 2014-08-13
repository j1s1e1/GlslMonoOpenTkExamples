using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace GlslTutorials
{
	public class MatrixSphere : Shape  
	{
		public MatrixSphere ()
		{
		}
		
		public static int mProgram = -1;
	    private int positionAttribute;
	
	    private int cameraToClipMatrixUnif;
	    private static int worldToCameraMatrixUnif;
	    private int modelToWorldMatrixUnif;
	    private int colorUnif;
	
	    static Matrix4 cameraToClip = new Matrix4();
	    static Matrix4 worldToCamera = new Matrix4();
	    Matrix4 modelToWorld = new Matrix4();
	
	    float center_x = 0f;
	    float center_y = 0f;
	    float center_z = 0f;
	
	    public void SetCenter(float[] center)
	    {
	        SetCenter(center[0], center[1], center[2] );
	    }
	
	    public void SetCenter(float x_center, float y_center, float z_center) {
	        float[] temp = GetCircleCoords(radius); // can't move cirlceCoords, so don't reinitialize
	        for (int i = 0; i < temp.Length; i++) {
	            vertexCoords[i] = temp[i];
	        }
	        for (int i = 0; i < vertexCoords.Length; i++) {
	            switch (i % 3) {
	                case 0:
	                    vertexCoords[i] = vertexCoords[i] + x_center;
	                    break;
	                case 1:
	                    vertexCoords[i] = vertexCoords[i] + y_center;
	                    break;
	                case 2:
	                    vertexCoords[i] = vertexCoords[i] + z_center;
	                    break;
	            }
	
	        }
	        SetupVertexBuffer();
	    }
	
	    public override void SetOffset (float x_in, float y_in, float z_in)
	    {
	        Move(x_in - x_offset, y_offset - y_in, z_offset - z_in );
	        x_offset = x_in;
	        y_offset = y_in;
	        z_offset = z_in;
	    }
	
	    public void Move(float[] coords)
	    {
	        Move(coords[0], coords[1], coords[2]);
	    }
	
	    public override void Move(float x_move, float y_move, float z_move) {
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
	        center_x = center_x + x_move;
	        center_y = center_y + y_move;
	        center_z = center_z + y_move;
	        SetupVertexBuffer();
	    }
	
	    float radius;
	    float angle_step = (float)(Math.PI / 6.0);
	
	    private float[] GetCircleCoords(float radius) {
	        float[] coords = Icosahedron.CloneTriangles();
	        for (int i = 0; i < coords.Length; i++) {
	            coords[i] = coords[i] * radius;
	        }
	        return coords;
	    }
	
	    public MatrixSphere(float radius_in, int new_divide_count=1) {
	        radius = radius_in;
	        vertexCoords = GetCircleCoords(radius);
	        vertexCount = vertexCoords.Length / COORDS_PER_VERTEX;
	        SetupVertexBuffer();
	
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
	        vertexStride = vertexStride/2; // no normals here
	    }
	
	    Vector3 axis = new Vector3(0f, 0f, 1f);
	    float angle = 0;
	
	    public void UpdateAngle(float degrees)
	    {
	        angle = degrees;
	    }
	
	    ///Applies a rotation matrix about the given axis, with the given angle in degrees.
	    public Matrix4 Rotate(Matrix4 input, Vector3 axis, float angDegCCW)
	    {
	        Matrix4 rotation = Matrix4.Rotate(axis, (float)Math.PI / 180.0f * angDegCCW);
	        return Matrix4.Mult(rotation, input);
	    }
	
	    private void drawSub(int first_triangle, int last_triangle)
	    {
	        int newVertexCount = (last_triangle - first_triangle + 1) * 3 * 3 / COORDS_PER_VERTEX;
	        // Add program to OpenGL environment
	        GL.UseProgram(mProgram);
	
	        Matrix4 mm = Rotate(modelToWorld, axis,angle);
	
	        GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref cameraToClip);
	        GL.UniformMatrix4(worldToCameraMatrixUnif, false, ref worldToCamera);
	        GL.UniformMatrix4(modelToWorldMatrixUnif, false, ref mm);
	        GL.Uniform4(colorUnif, 1, color);
	
	        // Enable a handle to the triangle vertices
	        GL.EnableVertexAttribArray(positionAttribute);
	
	        //vertexBuffer.position(first_triangle * 3 * 3);
	
	        // Prepare the triangle coordinate data
	        //************ FIXME  GL.VertexAttribPointer(positionAttribute, COORDS_PER_VERTEX, 
			//************ FIXME  VertexAttribPointerType.Float, false, vertexStride, vertexBuffer);
	
	        // Draw the triangle
	        //************ FIXME  GL.DrawArrays(PrimitiveType.Triangles, 0, newVertexCount);
	
	        // Disable vertex array
	        GL.DisableVertexAttribArray(positionAttribute);
	
	        GL.UseProgram(0);
	    }
	
	    public void draw() {
	        drawSub(0, 19);
	    }
	
	    public void drawSemi(int first_triangle, int last_triangle) {
	        drawSub(first_triangle, last_triangle);
	    }
	
	    public static void MoveWorld(float newx, float newy, float newz)
	    {
	        worldToCamera.M41 = newx;
	        worldToCamera.M42 = newy;
	        worldToCamera.M43 = newz;
	    }
	
	    public void MoveModel(float newx, float newy, float newz)
	    {
	        modelToWorld.M41 = newx;
	        modelToWorld.M42 = newy;
	        modelToWorld.M43 = newz;
	    }

	}
}

