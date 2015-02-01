using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public static class Programs
	{
		static List<ProgramData> ActivePrograms = new List<ProgramData>();
		
		public static void reset()
	    {
	        ActivePrograms = new List<ProgramData>();
	    }
		
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
		                 Matrix4 mm, int indexDataLength, float[] color)
		{
			ActivePrograms[program].Draw(vertexBufferObject, indexBufferObject, mm, indexDataLength, color);
		}

		public static void DrawWireFrame(int program, int[] vertexBufferObject, int[] indexBufferObject,
			Matrix4 mm, int indexDataLength, float[] color)
		{
			ActivePrograms[program].DrawWireFrame(vertexBufferObject, indexBufferObject, mm, indexDataLength, color);
		}
		
		public static void SetUniformColor(int program, Vector4 color)
		{
			ActivePrograms[program].SetUniformColor(color);
		}
		
		public static void SetUniformTexture(int program, int colorTexUnit)
		{
			ActivePrograms[program].SetUniformTexture(colorTexUnit);
		}

		public static void SetUniformScale(int program, float scale)
		{
			ActivePrograms[program].SetUniformScale(scale);
		}
		
		public static void SetTexture(int program, string texture, bool oneTwenty)
		{
			ActivePrograms[program].SetTexture(texture, oneTwenty);
		}

		public static void SetTexture(int program, int texture)
		{
			ActivePrograms[program].SetTexture(texture);
		}
		
		public static void SetLightPosition(int program, Vector3 lightPos)
		{
			ActivePrograms[program].SetLightPosition(lightPos);
		}
		
		public static void SetModelSpaceLightPosition(int program, Vector3 modelSpaceLightPos)
		{
			ActivePrograms[program].SetModelSpaceLightPosition(modelSpaceLightPos);
		}
		
		public static void SetDirectionToLight(int program, Vector3 dirToLight)
		{
			ActivePrograms[program].SetDirectionToLight(dirToLight);
		}
		
		public static void SetLightIntensity(int program, Vector4 lightIntensity)
		{
			ActivePrograms[program].SetLightIntensity(lightIntensity);
		}

		public static void SetAmbientIntensity(int program, Vector4 ambientIntensity)
		{
			ActivePrograms[program].SetAmbientIntensity(ambientIntensity);
		}
		
		public static void SetNormalModelToCameraMatrix(int program, Matrix3 normalModelToCameraMatrix)
		{
			ActivePrograms[program].SetNormalModelToCameraMatrix(normalModelToCameraMatrix);
		}
		
		public static void SetModelToCameraMatrix(int program, Matrix4 modelToCameraMatrix)
		{
			ActivePrograms[program].SetModelToCameraMatrix(modelToCameraMatrix);
		}

		public static void SetUpLightBlock(int program, int numberOfLights)
		{
			ActivePrograms[program].SetUpLightBlock(numberOfLights);
		}

		public static void UpdateLightBlock(int program, LightBlock lb)
		{
			ActivePrograms[program].UpdateLightBlock(lb);
		}
		
		// for testing only.  This should be calculated
		public static void SetVertexStride(int program, int vertexStride)
		{
			ActivePrograms[program].SetVertexStride(vertexStride);
		}
	}
}

