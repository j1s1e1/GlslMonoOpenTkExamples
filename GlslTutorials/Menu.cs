using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public enum TutorialsEnum
	{
		Tut_Blocks,
		Tut_02_Vertex_Colors,
		Tut_03_CPU_Position_Offset,
		Tut_03_Shader_Calc_Offset,
		Tut_04_MatrixPerspective,
		Tut_05_Depth_Buffering,
		Tut_06_Rotations,
		Tut_06_Scale,
		Tut_06_Translation,
		Tut_TiltBall,
		Tut_Spheres,
	}
	public class MenuClass
	{
		public static List<string> FillTestlist()
		{
			List<string> TestList = new List<string>();
			TestList.AddRange(Enum.GetNames(typeof(TutorialsEnum)));
			return TestList;
		}
		
		public static void StartTutorial(ref TutorialBase currentTutorial, TutorialsEnum tutorialSelection)
		{
			switch (tutorialSelection)
			{
				case TutorialsEnum.Tut_02_Vertex_Colors: currentTutorial = new Tut_02_Vertex_Colors(); break;
				case TutorialsEnum.Tut_03_CPU_Position_Offset: currentTutorial = new Tut_03_CPU_Position_Offset(); break;
				case TutorialsEnum.Tut_03_Shader_Calc_Offset: currentTutorial = new Tut_03_Shader_Calc_Offset(); break;
				case TutorialsEnum.Tut_04_MatrixPerspective: currentTutorial = new Tut_04_MatrixPerspective(); break;
				case TutorialsEnum.Tut_05_Depth_Buffering: currentTutorial = new Tut_05_Depth_Buffering(); break;
				case TutorialsEnum.Tut_06_Rotations: currentTutorial = new Tut_06_Rotations(); break;
				case TutorialsEnum.Tut_06_Scale: currentTutorial = new Tut_06_Scale(); break;
				case TutorialsEnum.Tut_06_Translation: currentTutorial = new Tut_06_Translation(); break;
				case TutorialsEnum.Tut_TiltBall: currentTutorial = new Tut_TiltBall(); break;
				case TutorialsEnum.Tut_Spheres: currentTutorial = new Tut_Spheres(); break;
				case TutorialsEnum.Tut_Blocks: currentTutorial = new Tut_Blocks(); break;
			}
		}
	}
}

