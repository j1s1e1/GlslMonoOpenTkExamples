using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_ObjectPoleQuaternion : TutorialBase
	{
		Vector3 cubeScaleFactor = new Vector3(0.1f, 1f, 0.1f);
		Vector3 cylinderScaleFactor = new Vector3(0.2f, 0.4f, 0.2f);

		float quaternionElement = 1f;

		TextClass quaternionText;
		TextClass axisAngleText;

		bool staticText = true;
		bool reverseRotation = true;
		Vector3 textOffset = new Vector3(-0.9f, -0.8f, 0f);
		Vector3 textOffset2 = new Vector3(-0.9f, 0.8f, 0f);

		int cube = 0;
		int cylinder = 1;
		bool updateText = false;

		class ProgramData
		{
			public string name = "";
			public int theProgram;
			public int positionAttribute;
			public int colorAttribute;
			public int modelToCameraMatrixUnif;
			public int modelToWorldMatrixUnif;
			public int worldToCameraMatrixUnif;
			public int cameraToClipMatrixUnif;
			public int baseColorUnif;

			public int normalModelToCameraMatrixUnif;
			public int dirToLightUnif;
			public int lightIntensityUnif;
			public int ambientIntensityUnif;
			public int normalAttribute;
		};

		int currentProgram = 0;
		List<ProgramData> programs = new List<ProgramData>();

		float perspectiveAngle = 60f;
		float newPerspectiveAngle = 60f;

		ProgramData LoadProgram(string programSetString)
		{
			ProgramSet programSet = ProgramSets.Find(programSetString);
			ProgramData data = new ProgramData();
			data.name = programSet.name;
			int vertex_shader = Shader.compileShader(ShaderType.VertexShader, programSet.vertexShader);
			int fragment_shader = Shader.compileShader(ShaderType.FragmentShader, programSet.fragmentShader);
			data.theProgram  = Shader.createAndLinkProgram(vertex_shader, fragment_shader);

			data.positionAttribute = GL.GetAttribLocation(data.theProgram, "position");
			data.colorAttribute = GL.GetAttribLocation(data.theProgram, "color");
			if (data.positionAttribute != -1) 
			{
				if (data.positionAttribute != 0)
				{
					MessageBox.Show("These meshes only work with position at location 0 " + programSet.vertexShader);
				}
			}
			if (data.colorAttribute != -1) 
			{
				if (data.colorAttribute != 1)
				{
					MessageBox.Show("These meshes only work with color at location 1" + programSet.vertexShader);
				}
			}

			data.modelToWorldMatrixUnif = GL.GetUniformLocation(data.theProgram, "modelToWorldMatrix");
			data.worldToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "worldToCameraMatrix");
			data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "cameraToClipMatrix");
			if (data.cameraToClipMatrixUnif == -1)
			{
				data.cameraToClipMatrixUnif = GL.GetUniformLocation(data.theProgram, "Projection.cameraToClipMatrix");
			}
			data.baseColorUnif = GL.GetUniformLocation(data.theProgram, "baseColor");
			if (data.baseColorUnif == -1)
			{
				data.baseColorUnif = GL.GetUniformLocation(data.theProgram, "objectColor");
			}

			data.modelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "modelToCameraMatrix");

			data.normalModelToCameraMatrixUnif = GL.GetUniformLocation(data.theProgram, "normalModelToCameraMatrix");
			data.dirToLightUnif =  GL.GetUniformLocation(data.theProgram, "dirToLight");
			data.lightIntensityUnif = GL.GetUniformLocation(data.theProgram, "lightIntensity");
			data.ambientIntensityUnif = GL.GetUniformLocation(data.theProgram, "ambientIntensity");
			data.normalAttribute = GL.GetAttribLocation(data.theProgram, "normal");

			return data;
		}

		void InitializeProgram()
		{
			ProgramData ObjectPositionColor = LoadProgram("ObjectPositionColor");
			programs.Add(ObjectPositionColor);
			ProgramData UniformColor = LoadProgram("PosOnlyWorldTransform_vert ColorUniform_frag");
			programs.Add(UniformColor);
		}
		int currentMesh = 0;
		List<Mesh> meshes = new List<Mesh>();

		private String QuaternionString()
		{
			Quaternion quaternion = Quaternion.FromMatrix(new Matrix3(g_objtPole.CalcMatrix()));
			return "X " + quaternion.X.ToString("n3") + " Y " + quaternion.Y.ToString("n3") + " Z " + 
				quaternion.Z.ToString("n3") + " W " + quaternion.W.ToString("n3");
		}

		private String AxisAngeString()
		{
			Quaternion quaternion = Quaternion.FromMatrix(new Matrix3(g_objtPole.CalcMatrix()));
			quaternion.ToAxisAngle(out axis, out angle);
			return "Axis X " +  axis.X.ToString("n3") + " Y " + axis.Y.ToString("n3") + " Z " + 
				axis.Z.ToString("n3") + " Angle " + (angle * 180f / (float)Math.PI).ToString("n3");
		}

		protected override void init()
		{
			quaternionText = new TextClass(QuaternionString(), 0.5f, 0.05f, staticText, reverseRotation);
			quaternionText.SetOffset(textOffset);

			axisAngleText = new TextClass(AxisAngeString(), 0.4f, 0.04f, staticText, reverseRotation);
			axisAngleText.SetOffset(textOffset2);

			InitializeProgram();
			try 
			{
				meshes.Add(new Mesh("unitcubecolor.xml"));
				meshes.Add(new Mesh("unitcylinder.xml"));
			} catch (Exception ex) {
				throw new Exception("Error " + ex.ToString());
			}

			SetupDepthAndCull();
			reshape();
			quaternionText.UpdateText(QuaternionString());
			axisAngleText.UpdateText(AxisAngeString());
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
			MatrixStack.rightMultiply = false;
		}

		Vector3 axis = new Vector3();
		float angle = 0;

		public override void display()
		{
			ClearDisplay();
			GL.FrontFace(FrontFaceDirection.Cw);
			quaternionText.Draw();
			axisAngleText.Draw();
			GL.FrontFace(FrontFaceDirection.Ccw);
			if (meshes[currentMesh] != null)
			{
				MatrixStack modelMatrix = new MatrixStack();
				modelMatrix.Scale(0.8f);
				modelMatrix.Translate(0.0f, 0.0f, 0f);

				// Apply object transforms first
				modelMatrix.ApplyMatrix(g_objtPole.CalcMatrix());

				// End  Object Pole Code

				using (PushStack pushstack = new PushStack(modelMatrix)) 
				{
					modelMatrix.Scale(cubeScaleFactor);
					GL.UseProgram(programs[currentProgram].theProgram);

					Matrix4 mm = modelMatrix.Top();

					GL.UniformMatrix4(programs[currentProgram].modelToWorldMatrixUnif, false, ref mm);
					if (programs[currentProgram].baseColorUnif != -1)
					{
						GL.Uniform4(programs[currentProgram].baseColorUnif, 0.5f, 0.5f, 0f, 1.0f);
					}
					meshes[cube].Render();
				}
					
				using (PushStack pushstack = new PushStack(modelMatrix)) 
				{
					modelMatrix.Translate(new Vector3(0f, 0.4f, 0.0f));
					modelMatrix.Scale(cylinderScaleFactor);
					GL.UseProgram(programs[currentProgram].theProgram);

					Matrix4 mm = modelMatrix.Top();

					GL.UniformMatrix4(programs[currentProgram].modelToWorldMatrixUnif, false, ref mm);
					if (programs[currentProgram].baseColorUnif != -1)
					{
						GL.Uniform4(programs[currentProgram].baseColorUnif, 0.0f, 0.5f, 0.5f, 1.0f);
					}
					meshes[cylinder].Render();
				}

				GL.UseProgram(0);
				if (perspectiveAngle != newPerspectiveAngle)
				{
					perspectiveAngle = newPerspectiveAngle;
					reshape();
				}
			}
			if (updateText)
			{
				quaternionText.UpdateText(QuaternionString());
				axisAngleText.UpdateText(AxisAngeString());
				updateText = false;
			}
		}
			
		static Matrix4 pm;
		static Matrix4 cm;

		static private void SetGlobalMatrices(ProgramData program)
		{
			GL.UseProgram(program.theProgram);
			GL.UniformMatrix4(program.cameraToClipMatrixUnif, false, ref pm);  // this one is first
			GL.UniformMatrix4(program.worldToCameraMatrixUnif, false, ref cm); // this is the second one
			GL.UseProgram(0);
		}
			
		public override void reshape()
		{
			MatrixStack camMatrix = new MatrixStack();
			cm = camMatrix.Top();

			MatrixStack persMatrix = new MatrixStack();
			pm = persMatrix.Top();

			SetGlobalMatrices(programs[currentProgram]);

			GL.Viewport(0, 0, width, height);

		}

		float sqrt2over2 = (float) Math.Sqrt(2)/2f;

		public override String keyboard(Keys keyCode, int x, int y)
		{
			Quaternion rotateX;
			Quaternion rotateY;
			Quaternion rotateZ;
			StringBuilder result = new StringBuilder();
			result.AppendLine(keyCode.ToString());
			switch (keyCode) {
			case Keys.D1:
				rotateX = Quaternion.FromAxisAngle(new Vector3(1f, 0f, 0f), 5f * (float)Math.PI / 180f);
				g_objtPole.Rotate(rotateX);
				break;	
			case Keys.D2:
				rotateY = Quaternion.FromAxisAngle(new Vector3(0f, 1f, 0f), 5f * (float)Math.PI / 180f);
				g_objtPole.Rotate(rotateY);
				break;		
			case Keys.D3:
				rotateZ = Quaternion.FromAxisAngle(new Vector3(0f, 0f, 1f), 5f * (float)Math.PI / 180f);
				g_objtPole.Rotate(rotateZ);
				break;	
			case Keys.D4:
				rotateX = Quaternion.FromAxisAngle(new Vector3(1f, 0f, 0f), -5f * (float)Math.PI / 180f);
				g_objtPole.Rotate(rotateX);
				break;	
			case Keys.D5:
				rotateY = Quaternion.FromAxisAngle(new Vector3(0f, 1f, 0f), -5f * (float)Math.PI / 180f);
				g_objtPole.Rotate(rotateY);
				break;		
			case Keys.D6:
				rotateZ = Quaternion.FromAxisAngle(new Vector3(0f, 0f, 1f), -5f * (float)Math.PI / 180f);
				g_objtPole.Rotate(rotateZ);
				break;	
			case Keys.D7:
				g_objtPole.SetOrient(new Quaternion(sqrt2over2, sqrt2over2, 0f, 0f));
				break;	
			case Keys.D8:
				g_objtPole.SetOrient(new Quaternion(0f, sqrt2over2, sqrt2over2, 0f));
				break;	
			case Keys.D9:
				g_objtPole.SetOrient(new Quaternion(sqrt2over2, 0f, sqrt2over2, 0f));
				break;	
			case Keys.NumPad6:
				g_objtPole.Move(0.1f, 0f, 0.0f);
				result.AppendFormat(g_objtPole.GetPosOrient().position.ToString());
				break;
			case Keys.NumPad4:
				g_objtPole.Move(-0.1f, 0f, 0.0f);
				result.AppendFormat(g_objtPole.GetPosOrient().position.ToString());
				break;
			case Keys.NumPad8:
				g_objtPole.Move(0.0f, 0.1f, 0.0f);
				result.AppendFormat(g_objtPole.GetPosOrient().position.ToString());
				break;
			case Keys.NumPad2:
				g_objtPole.Move(0f, -0.1f, 0.0f);
				result.AppendFormat(g_objtPole.GetPosOrient().position.ToString());
				break;
			case Keys.NumPad7:
				g_objtPole.Move(0.0f, 0.0f, 0.1f);
				result.AppendFormat(g_objtPole.GetPosOrient().position.ToString());
				break;
			case Keys.NumPad3:
				g_objtPole.Move(0f, 0.0f, -0.1f);
				result.AppendFormat(g_objtPole.GetPosOrient().position.ToString());
				break;
			case Keys.V:
				newPerspectiveAngle = perspectiveAngle + 5f;
				if (newPerspectiveAngle > 120f)
				{
					newPerspectiveAngle = 30f;
				}
				break;
			case Keys.M:
				currentMesh++;
				if (currentMesh > meshes.Count - 1) currentMesh = 0;
				result.AppendLine("Mesh = " + meshes[currentMesh].fileName);
				break;
			case Keys.P:
				currentProgram++;
				if (currentProgram > programs.Count - 1) currentProgram = 0;
				result.AppendLine("Program = " + programs[currentProgram].name);
				reshape();
				break;
			case Keys.Q:
				result.AppendLine("currentProgram = " + currentProgram.ToString());
				break;
			case Keys.O:
				cubeScaleFactor = meshes[currentMesh].GetUnitScaleFactor();
				result.Append(cubeScaleFactor.ToString());
				break;
			case Keys.Add:
				quaternionElement += 0.1f;
				result.AppendLine("quaternionElement = " + quaternionElement.ToString());
				break;
			case Keys.Subtract:
				quaternionElement -= 0.1f;
				result.AppendLine("quaternionElement = " + quaternionElement.ToString());
				break;
			case Keys.C:
				GL.FrontFace(FrontFaceDirection.Cw);
				result.AppendLine("FrontFaceDirection.Cw");
				break;
			case Keys.D:
				GL.FrontFace(FrontFaceDirection.Ccw);
				result.AppendLine("FrontFaceDirection.Ccw");
				break;
			case Keys.R:
				MatrixStack.rightMultiply = true;
				break;
			case Keys.L:
				MatrixStack.rightMultiply = false;
				break;
			case Keys.X:
				g_objtPole.SetOrient(new Quaternion(quaternionElement, 0f, 0f, 0f));
				break;
			case Keys.Y:
				g_objtPole.SetOrient(new Quaternion(0f, quaternionElement, 0f, 0f));
				break;
			case Keys.Z:
				g_objtPole.SetOrient(new Quaternion(0f, 0f, quaternionElement, 0f));
				break;
			case Keys.W:
				g_objtPole.SetOrient(new Quaternion(0f, 0f, 0f, quaternionElement));
				break;
			}
			updateText = true;

			return result.ToString();
		}

		////////////////////////////////
		//View setup.


		// Setup to do nothing, provides parameter for objectpole

		public static Vector3 viewTargetPos = new Vector3(0.0f, 0.0f, 0.0f);
		public static Quaternion viewOrient = new Quaternion(0, 0, 0, 1f);
		public static float radius = 0.0f;			///<The initial radius of the camera from the target point.
		public static float degSpinRotation = 0.0f;	

		static ViewData g_initialView = new ViewData
			(
				viewTargetPos,
				viewOrient,
				radius,
				degSpinRotation
			);

		static ViewScale g_initialViewScale = new ViewScale
			(
				5.0f, 70.0f,
				2.0f, 0.5f,
				2.0f, 0.5f,
				90.0f/250.0f
			);

		static ViewPole g_viewPole = new ViewPole(g_initialView, g_initialViewScale, MouseButtons.MB_LEFT_BTN);

		////////////////////////////////
		//Object setup.

		private static Vector3 targetPos = new Vector3(0.0f, 0.0f, 0.0f);
		private static Quaternion orient = new Quaternion(0.909845f, 0.16043f, -0.376867f, -0.0664516f);
		private static float rotateScale = (float)(90.0f / 250.0f);
		private static ViewPole LookatProvider = g_viewPole;

		public static ObjectData g_initialObjectData = new ObjectData(targetPos, orient);

		public static ObjectPole g_objtPole = new ObjectPole(g_initialObjectData, rotateScale,
			MouseButtons.MB_RIGHT_BTN, g_viewPole);

		public override void MouseMotion(int x, int y)
		{
			Framework.ForwardMouseMotion(g_objtPole, x, y);
			Framework.ForwardMouseMotion(g_viewPole, x, y);
		}

		public override void MouseButton(int button, int state, int x, int y)
		{
			Framework.ForwardMouseButton(g_objtPole, button, state, x, y);
			Framework.ForwardMouseButton(g_viewPole, button, state, x, y);
			updateText = true;
		}

		void MouseWheel(int wheel, int direction, int x, int y)
		{
			Framework.ForwardMouseWheel(g_objtPole, wheel, direction, x, y);
		}
	}
}

