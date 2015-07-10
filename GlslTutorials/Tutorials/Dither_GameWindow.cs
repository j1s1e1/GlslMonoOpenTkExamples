using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Dither_GameWindow : GameWindow
	{
		bool first;
		public Dither_GameWindow()
			: base(512, 512, new OpenTK.Graphics.GraphicsMode(16, 16), "Dither_GameWindow", GameWindowFlags.Default, 
				DisplayDevice.Default, 2, 0, OpenTK.Graphics.GraphicsContextFlags.Embedded)
		{ }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

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
	}
}

