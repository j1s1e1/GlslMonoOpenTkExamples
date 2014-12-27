using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Texture : TutorialBase
	{
		public Tut_Texture ()
		{
		}

		TextureElement wood;
		PaintWall paintWall;
		TextureElement flashlight;
		TextureElement bitmapWithAlpha;

		bool drawWood = true;
		bool drawFlashlight = true;
		bool drawPaintWall = true;
		
		protected override void init ()
		{
		    GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			wood = new TextureElement("wood4_rotate.png");
			wood.Scale(0.2f);
			wood.Move(0f, 0f, -0.2f);
			wood.RotateShape(Vector3.UnitX, 45f);

			flashlight = new TextureElement("flashlight.png");
			flashlight.Move(.04f, 0.4f, -0.1f);
			flashlight.Scale(0.4f);
			paintWall = new PaintWall();

			int width = 100;
			int height = 100;
			Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			for (int col = 0; col < width; col++)
			{
				for (int row = 0; row < width; row++)
				{
					if ((Math.Abs(width/2 - col) > 5) & (Math.Abs(height/2 - row) > 5))
					{
						bitmap.SetPixel(col, row, Color.FromArgb(255, 100, 200, 55));
					}
				}
			}

			bitmapWithAlpha = new TextureElement(bitmap);
			bitmapWithAlpha.Move(new Vector3(0f, 0f, 0.5f));

			SetupDepthAndCull();
			Textures.EnableTextures();
		}
		
	 	public override void display()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			if (drawFlashlight)flashlight.Draw();
			if (drawWood) wood.Draw();
			wood.Move(-0.02f, 0.05f, 0.00f);
			if (drawPaintWall) paintWall.Draw();
			bitmapWithAlpha.Draw();
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			Vector3 movement;
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			switch (keyCode) {
			case Keys.D1:
				flashlight.Replace("flashlight.png");
				break;	
			case Keys.D2:
				flashlight.Replace("wood4_rotate.png");
				break;		
			case Keys.D3:
				GL.Enable(EnableCap.DepthTest);
				result.AppendLine("DepthTest on");
				break;				
			case Keys.D4:
				GL.Disable(EnableCap.DepthTest);
				result.AppendLine("DepthTest off");
				break;	
			case Keys.D5:
				GL.Enable(EnableCap.Blend);
				result.AppendLine("Blend on");
				break;			
			case Keys.D6:
				GL.Disable(EnableCap.Blend);
				result.AppendLine("Blend off");
				break;
			case Keys.D7:
				movement = new Vector3(0.1f, 0.1f, 0.1f);
				flashlight.Move(movement);
				result.AppendLine("newTextureElement to " + (movement + flashlight.GetOffset()).ToString());
				break;
			case Keys.D8:
				movement = new Vector3(-0.1f, -0.1f, -0.1f);
				flashlight.Move(movement);
				result.AppendLine("newTextureElement to " + (movement + flashlight.GetOffset()).ToString());
				break;
			case Keys.D9:
				movement = new Vector3(0.0f, 0.0f, 0.1f);
				paintWall.Move(movement);
				result.AppendLine("paintWall to " + (movement + paintWall.GetOffset()).ToString());
				break;
			case Keys.D0:
				movement = new Vector3(0.0f, 0.0f, -0.1f);
				paintWall.Move(movement);
				result.AppendLine("paintWall to " + (movement + paintWall.GetOffset()).ToString());
				break;

			case Keys.A:
				paintWall.PaintRandom();
				break;
			case Keys.F:
				if (drawFlashlight)
					drawFlashlight = false;
				else
					drawFlashlight = true;
				break;
			case Keys.P:
				if (drawPaintWall)
					drawPaintWall = false;
				else
					drawPaintWall = true;
				break;
			case Keys.W:
				if (drawWood)
					drawWood = false;
				else
					drawWood = true;
				break;
			}

			reshape();
			display();
			return result.ToString();
		}
	}
}

