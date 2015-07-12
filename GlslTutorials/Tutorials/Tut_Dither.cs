using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Dither : TutorialBase
	{
		System.Windows.Forms.Timer watchdog;
		TextureElement image1;
		TextureElement image2;

		bool zero = false;
		bool one = false;
		bool finish = false;

		public class Worker
		{
			GameWindow gamewindow;
			// This method will be called when the thread is started. 
			public void DoWork()
			{
				gamewindow = new Dither_GameWindow();
				// locks up 
				gamewindow.Run(0.0, 0.0);
				
			}
			public void RequestStop()
			{
				_shouldStop = true;
			}
			// Volatile is used as hint to the compiler that this data 
			// member will be accessed by multiple threads. 
			private volatile bool _shouldStop;
		}

		protected override void init ()
		{
			watchdog = new System.Windows.Forms.Timer();
			watchdog.Interval = 1000;
			watchdog.Tick += Quit;
			watchdog.Enabled = true;
			watchdog.Start();
			Bitmap bitmap1 = new Bitmap(512, 512, System.Drawing.Imaging.PixelFormat.Format4bppIndexed);
			Bitmap bitmap2 = new Bitmap(512, 512, System.Drawing.Imaging.PixelFormat.Format4bppIndexed);
			byte[] bitmap1data = new byte[256 * 512];
			byte[] bitmap2data = new byte[256 * 512];
			for (int i = 0; i < 256; i++)
			{
				for (int j = 0; j < 512; j++)
				{
					bitmap1data[j * 256 + i] = (byte)(4 * (j / 128) + i / 64);
				}
			}
			FillBitmap(bitmap1, bitmap1data);
			image1 = new TextureElement(bitmap1);
			image2 = new TextureElement(bitmap2);

			Worker workerObject = new Worker();
			Thread workerThread = new Thread(workerObject.DoWork);
			workerThread.Start();
		}

		int quitCount = 0;

		private void Quit(Object sender, EventArgs e)
		{
			quitCount++;
			if (quitCount == 30)
			{
				Application.Exit();
			}
		}

		private void FillBitmap(Bitmap bitmap, byte[] rgbValues)
		{
			Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			System.Drawing.Imaging.BitmapData bmpData =
				bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
					bitmap.PixelFormat);

			IntPtr ptr = bmpData.Scan0;
			int bytes  = Math.Abs(bmpData.Stride) * bitmap.Height;
			System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
			bitmap.UnlockBits(bmpData);
		}

		int count = 0;

		bool skipDisplay = true;

		public override void display()
		{
			if (!skipDisplay)
			{
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
				if (zero) count = 0;
				if (one) count = 1;
				if (count == 0)
				{
					count = 1;
					image1.Draw();
				}
				else
				{
					count = 0;
					image2.Draw();	
				}
				if (finish) GL.Finish();
			}
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			switch (keyCode)
			{
			case Keys.D0:
				{
					zero = true;
					break;
				}
			case Keys.D1:
				{
					one = true;
					break;
				}
			case Keys.D2:
				{
					zero = false;
					one = false;
					break;
				}
			case Keys.F:
				{
					if (finish)
					{
						finish = false;
					}
					else
					{
						finish = true;
					}
					break;
				}
			}
			return result.ToString();
		}
	}
}

