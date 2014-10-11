using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class ProgramData
	{
        public int theProgram;
        public int positionAttribute;
        public int colorAttribute;
        public int modelToCameraMatrixUnif;
        public int modelToWorldMatrixUnif;
        public int worldToCameraMatrixUnif;
        public int cameraToClipMatrixUnif;
        public int baseColorUnif;

        public int normalModelToCameraMatrixUnif;
        public int dirToLightUnif;
        public int lightIntensityUnif;
        public int ambientIntensityUnif;
        public int normalAttribute;
		string vertexShader;
		string fragmentShader;
			
		
		public ProgramData(string vertexShaderIn, string fragmentShaderIn)
	    {
			vertexShader = vertexShaderIn;
			fragmentShader = fragmentShaderIn;
	        int vertex_shader = Shader.loadShader(ShaderType.VertexShader, vertexShader);
	        int fragment_shader = Shader.loadShader(ShaderType.FragmentShader, fragmentShader);
	        theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
	
	        positionAttribute = GL.GetAttribLocation(theProgram, "position");
	        colorAttribute = GL.GetAttribLocation(theProgram, "color");
			if (positionAttribute != -1) 
			{
				if (positionAttribute != 0)
				{
					MessageBox.Show("These meshes only work with position at location 0 " + vertexShader);
				}
			}
			if (colorAttribute != -1) 
			{
				if (colorAttribute != 1)
				{
					MessageBox.Show("These meshes only work with color at location 1" + vertexShader);
				}
			}
	
	        modelToWorldMatrixUnif = GL.GetUniformLocation(theProgram, "modelToWorldMatrix");
	        worldToCameraMatrixUnif = GL.GetUniformLocation(theProgram, "worldToCameraMatrix");
	        cameraToClipMatrixUnif = GL.GetUniformLocation(theProgram, "cameraToClipMatrix");
			if (cameraToClipMatrixUnif == -1)
			{
				cameraToClipMatrixUnif = GL.GetUniformLocation(theProgram, "Projection.cameraToClipMatrix");
			}
	        baseColorUnif = GL.GetUniformLocation(theProgram, "baseColor");
	
	        modelToCameraMatrixUnif = GL.GetUniformLocation(theProgram, "modelToCameraMatrix");
	
	        normalModelToCameraMatrixUnif = GL.GetUniformLocation(theProgram, "normalModelToCameraMatrix");
	        dirToLightUnif =  GL.GetUniformLocation(theProgram, "dirToLight");
	        lightIntensityUnif = GL.GetUniformLocation(theProgram, "lightIntensity");
	        ambientIntensityUnif = GL.GetUniformLocation(theProgram, "ambientIntensity");
	        normalAttribute = GL.GetAttribLocation(theProgram, "normal");
	    }
		
		public bool CompareShaders(string vertexShaderIn, string fragmentShaderIn)
		{
			return ((vertexShaderIn == vertexShader) & (fragmentShader == fragmentShaderIn));
		}
		
		public void Draw(int[] vertexBufferObject, int[] indexBufferObject,
		                 Matrix4 cameraToClip, Matrix4 worldToCamera, Matrix4 mm,
		                 int indexDataLength, float[] color, int COORDS_PER_VERTEX, int vertexStride)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);
			
	        GL.UseProgram(theProgram);	
	        GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref cameraToClip);
	        GL.UniformMatrix4(worldToCameraMatrixUnif, false, ref worldToCamera);
	        GL.UniformMatrix4(modelToWorldMatrixUnif, false, ref mm);
	        GL.Uniform4(baseColorUnif, 1, color);
			

			GL.EnableVertexAttribArray(positionAttribute);
	        // Prepare the triangle coordinate data
	        GL.VertexAttribPointer(positionAttribute, COORDS_PER_VERTEX, 
				VertexAttribPointerType.Float, false, vertexStride, (IntPtr)0);
			
	        // Draw the triangle
	 		GL.DrawElements(PrimitiveType.Triangles, indexDataLength, DrawElementsType.UnsignedShort, 0);
			
	        // Disable vertex array
	        GL.DisableVertexAttribArray(positionAttribute);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	        GL.UseProgram(0);	
		}
			
		public override string ToString()
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine("theProgram = " + theProgram.ToString());
			result.AppendLine("positionAttribute = " + positionAttribute.ToString());
			result.AppendLine("colorAttribute = " + colorAttribute.ToString());
			result.AppendLine("modelToCameraMatrixUnif = " + modelToCameraMatrixUnif.ToString());
			result.AppendLine("modelToWorldMatrixUnif = " + modelToWorldMatrixUnif.ToString());
			result.AppendLine("worldToCameraMatrixUnif = " + worldToCameraMatrixUnif.ToString());
			result.AppendLine("cameraToClipMatrixUnif = " + cameraToClipMatrixUnif.ToString());
			result.AppendLine("baseColorUnif = " + baseColorUnif.ToString());
			result.AppendLine("normalModelToCameraMatrixUnif = " + normalModelToCameraMatrixUnif.ToString());
			result.AppendLine("dirToLightUnif = " + dirToLightUnif.ToString());
			result.AppendLine("lightIntensityUnif = " + lightIntensityUnif.ToString());
			result.AppendLine("ambientIntensityUnif = " + ambientIntensityUnif.ToString());
			result.AppendLine("normalAttribute = " + normalAttribute.ToString());
			return result.ToString();
		}
	}
}

