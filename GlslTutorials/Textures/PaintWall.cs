using System;
using System.Drawing;

namespace GlslTutorials
{
	public class PaintWall : Shape
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

		public void PaintRandom()
		{
			int colStart = random.Next(width - 10);
			int rowStart = random.Next(height - 10);
			int red = random.Next(256);
			int green = random.Next(256);
			int blue = random.Next(256);
			Color color = Color.FromArgb(red, green, blue);
			for (int col = colStart; col < colStart + 9; col++)
			{
				for (int row = rowStart; row < rowStart + 9; row++)
				{
					bitmap.SetPixel(col, row, color);
				}
			}
			textureElement.Replace(bitmap);
		}

		public override void Draw()
		{
			textureElement.Draw();
		}
	}
}

