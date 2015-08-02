using System;
using System.Collections.Generic;
using System.Text;
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
		
		public static void Draw(int program, int vertexBufferObject, int indexBufferObject,
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

		public static int SetTexture(int program, string texture, bool oneTwenty)
		{
			return ActivePrograms[program].SetTexture(texture, oneTwenty);
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

		public static void SetUpMaterialBlock(int program)
		{
			ActivePrograms[program].SetUpMaterialBlock();
		}

		public static void UpdateLightBlock(int program, LightBlock lb)
		{
			ActivePrograms[program].UpdateLightBlock(lb);
		}

		public static void UpdateMaterialBlock(int program, MaterialBlock mb)
		{
			ActivePrograms[program].UpdateMaterialBlock(mb);
		}
		
		// for testing only.  This should be calculated
		public static void SetVertexStride(int program, int vertexStride)
		{
			ActivePrograms[program].SetVertexStride(vertexStride);
		}

		public static void SetShadowMap(int program, int shadowMap)
		{
			ActivePrograms[program].SetShadowMap(shadowMap);
		}

		public static String GetProgramInfoLog(int program)
		{
			return ActivePrograms[program].getProgramInfoLog();
		}

		public static String GetVertexShaderInfoLog(int program)
		{
			return ActivePrograms[program].getVertexShaderInfoLog();
		}

		public static int GetVertexShader(int program)
		{
			return ActivePrograms[program].getVertexShader();
		}

		public static int GetFragmentShader(int program)
		{
			return ActivePrograms[program].getFragmentShader();
		}

		public static String GetVertexAttributes(int program)
		{
			return ActivePrograms[program].getVertexAttributes();
		}

		public static String GetVertexShaderSource(int program)
		{
			return ActivePrograms[program].getVertexShaderSource();
		}

		public static String GetUniforms(int program)
		{
			return ActivePrograms[program].getUniforms();
		}

		public static String GetVertexShaderInfo(int program)
		{
			StringBuilder result = new StringBuilder();
			result.Append("\n");
			result.Append(GetProgramInfoLog(program));
			result.Append("\n");
			result.Append(GetVertexShaderInfoLog(program));
			result.Append("\n");
			result.Append("Vertex Shader = " + GetVertexShader(program).ToString());
			result.Append("\n");
			result.Append(GetVertexShaderSource(program));
			result.Append("\n");
			result.Append(GetVertexAttributes(program));
			result.Append(GetUniforms(program));
			return result.ToString();
		}

		public static String GetFragmentShaderSource(int program)
		{
			return ActivePrograms[program].getFragmentShaderSource();
		}

		public static String GetFragmentShaderInfo(int program)
		{
			StringBuilder result = new StringBuilder();
			result.Append("Fragment Shader = " + GetFragmentShader(program).ToString());
			result.Append("\n");
			result.Append(GetFragmentShaderSource(program));
			result.Append("\n");
			return result.ToString();
		}
			
		public static String DumpShaders()
		{
			StringBuilder result = new StringBuilder();
			for (int program = 0; program < ActivePrograms.Count; program++) {
				result.Append("\n");
				result.Append("Program " + program.ToString());
				result.Append("\n");
				result.Append(GetVertexShaderInfo(program));
				result.Append("\n");
				result.Append(GetFragmentShaderInfo(program));
				result.Append("\n");
			}
			return result.ToString();
		}

		public static int GetProgram(int program)
		{
			return ActivePrograms[program].GetProgram();
		}

		public static int GetModelToCameraMatrixUnif(int program)
		{
			return ActivePrograms[program].GetModelToCameraMatrixUnif();
		}

		public static int GetCameraToClipMatrixUniform(int program)
		{
			return ActivePrograms[program].GetCameraToClipMatrixUniform();
		}

		public static int GetShadowMapUniform(int program)
		{
			return ActivePrograms[program].GetShadowMapUniform();
		}
	}
}

