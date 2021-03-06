﻿using System;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tut_Dragonfly : TutorialBase
	{
		Dragonfly3d dragonfly;
		int dragonflyProgram;
		int sphericalProgram;
		bool automove = true;

		protected override void init()
		{
			dragonfly = new Dragonfly3d(0, 0, 0);
			dragonflyProgram = dragonfly.GetProgram();
			sphericalProgram = Programs.AddProgram(VertexShaders.spherical_lms, FragmentShaders.lms_fragmentShaderCode);
			SetupDepthAndCull();
		}

		public override void display()
		{
			ClearDisplay();
			dragonfly.Draw();
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			switch (keyCode)
			{
			case Keys.A:
				if (automove)
				{
					dragonfly.ClearAutoMove();
					automove = false;
				}
				else
				{
					dragonfly.SetAutoMove();
					automove = true;
				}
				break;
			case Keys.B:
				dragonfly.SetProgram(dragonflyProgram);
				dragonfly.SetBug2DMovement();
				break;
			case Keys.C:
				dragonfly.ChangeRadius(0.1f);
				break;
			case Keys.I:
				result.AppendLine(dragonfly.GetMovementInfo());
				break;
			case Keys.O:
				dragonfly.Translate(new Vector3(0.2f, 0.2f, 0.2f));
				break;
			case Keys.P:
				dragonfly.Translate(new Vector3(-0.2f, -0.2f, -0.2f));
				break;
			case Keys.R:
				dragonfly.SetProgram(sphericalProgram);
				dragonfly.SetSphericalMovement(sphericalProgram, 0.5f, 1f);
				break;
			case Keys.S:
				dragonfly.SetProgram(sphericalProgram);
				dragonfly.SetSphericalMovement(sphericalProgram, 1f, 0f);
				break;
			case Keys.T:
				dragonfly.SetProgram(sphericalProgram);
				dragonfly.SetSphericalMovement(sphericalProgram, 0f, 1f);
				break;
			}
			return result.ToString();
		}
	}
}

