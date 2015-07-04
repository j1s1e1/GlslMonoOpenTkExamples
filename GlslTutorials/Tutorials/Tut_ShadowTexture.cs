using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_ShadowTexture : TutorialBase
	{
		const float RENDER_WIDTH = 512.0f;
		const float RENDER_HEIGHT = 512.0f;
		const float SHADOW_MAP_RATIO = 2f;

		TextureElement test;


		//Camera position
		float[] p_camera = new float[] {32,20,0};

		//Camera lookAt
		float[] l_camera = new float[]{2,0,-10};

		//Light position
		float[] p_light = new float[]{3,20,0};

		//Light mouvement circle radius
		float light_mvnt = 30.0f;

		// Hold id of the framebuffer for light POV rendering
		int fboId;

		// Z values will be rendered to this texture when using fboId framebuffer
		int depthTextureId;

		int shadowProgram;

		string errorLocation = "";

		bool useTexture0 = false;

		bool drawTexture = false;

		int errorCount = 0;

		bool CheckForErrors()
		{
			StringBuilder result = new StringBuilder();
			bool errors = false;
			ErrorCode ec = GL.GetError();
			while (ec != ErrorCode.NoError)
			{
				errors = true;
				result.AppendLine("ec = " + ec.ToString());
					ec = GL.GetError();
			}
			if (errors)
			{
				errorCount++;
			}
			return errors;
		}

		void InitializeProgram()
		{
			shadowProgram = Programs.AddProgram(VertexShaders.ShadowMap, FragmentShaders.ShadowMap);
		}

		protected override void init()
		{
			//glutSetWindowTitle("Tut_ShadowMap");
			//glViewport(0, 0, (GLsizei) 512, (GLsizei) 512);
			InitializeProgram();
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);

			// test2
			//GL.Enable(EnableCap.Texture2D);
			//GL.Enable(EnableCap.Blend);

			// TEST
//			GL.ClearStencil(0);
//			GL.StencilMask(1);
//			GL.Enable(EnableCap.StencilTest);
//			GL.Enable(EnableCap.Texture2D);
//			GL.Enable(EnableCap.FramebufferSrgb);
//			GL.Enable(EnableCap.TextureGenS);
//			GL.Enable(EnableCap.TextureGenT);
//			GL.Enable(EnableCap.Lighting);
//			GL.Enable(EnableCap.Light0);
//			GL.Enable(EnableCap.DepthTest);
//			GL.DepthFunc(DepthFunction.Less);
//			GL.ClearDepth(1.0);
//
//			GL.Enable(EnableCap.StencilTest);
//			GL.ClearStencil(0);
//			GL.StencilMask(0xFFFFFFFF); // read&write
			// End Test

			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			generateShadowFBO();

			test = new TextureElement("wood4_rotate.png");
			test.Scale(0.8f);
			test.Move(0f, 0f, -0.2f);
			test.RotateShape(Vector3.UnitX, 45f);
			test.SetLightPosition(new Vector3(0f, 0f, -1f));
			if (CheckForErrors())
			{ 
				errorLocation = "init";
			}
		}

		public override void display()
		{
			ClearDisplay();
			if (CheckForErrors())
			{ 
				errorLocation = "display start";
			}
			update();

			//First step: Render from the light POV to a FBO, story depth values only
			GL.BindFramebuffer(FramebufferTarget.FramebufferExt,fboId);	//Rendering offscreen

			//Using the fixed pipeline to render to the depthbuffer
			GL.UseProgram(0);

			// In the case we render the shadowmap to a higher resolution, the viewport must be modified accordingly.
			GL.Viewport(0,0,(int)(RENDER_WIDTH * SHADOW_MAP_RATIO),(int)(RENDER_HEIGHT* SHADOW_MAP_RATIO));

			// Clear previous frame values
			GL.Clear(ClearBufferMask.DepthBufferBit);

			//Disable color rendering, we only want to write to the Z-Buffer
			GL.ColorMask(false, false, false, false); 

			//setupMatrices(p_light[0],p_light[1],p_light[2],l_light[0],l_light[1],l_light[2]);

			// Culling switching, rendering only backface, this is done to avoid self-shadowing
			GL.CullFace(CullFaceMode.Front);
			if (CheckForErrors())
			{ 
				errorLocation = "before first draw";
			}
			drawObjects();
			if (CheckForErrors())
			{ 
				errorLocation = "first draw";
			}

			//Save modelview/projection matrice into texture7, also add a biais
			setTextureMatrix();

			// Now rendering from the camera POV, using the FBO to generate shadows
			GL.BindFramebuffer(FramebufferTarget.FramebufferExt,0);

			GL.Viewport(0,0,(int)RENDER_WIDTH,(int)RENDER_HEIGHT);

			//Enabling color write (previously disabled for light POV z-buffer rendering)
			GL.ColorMask(true, true, true, true); 

			// Clear previous frame values
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			if (CheckForErrors())
			{ 
				errorLocation = "before shadow shader";
			}


			//Using the shadow shader
			GL.UseProgram(Programs.GetProgram(shadowProgram));

			//GL.Uniform1(shadowMapUniform, (int)TextureUnit.Texture7);
			int ShadowMapUniform = Programs.GetShadowMapUniform(shadowProgram);
			GL.Uniform1(ShadowMapUniform, 7);
			if (CheckForErrors())
			{ 
				errorLocation = "before active texture";
			}
			GL.ActiveTexture(TextureUnit.Texture7);
			GL.BindTexture(TextureTarget.Texture2D, depthTextureId);

			setupMatrices(p_camera[0],p_camera[1],p_camera[2],l_camera[0],l_camera[1],l_camera[2]);

			GL.CullFace(CullFaceMode.Back);
			if (CheckForErrors())
			{ 
				errorLocation = "before second draw";
			}
			drawObjects();

			GL.UseProgram(0);

			if (CheckForErrors())
			{ 
				errorLocation = "before test draw";
			}

			GL.BindFramebuffer(FramebufferTarget.FramebufferExt,0);	//clear again

			if (drawTexture)
			{
				GL.Enable(EnableCap.CullFace);
				GL.CullFace(CullFaceMode.Back);
				GL.FrontFace(FrontFaceDirection.Cw);

				GL.Enable(EnableCap.DepthTest);
				GL.DepthMask(true);
				GL.DepthFunc(DepthFunction.Lequal);
				GL.DepthRange(0.0f, 1.0f); 
				GL.CullFace(CullFaceMode.Back);

				GL.Enable(EnableCap.Texture2D);
				//GL.Enable(EnableCap.Blend);
				//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				GL.Enable(EnableCap.DepthTest);
				//GL.Enable(EnableCap.AlphaTest);
				//GL.AlphaFunc(AlphaFunction.Gequal, 0.01f);
				if (useTexture0) GL.ActiveTexture(TextureUnit.Texture0);
				test.Draw();
				if (CheckForErrors())
				{ 
					errorLocation = "display end";
				}
			}
		}

		private void drawObjects()
		{
			// Ground
			GL.Color4(0.3f,0.3f,0.3f,1);
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex3(-35,2,-35);
			GL.Vertex3(-35,2, 15);
			GL.Vertex3( 15,2, 15);
			GL.Vertex3( 15,2,-35);
			GL.End();

			GL.Color4(0.9f,0.9f,0.9f,1);

			// Instead of calling glTranslatef, we need a custom function that also maintain the light matrix
			startTranslate(0,4,-16);
			Glut.SolidCube(4);
			endTranslate();

			startTranslate(0,4,-5);
			Glut.SolidCube(4);
			endTranslate();

			GL.Color4(0.3f,0.3f,0.0f,1);
			startTranslate(p_light[0]/10f, p_light[1]/10f, p_light[2]/10f);
			Glut.SolidCube(0.5f);
			endTranslate();
		}

		private void drawObjects2()
		{
			// Ground
			GL.Color4(0.3f,0.3f,0.3f,1);
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex3(-35,2,-35);
			GL.Vertex3(-35,2, 15);
			GL.Vertex3( 15,2, 15);
			GL.Vertex3( 15,2,-35);
			GL.End();

//			GL.Color4(0.9f,0.9f,0.9f,1);
//
//			// Instead of calling glTranslatef, we need a custom function that also maintain the light matrix
//			startTranslate(0,4,-16);
//			Glut.SolidCube(4);
//			endTranslate();
//
//			startTranslate(0,4,-5);
//			Glut.SolidCube(4);
//			endTranslate();
		}

		private void startTranslate(float x,float y,float z)
		{
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PushMatrix();
			GL.Translate(x,y,z);

			GL.MatrixMode(MatrixMode.Texture);
			GL.ActiveTexture(TextureUnit.Texture7);
			GL.PushMatrix();
			GL.Translate(x,y,z);
		}

		private void endTranslate()
		{
			GL.MatrixMode(MatrixMode.Texture);
			GL.PopMatrix();
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PopMatrix();
		}

		private void generateShadowFBO()
		{
			int shadowMapWidth = (int)(RENDER_WIDTH * SHADOW_MAP_RATIO);
			int shadowMapHeight = (int)(RENDER_HEIGHT * SHADOW_MAP_RATIO);

			//GLfloat borderColor[4] = {0,0,0,0};

			FramebufferErrorCode FBOstatus;

			// Try to use a texture depth component
			GL.GenTextures(1, out depthTextureId);
			GL.BindTexture(TextureTarget.Texture2D, depthTextureId);


//			// added
//			GL.Ext.GenRenderbuffers(1, out depthBufferId);
//			GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, depthBufferId);
//			GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, (RenderbufferStorage)All.DepthComponent32, 512, 512);
//			GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, depthBufferId);
//			// end added

			// GL_LINEAR does not make sense for depth texture. However, next tutorial shows usage of GL_LINEAR and PCF
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);

			// Remove artefact on the edges of the shadowmap
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Clamp);
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Clamp );

			//glTexParameterfv( GL_TEXTURE_2D, GL_TEXTURE_BORDER_COLOR, borderColor );



			// No need to force GL_DEPTH_COMPONENT24, drivers usually give you the max precision if available 
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, shadowMapWidth, 
				shadowMapHeight, 0, PixelFormat.DepthComponent, PixelType.UnsignedByte, IntPtr.Zero);

			GL.BindTexture(TextureTarget.Texture2D, 0);

			// create a framebuffer object
			GL.GenFramebuffers(1, out fboId);
			GL.BindFramebuffer(FramebufferTarget.FramebufferExt, fboId);

			// Instruct openGL that we won't bind a color texture with the currently binded FBO
			GL.DrawBuffer(DrawBufferMode.None);
			GL.ReadBuffer(ReadBufferMode.None);

			// attach the texture to FBO depth attachment point
			GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, TextureTarget.Texture2D, depthTextureId, 0);

			// check FBO status
			FBOstatus = GL.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);
			if(FBOstatus != FramebufferErrorCode.FramebufferCompleteExt)
				MessageBox.Show("GL_FRAMEBUFFER_COMPLETE_EXT failed, CANNOT use FBO\n");

			// switch back to window-system-provided framebuffer
			GL.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
		}

		private void setupMatrices(float position_x,float position_y,float position_z,float lookAt_x,float lookAt_y,float lookAt_z)
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			OpenTK.Graphics.Glu.Perspective(45,RENDER_WIDTH/RENDER_HEIGHT,10,40000);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			OpenTK.Graphics.Glu.LookAt(position_x,position_y,position_z,lookAt_x,lookAt_y,lookAt_z,0,1,0);
		}

		static double[] modelView = new double[16];
		static double[] projection = new double[16];

		private void setTextureMatrix()
		{
			// This is matrix transform every coordinate x,y,z
			// x = x* 0.5 + 0.5 
			// y = y* 0.5 + 0.5 
			// z = z* 0.5 + 0.5 
			// Moving from unit cube [-1,1] to [0,1]  
			double[] bias = new double[]{	
				0.5, 0.0, 0.0, 0.0, 
				0.0, 0.5, 0.0, 0.0,
				0.0, 0.0, 0.5, 0.0,
				0.5, 0.5, 0.5, 1.0};

			// Grab modelview and transformation matrices
			GL.GetDouble(GetPName.ModelviewMatrix, modelView);
			GL.GetDouble(GetPName.ProjectionMatrix, projection);


			GL.MatrixMode(MatrixMode.Projection);
			GL.ActiveTexture(TextureUnit.Texture7);

			GL.LoadIdentity();	
			GL.LoadMatrix(bias);

			// concatating all matrice into one.
			GL.MultMatrix (projection);
			GL.MultMatrix (modelView);

			// Go back to normal matrix mode
			GL.MatrixMode(MatrixMode.Modelview);
		}

		private void update()
		{
			p_light[0] = light_mvnt * (float)Math.Cos((GetElapsedTime() % 3600)/10.0 * Math.PI / 180.0);
			p_light[2] = light_mvnt * (float)Math.Sin((GetElapsedTime() % 3600)/10.0 * Math.PI / 180.0);
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			if (displayOptions)
			{
				SetDisplayOptions(keyCode);
			}
			else {
				switch (keyCode) {
				case Keys.Enter:
					displayOptions = true;
					break;
				case Keys.D1:
					break;
				case Keys.D2:
					break;
				case Keys.D3:
					break;
				case Keys.D4:
					break;
				case Keys.D5:
					break;
				case Keys.D6:
					break;
				case Keys.D7:
					break;
				case Keys.D8:
					break;
				case Keys.D9:
					break;
				case Keys.D0:
					break;
				case Keys.A:
					break;
				case Keys.B:
					break;
				case Keys.C:
					break;
				case Keys.D:
					if (drawTexture)
					{
						drawTexture = false;
						result.AppendLine("drawTexture false");
					}
					else
					{
						drawTexture = true;
						result.AppendLine("drawTexture true");

					}
					break;		
				case Keys.E:
					result.AppendLine("errorLocation = " + errorLocation);
					break;
				case Keys.I:
					result.AppendLine("plight " + p_light[0].ToString() + " " + p_light[1].ToString() +
						" " + p_light[2].ToString());
					result.AppendLine("depthTextureId " + depthTextureId.ToString());
					result.Append(Programs.DumpShaders());
					break;
				case Keys.T:
					if (useTexture0)
					{
						useTexture0 = false;
						result.AppendLine("Texture 0 off");
					}
					else
					{
						useTexture0 = true;
						result.AppendLine("Texture 0 on");

					}
					break;
					
				}
			}
			return result.ToString();
		}
	}
}

