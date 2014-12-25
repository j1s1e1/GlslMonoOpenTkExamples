using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class TextureElement :Shape
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

		public TextureElement(Bitmap bitmap)
		{
			texture = Textures.CreateFromBitmap(bitmap);
		}

		public TextureElement(string fileName)
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

		public override void Draw()
		{
			GL.PushMatrix();

			//GL.Rotate(30f, Vector3.UnitY);
			GL.Translate(x, y, z);

			GL.Translate(256f, 256f, -5f);  // center
			GL.Scale(new Vector3(100f, 100f, 100f));

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

