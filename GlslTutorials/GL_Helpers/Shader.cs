using OpenTK.Graphics.OpenGL;
using System;
using System.Windows.Forms;

namespace GlslTutorials
{
	public class Shader
	{
		public Shader ()
		{
		}
		
		public static int loadShader(ShaderType shaderType, String shaderCode) 
		{
	        // create a vertex shader type (GLES20.GL_VERTEX_SHADER)
	        // or a fragment shader type (GLES20.GL_FRAGMENT_SHADER)
	        int shader = GL.CreateShader (shaderType);
	        // add the source code to the shader and compile it
			GL.ShaderSource (shader, shaderCode);
            GL.CompileShader (shader);
	        return shader;
    	}
		
		public static int createAndLinkProgram(int vertexShaderHandle, int fragmentShaderHandle)
    	{
	        int programHandle = GL.CreateProgram();
	
	        if (programHandle != 0)
	        {
	            // Bind the vertex shader to the program.
	            GL.AttachShader(programHandle, vertexShaderHandle);
	
	            // Bind the fragment shader to the program.
	            GL.AttachShader(programHandle, fragmentShaderHandle);
	
	            // Link the two shaders together into a program.
	            GL.LinkProgram(programHandle);
	
	            // Get the link status.
	            int[] linkStatus = new int[1];
	            GL.GetProgram(programHandle, ProgramParameter.LinkStatus, linkStatus);
	
	            // If the link failed, delete the program.
	            if (linkStatus[0] == 0)
	            {
	                MessageBox.Show("Error compiling program: " + GL.GetProgramInfoLog(programHandle));
	                GL.DeleteProgram(programHandle);
	                programHandle = 0;
	            }
        	}
			return programHandle;
		}

		
		public static int createAndLinkProgram(int vertexShaderHandle, int fragmentShaderHandle, String[] attributes)
	    {
	        int programHandle = GL.CreateProgram();
	
	        if (programHandle != 0)
	        {
	            // Bind the vertex shader to the program.
	            GL.AttachShader(programHandle, vertexShaderHandle);
	
	            // Bind the fragment shader to the program.
	            GL.AttachShader(programHandle, fragmentShaderHandle);
	
	            // Bind attributes
	            if (attributes != null)
	            {
	                int size = attributes.Length;
	                for (int i = 0; i < size; i++)
	                {
	                    GL.BindAttribLocation(programHandle, i, attributes[i]);
	                }
	            }
	
	            // Link the two shaders together into a program.
	            GL.LinkProgram(programHandle);
	
	            // Get the link status.
	            int[] linkStatus = new int[1];
	            GL.GetProgram(programHandle, ProgramParameter.LinkStatus, linkStatus);
	
	            // If the link failed, delete the program.
	            if (linkStatus[0] == 0)
	            {
	               	MessageBox.Show("Error compiling program: " + GL.GetProgramInfoLog(programHandle));
	                GL.DeleteProgram(programHandle);
	                programHandle = 0;
	            }
	        }
	
	        if (programHandle == 0)
	        {
				MessageBox.Show("Error creating program.");;
	        }
	
	        return programHandle;
	    }

	}
}

