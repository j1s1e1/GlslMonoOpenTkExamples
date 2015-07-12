using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Dither_GameWindow : GameWindow
	{
		bool first;
		bool zero = false;
		bool one = false;
		int frameCount = 0;
		int ditherCount = 1;
		int squaresize = 16;
		bool updateTextures = false;
		Color color1 = Color.Green;
		Color color2 = Color.Blue;
		int colorsCount = 0;

		TextureElement image1;
		TextureElement image2;
		double fps;

		public Dither_GameWindow()
			: base(512, 512, new OpenTK.Graphics.GraphicsMode(16, 16), "Dither_GameWindow", GameWindowFlags.Default, 
				DisplayDevice.Default, 2, 0, OpenTK.Graphics.GraphicsContextFlags.Embedded)
		{
			VSync = VSyncMode.On;
			KeyDown += Dither_GameWindow_KeyDown;
		}

		void Dither_GameWindow_KeyDown (object sender, OpenTK.Input.KeyboardKeyEventArgs e)
		{
			switch(e.Key)
			{
			case Key.Escape:
				this.Exit();
				return;
			case Key.Up:
				ditherCount++;
				break;
			case Key.Down:
				if (ditherCount > 1) ditherCount--;
				break;
			case Key.Number0:
				zero = true;
				break;
			case Key.Number1:
				one = true;
				break;
			case Key.Number2:
				zero = false;
				one = false;
				break;
			case Key.S:
				squaresize *= 2;
				if (squaresize > 128)
					squaresize = 1;
				updateTextures = true;
				break;
			case Key.C:
				colorsCount ++;
				if (colorsCount > 9)
				{
					colorsCount = 0;
				}
				switch (colorsCount)
				{
				case 0: color1 = Color.Green; color2 = Color.Blue; break;
				case 1: color1 = Color.Red; color2 = Color.Yellow; break;
				case 2: color1 = Color.Black; color2 = Color.White; break;
				case 3: color1 = Color.Purple; color2 = Color.Green; break;
				case 4: color1 = Color.PowderBlue; color2 = Color.Pink; break;
				case 5: color1 = Color.DarkMagenta; color2 = Color.LightGreen; break;
				case 6: color1 = Color.LemonChiffon; color2 = Color.Khaki; break;
				case 7: color1 = Color.RoyalBlue; color2 = Color.Yellow; break;
				case 8: color1 = Color.White; color2 = Color.Red; break;
				case 9: color1 = Color.PaleGoldenrod; color2 = Color.Peru; break;
				}
				updateTextures = true;
				break;
			}
		}

		private void SetTextures()
		{
			image1 = new TextureElement(SampleTextures.GetBitmapSquares(squaresize, squaresize, color1, color2));
			image2 = new TextureElement(SampleTextures.GetBitmapSquares(squaresize, squaresize, color2, color1));
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			SetTextures();
			Color color; 
			if (first)
			{
				first = false;
				color = Color.MidnightBlue;
			}
			else
			{
				first = true;
				color = Color.AntiqueWhite;
			}
			GL.ClearColor(color.R, color.G, color.B, color.A);
			GL.Enable(EnableCap.DepthTest);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			fps = ((double)(1.0 / e.Time) + fps) / 2.0;
			Title = String.Format("Dither FPS {0} Dither {1} Square {2} Color {3} ", 
				RenderFrequency.ToString("##.###"), (fps/ditherCount).ToString("##.###"), squaresize, colorsCount);
			GL.ClearColor(Color.Black);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			frameCount++;
			if (zero)
			{
				first = true;
				frameCount = 0;
			}
			if (one)
			{
				first = false;
				frameCount = 0;
			}
			if (first)
			{
				if (frameCount >= ditherCount) 
				{
					frameCount = 0;
					first = false;
				}
				image1.Draw();
			}
			else
			{
				if (frameCount >= ditherCount) 
				{
					frameCount = 0;
					first = true;
				}
				image2.Draw();
			}
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GL.Finish();
			this.SwapBuffers();
			if (updateTextures)
			{
				updateTextures = false;
				SetTextures();
			}
		}
	}
}

