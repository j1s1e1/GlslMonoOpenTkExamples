using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class LightBlock
	{
		public LightBlock(int NUMBER_OF_LIGHTS_IN = 2)
		{
			NUMBER_OF_LIGHTS = NUMBER_OF_LIGHTS_IN;
			lights = new PerLight[NUMBER_OF_LIGHTS];
			for (int i = 0; i < NUMBER_OF_LIGHTS; i++)
			{
				lights[i] = new PerLight();
			}
		}
		
		int NUMBER_OF_LIGHTS;
		public Vector4 ambientIntensity;
		public float lightAttenuation;
		public float maxIntensity;
		public float gamma;
		public float padding;
		public PerLight[] lights;
		
		
		public static int Size(int numberOfLights)
		{
			int size = 0;
			size += Vector4.SizeInBytes;
			size += sizeof(float) * 4;
			size += numberOfLights * PerLight.Size();
			return size;
		}
					
		public float[] ToFloat()
	    {
			float[] result = new float[Size(NUMBER_OF_LIGHTS)/4];
			int position = 0;
			Array.Copy(ambientIntensity.ToFloat(), 0, result, position, 4);
			position += 4;
			result[position++] = lightAttenuation;
			result[position++] = maxIntensity;
			result[position++] = gamma;
			position += 1;
			for (int i = 0; i < NUMBER_OF_LIGHTS; i++)
			{
				Array.Copy(lights[i].ToFloat(), 0, result, position, PerLight.Size()/4);
				position += PerLight.Size()/4;
			}
			return result;
		}
		/*
		"struct PerLight" +
		"{" +
			"vec4 cameraSpaceLightPos;" +
			"vec4 lightIntensity;" +
		"};" +
		
		"struct Light" +
		"{" +
			"vec4 ambientIntensity;" +
			"float lightAttenuation;" +
			"float maxIntensity;" +
			"PerLight lights[4];" +
		"};" +
		*/
		
		int programNumber;
		int ambientIntensityUnif;
		int lightAttenuationUnif;
		int maxIntensityUnif;
		int[] cameraSpaceLightPosUnif;
		int[] lightIntensityUnif;
		int gammaUnif;
		int numberOfLightsUnif;
		
		public void SetUniforms(int program)
		{
			programNumber = program;
			ambientIntensityUnif = GL.GetUniformLocation(program, "Lgt.ambientIntensity");
			lightAttenuationUnif = GL.GetUniformLocation(program, "Lgt.lightAttenuation");
			maxIntensityUnif = GL.GetUniformLocation(program, "Lgt.maxIntensity");
			gammaUnif = GL.GetUniformLocation(program, "Lgt.gamma");
			cameraSpaceLightPosUnif = new int[NUMBER_OF_LIGHTS];
			lightIntensityUnif = new int[NUMBER_OF_LIGHTS];
			for (int i = 0 ; i < NUMBER_OF_LIGHTS; i++)
			{
				cameraSpaceLightPosUnif[i] = GL.GetUniformLocation(program, "Lgt.lights[" + i.ToString() + "].cameraSpaceLightPos");
				lightIntensityUnif[i] =  GL.GetUniformLocation(program, "Lgt.lights[" + i.ToString() + "].lightIntensity");
			}
			numberOfLightsUnif = GL.GetUniformLocation(program, "numberOfLights");
		}
		
		public void Update(LightBlock lightblock)
		{
			GL.UseProgram(programNumber);
			GL.Uniform4(ambientIntensityUnif, lightblock.ambientIntensity);
			GL.Uniform1(lightAttenuationUnif, lightblock.lightAttenuation);
			GL.Uniform1(maxIntensityUnif, lightblock.maxIntensity);
			if (gammaUnif != -1) GL.Uniform1(gammaUnif, lightblock.gamma);
			for (int i = 0; i < NUMBER_OF_LIGHTS; i++)
			{
				GL.Uniform4(cameraSpaceLightPosUnif[i], lightblock.lights[i].cameraSpaceLightPos);
				GL.Uniform4(lightIntensityUnif[i], lightblock.lights[i].lightIntensity);
			}
			if (numberOfLightsUnif != -1) GL.Uniform1(numberOfLightsUnif, lightblock.NUMBER_OF_LIGHTS);
			GL.UseProgram(programNumber);
		}		

		public void UpdateInternal()
		{
			GL.UseProgram(programNumber);
			GL.Uniform4(ambientIntensityUnif, ambientIntensity);
			GL.Uniform1(lightAttenuationUnif, lightAttenuation);
			GL.Uniform1(maxIntensityUnif, maxIntensity);
			if (gammaUnif != -1) GL.Uniform1(gammaUnif, gamma);
			for (int i = 0; i < NUMBER_OF_LIGHTS; i++)
			{
				GL.Uniform4(cameraSpaceLightPosUnif[i], lights[i].cameraSpaceLightPos);
				GL.Uniform4(lightIntensityUnif[i], lights[i].lightIntensity);
			}
			if (numberOfLightsUnif != -1) GL.Uniform1(numberOfLightsUnif, NUMBER_OF_LIGHTS);
			GL.UseProgram(programNumber);
		}
	};
}

