using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_GlutTest : TutorialBase
	{
		LitMatrixBlock3 lmb3;

		Vector3 cube1Traslate = new Vector3(100f, 100f, 0f);
		Vector3 cube1Axis = new Vector3(1f, 1f, 0f);
		float cube1Angle = 120f;

		Vector3 cube2Traslate = new Vector3(300f, 300f, 0f);
		Vector3 cube2Axis = new Vector3(0f, 1f, 1f);
		float cube2Angle = 45f;

		const float RENDER_WIDTH = 512.0f;
		const float RENDER_HEIGHT = 512.0f;
		const float SHADOW_MAP_RATIO = 2f;

		protected override void init()
		{
			//glutSetWindowTitle("Tut_ShadowMap");
			//glViewport(0, 0, (GLsizei) 512, (GLsizei) 512);
			//GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.Viewport(0,0,512,512);
			lmb3 = new LitMatrixBlock3(new Vector3(0.1f, 0.1f, 0.1f), Colors.BLUE_COLOR);
		}

		public override void display()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Color4(0.9f,0.9f,0.9f,1);
			GL.CullFace(CullFaceMode.Back);
			GL.Translate(cube1Traslate);
			GL.Rotate(cube1Angle, cube1Axis);
			Glut.SolidCube(50f);
			GL.Disable(EnableCap.CullFace);
			Glut.SolidCube(25f);
			GL.Enable(EnableCap.CullFace);
			GL.Rotate(-cube1Angle, cube1Axis);
			GL.Translate(-cube1Traslate);
			//GL.CullFace(CullFaceMode.Front);
			GL.Color4(0.9f,0.0f,0.0f,1);
			GL.Translate(cube2Traslate);
			GL.Rotate(cube2Angle, cube2Axis);
			Glut.SolidCube(50f);
			GL.Disable(EnableCap.CullFace);
			Glut.SolidCube(25f);
			GL.Enable(EnableCap.CullFace);
			GL.Rotate(-cube2Angle, cube2Axis);
			GL.Translate(-cube2Traslate);
			cube1Angle++;
			cube2Angle--;
			lmb3.Draw();
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
					OpenTK.Graphics.Glu.Perspective(45,RENDER_WIDTH/RENDER_HEIGHT,10,40000);
					break;
				case Keys.D2:
					break;
				case Keys.D3:
					{
					Vector3 cameraPos = new Vector3(32.0f, 20.0f, 0f);
					Vector3 lookatPos = new Vector3(2f, 0f, -10f);
					Vector3 upDir = new Vector3(0.0f, 1.0f, 0.0f);
					OpenTK.Graphics.Glu.LookAt(cameraPos, lookatPos, upDir);
					}
					break;
				case Keys.D4:
					break;
				case Keys.D5:
					{
						Vector3 cameraPos = new Vector3(0.2f, 0.2f, -1f);
						Vector3 lookatPos = new Vector3(-0.3f, 0f, 0f);
						Vector3 upDir = new Vector3(0.0f, 1.0f, 0.0f);
						Matrix4 lookAt = Matrix4.LookAt(cameraPos,  lookatPos, upDir);
						Shape.SetCameraToClipMatrix(lookAt);
						//Shape.SetWorldToCameraMatrix(lookAt);
					}
					break;
				case Keys.D6:
					Shape.SetCameraToClipMatrix(Matrix4.Identity);
					break;
				case Keys.D7:
					{
						Vector3 cameraPos = new Vector3(0.2f, 0.2f, -1f);
						Vector3 lookatPos = new Vector3(0.3f, 0.3f, 0f);
						Vector3 upDir = new Vector3(0.0f, 1.0f, 0.0f);
						Matrix4 lookAt = Matrix4.LookAt(cameraPos,  lookatPos, upDir);
						Shape.SetCameraToClipMatrix(lookAt);
						//Shape.SetWorldToCameraMatrix(lookAt);
					}
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
					break;				
				}
			}
			return result.ToString();
		}

	}
}

