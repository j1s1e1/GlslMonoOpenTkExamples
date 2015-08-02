using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class TextureElement2 : Shape
	{
		int texture;
		int texUnit = 0;

		float scale = 1.0f;
		Vector3 lightPosition = new Vector3(0f, 0f, 1f);

		public TextureElement2(Bitmap bitmap)
		{
			texture = Textures.CreateFromBitmap(bitmap);
			Setup();
		}

		public TextureElement2(string fileName)
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

			programNumber = Programs.AddProgram(VertexShaders.MatrixTexture, FragmentShaders.MatrixTextureScale);
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
			modelToWorld.Row3 += new Vector4(v, 0.0f);
			lightPosition = lightPosition + v * scale;
		}

		public void Scale(float scaleIn)
		{
			base.Scale(new Vector3(scaleIn, scaleIn, scaleIn));
			scale = scale * scaleIn;
			lightPosition = lightPosition * scale;
		}

		public override void SetLightPosition(Vector3 v)
		{
			lightPosition = v;
		}

		public override void Draw()
		{
			Programs.SetLightPosition(programNumber, lightPosition);
			Programs.SetUniformScale(programNumber, scale);
			Programs.SetTexture(programNumber, texture);
			Programs.Draw(programNumber, vertexBufferObject[0], indexBufferObject[0], modelToWorld, indexData.Length, color);
		}			
	}
}

