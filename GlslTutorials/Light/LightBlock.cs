using System;
using OpenTK;

namespace GlslTutorials
{
	public class LightBlock
	{
		public LightBlock(int NUMBER_OF_LIGHTS_IN = 2)
		{
			NUMBER_OF_LIGHTS = NUMBER_OF_LIGHTS_IN;
			lights = new PerLight[NUMBER_OF_LIGHTS];
		}
		
		int NUMBER_OF_LIGHTS;
		public Vector4 ambientIntensity;
		public float lightAttenuation;
		public float maxIntensity;
		public float[] padding = new float[2];
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
			position += 2;
			for (int i = 0; i < NUMBER_OF_LIGHTS; i++)
			{
				Array.Copy(lights[i].ToFloat(), 0, result, position, PerLight.Size()/4);
				position += PerLight.Size()/4;
			}
			return result;
		}
	};
}

