using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public static class Programs
	{
		static List<ProgramData> ActivePrograms = new List<ProgramData>();
		
		public static int AddProgram(string vertexShader, string fragmentShader)
		{
			int program_number = -1;
			bool new_program = true;
			for (int i = 0; i < ActivePrograms.Count; i++)
			{
				ProgramData pd = ActivePrograms[i];
			
				if (pd.CompareShaders(vertexShader, fragmentShader))
				{
					new_program = false;
					program_number = i;
					break;
				}
			}
			
			if (new_program == true)
			{
				ProgramData pd = new ProgramData(vertexShader, fragmentShader);
				ActivePrograms.Add(pd);
				program_number = ActivePrograms.Count - 1;
			}
			return program_number;	
		}
		
		public static void Draw(int program, int[] vertexBufferObject, int[] indexBufferObject,
		                 Matrix4 cameraToClip, Matrix4 worldToCamera, Matrix4 mm,
		                 int indexDataLength, float[] color, int COORDS_PER_VERTEX, int vertexStride)
		{
			ActivePrograms[program].Draw (vertexBufferObject, indexBufferObject,
		                 cameraToClip, worldToCamera, mm,
		                 indexDataLength, color, COORDS_PER_VERTEX, vertexStride);
		}
	}
}

