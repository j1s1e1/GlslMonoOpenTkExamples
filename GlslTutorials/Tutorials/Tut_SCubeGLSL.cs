using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_SCubeGLSL : TutorialBase
	{
		bool cubeLMB = true;
		Matrix3 rotation;
		bool rotateCube = false;
		bool drawGround = true;
		bool drawBack = true;
		bool shadowOffsetZero = false;

		const int GREY = 0;
		const int  RED = 1;
		const int  GREEN = 2;
		const int  BLUE	= 3;
		const int  CYAN	= 4;
		const int  MAGENTA = 5;
		const int  YELLOW = 6;
		const int  SHADOW = 7;

		int tick = -1;

		Vector4 lightPos = new Vector4(2.0f, 4.0f, 2.0f, 1.0f);
		Vector4 groundPlane = new Vector4(0.0f, 1.0f, 0.0f, 1.499f);
		Vector4 backPlane =  new Vector4(0.0f, 0.0f, 1.0f, 0.899f);

		float[][][] cube_vertexes = new float[][][]
		{
			new float[][]{
				new float[]{-1.0f, -1.0f, -1.0f, 1.0f},
				new float[]{-1.0f, -1.0f, 1.0f, 1.0f},
				new float[]{-1.0f, 1.0f, 1.0f, 1.0f},
				new float[]{-1.0f, 1.0f, -1.0f, 1.0f}},

			new float[][]{
				new float[]{1.0f, 1.0f, 1.0f, 1.0f},
				new float[]{1.0f, -1.0f, 1.0f, 1.0f},
				new float[]{1.0f, -1.0f, -1.0f, 1.0f},
				new float[]{1.0f, 1.0f, -1.0f, 1.0f}},

			new float[][]{
				new float[]{-1.0f, -1.0f, -1.0f, 1.0f},
				new float[]{1.0f, -1.0f, -1.0f, 1.0f},
				new float[]{1.0f, -1.0f, 1.0f, 1.0f},
				new float[]{-1.0f, -1.0f, 1.0f, 1.0f}},

			new float[][]{
				new float[]{1.0f, 1.0f, 1.0f, 1.0f},
				new float[]{1.0f, 1.0f, -1.0f, 1.0f},
				new float[]{-1.0f, 1.0f, -1.0f, 1.0f},
				new float[]{-1.0f, 1.0f, 1.0f, 1.0f}},

			new float[][]{
				new float[]{-1.0f, -1.0f, -1.0f, 1.0f},
				new float[]{-1.0f, 1.0f, -1.0f, 1.0f},
				new float[]{1.0f, 1.0f, -1.0f, 1.0f},
				new float[]{1.0f, -1.0f, -1.0f, 1.0f}},

			new float[][]{
				new float[]{1.0f, 1.0f, 1.0f, 1.0f},
				new float[]{-1.0f, 1.0f, 1.0f, 1.0f},
				new float[]{-1.0f, -1.0f, 1.0f, 1.0f},
				new float[]{1.0f, -1.0f, 1.0f, 1.0f}}
		};

		float[][] cube_normals = new float[][]
		{
			new float[]{-1.0f, 0.0f, 0.0f, 0.0f},
			new float[]{1.0f, 0.0f, 0.0f, 0.0f},
			new float[]{0.0f, -1.0f, 0.0f, 0.0f},
			new float[]{0.0f, 1.0f, 0.0f, 0.0f},
			new float[]{0.0f, 0.0f, -1.0f, 0.0f},
			new float[]{0.0f, 0.0f, 1.0f, 0.0f}
		};

		int sCubeProgram;

		bool matrixesAreDifferent = false;

		LitMatrixBlock3 lmb3;

		float lmb3Offset = -2;
		float shadowOffset = -2f;
		int offsetUnif;

		LitMatrixBlock3 backLMB;
		LitMatrixBlock3 groundLMB;

		Matrix4 perspectiveFOV;

		protected override void init()
		{
			float extra = -2.0f;
			//groundPlane.Z = groundPlane.Z + extra;
			//backPlane.Z = backPlane.Z + extra;
			//lightPos.Z = lightPos.Z + extra;
			sCubeProgram = Programs.AddProgram(VertexShaders.sCube, FragmentShaders.sCube);
			lmb3 = new LitMatrixBlock3(new Vector3(1.0f, 1.0f, 1.0f), Colors.BLUE_COLOR);
			lmb3.SetProgram(sCubeProgram);

			backLMB = new LitMatrixBlock3(new Vector3(0.666f, 0.666f, 0.2f), Colors.YELLOW_COLOR);
			backLMB.SetProgram(sCubeProgram);

			groundLMB = new LitMatrixBlock3(new Vector3(0.666f, 0.2f, 0.666f), Colors.YELLOW_COLOR);
			groundLMB.SetProgram(sCubeProgram);

			offsetUnif = GL.GetUniformLocation(Programs.GetProgram(sCubeProgram), "offset");
			SetOffset(lmb3Offset);

			/* setup context */
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Frustum(-1.0, 1.0, -1.0, 1.0, 1.0, 3.0);
			float[] projectionMatrixf = new float[16];
			GL.GetFloat(GetPName.ProjectionMatrix, projectionMatrixf);

			perspectiveFOV = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI/2), width/height, 1f, 3f);
			//Matrix4 perspectiveOffCenter = Matrix4.CreatePerspectiveOffCenter(-1.0f, 1.0f, -1.0f, 1.0f, 1f, 3f);

			Shape.SetCameraToClipMatrix(perspectiveFOV);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.Translate(0.0, 0.0, -2.0);

			GL.Enable(EnableCap.DepthTest);


			GL.Enable(EnableCap.Normalize);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			GL.ShadeModel(ShadingModel.Smooth);

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
			GL.ClearIndex(0);
			GL.ClearDepth(1);
		}

		private void SetOffset(float offset)
		{
			int program = Programs.GetProgram(sCubeProgram);
			GL.UseProgram(program);
			GL.Uniform4(offsetUnif, 0f, 0f, offset, 0f);
			GL.UseProgram(0);
		}

		private void DrawGround()
		{
			if (cubeLMB)
			{
				for (int x = 0; x < 6; x++)
				{
					for (int z = 0; z < 6; z++)
					{
						groundLMB.SetOffset(new Vector3(-2f + 0.666f/2f + x * 0.666f,  -1.6f, 
														-2f + 0.666f/2f + z * 0.666f));
						if (((x + z) % 2) == 0)
						{
							groundLMB.SetColor(Colors.YELLOW_COLOR);
						}
						else
						{
							groundLMB.SetColor(Colors.BLUE_COLOR);
						}
						groundLMB.Draw();
					}
				}
			}
			else
			{
				GL.PushMatrix();
				Matrix4 testMatrix1 = Matrix4.Identity;
				Matrix4 translate = Matrix4.CreateTranslation(new Vector3(0.0f, -1.5f, 0.0f));
				Matrix4 rotate = Matrix4.CreateRotationX((float)(-Math.PI/2.0));
				Matrix4 scale = Matrix4.CreateScale(2.0f);
				testMatrix1 = Matrix4.Mult(translate, testMatrix1);
				testMatrix1 = Matrix4.Mult(rotate, testMatrix1);
				testMatrix1 =  Matrix4.Mult(scale, testMatrix1);

				GL.MultMatrix(ref testMatrix1);

				drawCheck(6, 6, BLUE, YELLOW);  /* draw ground */
				GL.PopMatrix();
			}
		}

		private void DrawBack()
		{
			if (cubeLMB)
			{
				for (int x = 0; x < 6; x++)
				{
					for (int y = 0; y < 6; y++)
					{
						backLMB.SetOffset(new Vector3(-2f + 0.666f/2f + x * 0.666f, 
							-2f + 0.666f/2f + y * 0.666f, -1f));
						if (((x + y) % 2) == 1)
						{
							backLMB.SetColor(Colors.YELLOW_COLOR);
						}
						else
						{
							backLMB.SetColor(Colors.BLUE_COLOR);
						}
						backLMB.Draw();
					}
				}
			}
			else
			{
				GL.PushMatrix();
				GL.Translate(0.0, 0.0, -0.9);
				GL.Scale(2.0, 2.0, 2.0);
				drawCheck(6, 6, BLUE, YELLOW);  /* draw back */
				GL.PopMatrix();
			}
		}

		private Matrix4 CalculateCubeXform()
		{
			float speed = 0.1f;
			Matrix4 cubeXformMatrix = new Matrix4();
			Matrix4 translate2 = Matrix4.CreateTranslation(new Vector3(0.0f, 0.2f, 0.0f));
			Matrix4 scaleMatrix = Matrix4.CreateScale(new Vector3(0.3f, 0.3f, 0.3f));
			rotation = Matrix3.Identity;
			rotation = Matrix3.Mult(Matrix3.CreateFromAxisAngle(Vector3.UnitX,(360.0f / (30f * 1) * (float)Math.PI/180f) * tick * speed), rotation);
			rotation = Matrix3.Mult(Matrix3.CreateFromAxisAngle(Vector3.UnitY,(360.0f / (30f * 2) * (float)Math.PI/180f) * tick * speed), rotation);
			rotation = Matrix3.Mult(Matrix3.CreateFromAxisAngle(Vector3.UnitZ,(360.0f / (30f * 4) * (float)Math.PI/180f) * tick * speed), rotation);
			rotation.Normalize();
			Matrix4 rotation4 = Matrix4.Identity;
			rotation4.Row0 = new Vector4(rotation.Row0, 0f);
			rotation4.Row1 = new Vector4(rotation.Row1, 0f);
			rotation4.Row2 = new Vector4(rotation.Row2, 0f);
			cubeXformMatrix = Matrix4.Mult(scaleMatrix, translate2);
			cubeXformMatrix = Matrix4.Mult(rotation4, cubeXformMatrix);
			scaleMatrix = Matrix4.CreateScale(new Vector3(1f, 2f, 1f));

			cubeXformMatrix = Matrix4.Mult(scaleMatrix, cubeXformMatrix);
			if (cubeLMB)
			{
				Matrix4 scale = Matrix4.CreateScale(2.0f);
				cubeXformMatrix = Matrix4.Mult(scale, cubeXformMatrix);
			}
			return cubeXformMatrix;
		}

		private void DrawMainBlock(Matrix4 cubeXform)
		{
			if (cubeLMB)
			{
				lmb3.SetColor(Colors.RED_COLOR);
				lmb3.modelToWorld = cubeXform;
				lmb3.Draw();
			}
			else
			{
				GL.PushMatrix();
				GL.MultMatrix(ref cubeXform);
				drawCube(RED);        /* draw cube */
				GL.PopMatrix();
			}
		}

		private void DrawGroundShadow(Matrix4 cubeXform)
		{
			Matrix4 shadowMat = myShadowMatrix(groundPlane, lightPos);
			//Matrix4 scale = Matrix4.CreateScale(0.5f);
			//Matrix4 cubeXformMatrix = Matrix4.Mult(scale, cubeXform);
			Matrix4 modelToWorld = Matrix4.Mult(cubeXform, shadowMat);
			if (cubeLMB)
			{
				lmb3.SetColor(Colors.SHADOW_COLOR);
				lmb3.modelToWorld = modelToWorld;
				SetOffset(shadowOffset);
				lmb3.Draw();
				SetOffset(lmb3Offset);
			}
			else
			{
				GL.PushMatrix();

				GL.MultMatrix(ref modelToWorld);
				drawCube(SHADOW);      /* draw ground shadow */
				GL.PopMatrix();
			}
		}

		private void DrawBackShadow(Matrix4 cubeXform)
		{
			Matrix4 shadowMat = myShadowMatrix(backPlane, lightPos);
			Matrix4 modelToWorld = Matrix4.Mult(cubeXform, shadowMat);
			if (cubeLMB)
			{
				lmb3.SetColor(Colors.SHADOW_COLOR);
				lmb3.modelToWorld = modelToWorld;
				SetOffset(shadowOffset);
				lmb3.Draw();
				SetOffset(lmb3Offset);
			}
			else
			{
				GL.PushMatrix();
				GL.MultMatrix(ref modelToWorld);
				drawCube(SHADOW);      /* draw back shadow */
				GL.PopMatrix();
			}
		}

		public override void display()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			if (drawGround) DrawGround();
			if (drawBack) DrawBack();
			Matrix4 cubeXform = CalculateCubeXform();

			DrawMainBlock(cubeXform);

			GL.DepthMask(false);

			GL.Enable(EnableCap.Blend);
			if (shadowOffsetZero) SetOffset(0f);
			if (drawGround) DrawGroundShadow(cubeXform);
			if (drawBack) DrawBackShadow(cubeXform);
			if (shadowOffsetZero) SetOffset(lmb3Offset);

			GL.DepthMask(true);

			GL.Disable(EnableCap.Blend);
			if (rotateCube) tick++;
			if (tick >= 1200) {
				tick = 0;
			}
		}


		void setColor(int c)
		{
			switch (c)
			{
			case GREY: GL.Color4(Colors.GREY_COLOR); break;
			case RED: GL.Color4(Colors.RED_COLOR); break;
			case GREEN: GL.Color4(Colors.GREEN_COLOR); break;
			case BLUE: GL.Color4(Colors.BLUE_COLOR); break;
			case CYAN: GL.Color4(Colors.CYAN_COLOR); break;
			case MAGENTA: GL.Color4(Colors.MAGENTA_COLOR); break;
			case YELLOW: GL.Color4(Colors.YELLOW_COLOR); break;
			case SHADOW: GL.Color4(Colors.SHADOW_COLOR); break;
			}
		}

		void drawCube(int color)
		{
			int i;

			setColor(color);

			for (i = 0; i < 6; ++i) {
				//GL.Normal3(cube_normals[i]);
				GL.Begin(PrimitiveType.Polygon);
				GL.Vertex4(cube_vertexes[i][0]);
				GL.Vertex4(cube_vertexes[i][1]);
				GL.Vertex4(cube_vertexes[i][2]);
				GL.Vertex4(cube_vertexes[i][3]);
				GL.End();
			}
		}

		int initialized = 0;
		int checklist = 0;

		void drawCheck(int w, int h, int evenColor, int oddColor)
		{
			if (initialized == 0) {
				float[] square_normal = new float[4]{0.0f, 0.0f, 1.0f, 0.0f};
				float[][] square = new float[4][];
				for (int k = 0; k < square.Length; k++)
				{
					square[k] = new float[4];
				}
				int i, j;

				if (checklist == 0) {
					checklist = GL.GenLists(1);
				}
				GL.NewList(checklist, ListMode.CompileAndExecute);

				GL.Normal3(square_normal);
				GL.Begin(PrimitiveType.Quads);
				for (j = 0; j < h; ++j) {
					for (i = 0; i < w; ++i) {
						square[0][0] = -1.0f + 2.0f / w * i;
						square[0][1] = -1.0f + 2.0f / h * (j + 1);
						square[0][2] = 0.0f;
						square[0][3] = 1.0f;

						square[1][0] = -1.0f + 2.0f / w * i;
						square[1][1] = -1.0f + 2.0f / h * j;
						square[1][2] = 0.0f;
						square[1][3] = 1.0f;

						square[2][0] = -1.0f + 2.0f / w * (i + 1);
						square[2][1] = -1.0f + 2.0f / h * j;
						square[2][2] = 0.0f;
						square[2][3] = 1.0f;

						square[3][0] = -1.0f + 2.0f / w * (i + 1);
						square[3][1] = -1.0f + 2.0f / h * (j + 1);
						square[3][2] = 0.0f;
						square[3][3] = 1.0f;

						if ((i & 1 ^ j & 1) != 0) {
							setColor(oddColor);
						} else {
							setColor(evenColor);
						}
							
						GL.Vertex4(square[0]);
						GL.Vertex4(square[1]);
						GL.Vertex4(square[2]);
						GL.Vertex4(square[3]);
					}
				}
					
				GL.End();
				GL.EndList();

				initialized = 1;
			} else {
				GL.CallList(checklist);
			}
		}

		Matrix4 myShadowMatrix(Vector4 groundVector, Vector4 lightVector)
		{
			
			float dot;
			Matrix4 shadowMat = Matrix4.Identity;

			dot = Vector4.Dot(groundVector, lightVector);
			shadowMat = shadowMat * dot;
			shadowMat.Row0 = shadowMat.Row0 - lightVector.X * groundVector;
			shadowMat.Row1 = shadowMat.Row1 - lightVector.Y * groundVector;
			shadowMat.Row2 = shadowMat.Row2 - lightVector.Z * groundVector;
			shadowMat.Row3 = shadowMat.Row3 - lightVector.W * groundVector;

			shadowMat.Transpose();
			return shadowMat;
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
					if (drawBack)
					{ 
						drawBack = false;
					}
					else
					{
						drawBack = true;
					}
					break;
				case Keys.C:
					if (cubeLMB)
					{ 
						result.AppendLine("cubeLMB = false");
						cubeLMB = false;
					}
					else
					{
						result.AppendLine("cubeLMB = true");
						cubeLMB = true;
					}
					break;
				case Keys.G:
					if (drawGround)
					{ 
						drawGround = false;
					}
					else
					{
						drawGround = true;
					}
					break;
				case Keys.I:
					result.AppendLine("rotation = " + rotation.ToString());
					result.AppendLine("lmb3.modelToWorld = " + lmb3.modelToWorld.ToString());
					result.AppendLine("matrixesAreDifferent = " + matrixesAreDifferent.ToString());
					result.AppendLine("myShadowMatrix(backPlane, lightPos) = " + myShadowMatrix(backPlane, lightPos).ToString());
					result.AppendLine(Programs.DumpShaders());
					break;
				case Keys.K:
					lightPos[3] = lightPos[3] - 0.1f;
					result.AppendLine("lightPos[3] = " + lightPos[3].ToString());
					break;
				case Keys.L:
					lightPos[3] = lightPos[3] + 0.1f;
					result.AppendLine("lightPos[3] = " + lightPos[3].ToString());
					break;
				case Keys.M:
					{
						lmb3Offset -= 0.1f;
						SetOffset(lmb3Offset);
						result.AppendLine("lmb3Offset = " + lmb3Offset);
					}
					break;
				case Keys.P:
					{
						lmb3Offset += 0.1f;
						SetOffset(lmb3Offset);
						result.AppendLine("lmb3Offset = " + lmb3Offset);
					}
					break;
				case Keys.Q:
					tick += 10;
					break;
				case Keys.R:
					if (rotateCube)
					{ 
						rotateCube = false;
					}
					else
					{
						rotateCube = true;
					}
					break;
				case Keys.S:
					shadowOffset += -0.1f;
					if (shadowOffset < -4f) shadowOffset = 4f;
					result.AppendLine("shadowOffset = " + shadowOffset.ToString());
					break;
				case Keys.Z:
					if (shadowOffsetZero)
					{ 
						shadowOffsetZero = false;
					}
					else
					{
						shadowOffsetZero = true;
					}
					break;

				}
			}
			return result.ToString();
		}
	}
}

