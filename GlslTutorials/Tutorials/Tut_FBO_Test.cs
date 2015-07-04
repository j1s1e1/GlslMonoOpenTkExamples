using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace GlslTutorials 
{
	public class Tut_FBO_Test: TutorialBase
	{
		int FboHandle;
		int ColorTexture;
		int DepthRenderbuffer;
		int FboWidth = 512;
		int FboHeight = 512;

		Vector3 cube1Traslate = new Vector3(100f, 100f, 0f);
		Vector3 cube1Axis = new Vector3(1f, 1f, 0f);
		float cube1Angle = 120f;

		protected override void init()
		{
			GL.Enable(EnableCap.FramebufferSrgb);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable( EnableCap.DepthTest );
			GL.ClearDepth( 1.0f );
			GL.DepthFunc( DepthFunction.Lequal );

			ColorTexture = Textures.Load("checker.png");
			// Create a FBO and attach the textures
			GL.Ext.GenFramebuffers( 1, out FboHandle );
			GL.Ext.BindFramebuffer( FramebufferTarget.FramebufferExt, FboHandle );
			GL.Ext.FramebufferTexture2D( FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, ColorTexture, 0 );
			GL.Ext.FramebufferRenderbuffer( FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, DepthRenderbuffer );

			// now GL.Ext.CheckFramebufferStatus( FramebufferTarget.FramebufferExt ) can be called, check the end of this page for a snippet.

			// since there's only 1 Color buffer attached this is not explicitly required
			GL.DrawBuffer( (DrawBufferMode)FramebufferAttachment.ColorAttachment0Ext );

			GL.PushAttrib( AttribMask.ViewportBit ); // stores GL.Viewport() parameters
			GL.Viewport( 0, 0, FboWidth, FboHeight );

			// render whatever your heart desires, when done ...
			GL.Color4(0.9f,0.0f,0.0f,1);

			Glut.SolidCube(0.5f);
			GL.Translate(cube1Traslate);
			GL.Rotate(cube1Angle, cube1Axis);
			Glut.SolidCube(50f);
			GL.Disable(EnableCap.CullFace);
			Glut.SolidCube(25f);
			GL.Enable(EnableCap.CullFace);
			GL.Rotate(-cube1Angle, cube1Axis);
			GL.Translate(-cube1Traslate);

			GL.PopAttrib( ); // restores GL.Viewport() parameters
			GL.Ext.BindFramebuffer( FramebufferTarget.FramebufferExt, 0 ); // return to visible framebuffer
			GL.DrawBuffer( DrawBufferMode.Back );

		}

		public override void display()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			DrawBuffersEnum[] bufs = new DrawBuffersEnum[2] { (DrawBuffersEnum)FramebufferAttachment.ColorAttachment0Ext, (DrawBuffersEnum)FramebufferAttachment.ColorAttachment1Ext }; // fugly, will be addressed in 0.9.2

			GL.DrawBuffers( bufs.Length, bufs );
		}
	}
}

