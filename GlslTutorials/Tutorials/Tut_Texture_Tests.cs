using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace GlslTutorials
{
	public class Tut_Texture_Tests : TutorialBase
	{
		public Tut_Texture_Tests ()
		{
		}

		int current_texture;

		protected override void init ()
		{
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			current_texture = Textures.Load("Mars_MGS_colorhillshade_mola_1024.jpg", 1);
			//current_texture = LoadTexture("hst_mars_9927e.jpg", 1);
			GL.Enable(EnableCap.Texture2D);
			//Basically enables the alpha channel to be used in the color buffer
			GL.Enable(EnableCap.Blend);
			//The operation/order to blend
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//Use for pixel depth comparing before storing in the depth buffer
			GL.Enable(EnableCap.DepthTest);
		}

		public override void display()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			Textures.DrawTexture2D(current_texture);
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			switch (keyCode)
			{
			case Keys.Escape:
				break;

			case Keys.Space:
				break;
			case Keys.D1: current_texture = Textures.Load("Mars_MGS_colorhillshade_mola_1024.jpg", 1); break;
			case Keys.D2: current_texture = Textures.Load("flashlight.png", 1); break;
			case Keys.D3: current_texture = Textures.Load("pointsoflight.png", 1); break;
			case Keys.D4: current_texture = Textures.Load("bands.png", 1); break;
			}
			return result.ToString();
		}
	}
}

