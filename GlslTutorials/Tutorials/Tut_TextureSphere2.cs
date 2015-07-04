using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_TextureSphere2 : TutorialBase
	{
		private TextureSphere textureSphere;
		int currentProgram = 0;
		List<int> programs = new List<int>();
		bool nextPlanet = false;
		int planet = 0;
		int sun;
		int mercury;
		int venus;
		int earth;
		int mars;
		int jupiter;
		int saturn;
		int uranus;
		int neptune;
		int pluto;

		bool queryShaderInfo = false;
		bool queryVertexShaderInfo = false;
		bool queryFragmentShaderInfo = false;
		bool setLightPosition = false;
		bool setScale = false;
		Vector3 lightPos = new Vector3();
		float scale = 0f;
		float currentScale = 1f;
		float minScale = 0.1f;
		float maxScale = 4.0f;

		bool rotatePlanet = false;
		float rotateSpeed = 1f;
		Vector3 axis = new Vector3(0f, 1f, 0f);

		protected override void init ()
		{
			textureSphere = new TextureSphere(0.8f, "Venus_Magellan_C3-MDIR_ClrTopo_Global_Mosaic_1024.jpg");
			programs.Add(Programs.AddProgram(VertexShaders.SimpleTexture, FragmentShaders.SimpleTexture));
			textureSphere.SetProgram(programs[0]);
			ClearDisplay();	
			//GLES20.glEnable(GLES20.GL_TEXTURE_2D);  //  GL_INVALID_ENUM ZTE
			SetupDepthAndCull();
			sun = Textures.Load(Planet.sunFileName);
			mercury = Textures.Load(Planet.mercuryFileName);
			venus = Textures.Load(Planet.venusFileName);
			earth = Textures.Load(Planet.earthFileName);
			mars = Textures.Load(Planet.marsFileName);
			jupiter = Textures.Load(Planet.jupiterFileName);
			saturn = Textures.Load(Planet.saturnFileName);
			uranus = Textures.Load(Planet.uranusFileName);
			neptune = Textures.Load(Planet.neptuneFileName);
			pluto = Textures.Load(Planet.plutoFileName);

			// test some other shaders
			programs.Add(Programs.AddProgram(VertexShaders.MatrixTexture, FragmentShaders.MatrixTexture));
			programs.Add(Programs.AddProgram(VertexShaders.MatrixTexture, FragmentShaders.MatrixTextureScale));
		}

		public override void display()
		{
			ClearDisplay();
			textureSphere.Draw();
			textureSphere.RotateShape(new Vector3(1f, 0f, 0f), 1f);
			if (nextPlanet)
			{
				nextPlanet = false;
				planet++;
				if(planet > 9) planet = 0;
				switch (planet)
				{
				case 0: textureSphere.SetTexture(sun); break;
				case 1: textureSphere.SetTexture(mercury); break;
				case 2: textureSphere.SetTexture(venus); break;
				case 3: textureSphere.SetTexture(earth); break;
				case 4: textureSphere.SetTexture(mars); break;
				case 5: textureSphere.SetTexture(jupiter); break;
				case 6: textureSphere.SetTexture(saturn); break;
				case 7: textureSphere.SetTexture(uranus); break;
				case 8: textureSphere.SetTexture(neptune); break;
				case 9: textureSphere.SetTexture(pluto); break;
				}
				//result.AppendLine("Planet = " + planet.ToString());
			}
			if (queryShaderInfo)
			{
				queryShaderInfo = false;
				//result.AppendLine("ShaderInfo" + Programs.dumpShaders());
			}
			if (queryVertexShaderInfo)
			{
				queryVertexShaderInfo = false;
				//result.AppendLine("KeyEvent", Programs.getVertexShaderInfo(programs.get(currentProgram)));
			}
			if (queryFragmentShaderInfo)
			{
				queryFragmentShaderInfo = false;
				//result.AppendLine("KeyEvent", Programs.getFragmentShaderInfo(programs.get(currentProgram)));
			}
			if (setLightPosition)
			{
				setLightPosition = false;
				Programs.SetLightPosition(programs[currentProgram], lightPos);
			}
			if (setScale)
			{
				setScale = false;
				Programs.SetUniformScale(programs[currentProgram], scale);
			}
			if (rotatePlanet)
			{
				textureSphere.RotateShape(axis, rotateSpeed);
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
					textureSphere.Move(new Vector3(0f, 0f, 1f));
					result.AppendLine("KeyEvent Move 1Z");
					break;
				case Keys.D2:
					textureSphere.Move(new Vector3(0f, 0f, -1f));
					result.AppendLine("KeyEvent Move -1Z");
					break;
				case Keys.D3:
					textureSphere.Move(new Vector3(0f, 0f, 10f));
					result.AppendLine("KeyEvent Move 10Z");
					break;
				case Keys.D4:
					textureSphere.Move(new Vector3(0f, 0f, -10f));
					result.AppendLine("KeyEvent Move -10Z");
					break;
				case Keys.D5:
					break;
				case Keys.D6:
					break;
				case Keys.A:
					setLightPosition = true;
					lightPos = Vector3.Add(lightPos, new Vector3(0f, 0f, 1f));
					break;
				case Keys.B:
					setLightPosition = true;
					lightPos = Vector3.Add(lightPos, new Vector3(0f, 0f, -1f));
					break;
				case Keys.D:
					scale *= 0.8f;
					setScale = true;
					break;
				case Keys.U:
					scale *= 1.2f;
					setScale = true;
					break;
				case Keys.N:
					nextPlanet = true;
					break;
				case Keys.P:
					currentProgram++;
					if (currentProgram >= programs.Count)
					{
						currentProgram = 0;
					}
					textureSphere.SetProgram(programs[currentProgram]);
					break;
				case Keys.F:
					queryFragmentShaderInfo = true;
					break;
				case Keys.R:
					if (rotatePlanet)
					{
						rotatePlanet = false;
					}
					else
					{
						rotatePlanet = true;
					}
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

		public void receiveMessage(String message)
		{
			/*
			String[] words = message.Split(' ');
			//result.AppendLine("Tut_TextureSphere2 " + message);
			switch (words[0])
			{
			case "scale":
				if (words.Length == 2) {
					scale = Float.parseFloat(words[1]);
					setScale = true;
				}
				break;
			case "lightPos":
				if (words.length == 4) {
					lightPos = new Vector3(Float.parseFloat(words[1]), Float.parseFloat(words[2]), Float.parseFloat(words[3]));
					setLightPosition = true;
				}
				break;
			case "MoveWorld":
				if (words.length == 4) {
					Vector3 Move = new Vector3(Float.parseFloat(words[1]), Float.parseFloat(words[2]), Float.parseFloat(words[3]));
					Shape.MoveWorld(Move);
				}
			case "SetWorldOffset":
				if (words.length == 4) {
					Vector3 worldOffset = new Vector3(Float.parseFloat(words[1]), Float.parseFloat(words[2]), Float.parseFloat(words[3]));
					Shape.setWorldOffset(worldOffset);
				}
			case "Program":
				if (words.length == 2) {
					currentProgram = Integer.parseInt(words[1]);
					if (currentProgram >= programs.size())
					{
						currentProgram = 0;
					}
					textureSphere.setProgram(programs.get(currentProgram));
				}
				break;
			default:
				break;
			}
			*/
		}


		public void setScaleSub(float scale)
		{
			currentScale = currentScale * scale;
			if (currentScale > maxScale) currentScale = maxScale;
			if (currentScale < minScale) currentScale = minScale;
			Shape.SetScale(currentScale);
		}
	}
}

