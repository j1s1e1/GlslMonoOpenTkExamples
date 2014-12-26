using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class TextureElementOld :Shape
	{
		int texture;
		Vector2 topRightCoord = new Vector2(1.0f, 1.0f);
		Vector3 topRightVertex = new Vector3(1.0f, 1.0f, 0.5f);

		Vector2 topLeftCoord = new Vector2(0.0f, 1.0f);
		Vector3 topLeftVertex = new Vector3(-1.0f, 1.0f, 0.5f);

		Vector2 bottomLeftCoord = new Vector2(0.0f, 0.0f);
		Vector3 bottomLeftVertex = new Vector3(-1.0f, -1.0f, 0.5f);

		Vector2 bottomRightCoord = new Vector2(1.0f, 0.0f);
		Vector3 bottomRightVertex = new Vector3(1.0f, -1.0f, 0.5f);

		Vector3 rotate = new Vector3(0f, 0f, 0f);

		public TextureElementOld(Bitmap bitmap)
		{
			texture = Textures.CreateFromBitmap(bitmap);
		}

		public TextureElementOld(string fileName)
		{
			texture = Textures.Load(fileName);
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

		public void SetRotations(Vector3 rotations)
		{
			rotate = rotations;
		}

		public void Scale(Vector3 scale)
		{
			topRightVertex *= scale;
			topLeftVertex *= scale;
			bottomLeftVertex *= scale;
			bottomRightVertex *= scale;
		}

		public override void Draw()
		{
			GL.PushMatrix();

			GL.Translate(x, y, z);

			GL.Translate(256f, 256f, -5f);  // center

			GL.Scale(new Vector3(100f, 100f, 100f));

			GL.Rotate(rotate.X, Vector3.UnitX);
			GL.Rotate(rotate.Y, Vector3.UnitY);
			GL.Rotate(rotate.Z, Vector3.UnitZ);

			//GL.Translate(0, 0, -20f);  // center
			//GL.Scale(new Vector3(0.01f, 0.01f, 0.01f));

			GL.BindTexture(TextureTarget.Texture2D, texture);

			GL.Begin(BeginMode.Quads);

			//Bind texture coordinates to vertices in ccw order

			GL.TexCoord2(topRightCoord);
			GL.Vertex3(topRightVertex);

			GL.TexCoord2(topLeftCoord);
			GL.Vertex3(topLeftVertex);

			//Bottom-Left
			GL.TexCoord2(bottomLeftCoord);
			GL.Vertex3(bottomLeftVertex);

			//Bottom-Right
			GL.TexCoord2(bottomRightCoord);
			GL.Vertex3(bottomRightVertex);

			GL.End();

			GL.BindTexture(TextureTarget.Texture2D, 0);

			GL.PopMatrix();
		}
			
	}
}

