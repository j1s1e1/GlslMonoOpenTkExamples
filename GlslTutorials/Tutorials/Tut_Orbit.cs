using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Orbit : TutorialBase
	{
		List<Planet> planets;
		int currentProgram = 0;
		List<int> programs = new List<int>();

		float sunRadius = 25f;

		static float planetGain = 5f;

		public float mercuryRadius = 2f * planetGain;
		public float venusRadius = 2f * planetGain;
		public float earthRadius = 2f * planetGain;
		public float marsRadius = 2f * planetGain;
		public float jupiterRadius = 2f * planetGain;
		public float saturnRadius = 2f * planetGain;
		public float uranusRadius = 2f * planetGain;
		public float neptuneRadius = 2f * planetGain;
		public float plutoRadius = 2f * planetGain;

		static float speedGain = 1f;

		public float mercurySpeed = 5f * speedGain;
		public float venusSpeed = 0.1f * speedGain;
		public float earthSpeed = 0.2f * speedGain;
		public float marsSpeed = 0.1f * speedGain;
		public float jupiterSpeed = 0.05f * speedGain;
		public float saturnSpeed  = 0.4f * speedGain;
		public float uranusSpeed  = 0.3f * speedGain;
		public float neptuneSpeed  = 0.5f * speedGain;
		public float plutoSpeed  = 0.2f * speedGain;

		static float startZdistance = -1000f;

		static float offsetX = 500f;
		static float offsetY = 500f;

		public Vector3 mercuryOffset = new Vector3(-offsetX, 0f, startZdistance);
		public Vector3 venusOffset = new Vector3(-2f * offsetX, 0f, startZdistance);
		public Vector3 earthOffset = new Vector3(offsetX, 0f, startZdistance);
		public Vector3 marsOffset = new Vector3(-offsetX, -offsetY, startZdistance);
		public Vector3 jupiterOffset = new Vector3(0f, -offsetY, startZdistance);
		public Vector3 saturnOffset = new Vector3(offsetX, offsetY, startZdistance);
		public Vector3 uranusOffset = new Vector3(-offsetX, -offsetY, startZdistance);
		public Vector3 neptuneOffset = new Vector3(0f, offsetY, startZdistance);
		public Vector3 plutoOffset = new Vector3(offsetX, offsetY, startZdistance);

		static float initialSpeedScale = 3f;
		public Vector3 mercuryInitialSpeed = new Vector3(0, -3f, 0f);
		public Vector3 venusInitialSpeed = new Vector3(0, -4.5f, 0f);
		public Vector3 earthInitialSpeed = new Vector3(0, 1f, 0f);
		public Vector3 marsInitialSpeed = new Vector3(-1 * initialSpeedScale, 1f * initialSpeedScale, 0f);
		public Vector3 jupiterInitialSpeed = new Vector3(0, 0f, 0f);
		public Vector3 saturnInitialSpeed = new Vector3(0, 0f, 0f);
		public Vector3 uranusInitialSpeed = new Vector3(0, 0f, 0f);
		public Vector3 neptuneInitialSpeed = new Vector3(0, 0f, 0f);
		public Vector3 plutoInitialSpeed = new Vector3(0, 3f, 0f);

		int sunProgram = 0;

		Vector3 goal = new Vector3();
		bool moveToGoal = false;

		bool queryShaderInfo = false;
		bool queryVertexShaderInfo = false;
		bool queryFragmentShaderInfo = false;
		bool setLightPosition = false;
		bool setScale = false;

		Vector3 lightPos = new Vector3();
		float scale = 500f;
		bool follow = false;

		float currentScale = 1f;
		float minScale = 0.1f;
		float maxScale = 10.0f;

		float scrollScale = 10f;

		Random random = new Random();

		Vector3 injectVector;
		bool AddPlanet = false;
		bool enableAddPlanet = false;

		TextureSphere stars;

		private void AddPlanetSub(String name, string file, float radius, Vector3 offset, float angleStep)
		{
			Planet planet = new Planet(name, radius, file);
			planet.Move(offset);
			planet.SetAngleStep(angleStep);
			planets.Add(planet);
		}

		protected override void init()
		{
			stars = new TextureSphere(5000f, "starmap.png");
			planets = new List<Planet>();
			AddPlanetSub("Sun","suncyl1.jpg", sunRadius, new Vector3(0f, 0f, startZdistance), 1f);
			sunProgram = Programs.AddProgram(VertexShaders.MatrixTexture,
				FragmentShaders.MatrixTextureScale);
			Programs.SetUniformScale(sunProgram, 500f);

			AddPlanetSub("Mercury", Planet.mercuryFileName, mercuryRadius, mercuryOffset, mercurySpeed);
			planets[planets.Count - 1].SetUpOrbit(new Vector3(0f, 0f, startZdistance), mercuryOffset,
				mercuryInitialSpeed, 2.3e4f);
			AddPlanetSub("Venus", Planet.venusFileName, venusRadius, venusOffset, venusSpeed);
			planets[planets.Count - 1].SetUpOrbit(new Vector3(0f, 0f, startZdistance), venusOffset,
				venusInitialSpeed, 2.3e4f);
			AddPlanetSub("Earth",  "PathfinderMap.jpg", earthRadius, earthOffset, earthSpeed);
			//planets.get(planets.size() - 1).setUpOrbit(new Vector3(0f, 0f, startZdistance), earthOffset,
			//        earthInitialSpeed, 500f);

			AddPlanetSub("Mars", "Mars_Viking_MDIM21_ClrMosaic_global_1024.jpg", marsRadius, marsOffset, marsSpeed);
			planets[planets.Count - 1].SetUpOrbit(new Vector3(0f, 0f, startZdistance), marsOffset,
				marsInitialSpeed, 2.3e4f);

			foreach (Planet p in planets)
			{
				p.SetProgram(sunProgram);
				p.SetLightScale(100f);
				programs.Add(p.GetProgram());
			}
			planets[0].SetLightScale(500f);
			SetupDepthAndCull();
			g_fzNear = 1f;
			g_fzFar = 10000f;
			worldToCameraMatrix = Matrix4.Identity;
			reshape();
			Shape.worldToCamera.M41 = 0;
			Shape.worldToCamera.M42 = 0;
			Shape.worldToCamera.M43 = - 1000f;
		}

		float offsetLimit = 5f;

		private void moveTowardsGoal()
		{
			updateGoal();
			float xOffset = Shape.worldToCamera.M41 - goal.X;
			float yOffset = Shape.worldToCamera.M42 - goal.Y;
			float zOffset = Shape.worldToCamera.M43 - goal.Z;
			if (Math.Abs(xOffset) > Math.Abs(yOffset))
			{
				if (Math.Abs(xOffset) > offsetLimit)
				{
					if (xOffset > 0)
					{
						Shape.worldToCamera.M41 -= offsetLimit;
					}
					else
					{
						Shape.worldToCamera.M41 += offsetLimit;
					}
				}
				else
				{
					if (Math.Abs(zOffset) > offsetLimit)
					{
						if (zOffset > 0)
						{
							Shape.worldToCamera.M43 -= offsetLimit;
						}
						else
						{
							Shape.worldToCamera.M43 += offsetLimit;
						}
					}
					else
					{
						moveToGoal = false;
					}
				}
			}
			else
			{
				if (Math.Abs(yOffset) > offsetLimit)
				{
					if (yOffset > 0)
					{
						Shape.worldToCamera.M42 -= offsetLimit;
					}
					else
					{
						Shape.worldToCamera.M42 += offsetLimit;
					}
				}
				else
				{
					if (Math.Abs(zOffset) > offsetLimit)
					{
						if (zOffset > 0)
						{
							Shape.worldToCamera.M43 -= offsetLimit;
						}
						else
						{
							Shape.worldToCamera.M43 += offsetLimit;
						}
					}
					else
					{
						moveToGoal = false;
					}
				}
			}
		}

		public override void display()
		{
			ClearDisplay();
			stars.Draw();
			foreach (Planet planet in planets)
			{
				planet.UpdatePosition();
				planet.Draw();
			}
			if (perspectiveAngle != newPerspectiveAngle)
			{
				perspectiveAngle = newPerspectiveAngle;
				reshape();
			}
			if (moveToGoal)
			{
				moveTowardsGoal();
			}
			if (follow)
			{
				updateGoal();
				moveTowardsGoal();
			}
			if (queryShaderInfo)
			{
				queryShaderInfo = false;
				//Log.i("ShaderInfo", Programs.dumpShaders());
			}
			if (queryVertexShaderInfo)
			{
				queryVertexShaderInfo = false;
				//Log.i("KeyEvent", Programs.getVertexShaderInfo(programs.get(currentProgram)));
			}
			if (queryFragmentShaderInfo)
			{
				queryFragmentShaderInfo = false;
				//Log.i("KeyEvent", Programs.getFragmentShaderInfo(programs.get(currentProgram)));
			}
			if (setLightPosition)
			{
				setLightPosition = false;
				//Programs.setLightPosition(programs.get(lightPosSelection), lightPos);
			}
			if (setScale)
			{
				setScale = false;
				Programs.SetUniformScale(programs[currentProgram], scale);
			}
			if (AddPlanet)
			{
				AddPlanet = false;
				AddPlanetSub(injectVector);
			}
		}

		int planet = 0;

		private void updateGoal()
		{
			Vector3 planetLocation = planets[planet].GetLocation();
			goal.X = -planetLocation.X;
			goal.Y = -planetLocation.Y;
			if (planet == 0)
			{
				goal.Z = -planetLocation.Z - 1000f;
			}
			else
			{
				goal.Z = -planetLocation.Z - 10f;
			}
		}

		private void nextPlanet()
		{
			planet++;
			if (planet > 9) planet = 0;
			//Log.i("nextPlanet", "planet = " + String.valueOf(planet));
			updateGoal();
			Shape.worldToCamera.M41 = 0;
			Shape.worldToCamera.M42 = 0;
			Shape.worldToCamera.M43 = - 1000f;
			if (planet != 0) {
				moveToGoal = true;
			}
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
					Shape.MoveWorld(new Vector3(0f, 0f, 1f));
					break;
				case Keys.D2:
					Shape.MoveWorld(new Vector3(0f, 0f, -1f));
					break;
				case Keys.D3:
					Shape.MoveWorld(new Vector3(0f, 0f, 10f));
					break;
				case Keys.D4:
					Shape.MoveWorld(new Vector3(0f, 0f, -10f));
					result.AppendLine("KeyEvent" + "RotateShape 5X");
					break;
				case Keys.D5:
					//planet.RotateAboutCenter(Vector3.UnitY, 5f);
					result.AppendLine("KeyEvent" + "RotateShape 5Y");
					break;
				case Keys.D6:
					//planet.RotateAboutCenter(Vector3.UnitZ, 5f);
					result.AppendLine("KeyEvent" + "RotateShape 5Z");
					break;
				case Keys.NumPad6:
					Shape.MoveWorld(new Vector3(10f, 0.0f, 0.0f));
					break;
				case Keys.NumPad4:
					Shape.MoveWorld(new Vector3(-10f, 0.0f, 0.0f));
					break;
				case Keys.NumPad8:
					Shape.MoveWorld(new Vector3(0.0f, 10f, 0.0f));
					break;
				case Keys.NumPad2:
					Shape.MoveWorld(new Vector3(0.0f, -10f, 0.0f));
					break;
				case Keys.NumPad7:
					Shape.MoveWorld(new Vector3(0.0f, 0.0f, 10f));
					break;
				case Keys.NumPad3:
					Shape.MoveWorld(new Vector3(0.0f, 0.0f, -10f));
					break;
				case Keys.A:
					setLightPosition = true;
					lightPos += new Vector3(0f, 0f, 1f);
					break;
				case Keys.B:
					setLightPosition = true;
					lightPos += new Vector3(0f, 0f, -1f);
					break;
				case Keys.C:
					break;
				case Keys.D:
					scale *= 0.8f;
					setScale = true;
					break;
				case Keys.U:
					scale *= 1.2f;
					setScale = true;
					break;
				case Keys.I:
					result.AppendLine("KeyEvent" + "worldToCamera");
					result.AppendLine("KeyEvent" + Shape.worldToCamera.ToString());
					result.AppendLine("KeyEvent" + "modelToWorld");
					//result.AppendLine(planet.modelToWorld.ToString());
					//result.AppendLine(AnalysisTools.CalculateMatrixEffects(planet.modelToWorld));
					break;
				case Keys.N:
					nextPlanet();
					break;
				case Keys.O:
					if (follow) {
						follow = false;
					}
					else
					{
						follow = true;
					}
					break;
				case Keys.P:
					newPerspectiveAngle = perspectiveAngle + 5f;
					if (newPerspectiveAngle > 170f) {
						newPerspectiveAngle = 30f;
					}
					break;
				case Keys.R:
					break;
				case Keys.F:
					queryFragmentShaderInfo = true;
					break;
				case Keys.V:
					queryVertexShaderInfo = true;
					break;
				case Keys.Z:
					queryShaderInfo = true;
					break;
				}
			}
			return result.ToString();
		}

		static private void SetGlobalMatrices()
		{
			Shape.SetCameraToClipMatrix(cameraToClipMatrix);
			Shape.SetWorldToCameraMatrix(worldToCameraMatrix);
		}

		float perspectiveAngle = 90f;
		float newPerspectiveAngle = 90f;

		public override void reshape()
		{
			MatrixStack persMatrix = new MatrixStack();
			persMatrix.Perspective(perspectiveAngle, (width / (float) height), g_fzNear, g_fzFar);

			cameraToClipMatrix = persMatrix.Top();

			SetGlobalMatrices();

			GL.Viewport(0, 0, width, height);
		}
		/*
		public void receiveMessage(String message)
		{
			Log.i("Tut_SolarSystem", message);
			String[] words = message.split(" ");
			switch (words[0])
			{
			case "scale":
				if (words.length == 2) {
					scale = Float.parseFloat(words[1]);
					setScale = true;
				}
				break;
			case "lightPos":
				if (words.length == 4) {
					lightPos = new Vector3(Float.parseFloat(words[1]), Float.parseFloat(words[2]), Float.parseFloat(words[3]));
					lightPosSelection = 0;
					setLightPosition = true;
				}
				if (words.length == 5)
				{
					lightPos = new Vector3(Float.parseFloat(words[1]), Float.parseFloat(words[2]), Float.parseFloat(words[3]));
					lightPosSelection = Integer.parseInt(words[4]);
					setLightPosition = true;
				}
				break;
			case "MoveWorld":
				if (words.length == 4) {
					Vector3 move = new Vector3(Float.parseFloat(words[1]), Float.parseFloat(words[2]), Float.parseFloat(words[3]));
					Shape.MoveWorld(move);
				}
				break;
			case "Next":
				if (words.length == 4) {
					Vector3 move = new Vector3(Float.parseFloat(words[1]), Float.parseFloat(words[2]), Float.parseFloat(words[3]));
					Shape.MoveWorld(move);
				}
				break;
			default:
				break;
			}
		}
		*/

		public void SetScale(float scale)
		{
			currentScale = currentScale * scale;
			if (currentScale > maxScale) currentScale = maxScale;
			if (currentScale < minScale) currentScale = minScale;
			Shape.SetScale(currentScale);
		}

		private void AddPlanetSub(Vector3 injectVector)
		{
			int planetSelection = random.Next(9);
			string filename = "";
			switch (planetSelection)
			{
			case 0: filename = Planet.mercuryFileName; break;
			case 1: filename = Planet.venusFileName; break;
			case 2: filename = Planet.earthFileName; break;
			case 3: filename = Planet.marsFileName; break;
			case 4: filename = Planet.jupiterFileName; break;
			case 5: filename = Planet.saturnFileName; break;
			case 6: filename = Planet.uranusFileName; break;
			case 7: filename = Planet.neptuneFileName; break;
			case 8: filename = Planet.plutoFileName; break;

			}
			AddPlanetSub("New", filename, sunRadius, injectVector, 1f);
			planets[planets.Count - 1].SetProgram(sunProgram);
			planets[planets.Count - 1].SetLightScale(100f);
			programs.Add(planets[planets.Count - 1].GetProgram());
		}

		public void scroll(float distanceX, float distanceY)
		{
			if (enableAddPlanet) {
				injectVector = new Vector3(scrollScale * distanceX, scrollScale * distanceY, 0f);
				AddPlanet = true;
				//Log.i("scroll", "AddPlanet " + injectVector.toString());
			}
		}
	}
}

