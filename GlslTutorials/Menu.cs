using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public enum TutorialsEnum
	{
		Tut_02_Vertex_Colors,
		Tut_03_CPU_Position_Offset,
		Tut_TiltBall
	}
	public class MenuClass
	{
		public MenuClass ()
		{
		}
		
		
		
		public static List<string> FillTestlist()
		{
			List<string> TestList = new List<string>();
			
			TestList.Add(TutorialsEnum.Tut_02_Vertex_Colors.ToString());
			TestList.Add(TutorialsEnum.Tut_03_CPU_Position_Offset.ToString());
			TestList.Add(TutorialsEnum.Tut_TiltBall.ToString());
			return TestList;
		}
		
		public static void StartTutorial(ref TutorialBase currentTutorial, TutorialsEnum tutorialSelection)
		{
			switch (tutorialSelection)
			{
				case TutorialsEnum.Tut_02_Vertex_Colors: currentTutorial = new Tut_02_Vertex_Colors(); break;
				case TutorialsEnum.Tut_03_CPU_Position_Offset: currentTutorial = new Tut_03_CPU_Position_Offset(); break;
				case TutorialsEnum.Tut_TiltBall: currentTutorial = new Tut_TiltBall(); break;
			}
		}
	}
}

