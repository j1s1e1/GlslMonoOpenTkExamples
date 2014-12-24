using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GlslTutorials
{
	public class Shader
	{
		public Shader ()
		{
		}
		
		public static int compileShader(ShaderType shaderType, String shaderSource)
	    {
	        int shaderHandle = GL.CreateShader(shaderType);
	
	        if (shaderHandle != 0)
	        {
	            // Pass in the shader source.
	            GL.ShaderSource(shaderHandle, shaderSource);
	
	            // Compile the shader.
	            GL.CompileShader(shaderHandle);
	
	            // Get the compilation status.
	            int[] compileStatus = new int[1];
	            GL.GetShader(shaderHandle,ShaderParameter.CompileStatus, compileStatus);
	
	            // If the compilation failed, delete the shader.
	            if (compileStatus[0] == 0)
	            {
					string shaderInfoLog = GL.GetShaderInfoLog(shaderHandle);
					MessageBox.Show("Error creating shader: " + shaderInfoLog);
	                GL.DeleteShader(shaderHandle);
	                shaderHandle = 0;
	            }
	        }
	
	        if (shaderHandle == 0)
	        {
				 MessageBox.Show("Error creating shader.");
	        }
	
	        return shaderHandle;
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
					String errorInfo = GL.GetProgramInfoLog(programHandle);
					MessageBox.Show("Error compiling program: " + errorInfo);
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

		public static int LinkProgram(List<int> shaderHandles)
		{
			int programHandle = GL.CreateProgram();

			if (programHandle != 0)
			{
				// Bind the shaders to the program.
				foreach (int handle in shaderHandles)
				{
					GL.AttachShader(programHandle, handle);
				}
					
				// Link the two shaders together into a program.
				GL.LinkProgram(programHandle);

				// Get the link status.
				int[] linkStatus = new int[1];
				GL.GetProgram(programHandle, ProgramParameter.LinkStatus, linkStatus);

				// If the link failed, delete the program.
				if (linkStatus[0] == 0)
				{
					String errorInfo = GL.GetProgramInfoLog(programHandle);
					MessageBox.Show("Error compiling program: " + errorInfo);
					GL.DeleteProgram(programHandle);
					programHandle = 0;
				}
			}
			return programHandle;
		}

	}
}

