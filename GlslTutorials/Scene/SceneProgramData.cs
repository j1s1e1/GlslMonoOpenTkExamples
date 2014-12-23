using System;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class SceneProgramData
	{
		public int theProgram;

		public int modelToCameraMatrixUnif;
		public int normalModelToCameraMatrixUnif;
		public LightBlock lightBlock;
		public MaterialBlock materialBlock;

		public int cameraToClipMatrixUnif;

		public static SceneProgramData LoadLitProgram(ShadersNames  shaders)
		{
			SceneProgramData data = new SceneProgramData();
			int vertexShaderInt = Shader.compileShader(ShaderType.VertexShader, shaders.vertexShader);
			int fragmentShaderInt = Shader.compileShader(ShaderType.FragmentShader, shaders.fragmentShader);

			data.theProgram = Shader.createAndLinkProgram(vertexShaderInt, fragmentShaderInt);
			data.modelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");

			data.lightBlock = new LightBlock(4);
			data.lightBlock.SetUniforms(data.theProgram);

			data.normalModelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "normalModelToCameraMatrix");

			data.materialBlock = new MaterialBlock();
			data.materialBlock.SetUniforms(data.theProgram);

			data.cameraToClipMatrixUnif =  GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");

			return data;
		}
	};
}

