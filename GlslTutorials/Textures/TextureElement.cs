﻿using System;
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
			Textures.EnableTextures();
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
			base.Move(v);
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

		public int GetTexture()
		{
			return texture;
		}

		public void SetTexture(int i)
		{
			texture = i;
		}

		public override void Draw()
		{
			Programs.SetLightPosition(programNumber, lightPosition);
			Programs.SetUniformScale(programNumber, scale);
			Matrix4 mm = Rotate(modelToWorld, axis, angle);	
			mm = Matrix4.Mult(mm, Matrix4.CreateScale(scale));
			Programs.SetUniformTexture(programNumber, texUnit);
			Programs.SetTexture(programNumber, texture);
			Programs.Draw(programNumber, vertexBufferObject[0], indexBufferObject[0], mm, indexData.Length, color);
		}
			
	}
}

