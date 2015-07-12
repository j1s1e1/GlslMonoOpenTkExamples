using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace GlslTutorials
{
	public class SampleTextures
	{
		public static Bitmap GetBitmapSquares(int row, int col, Color color1, Color color2)
		{
			Bitmap bitmap = new Bitmap(512, 512);
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					if ((((i / row) + (j / col)) % 2) == 0)
					{
						bitmap.SetPixel(i, j, color1);
					}
					else
					{
						bitmap.SetPixel(i, j, color2);
					}
				}
			}
			return bitmap;	
		}
	}
}

