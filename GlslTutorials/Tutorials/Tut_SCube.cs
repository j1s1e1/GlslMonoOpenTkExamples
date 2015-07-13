using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_SCube : TutorialBase
	{
		const int GREY = 0;
		const int  RED = 1;
		const int  GREEN = 2;
		const int  BLUE	= 3;
		const int  CYAN	= 4;
		const int  MAGENTA = 5;
		const int  YELLOW = 6;
		const int  BLACK = 7;

		int useLighting = 1;
		int useQuads = 1;

		int tick = -1;

		float[][] materialColor = new float[][]{
			new float[]{0.8f, 0.8f, 0.8f, 1.0f},
			new float[]{0.8f, 0.0f, 0.0f, 1.0f},
			new float[]{0.0f, 0.8f, 0.0f, 1.0f},
			new float[]{0.0f, 0.0f, 0.8f, 1.0f},
			new float[]{0.0f, 0.8f, 0.8f, 1.0f},
			new float[]{0.8f, 0.0f, 0.8f, 1.0f},
			new float[]{0.8f, 0.8f, 0.0f, 1.0f},
			new float[]{0.0f, 0.0f, 0.0f, 0.6f},
		};

		float[] lightPos = new float[]{2.0f, 4.0f, 2.0f, 1.0f};
		float[] groundPlane = new float[]{0.0f, 1.0f, 0.0f, 1.499f};
		float[] backPlane = new float[]{0.0f, 0.0f, 1.0f, 0.899f};

		byte[] shadowPattern = new byte[]
		{
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,  /* 50% Grey */
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55,
			0xaa, 0xaa, 0xaa, 0xaa, 0x55, 0x55, 0x55, 0x55
		};

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

		bool rotateCube = false;

		protected override void init()
		{
			/* setup context */
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Frustum(-1.0, 1.0, -1.0, 1.0, 1.0, 3.0);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.Translate(0.0, 0.0, -2.0);

			GL.Enable(EnableCap.DepthTest);


			GL.Enable(EnableCap.Normalize);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			GL.ShadeModel(ShadingModel.Smooth);

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			GL.PolygonStipple(shadowPattern);

			GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
			GL.ClearIndex(0);
			GL.ClearDepth(1);
		}

		public override void display()
		{
			float[] cubeXform = new float[16];

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.PushMatrix();
			GL.Translate(0.0f, -1.5f, 0.0f);
			GL.Rotate(-90.0f, 1.0f, 0.0f, 0.0f);
			GL.Scale(2.0f, 2.0f, 2.0f);

			drawCheck(6, 6, BLUE, YELLOW);  /* draw ground */
			GL.PopMatrix();

			GL.PushMatrix();
			GL.Translate(0.0, 0.0, -0.9);
			GL.Scale(2.0, 2.0, 2.0);

			drawCheck(6, 6, BLUE, YELLOW);  /* draw back */
			GL.PopMatrix();

			GL.PushMatrix();
			GL.Translate(0.0, 0.2, 0.0);
			GL.Scale(0.3, 0.3, 0.3);
			GL.Rotate((360.0 / (30 * 1)) * tick / 10.0, 1, 0, 0);
			GL.Rotate((360.0 / (30 * 2)) * tick / 10.0, 0, 1, 0);
			GL.Rotate((360.0 / (30 * 4)) * tick / 10.0, 0, 0, 1);
			GL.Scale(1.0, 2.0, 1.0);
			GL.GetFloat(GetPName.ModelviewMatrix, cubeXform);

			drawCube(RED);        /* draw cube */
			GL.PopMatrix();

			GL.DepthMask(false);

			GL.Enable(EnableCap.Blend);

			GL.PushMatrix();
			myShadowMatrix(groundPlane, lightPos);
			GL.Translate(0.0f, 0.0f, 2.0f);
			GL.MultMatrix(cubeXform);

			drawCube(BLACK);      /* draw ground shadow */
			GL.PopMatrix();

			GL.PushMatrix();
			myShadowMatrix(backPlane, lightPos);
			GL.Translate(0.0, 0.0, 2.0);
			GL.MultMatrix(cubeXform);

			drawCube(BLACK);      /* draw back shadow */
			GL.PopMatrix();

			GL.DepthMask(true);

			GL.Disable(EnableCap.Blend);
			if (rotateCube) tick++;
			if (tick >= 1200) {
				tick = 0;
			}
		}


		void setColor(int c)
		{
			GL.Color4(materialColor[c]);
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

		void drawCheck(int w, int h, int evenColor, int oddColor)
		{
			int initialized = 0;
			int usedLighting = 0;
			int checklist = 0;

			if ((initialized == 0) || (usedLighting != useLighting)) {
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

				if (useQuads != 0) {
					GL.Normal3(square_normal);
					GL.Begin(PrimitiveType.Quads);
				}
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

						if (useQuads == 0) {
							GL.Begin(PrimitiveType.Polygon);
						}
						GL.Vertex4(square[0]);
						GL.Vertex4(square[1]);
						GL.Vertex4(square[2]);
						GL.Vertex4(square[3]);
						if (useQuads == 0) {
							GL.End();
						}
					}
				}

				if (useQuads == 1) {
					GL.End();
				}
				GL.EndList();

				initialized = 1;
				usedLighting = useLighting;
			} else {
				GL.CallList(checklist);
			}
		}

		float[][] myShadowMatrix(float[] ground, float[] light)
		{
			float dot;
			float[][] shadowMat = new float[4][];
			for (int i = 0; i < shadowMat.Length; i++)
			{
				shadowMat[i] = new float[4];
			}
			dot = ground[0] * light[0] +
				ground[1] * light[1] +
				ground[2] * light[2] +
				ground[3] * light[3];

			shadowMat[0][0] = dot - light[0] * ground[0];
			shadowMat[1][0] = 0.0f - light[0] * ground[1];
			shadowMat[2][0] = 0.0f - light[0] * ground[2];
			shadowMat[3][0] = 0.0f - light[0] * ground[3];

			shadowMat[0][1] = 0.0f - light[1] * ground[0];
			shadowMat[1][1] = dot - light[1] * ground[1];
			shadowMat[2][1] = 0.0f - light[1] * ground[2];
			shadowMat[3][1] = 0.0f - light[1] * ground[3];

			shadowMat[0][2] = 0.0f - light[2] * ground[0];
			shadowMat[1][2] = 0.0f - light[2] * ground[1];
			shadowMat[2][2] = dot - light[2] * ground[2];
			shadowMat[3][2] = 0.0f - light[2] * ground[3];

			shadowMat[0][3] = 0.0f - light[3] * ground[0];
			shadowMat[1][3] = 0.0f - light[3] * ground[1];
			shadowMat[2][3] = 0.0f - light[3] * ground[2];
			shadowMat[3][3] = dot - light[3] * ground[3];

			float[] singleArrayShadowMat = new float[16];
			Array.Copy(shadowMat[0], 0, singleArrayShadowMat, 0, 4);
			Array.Copy(shadowMat[1], 0, singleArrayShadowMat, 4, 4);
			Array.Copy(shadowMat[2], 0, singleArrayShadowMat, 8, 4);
			Array.Copy(shadowMat[3], 0, singleArrayShadowMat, 12, 4);

			GL.MultMatrix(singleArrayShadowMat);

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
				case Keys.I:
					result.AppendLine(AnalysisTools.CreateFromFloats(myShadowMatrix(backPlane, lightPos)).ToString());
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
				}
			}
			return result.ToString();
		}
	}
}

