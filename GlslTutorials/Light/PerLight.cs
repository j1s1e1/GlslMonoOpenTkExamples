using System;
using OpenTK;

namespace GlslTutorials
{
	public class PerLight
	{
		public Vector4 cameraSpaceLightPos;
		public Vector4 lightIntensity;
		
		public static int Size()
		{
			int size = 0;
			size += Vector4.SizeInBytes;
			return size;
		}
		
		public float[] ToFloat()
	    {
			float[] result = new float[Size()/4];
			int position = 0;
			Array.Copy(cameraSpaceLightPos.ToFloat(), 0, result, position, 4);
			position += 4;
			Array.Copy(lightIntensity.ToFloat(), 0, result, position, 4);
			return result;
		}
	}
}

