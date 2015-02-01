using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_RotateTexture : TutorialBase
	{
		bool cull = true;
		static TextureElement2 wood;
		bool drawWood = true;

		float perspectiveAngle = 60f;
		float newPerspectiveAngle = 60f;

		int newProgram;

		static int numberOfLights = 2;
		LightBlock lightBlock = new LightBlock(numberOfLights);
		
		protected override void init ()
		{
		    GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			wood = new TextureElement2("wood4_rotate.png");
			wood.Scale(0.5f);
			wood.Move(0f, 0f, -0.2f);
			SetupDepthAndCull();
			Textures.EnableTextures();
			MatrixStack.rightMultiply = true;
		}

		static private void SetGlobalMatrices()
		{
			Shape.SetCameraToClipMatrix(cameraToClipMatrix);
		}

		public override void reshape()
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(perspectiveAngle, (width / (float)height), g_fzNear, g_fzFar);
		
			cameraToClipMatrix = Matrix4.Identity;
			cameraToClipMatrix.M34 = -1f;

			SetGlobalMatrices();

			GL.Viewport(0, 0, width, height);

		}
		
	 	public override void display()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			if (drawWood) wood.Draw();
			if (perspectiveAngle != newPerspectiveAngle)
			{
				perspectiveAngle = newPerspectiveAngle;
				reshape();
			}
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			switch (keyCode) {
			case Keys.D1:
				wood.RotateShape(Vector3.UnitX, 5f);
				break;	
			case Keys.D2:
				wood.RotateShape(Vector3.UnitY, 5f);
				break;		
			case Keys.D3:
				wood.RotateShape(Vector3.UnitZ, 5f);
				break;				
			case Keys.D4:
				wood.RotateShape(Vector3.UnitX, -5f);
				break;	
			case Keys.D5:
				wood.RotateShape(Vector3.UnitY, -5f);
				break;			
			case Keys.D6:
				wood.RotateShape(Vector3.UnitZ, -5f);
				break;
			case Keys.D7:
				newProgram = Programs.AddProgram(VertexShaders.MatrixTexture, FragmentShaders.MatrixTextureScale);
				wood.SetProgram(newProgram);
				break;
			case Keys.D8:
				newProgram = Programs.AddProgram(VertexShaders.MatrixTexture, FragmentShaders.TextureMultipleLightScale);
				wood.SetProgram(newProgram);
				Programs.SetUpLightBlock(newProgram, numberOfLights);
				lightBlock.ambientIntensity = new Vector4(0.1f, 0.1f, 0.1f, 1.0f);
				lightBlock.lightAttenuation = 0.1f;
				lightBlock.maxIntensity = 0.5f;
				lightBlock.lights[0].cameraSpaceLightPos = new Vector4(0.5f, 0.0f, 0.0f, 1f);
				lightBlock.lights[0].lightIntensity = new Vector4(0.0f, 0.0f, 0.6f, 1.0f);
				lightBlock.lights[1].cameraSpaceLightPos = new Vector4(0.0f, 0.5f, 1.0f, 1f);
				lightBlock.lights[1].lightIntensity = new Vector4(0.4f, 0.0f, 0.0f, 1.0f);
				Programs.UpdateLightBlock(newProgram, lightBlock);
				break;
			case Keys.D9:
				newProgram = Programs.AddProgram(VertexShaders.MatrixTexture, FragmentShaders.Test);
				wood.SetProgram(newProgram);
				break;
			case Keys.D0:
				wood.SetRotation(Matrix3.CreateFromAxisAngle(new Vector3(1f, 0f, 0f), 0f));
				break;
			case Keys.A:
				break;
			case Keys.C:
				if (cull)
				{
					cull = false;
					GL.Disable(EnableCap.CullFace);
					result.AppendLine("cull disabled");
				}
				else
				{
					cull = true;
					GL.Enable(EnableCap.CullFace);
					result.AppendLine("cull enabled");
				}
				break;
			case Keys.D:
				lightBlock.MoveLight(0, new Vector3(0.25f, 0f, 0f));
				Programs.UpdateLightBlock(newProgram, lightBlock);
				break;
			case Keys.E:
				lightBlock.MoveLight(0, new Vector3(-0.25f, 0f, 0f));
				Programs.UpdateLightBlock(newProgram, lightBlock);
				break;
			case Keys.F:
				lightBlock.MoveLight(1, new Vector3(0f, 0.25f, 0f));
				Programs.UpdateLightBlock(newProgram, lightBlock);
				break;
			case Keys.G:
				lightBlock.MoveLight(1, new Vector3(0f, -0.25f, 0f));
				Programs.UpdateLightBlock(newProgram, lightBlock);
				break;
			case Keys.P:
				newPerspectiveAngle = perspectiveAngle + 5f;
				if (newPerspectiveAngle > 120f)
				{
					newPerspectiveAngle = 30f;
				}
				break;
			case Keys.W:
				if (drawWood)
					drawWood = false;
				else
					drawWood = true;
				break;
			}
			return result.ToString();
		}
	}
}

