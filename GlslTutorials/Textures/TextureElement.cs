using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class TextureElement : Shape
	{
		int texture;
		int texUnit = 0;

		float scale = 1.0f;
		Vector3 lightPosition = new Vector3(0f, 0f, 1f);

		public TextureElement(Bitmap bitmap)
		{
			texture = Textures.CreateFromBitmap(bitmap);
			Setup();
		}

		public TextureElement(string fileName)
		{
			texture = Textures.Load(fileName);
			Setup();
		}

		private void Setup()
		{
			vertexData = new float[]{	
				// x y z xn yn zn tx ty
				-1f, -1f, 0f, 0f, 0f, 1f, 0f, 0f,
				-1f, 1f, 0f, 0f, 0f, 1f, 0f, 1f,
				1f, 1f, 0f, 0f, 0f, 1f, 1f, 1f,

				-1f, -1f, 0f, 0f, 0f, 1f, 0f, 0f,
				1f, 1f, 0f, 0f, 0f, 1f, 1f, 1f,
				1f, -1f, 0f, 0f, 0f, 1f, 1f, 0f
										};
			COORDS_PER_VERTEX = 8;
			SetupSimpleIndexBuffer();
			InitializeVertexBuffer();

			programNumber = Programs.AddProgram(VertexShaders.MatrixTexture, FragmentShaders.MatrixTexture);
			Programs.SetUniformTexture(programNumber, texUnit);
			Programs.SetTexture(programNumber, texture);
		}

		public void Replace(string fileName)
		{
			int texture2 = Textures.Load(fileName);
			texture = texture2;
		}

		public void Replace(Bitmap bitmap)
		{
			int texture2 = Textures.CreateFromBitmap(bitmap);
			texture = texture2;
		}

		public override void Move (Vector3 v)
		{
			base.Move(v);
			lightPosition = lightPosition + v * scale;
		}

		public void Scale(float scaleIn)
		{
			scale = scale * scaleIn;
		}

		public override void Draw()
		{
			Programs.SetLightPosition(programNumber, lightPosition);
			Matrix4 mm = Rotate(modelToWorld, axis, angle);
			mm.M41 = offset.X;
			mm.M42 = offset.Y;
			mm.M43 = offset.Z;	
			mm = Matrix4.Mult(mm, Matrix4.CreateScale(scale));
			Programs.SetTexture(programNumber, texture);
			Programs.Draw(programNumber, vertexBufferObject, indexBufferObject, mm, indexData.Length, color);
		}
			
	}
}

