using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_06_Hierarchy : TutorialBase
	{
		void InitializeProgram()
		{	
			fFrustumScale = CalcFrustumScale(45.0f);
			int vertex_shader = Shader.compileShader(ShaderType.VertexShader, VertexShaders.PosColorLocalTransform_vert);
			int fragment_shader = Shader.compileShader(ShaderType.FragmentShader, FragmentShaders.ColorPassthrough_frag);
			theProgram = Shader.createAndLinkProgram(vertex_shader, fragment_shader);
			positionAttribute = GL.GetAttribLocation(theProgram, "position");
			colorAttribute = GL.GetAttribLocation(theProgram, "color");

			modelToCameraMatrixUnif = GL.GetUniformLocation(theProgram, "modelToCameraMatrix");
			cameraToClipMatrixUnif = GL.GetUniformLocation(theProgram, "cameraToClipMatrix");

			fFrustumScale = CalcFrustumScale(45.0f);
			float fzNear = 1.0f;
			float fzFar = 61.0f;

			cameraToClipMatrix.M11 = fFrustumScale;
			cameraToClipMatrix.M22 = fFrustumScale;
			cameraToClipMatrix.M33 = (fzFar + fzNear) / (fzNear - fzFar);
			cameraToClipMatrix.M34 = -1.0f;
			cameraToClipMatrix.M43 = (2 * fzFar * fzNear) / (fzNear - fzFar);

			GL.UseProgram(theProgram);
			GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref cameraToClipMatrix);
			GL.UseProgram(0);
		}

		const int numberOfVertices = 24;
		private int COLOR_START = numberOfVertices * POSITION_DATA_SIZE_IN_ELEMENTS * BYTES_PER_FLOAT;


		static float[] vertexData = new float[]
		{
			//Front
			+1.0f, +1.0f, +1.0f, 1.0f,
			+1.0f, -1.0f, +1.0f, 1.0f,
			-1.0f, -1.0f, +1.0f, 1.0f,
			-1.0f, +1.0f, +1.0f, 1.0f,

			//Top
			+1.0f, +1.0f, +1.0f, 1.0f,
			-1.0f, +1.0f, +1.0f, 1.0f,
			-1.0f, +1.0f, -1.0f, 1.0f,
			+1.0f, +1.0f, -1.0f, 1.0f,

			//Left
			+1.0f, +1.0f, +1.0f, 1.0f,
			+1.0f, +1.0f, -1.0f, 1.0f,
			+1.0f, -1.0f, -1.0f, 1.0f,
			+1.0f, -1.0f, +1.0f, 1.0f,

			//Back
			+1.0f, +1.0f, -1.0f, 1.0f,
			-1.0f, +1.0f, -1.0f, 1.0f,
			-1.0f, -1.0f, -1.0f, 1.0f,
			+1.0f, -1.0f, -1.0f, 1.0f,

			//Bottom
			+1.0f, -1.0f, +1.0f, 1.0f,
			+1.0f, -1.0f, -1.0f, 1.0f,
			-1.0f, -1.0f, -1.0f, 1.0f,
			-1.0f, -1.0f, +1.0f, 1.0f,

			//Right
			-1.0f, +1.0f, +1.0f, 1.0f,
			-1.0f, -1.0f, +1.0f, 1.0f,
			-1.0f, -1.0f, -1.0f, 1.0f,
			-1.0f, +1.0f, -1.0f, 1.0f,


			Colors.GREEN_COLOR[0], Colors.GREEN_COLOR[1], Colors.GREEN_COLOR[2], Colors.GREEN_COLOR[3],
			Colors.GREEN_COLOR[0], Colors.GREEN_COLOR[1], Colors.GREEN_COLOR[2], Colors.GREEN_COLOR[3],
			Colors.GREEN_COLOR[0], Colors.GREEN_COLOR[1], Colors.GREEN_COLOR[2], Colors.GREEN_COLOR[3],
			Colors.GREEN_COLOR[0], Colors.GREEN_COLOR[1], Colors.GREEN_COLOR[2], Colors.GREEN_COLOR[3],

			Colors.BLUE_COLOR[0], Colors.BLUE_COLOR[1], Colors.BLUE_COLOR[2], Colors.BLUE_COLOR[3],
			Colors.BLUE_COLOR[0], Colors.BLUE_COLOR[1], Colors.BLUE_COLOR[2], Colors.BLUE_COLOR[3],
			Colors.BLUE_COLOR[0], Colors.BLUE_COLOR[1], Colors.BLUE_COLOR[2], Colors.BLUE_COLOR[3],
			Colors.BLUE_COLOR[0], Colors.BLUE_COLOR[1], Colors.BLUE_COLOR[2], Colors.BLUE_COLOR[3],

			Colors.RED_COLOR[0], Colors.RED_COLOR[1], Colors.RED_COLOR[2], Colors.RED_COLOR[3],
			Colors.RED_COLOR[0], Colors.RED_COLOR[1], Colors.RED_COLOR[2], Colors.RED_COLOR[3],
			Colors.RED_COLOR[0], Colors.RED_COLOR[1], Colors.RED_COLOR[2], Colors.RED_COLOR[3],
			Colors.RED_COLOR[0], Colors.RED_COLOR[1], Colors.RED_COLOR[2], Colors.RED_COLOR[3],

			Colors.YELLOW_COLOR[0], Colors.YELLOW_COLOR[1], Colors.YELLOW_COLOR[2], Colors.YELLOW_COLOR[3],
			Colors.YELLOW_COLOR[0], Colors.YELLOW_COLOR[1], Colors.YELLOW_COLOR[2], Colors.YELLOW_COLOR[3],
			Colors.YELLOW_COLOR[0], Colors.YELLOW_COLOR[1], Colors.YELLOW_COLOR[2], Colors.YELLOW_COLOR[3],
			Colors.YELLOW_COLOR[0], Colors.YELLOW_COLOR[1], Colors.YELLOW_COLOR[2], Colors.YELLOW_COLOR[3],

			Colors.CYAN_COLOR[0], Colors.CYAN_COLOR[1], Colors.CYAN_COLOR[2], Colors.CYAN_COLOR[3],
			Colors.CYAN_COLOR[0], Colors.CYAN_COLOR[1], Colors.CYAN_COLOR[2], Colors.CYAN_COLOR[3],
			Colors.CYAN_COLOR[0], Colors.CYAN_COLOR[1], Colors.CYAN_COLOR[2], Colors.CYAN_COLOR[3],
			Colors.CYAN_COLOR[0], Colors.CYAN_COLOR[1], Colors.CYAN_COLOR[2], Colors.CYAN_COLOR[3],

			Colors.MAGENTA_COLOR[0], Colors.MAGENTA_COLOR[1], Colors.MAGENTA_COLOR[2], Colors.MAGENTA_COLOR[3],
			Colors.MAGENTA_COLOR[0], Colors.MAGENTA_COLOR[1], Colors.MAGENTA_COLOR[2], Colors.MAGENTA_COLOR[3],
			Colors.MAGENTA_COLOR[0], Colors.MAGENTA_COLOR[1], Colors.MAGENTA_COLOR[2], Colors.MAGENTA_COLOR[3],
			Colors.MAGENTA_COLOR[0], Colors.MAGENTA_COLOR[1], Colors.MAGENTA_COLOR[2], Colors.MAGENTA_COLOR[3],
		};

		static short[] indexData = new short[]
		{
			0, 1, 2,
			2, 3, 0,

			4, 5, 6,
			6, 7, 4,

			8, 9, 10,
			10, 11, 8,

			12, 13, 14,
			14, 15, 12,

			16, 17, 18,
			18, 19, 16,

			20, 21, 22,
			22, 23, 20,
		};
			
		int vao;

		void InitializeVAO()
		{
			InitializeVertexBuffer(vertexData, indexData);

			GL.GenVertexArrays(1, out vao);
			GL.BindVertexArray(vao);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
			// Bind Attributes
			GL.EnableVertexAttribArray(positionAttribute);
			GL.EnableVertexAttribArray(colorAttribute);
			GL.VertexAttribPointer(positionAttribute, POSITION_DATA_SIZE_IN_ELEMENTS, 
				VertexAttribPointerType.Float, false, POSITION_STRIDE, 0);
			GL.VertexAttribPointer(colorAttribute, COLOR_DATA_SIZE_IN_ELEMENTS, 
				VertexAttribPointerType.Float, false, COLOR_STRIDE, COLOR_START);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferObject[0]);

			GL.BindVertexArray(0);
		}

		public static float Clamp(float fValue, float fMinValue, float fMaxValue)
		{
			if(fValue < fMinValue)
				return fMinValue;

			if(fValue > fMaxValue)
				return fMaxValue;

			return fValue;
		}

		Matrix3 RotateX(float fAngDeg)
		{
			float fAngRad = DegToRad(fAngDeg);
			float fCos = (float) Math.Cos(fAngRad);
			float fSin = (float) Math.Sin(fAngRad);

			Matrix3 theMat = Matrix3.Identity;
			theMat.M22 = fCos; theMat.M32 = -fSin;
			theMat.M23 = fSin; theMat.M33 = fCos;
			return theMat;
		}

		Matrix3 RotateY(float fAngDeg)
		{
			float fAngRad = DegToRad(fAngDeg);
			float fCos = (float) Math.Cos(fAngRad);
			float fSin = (float) Math.Sin(fAngRad);

			Matrix3 theMat = Matrix3.Identity;
			theMat.M11 = fCos; theMat.M31 = fSin;
			theMat.M13 = -fSin; theMat.M33 = fCos;
			return theMat;
		}

		Matrix3 RotateZ(float fAngDeg)
		{
			float fAngRad = DegToRad(fAngDeg);
			float fCos = (float) Math.Cos(fAngRad);
			float fSin = (float) Math.Sin(fAngRad);

			Matrix3 theMat = Matrix3.Identity;
			theMat.M11 = fCos; theMat.M21 = -fSin;
			theMat.M12 = fSin; theMat.M22 = fCos;
			return theMat;
		}

		class Hierarchy
		{
			public Hierarchy(int theProgramIn, int vaoIn, int modelToCameraMatrixUnifIn)
			{
				theProgram = theProgramIn;
				vao = vaoIn;
				modelToCameraMatrixUnif = modelToCameraMatrixUnifIn;
				posBase = new Vector3(3.0f, -5.0f, -40.0f);
				angBase = -45.0f;
				posBaseLeft = new Vector3(2.0f, 0.0f, 0.0f);
				posBaseRight = new Vector3(-2.0f, 0.0f, 0.0f);
				scaleBaseZ = 3.0f;
				angUpperArm = -33.75f;
				sizeUpperArm = 9.0f;
				posLowerArm = new Vector3(0.0f, 0.0f, 8.0f);
				angLowerArm = 146.25f;
				lenLowerArm = 5.0f;
				widthLowerArm = 1.5f;
				posWrist = new Vector3(0.0f, 0.0f, 5.0f);
				angWristRoll = 0.0f;
				angWristPitch = 67.5f;
				lenWrist = 2.0f;
				widthWrist = 2.0f;
				posLeftFinger = new Vector3(1.0f, 0.0f, 1.0f);
				posRightFinger = new Vector3(-1.0f, 0.0f, 1.0f);
				angFingerOpen = 180.0f;
				lenFinger = 2.0f;
				widthFinger = 0.5f;
				angLowerFinger = 45.0f;
			}

			public void Draw()
			{
				MatrixStack modelToCameraStack = new MatrixStack();

				GL.UseProgram(theProgram);
				GL.BindVertexArray(vao);

				modelToCameraStack.Translate(posBase);
				modelToCameraStack.RotateY(angBase);

				//Draw left base.
				{
					modelToCameraStack.Push();
					modelToCameraStack.Translate(posBaseLeft);
					modelToCameraStack.Scale(new Vector3(1.0f, 1.0f, scaleBaseZ));
					Matrix4 mm = modelToCameraStack.Top();
					GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref mm);
					GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
					modelToCameraStack.Pop();
				}

				//Draw right base.
				{
					modelToCameraStack.Push();
					modelToCameraStack.Translate(posBaseRight);
					modelToCameraStack.Scale(new Vector3(1.0f, 1.0f, scaleBaseZ));
					Matrix4 mm = modelToCameraStack.Top();
					GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref mm);
					GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
					modelToCameraStack.Pop();
				}

				//Draw main arm.
				DrawUpperArm(modelToCameraStack);

				GL.BindVertexArray(0);
				GL.UseProgram(0);
			}

			static float STANDARD_ANGLE_INCREMENT = 11.25f;
			static float SMALL_ANGLE_INCREMENT = 9.0f;

			public void AdjBase(bool bIncrement)
			{
				angBase += bIncrement ? STANDARD_ANGLE_INCREMENT : -STANDARD_ANGLE_INCREMENT;
				angBase = angBase % 360.0f;
			}

			public void AdjUpperArm(bool bIncrement)
			{
				angUpperArm += bIncrement ? STANDARD_ANGLE_INCREMENT : -STANDARD_ANGLE_INCREMENT;
				angUpperArm = Clamp(angUpperArm, -90.0f, 0.0f);
			}

			public void AdjLowerArm(bool bIncrement)
			{
				angLowerArm += bIncrement ? STANDARD_ANGLE_INCREMENT : -STANDARD_ANGLE_INCREMENT;
				angLowerArm = Clamp(angLowerArm, 0.0f, 146.25f);
			}

			public void AdjWristPitch(bool bIncrement)
			{
				angWristPitch += bIncrement ? STANDARD_ANGLE_INCREMENT : -STANDARD_ANGLE_INCREMENT;
				angWristPitch = Clamp(angWristPitch, 0.0f, 90.0f);
			}

			public void AdjWristRoll(bool bIncrement)
			{
				angWristRoll += bIncrement ? STANDARD_ANGLE_INCREMENT : -STANDARD_ANGLE_INCREMENT;
				angWristRoll = angWristRoll % 360.0f;
			}

			public void AdjFingerOpen(bool bIncrement)
			{
				angFingerOpen += bIncrement ? SMALL_ANGLE_INCREMENT : -SMALL_ANGLE_INCREMENT;
				angFingerOpen = Clamp(angFingerOpen, 9.0f, 180.0f);
			}

			public string WritePose()
			{
				StringBuilder result = new StringBuilder();
				result.AppendLine("angBase: " + angBase.ToString());

				result.AppendLine("angUpperArm: " + angUpperArm.ToString());
				result.AppendLine("angLowerArm: " + angLowerArm.ToString());
				result.AppendLine("angWristPitch: " + angWristPitch.ToString());
				result.AppendLine("angWristRoll: " + angWristRoll.ToString());
				result.AppendLine("angFingerOpen: " + angFingerOpen.ToString());
				result.AppendLine("");
				return result.ToString();
			}

			private void DrawFingers(MatrixStack modelToCameraStack)
			{
				//Draw left finger
				modelToCameraStack.Push();
				modelToCameraStack.Translate(posLeftFinger);
				modelToCameraStack.RotateY(angFingerOpen);

				modelToCameraStack.Push();
				modelToCameraStack.Translate(new Vector3(0.0f, 0.0f, lenFinger / 2.0f));
				modelToCameraStack.Scale(new Vector3(widthFinger / 2.0f, widthFinger/ 2.0f, lenFinger / 2.0f));
				Matrix4 mm = modelToCameraStack.Top();
				GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref mm);
				GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
				modelToCameraStack.Pop();

				{
					//Draw left lower finger
					modelToCameraStack.Push();
					modelToCameraStack.Translate(new Vector3(0.0f, 0.0f, lenFinger));
					modelToCameraStack.RotateY(-angLowerFinger);

					modelToCameraStack.Push();
					modelToCameraStack.Translate(new Vector3(0.0f, 0.0f, lenFinger / 2.0f));
					modelToCameraStack.Scale(new Vector3(widthFinger / 2.0f, widthFinger/ 2.0f, lenFinger / 2.0f));

					mm = modelToCameraStack.Top();
					GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref mm);
					GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
					modelToCameraStack.Pop();

					modelToCameraStack.Pop();
				}

				modelToCameraStack.Pop();

				//Draw right finger
				modelToCameraStack.Push();
				modelToCameraStack.Translate(posRightFinger);
				modelToCameraStack.RotateY(-angFingerOpen);

				modelToCameraStack.Push();
				modelToCameraStack.Translate(new Vector3(0.0f, 0.0f, lenFinger / 2.0f));
				modelToCameraStack.Scale(new Vector3(widthFinger / 2.0f, widthFinger/ 2.0f, lenFinger / 2.0f));
				mm = modelToCameraStack.Top();
				GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref mm);
				GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
				modelToCameraStack.Pop();

				{
					//Draw right lower finger
					modelToCameraStack.Push();
					modelToCameraStack.Translate(new Vector3(0.0f, 0.0f, lenFinger));
					modelToCameraStack.RotateY(angLowerFinger);

					modelToCameraStack.Push();
					modelToCameraStack.Translate(new Vector3(0.0f, 0.0f, lenFinger / 2.0f));
					modelToCameraStack.Scale(new Vector3(widthFinger / 2.0f, widthFinger/ 2.0f, lenFinger / 2.0f));
					mm = modelToCameraStack.Top();
					GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref mm);
					GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
					modelToCameraStack.Pop();

					modelToCameraStack.Pop();
				}

				modelToCameraStack.Pop();
			}

			private void DrawWrist(MatrixStack modelToCameraStack)
			{
				modelToCameraStack.Push();
				modelToCameraStack.Translate(posWrist);
				modelToCameraStack.RotateZ(angWristRoll);
				modelToCameraStack.RotateX(angWristPitch);

				modelToCameraStack.Push();
				modelToCameraStack.Scale(new Vector3(widthWrist / 2.0f, widthWrist/ 2.0f, lenWrist / 2.0f));
				Matrix4 mm = modelToCameraStack.Top();
				GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref mm);
				GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
				modelToCameraStack.Pop();

				DrawFingers(modelToCameraStack);

				modelToCameraStack.Pop();
			}

			private void DrawLowerArm(MatrixStack modelToCameraStack)
			{
				modelToCameraStack.Push();
				modelToCameraStack.Translate(posLowerArm);
				modelToCameraStack.RotateX(angLowerArm);

				modelToCameraStack.Push();
				modelToCameraStack.Translate(new Vector3(0.0f, 0.0f, lenLowerArm / 2.0f));
				modelToCameraStack.Scale(new Vector3(widthLowerArm / 2.0f, widthLowerArm / 2.0f, lenLowerArm / 2.0f));
				Matrix4 mm = modelToCameraStack.Top();
				GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref mm);
				GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
				modelToCameraStack.Pop();

				DrawWrist(modelToCameraStack);

				modelToCameraStack.Pop();
			}

			private void DrawUpperArm(MatrixStack modelToCameraStack)
			{
				modelToCameraStack.Push();
				modelToCameraStack.RotateX(angUpperArm);

				{
					modelToCameraStack.Push();
					modelToCameraStack.Translate(new Vector3(0.0f, 0.0f, (sizeUpperArm / 2.0f) - 1.0f));
					modelToCameraStack.Scale(new Vector3(1.0f, 1.0f, sizeUpperArm / 2.0f));
					Matrix4 mm = modelToCameraStack.Top();
					GL.UniformMatrix4(modelToCameraMatrixUnif, false, ref mm);
					GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedShort, 0);
					modelToCameraStack.Pop();
				}

				DrawLowerArm(modelToCameraStack);

				modelToCameraStack.Pop();
			}

			private Vector3	posBase;
			private float	angBase;

			private Vector3	posBaseLeft, posBaseRight;
			private float	scaleBaseZ;

			private float	angUpperArm;
			private float	sizeUpperArm;

			private Vector3	posLowerArm;
			private float	angLowerArm;
			private float	lenLowerArm;
			private float	widthLowerArm;

			private Vector3	posWrist;
			private float	angWristRoll;
			private float	angWristPitch;
			private float	lenWrist;
			private float	widthWrist;

			private Vector3	posLeftFinger;
			private Vector3	posRightFinger;
			private float	angFingerOpen;
			private float	lenFinger;
			private float	widthFinger;
			private float	angLowerFinger;

			private int theProgram;
			private int vao;
			private int modelToCameraMatrixUnif;
		};


		Hierarchy g_armature;

		protected override void init()
		{
			InitializeProgram();
			InitializeVAO();

			SetupDepthAndCull();
			g_armature = new Hierarchy(theProgram, vao, modelToCameraMatrixUnif);
			MatrixStack.rightMultiply = false; // TEST
		}

		//Called to update the display.
		//You should call glutSwapBuffers after all of your rendering to display what you rendered.
		//If you need continuous updates of the screen, call glutPostRedisplay() at the end of the function.
		public override void display()
		{
			ClearDisplay();
			g_armature.Draw();
		}

		public override void reshape ()
		{
			cameraToClipMatrix.M11 = fFrustumScale * (height / (float)width);
			cameraToClipMatrix.M22 = fFrustumScale;

			GL.UseProgram(theProgram); 
			GL.UniformMatrix4(cameraToClipMatrixUnif, false, ref cameraToClipMatrix);
			GL.UseProgram(0);

			GL.Viewport(0, 0, width, height);
		}
			
		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			switch (keyCode)
			{
			case Keys.Escape:
				//glutLeaveMainLoop();
				break;
			case Keys.A: g_armature.AdjBase(true); break;
			case Keys.D: g_armature.AdjBase(false); break;
			case Keys.W: g_armature.AdjUpperArm(false); break;
			case Keys.S: g_armature.AdjUpperArm(true); break;
			case Keys.R: g_armature.AdjLowerArm(false); break;
			case Keys.F: g_armature.AdjLowerArm(true); break;
			case Keys.T: g_armature.AdjWristPitch(false); break;
			case Keys.G: g_armature.AdjWristPitch(true); break;
			case Keys.Z: g_armature.AdjWristRoll(true); break;
			case Keys.C: g_armature.AdjWristRoll(false); break;
			case Keys.Q: g_armature.AdjFingerOpen(true); break;
			case Keys.E: g_armature.AdjFingerOpen(false); break;
			case Keys.Space: result.Append(g_armature.WritePose()); break;
			}
			return result.ToString();
		}
	}
}

