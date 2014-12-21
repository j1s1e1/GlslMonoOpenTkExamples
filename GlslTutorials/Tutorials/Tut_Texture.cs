using System;
using System.Drawing;
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
	}
}

