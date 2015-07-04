using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_SCubeGLSL : TutorialBase
	{
		bool cubeLMB = false;
		Matrix3 rotation;
		bool rotateCube = true;
		Vector3 lmb3Scale = new Vector3(0.3f, 0.6f, 0.3f);

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

		protected override void init()
		{
			sCubeProgram = Programs.AddProgram(VertexShaders.sCube, FragmentShaders.sCube);
			lmb3 = new LitMatrixBlock3(new Vector3(1.0f, 1.0f, 1.0f), Colors.BLUE_COLOR);
			lmb3.Scale(lmb3Scale);
			lmb3.SetProgram(sCubeProgram);
			/* setup context */
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Frustum(-1.0, 1.0, -1.0, 1.0, 1.0, 3.0);
			float[] projectionMatrixf = new float[16];
			GL.GetFloat(GetPName.ProjectionMatrix, projectionMatrixf);

			Matrix4 perspectiveOffCenter = Matrix4.CreatePerspectiveOffCenter(-1.0f, 1.0f, -1.0f, 1.0f, 1f, 3f);

			Shape.SetCameraToClipMatrix(perspectiveOffCenter);

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

		private void DrawGround()
		{
			GL.PushMatrix();
			{
				Matrix4 testMatrix1 = Matrix4.Identity;
				Matrix4 translate = Matrix4.CreateTranslation(new Vector3(0.0f, -1.5f, 0.0f));
				Matrix4 rotate = Matrix4.CreateRotationX((float)(-Math.PI/2.0));
				Matrix4 scale = Matrix4.CreateScale(2.0f);
				testMatrix1 = Matrix4.Mult(translate, testMatrix1);
				testMatrix1 = Matrix4.Mult(rotate, testMatrix1);
				testMatrix1 =  Matrix4.Mult(scale, testMatrix1);

				GL.MultMatrix(ref testMatrix1);

				drawCheck(6, 6, BLUE, YELLOW);  /* draw ground */
			}
			GL.PopMatrix();
		}

		private void DrawBack()
		{
			GL.PushMatrix();
			GL.Translate(0.0, 0.0, -0.9);
			GL.Scale(2.0, 2.0, 2.0);

			drawCheck(6, 6, BLUE, YELLOW);  /* draw back */
			GL.PopMatrix();
		}

		private float[] CalculateCubeXform()
		{
			float[] cubeXform = new float[16];
			Matrix4 cubeXformMatrix = new Matrix4();
			Matrix4 translate2 = Matrix4.CreateTranslation(new Vector3(0.0f, 0.2f, 0.0f));
			Matrix4 scaleMatrix = Matrix4.CreateScale(new Vector3(0.3f, 0.3f, 0.3f));
			rotation = Matrix3.Identity;
			rotation = Matrix3.Mult(Matrix3.CreateFromAxisAngle(Vector3.UnitX,(360.0f / (30f * 1) * (float)Math.PI/180f) * tick), rotation);
			rotation = Matrix3.Mult(Matrix3.CreateFromAxisAngle(Vector3.UnitY,(360.0f / (30f * 2) * (float)Math.PI/180f) * tick), rotation);
			rotation = Matrix3.Mult(Matrix3.CreateFromAxisAngle(Vector3.UnitZ,(360.0f / (30f * 4) * (float)Math.PI/180f) * tick), rotation);
			rotation.Normalize();
			Matrix4 rotation4 = Matrix4.Identity;
			rotation4.Row0 = new Vector4(rotation.Row0, 0f);
			rotation4.Row1 = new Vector4(rotation.Row1, 0f);
			rotation4.Row2 = new Vector4(rotation.Row2, 0f);
			cubeXformMatrix = Matrix4.Mult(scaleMatrix, translate2);
			cubeXformMatrix = Matrix4.Mult(rotation4, cubeXformMatrix);
			scaleMatrix = Matrix4.CreateScale(new Vector3(1f, 2f, 1f));

			cubeXformMatrix = Matrix4.Mult(scaleMatrix, cubeXformMatrix);
			cubeXform = AnalysisTools.CreateFromMatrix(cubeXformMatrix);
			return cubeXform;
		}

		private void DrawMainBlock(float[] cubeXform)
		{
			Matrix4  cubeXformMatrix;
			GL.PushMatrix();
			GL.MultMatrix(cubeXform);

			if (cubeLMB)
			{
				//				if (rotateCube)
				//				{
				//					rotation = Matrix3.Identity;
				//					rotation = Matrix3.Mult(rotation, Matrix3.CreateFromAxisAngle(Vector3.UnitX,(360.0f / (30f * 1) * (float)Math.PI/180f) * tick));
				//					rotation = Matrix3.Mult(rotation, Matrix3.CreateFromAxisAngle(Vector3.UnitY,(360.0f / (30f * 2) * (float)Math.PI/180f) * tick));
				//					rotation = Matrix3.Mult(rotation, Matrix3.CreateFromAxisAngle(Vector3.UnitZ,(360.0f / (30f * 4) * (float)Math.PI/180f) * tick));
				//					rotation.Normalize();
				//					lmb3.SetRotation(rotation);
				//					lmb3.Scale(lmb3Scale);
				//				}
				cubeXformMatrix = Matrix4.Identity;
				Matrix4 translate2 = Matrix4.CreateTranslation(new Vector3(0.0f, 0.2f, 0.0f));
				Matrix4 scaleMatrix = Matrix4.CreateScale(new Vector3(0.3f, 0.3f, 0.3f));
				rotation = Matrix3.Identity;
				rotation = Matrix3.Mult(Matrix3.CreateFromAxisAngle(Vector3.UnitX,(360.0f / (30f * 1) * (float)Math.PI/180f) * tick), rotation);
				rotation = Matrix3.Mult(Matrix3.CreateFromAxisAngle(Vector3.UnitY,(360.0f / (30f * 2) * (float)Math.PI/180f) * tick), rotation);
				rotation = Matrix3.Mult(Matrix3.CreateFromAxisAngle(Vector3.UnitZ,(360.0f / (30f * 4) * (float)Math.PI/180f) * tick), rotation);
				rotation.Normalize();
				Matrix4 rotation4 = Matrix4.Identity;
				rotation4.Row0 = new Vector4(rotation.Row0, 0f);
				rotation4.Row1 = new Vector4(rotation.Row1, 0f);
				rotation4.Row2 = new Vector4(rotation.Row2, 0f);
				cubeXformMatrix = Matrix4.Mult(scaleMatrix, translate2);
				cubeXformMatrix = Matrix4.Mult(rotation4, cubeXformMatrix);
				scaleMatrix = Matrix4.CreateScale(new Vector3(1f, 2f, 1f));

				cubeXformMatrix = Matrix4.Mult(scaleMatrix, cubeXformMatrix);
				Programs.SetModelToCameraMatrix(sCubeProgram, cubeXformMatrix);
				lmb3.Draw();
			}
			else
			{
				drawCube(RED);        /* draw cube */
			}

			GL.PopMatrix();
		}

		public override void display()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			DrawGround();
			DrawBack();
			float[] cubeXform = CalculateCubeXform();

			DrawMainBlock(cubeXform);

			GL.DepthMask(false);

			GL.Enable(EnableCap.Blend);

			GL.PushMatrix();
			//myShadowMatrix(groundPlane, lightPos);
			Matrix4 change = myShadowMatrix(groundPlane, lightPos);

			if (cubeLMB)
			{
				if (rotateCube)
				{
					rotation = Matrix3.Identity;
					rotation = Matrix3.Mult(rotation, Matrix3.CreateFromAxisAngle(Vector3.UnitX,(360.0f / (30f * 1) * (float)Math.PI/180f) * tick));
					rotation = Matrix3.Mult(rotation, Matrix3.CreateFromAxisAngle(Vector3.UnitY,(360.0f / (30f * 2) * (float)Math.PI/180f) * tick));
					rotation = Matrix3.Mult(rotation, Matrix3.CreateFromAxisAngle(Vector3.UnitZ,(360.0f / (30f * 4) * (float)Math.PI/180f) * tick));
					rotation.Normalize();
					lmb3.SetRotation(rotation);
					lmb3.Scale(lmb3Scale);
				}
				Matrix4 remember = lmb3.modelToWorld;
				lmb3.SetColor(Colors.SHADOW_COLOR);
				lmb3.modelToWorld = Matrix4.Mult(change, lmb3.modelToWorld);
				//lmb3.SetProgram(sCubeProgram);
				lmb3.Draw();
				//lmb3.SetProgram(lmbProgram);
				lmb3.modelToWorld = remember;
				lmb3.SetColor(Colors.BLUE_COLOR);

//				Matrix4 remember = Shape.worldToCamera;
//				Shape.worldToCamera = Matrix4.Mult(remember, change);
//				lmb3.Draw();
//				Shape.worldToCamera = remember;
			}
			else
			{
				GL.MultMatrix(cubeXform);
				drawCube(SHADOW);      /* draw ground shadow */
			}

			GL.PopMatrix();

			GL.PushMatrix();
			myShadowMatrix(backPlane, lightPos);
			GL.MultMatrix(cubeXform);

			drawCube(SHADOW);      /* draw back shadow */
			GL.PopMatrix();

			GL.DepthMask(true);

			GL.Disable(EnableCap.Blend);
			if (rotateCube) tick++;
			if (tick >= 120) {
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
				GL.Normal3(cube_normals[i]);
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
			GL.MultMatrix(ref shadowMat);
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
					break;
				case Keys.C:
					if (cubeLMB)
					{ 
						cubeLMB = false;
					}
					else
					{
						cubeLMB = true;
					}
					break;
				case Keys.I:
					result.AppendLine("rotation = " + rotation.ToString());
					result.AppendLine("lmb3.modelToWorld = " + lmb3.modelToWorld.ToString());
					result.AppendLine("matrixesAreDifferent = " + matrixesAreDifferent.ToString());
					result.AppendLine(Programs.DumpShaders());
					break;
				case Keys.M:
					{
						lmb3Offset -= 0.1f;
						int program = Programs.GetProgram(sCubeProgram);
						GL.UseProgram(program);
						int offsetUnif = GL.GetUniformLocation(program, "offset");
						GL.Uniform4(offsetUnif, new Vector4(0f, 0f, lmb3Offset, 0f));
						result.AppendLine("lmb3Offset = " + lmb3Offset);
						GL.UseProgram(0);
					}
					break;
				case Keys.P:
					{
						lmb3Offset += 0.1f;
						int program = Programs.GetProgram(sCubeProgram);
						GL.UseProgram(program);
						int offsetUnif = GL.GetUniformLocation(program, "offset");
						GL.Uniform4(offsetUnif, new Vector4(0f, 0f, lmb3Offset, 0f));
						result.AppendLine("lmb3Offset = " + lmb3Offset);
						GL.UseProgram(0);
					}
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
				case Keys.T:
					break;

				}
			}
			return result.ToString();
		}
	}
}

