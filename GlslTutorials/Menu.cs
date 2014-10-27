using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public enum TutorialsEnum
	{		Tut_Blender,
		Tut_BlenderBinary,
		Tut_3D_Shooter,
		Tut_SingleMeshItem,
		
		Tut_MultipleShaders,
		Tut_02_Vertex_Colors,
		Tut_03_CPU_Position_Offset,
		Tut_03_Shader_Calc_Offset,
		Tut_04_MatrixPerspective,
		Tut_05_Depth_Buffering,
		Tut_06_Rotations,
		Tut_06_Scale,
		Tut_06_Translation,
		Tut_07_World_Scene,
		Tut_08_Gimbal_Lock,
		Tut_09_Ambient_Lighting,
		Tut_10_Fragment_Point_Lighting,
		Tut_CheckObjects,
		Tut_Spheres,
		Tut_Blocks,
		Tut_Vectors,
		Tut_Text,
		Tut_TiltBall,

		Tut_Camera,
		Tut_3D_Shooter2,
		
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
				case TutorialsEnum.Tut_CheckObjects: currentTutorial = new Tut_CheckObjects(); break;
				case TutorialsEnum.Tut_SingleMeshItem: currentTutorial = new Tut_SingleMeshItem(); break;
				case TutorialsEnum.Tut_07_World_Scene: currentTutorial = new Tut_07_World_Scene(); break;
				case TutorialsEnum.Tut_08_Gimbal_Lock: currentTutorial = new Tut_08_Gimbal_Lock(); break;
				case TutorialsEnum.Tut_09_Ambient_Lighting: currentTutorial = new Tut_09_Ambient_Lighting(); break;
				case TutorialsEnum.Tut_10_Fragment_Point_Lighting: currentTutorial = new Tut_10_Fragment_Point_Lighting(); break;
				case TutorialsEnum.Tut_Camera: currentTutorial = new Tut_Camera(); break;		
				case TutorialsEnum.Tut_Blender: currentTutorial = new Tut_Blender(); break;		
				case TutorialsEnum.Tut_BlenderBinary: currentTutorial = new Tut_BlenderBinary(); break;		
				case TutorialsEnum.Tut_3D_Shooter: currentTutorial = new Tut_3D_Shooter(); break;
				case TutorialsEnum.Tut_3D_Shooter2: currentTutorial = new Tut_3D_Shooter2(); break;
				case TutorialsEnum.Tut_Vectors: currentTutorial = new Tut_Vectors(); break;	
				case TutorialsEnum.Tut_Text: currentTutorial = new Tut_Text(); break;	
				case TutorialsEnum.Tut_MultipleShaders: currentTutorial = new Tut_MultipleShaders(); break;	
			}
		}
	}
}

