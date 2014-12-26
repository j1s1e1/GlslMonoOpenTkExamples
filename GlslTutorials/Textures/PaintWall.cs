using System;
using System.Drawing;
using OpenTK;

namespace GlslTutorials
{
	public class PaintWall
	{
		Bitmap bitmap;
		int width = 256;
		int height = 256;
		TextureElement textureElement;
		Random random;
		public PaintWall ()
		{
			bitmap = new Bitmap(width, height);
			for (int col = 0; col < width; col++)
			{
				for (int row = 0; row < height; row++)
				{
					if ((Math.Abs(row - 128) < 100) & (Math.Abs(col - 128) < 100))
					{
						// clear pixels bitmap.SetPixel(col, row, Color.White);
					}
					else
					{
						bitmap.SetPixel(col, row, Color.Blue);
					}
				}
			}
			textureElement = new TextureElement(bitmap);
			random = new Random();
		}

		private void Paint(int colStart, int rowStart, int size, Color color)
		{
			int distanceFromCenter;
			int sizeSquared = (int)Math.Pow(size, 2);
			int colCenter = colStart + size/2;
			int rowCenter = rowStart + size/2;
			if (colStart < 0) colStart = 0;
			if (rowStart < 0) rowStart = 0;
			if (colStart + size > width) colStart = width - size;
			if (rowStart + size > height) rowStart = height - size;
			for (int col = colStart; col < colStart + size; col++)
			{
				for (int row = rowStart; row < rowStart + size; row++)
				{
					distanceFromCenter = (int)(Math.Pow((col - colCenter),2) + Math.Pow((row - rowCenter),2));
					if (distanceFromCenter < sizeSquared/4)
					{
						if ((random.NextDouble() * distanceFromCenter) < size)
						{
							bitmap.SetPixel(col, row, color);
						}
					}
				}
			}
			textureElement.Replace(bitmap);
		}

		public void PaintRandom()
		{
			int colStart = random.Next(width - 10);
			int rowStart = random.Next(height - 10);
			int red = random.Next(256);
			int green = random.Next(256);
			int blue = random.Next(256);
			Color color = Color.FromArgb(red, green, blue);
			Paint(colStart, rowStart, 9, color);
		}

		public void Paint(float x, float y)
		{
			int colStart = (int)(width/2 + x * width/2);
			int rowStart = (int)(height/2 + y * width/2);
			int red = random.Next(256);
			int green = random.Next(256);
			int blue = random.Next(256);
			Color color = Color.FromArgb(red, green, blue);
			Paint(colStart, rowStart, 50, color);
		}

		public void Move(float x, float y, float z)
		{
			textureElement.Move(x, y, z);
		}

		public void Move(Vector3 movement)
		{
			textureElement.Move(movement.X, movement.Y, movement.Z);
		}

		public void Scale(float scale)
		{
			textureElement.Scale(scale);
		}

		public void SetRotations(Vector3 rotations)
		{
			textureElement.SetRotations(rotations);
		}

		public void Draw()
		{
			textureElement.Draw();
		}
	}
}

