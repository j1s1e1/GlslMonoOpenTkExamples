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
		
		private int current_texture;

		TextureElement textureElement;
		PaintWall paintWall;
		TextureElement newTextureElement;
		
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
			textureElement = new TextureElement("wood4_rotate.png");
			textureElement.Scale(0.2f);

			newTextureElement = new TextureElement("flashlight.png");
			newTextureElement.Move(10f, 10f, 10f);
			newTextureElement.Scale(0.2f);
			paintWall = new PaintWall();
		}
		
	 	public override void display()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		    GL.MatrixMode(MatrixMode.Modelview);
		    GL.LoadIdentity();

			//Textures.DrawTexture2D(current_texture);
			textureElement.Draw();
			textureElement.Move(-0.02f, 0.05f, 0.00f);

			//newTextureElement.Draw();
			//paintWall.Draw();
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			switch (keyCode) {
			case Keys.D1:
				newTextureElement.Replace("flashlight.png");
				break;	
			case Keys.D2:
				newTextureElement.Replace("wood4_rotate.png");
				break;		
			case Keys.D3:
				break;				
			case Keys.D4:
				break;	
			case Keys.D5:
				break;			
			case Keys.D6:
				break;
			case Keys.A:
				paintWall.PaintRandom();
				break;
			}

			reshape();
			display();
			return result.ToString();
		}
	}
}

