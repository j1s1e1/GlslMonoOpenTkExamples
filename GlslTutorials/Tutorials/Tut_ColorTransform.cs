using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_ColorTransform : TutorialBase
	{
		TextureElement image;

		float[] colors;

		bool useColors = true;
		int colorTransformProgram;
		int standardProgram;

		protected override void init ()
		{
			colors = new float[16 * 4]
			{
				1f, 0f, 0f, 1f,
				0f, 1f, 0f, 1f,
				0f, 0f, 1f, 1f,
				1f, 1f, 0f, 1f,
				1f, 0f, 1f, 1f,
				0f, 1f, 1f, 1f,
				0f, 0f, 0f, 1f,
				1f, 1f, 1f, 1f,
				0.5f, 0f, 0.5f, 1f,
				0.5f, 0.5f, 0f, 1f,
				0f, 0.5f, 0.5f, 1f,
				0.5f, 0.5f, 0.5f, 1f,
				0.5f, 0.333f, 0f, 1f,
				0f, 0.5f, 0.333f, 1f,
				0.333f, 0f, 0.5f, 1f,
				0.333f, 0.333f, 0.333f, 1f,
			};
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			image = new TextureElement("colors.png");
			standardProgram = image.GetProgram();
			colorTransformProgram = Programs.AddProgram(VertexShaders.MatrixTexture, FragmentShaders.ColorSwapTexture);
			image.SetProgram(colorTransformProgram);
			GL.UseProgram(Programs.GetProgram(colorTransformProgram));
			int COLOR_MASKS_location =  GL.GetUniformLocation(Programs.GetProgram(colorTransformProgram), "COLOR_MASKS");
			GL.Uniform4(COLOR_MASKS_location, 16, colors);
			GL.UseProgram(0);
			SetupDepthAndCull();
			Textures.EnableTextures();
		}

		public override void display()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			image.Draw();
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			switch (keyCode)
			{
				case Keys.P:
				{
					if (useColors)
					{
						image.SetProgram(standardProgram);
						useColors = false;
					}
					else
					{
						image.SetProgram(colorTransformProgram);
						useColors = true;
					}
					break;
				}
				
			}
			return result.ToString();
		}
	}
}

