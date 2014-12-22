using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class MaterialBlock
	{
		public Vector4 diffuseColor;
		public Vector4 specularColor;
		public float specularShininess;
		public float[] padding = new float[3];

		int programNumber;
		int diffuseColorUnif;
		int specularColorUnif;
		int specularShininessUnif;

		public MaterialBlock(Vector4 diffuseColorIn, Vector4 specularColorIn, float specularShininessIn)
		{
			diffuseColor = diffuseColorIn;
			specularColor = specularColorIn;
			specularShininess = specularShininessIn;
		}

		public MaterialBlock()
		{
		}
		
		public static int Size()
		{
			int size = 0;
			size += 2 * Vector4.SizeInBytes;
			size += sizeof(float) * 4;
			return size;
		}
		
		public float[] ToFloat()
	    {
			float[] result = new float[Size()/4];
			int position = 0;
			Array.Copy(diffuseColor.ToFloat(), 0, result, position, 4);
			position += 4;
			Array.Copy(specularColor.ToFloat(), 0, result, position, 4);
			position += 4;
			result[position] = specularShininess;
			return result;
		}
		
		public void SetUniforms(int program)
		{
			programNumber = program;
			diffuseColorUnif = GL.GetUniformLocation(program, "Mtl.diffuseColor");
			specularColorUnif = GL.GetUniformLocation(program, "Mtl.specularColor");
			specularShininessUnif = GL.GetUniformLocation(program, "Mtl.specularShininess");
		}
		
		public void Update(MaterialBlock materialBlock)
		{
			GL.UseProgram(programNumber);
			GL.Uniform4(diffuseColorUnif, materialBlock.diffuseColor);
			GL.Uniform4(specularColorUnif, materialBlock.specularColor);
			GL.Uniform1(specularShininessUnif, materialBlock.specularShininess);
			GL.UseProgram(programNumber);
		}
	}
}

