using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_SolarSystem : TutorialBase
	{
		List<Planet> planets;

		float sunRadius = 100f;
		float mercuryRadius = 2f;
		float venusRadius = 2f;
		float earthRadius = 2f;
		float marsRadius = 2f;
		float jupiterRadius = 2f;
		float saturnRadius = 2f;
		float uranusRadius = 2f;
		float neptuneRadius = 2f;
		float plutoRadius = 2f;

		float startZdistance = -1000f;

		int sunProgram = 0;

		Vector3 goal;
		bool moveToGoal = false;

		private void AddPlanet(string file, float radius, Vector3 offset, float angleStep)
		{
			Planet planet = new Planet(radius, file);
			planet.Move(offset);
			planet.SetAngleStep(angleStep);
			planets.Add(planet);
		}

		protected override void init()
		{
			planets = new List<Planet>();
			AddPlanet("suncyl1.jpg", sunRadius, new Vector3(0f, 0f, startZdistance), 1f);
			sunProgram = Programs.AddProgram(VertexShaders.MatrixTexture, 
				FragmentShaders.MatrixTextureScale);
			Programs.SetUniformScale(sunProgram, 500f);
			planets[0].SetProgram(sunProgram);
			AddPlanet("mercurymap.jpg", mercuryRadius, new Vector3(100f, 0f, startZdistance), 0.05f);
			AddPlanet("Venus_Magellan_C3-MDIR_ClrTopo_Global_Mosaic_1024.jpg", venusRadius, new Vector3(200f, 0f, startZdistance), -0.025f);
			AddPlanet("PathfinderMap.jpg", earthRadius, new Vector3(300f, 0f, startZdistance), 0.005f);
			AddPlanet("Mars_Viking_MDIM21_ClrMosaic_global_1024.jpg", marsRadius, new Vector3(400f, 0f, startZdistance), -0.025f);
			AddPlanet("jup0vss1.jpg", jupiterRadius, new Vector3(500f, 0f, startZdistance), -0.025f);
			AddPlanet("saturnmap.jpg", saturnRadius, new Vector3(600f, 0f, startZdistance), -0.005f);
			AddPlanet("uranusmap.jpg", uranusRadius, new Vector3(700f, 0f, startZdistance), -0.025f);
			AddPlanet("neptunemap.jpg", neptuneRadius, new Vector3(800f, 0f, startZdistance), -0.025f);
			AddPlanet("plutomap1k.jpg", plutoRadius, new Vector3(900f, 0f, startZdistance), -0.025f);
			SetupDepthAndCull();
			g_fzNear = 1f;
			g_fzFar = 10000f;
			worldToCameraMatrix = Matrix4.Identity;
			reshape();
		}

		float offsetLimit = 5f;

		private void MoveTowardsGoal()
		{
			UpdateGoal();
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
				MoveTowardsGoal();
			}
		}

		int planet = 0;

		private void UpdateGoal()
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

		private void NextPlanet()
		{
			planet++;
			if (planet > 9) planet = 0;
			UpdateGoal();
			Vector3 sunLocatoin =  planets[0].GetLocation();
			Shape.worldToCamera.M41 = -sunLocatoin.X;
			Shape.worldToCamera.M42 = -sunLocatoin.Y;
			Shape.worldToCamera.M43 = -sunLocatoin.Z - 1000f;
			moveToGoal = true;
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
					result.AppendLine("RotateShape 5X");
					break;
				case Keys.D5:
					//planet.RotateAboutCenter(Vector3.UnitY, 5f);
					result.AppendLine("RotateShape 5Y");
					break;
				case Keys.D6:
					//planet.RotateAboutCenter(Vector3.UnitZ, 5f);
					result.AppendLine("RotateShape 5Z");
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
					break;
				case Keys.B:
					break;
				case Keys.C:
					break;
				case Keys.D:
					break;
				case Keys.F:
					break;
				case Keys.I:
					result.AppendLine("worldToCamera");
					result.AppendLine(Shape.worldToCamera.ToString());
					result.AppendLine("modelToWorld");
					//result.AppendLine(planet.modelToWorld.ToString());
					//result.AppendLine(AnalysisTools.CalculateMatrixEffects(planet.modelToWorld));
					break;
				case Keys.N:
					NextPlanet();
					break;
				case Keys.P:
					newPerspectiveAngle = perspectiveAngle + 5f;
					if (newPerspectiveAngle > 170f) {
						newPerspectiveAngle = 30f;
					}
					break;
				case Keys.R:
					break;
				case Keys.Z:
					result.Append("ShaderInfo " + Programs.DumpShaders());
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
			persMatrix.Perspective(perspectiveAngle, (width / (float)height), g_fzNear, g_fzFar);

			cameraToClipMatrix = persMatrix.Top();

			SetGlobalMatrices();

			GL.Viewport(0, 0, width, height);
		}
	}
}

