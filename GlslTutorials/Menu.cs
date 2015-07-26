using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public enum TutorialsEnum
	{

		Tut_Zoo,
		Tut_Hand,
		Tut_Skeleton,
		Tut_Dragonfly,
		Tut_Swarm,
		Tut_SphericalCoordinates,

		Tut_Tanks,

		Tut_FlightControls,
		Tut_SCubeGLSL,
		Tut_Dither,
		Tut_ColorTransform,
		Tut_Horizon,

		Tut_SCube,
		Tut_ShadowTexture,

		Tut_GlutTest,

		Tut_15_ManyImages2,
		Tut_15_ManyImages,
		Tut_FBO_Test,
		Tut_ShadowMap,

		Tut_PaintBox,
		Tut_PaintBoxSlave,

		Tut_TiltBall3D,
		Tut_TextureSphere2,
		Tut_TextureSphere,

		Tut_Collisions,
		Tut_Paddle,
		Tut_TiltBall,
		Tut_Ball,

		Tut_Orbit,
		Tut_StarMap,
		Tut_SolarSystem,

		Tut_Plane,
		Tut_Cans,
		Tut_Throw,
		Tut_Marbles,

		Tut_PaintBox2,

		Tut_RotationTest,
		Tut_RotateTexture,

		Tut_Buildings,
		Tut_Stars,

		Tut_TexturePerspective,
		Tut_WireFramePerspective,

		Tut_Tennis3D,
		Tut_MeshTransforms,
		Tut_Tennis,

		Tut_Texture,
		Tut_ViewPoleQuaternion,
		Tut_ObjectPoleQuaternion,
		Tut_TransformOrder,

		Tut_Quaternion,
		Tut_Projected_Light_Test,
		Tut_16_Gamma_Landscape,

		Tut_Spheres,

		Tut_ShaderTexture,
		
		Tut_3D_Shooter2,
		Tut_Blender,
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
		Tut_06_Hierarchy,
		Tut_07_World_Scene,
		Tut_08_Gimbal_Lock,
		Tut_09_Ambient_Lighting,
		Tut_10_Fragment_Point_Lighting,
		Tut_10_Fragment_Attenuation,

		Tut_12_Gamma_Correction,
		Tut_12_HDR_Lighting,
		Tut_14_Basic_Textures,

		Tut_17_Projected_Light,
		Tut_17_DoubleProjection,
		Tut_CheckObjects,
		Tut_RelativePositions,
		Tut_MoveMeshItem,
		
		Tut_Blocks,
		Tut_Vectors,
		Tut_Text,

		Tut_Camera,
		Tut_Colors,

		Tut_Texture_Tests,
		
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
			Programs.reset();
			Shape.ResetWorldToCameraMatrix();
			MatrixStack.rightMultiply = true;
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
				case TutorialsEnum.Tut_06_Hierarchy: currentTutorial = new Tut_06_Hierarchy(); break;
				case TutorialsEnum.Tut_07_World_Scene: currentTutorial = new Tut_07_World_Scene(); break;
				case TutorialsEnum.Tut_08_Gimbal_Lock: currentTutorial = new Tut_08_Gimbal_Lock(); break;
				case TutorialsEnum.Tut_09_Ambient_Lighting: currentTutorial = new Tut_09_Ambient_Lighting(); break;
				case TutorialsEnum.Tut_10_Fragment_Point_Lighting: currentTutorial = new Tut_10_Fragment_Point_Lighting(); break;
				case TutorialsEnum.Tut_10_Fragment_Attenuation: currentTutorial = new Tut_10_Fragment_Attenuation(); break;
				case TutorialsEnum.Tut_12_Gamma_Correction: currentTutorial = new Tut_12_Gamma_Correction(); break;
				case TutorialsEnum.Tut_12_HDR_Lighting: currentTutorial = new Tut_12_HDR_Lighting(); break;
				case TutorialsEnum.Tut_14_Basic_Textures: currentTutorial = new Tut_14_Basic_Textures(); break;
				case TutorialsEnum.Tut_15_ManyImages: currentTutorial = new Tut_15_ManyImages(); break;
				case TutorialsEnum.Tut_15_ManyImages2: currentTutorial = new Tut_15_ManyImages2(); break;
				case TutorialsEnum.Tut_16_Gamma_Landscape: currentTutorial = new Tut_16_Gamma_Landscape(); break;
				case TutorialsEnum.Tut_17_DoubleProjection: currentTutorial = new Tut_17_DoubleProjection(); break;
				case TutorialsEnum.Tut_17_Projected_Light: currentTutorial = new Tut_17_Projected_Light(); break;
				case TutorialsEnum.Tut_Camera: currentTutorial = new Tut_Camera(); break;		
				case TutorialsEnum.Tut_Blender: currentTutorial = new Tut_Blender(); break;		
				case TutorialsEnum.Tut_BlenderBinary: currentTutorial = new Tut_BlenderBinary(); break;		
				case TutorialsEnum.Tut_3D_Shooter: currentTutorial = new Tut_3D_Shooter(); break;
				case TutorialsEnum.Tut_3D_Shooter2: currentTutorial = new Tut_3D_Shooter2(); break;
				case TutorialsEnum.Tut_Vectors: currentTutorial = new Tut_Vectors(); break;	
				case TutorialsEnum.Tut_Text: currentTutorial = new Tut_Text(); break;	
				case TutorialsEnum.Tut_MultipleShaders: currentTutorial = new Tut_MultipleShaders(); break;	
				case TutorialsEnum.Tut_Texture: currentTutorial = new Tut_Texture(); break;	
				case TutorialsEnum.Tut_ShaderTexture: currentTutorial = new Tut_ShaderTexture(); break;	
				case TutorialsEnum.Tut_TextureSphere: currentTutorial = new Tut_TextureSphere(); break;	
				case TutorialsEnum.Tut_TextureSphere2: currentTutorial = new Tut_TextureSphere2(); break;	
				case TutorialsEnum.Tut_Colors: currentTutorial = new Tut_Colors(); break;
				case TutorialsEnum.Tut_Tanks: currentTutorial = new Tut_Tanks(); break;
				case TutorialsEnum.Tut_Tennis: currentTutorial = new Tut_Tennis(); break;
				case TutorialsEnum.Tut_Texture_Tests: currentTutorial = new Tut_Texture_Tests(); break;
				case TutorialsEnum.Tut_MoveMeshItem: currentTutorial = new Tut_MoveMeshItem(); break;
				case TutorialsEnum.Tut_MeshTransforms: currentTutorial = new Tut_MeshTransforms(); break;
				case TutorialsEnum.Tut_Tennis3D: currentTutorial = new Tut_Tennis3D(); break;
				case TutorialsEnum.Tut_Projected_Light_Test: currentTutorial = new Tut_Projected_Light_Test(); break;
				case TutorialsEnum.Tut_RelativePositions: currentTutorial = new Tut_RelativePositions(); break;
				case TutorialsEnum.Tut_Quaternion: currentTutorial = new Tut_Quaternion(); break;
				case TutorialsEnum.Tut_ObjectPoleQuaternion: currentTutorial = new Tut_ObjectPoleQuaternion(); break;
				case TutorialsEnum.Tut_TransformOrder: currentTutorial = new Tut_TransformOrder(); break;
				case TutorialsEnum.Tut_ViewPoleQuaternion: currentTutorial = new Tut_ViewPoleQuaternion(); break;
				case TutorialsEnum.Tut_RotateTexture: currentTutorial = new Tut_RotateTexture(); break;
				case TutorialsEnum.Tut_TexturePerspective: currentTutorial = new Tut_TexturePerspective(); break;
				case TutorialsEnum.Tut_WireFramePerspective: currentTutorial = new Tut_WireFramePerspective(); break;
				case TutorialsEnum.Tut_PaintBox: currentTutorial = new Tut_PaintBox(); break;
				case TutorialsEnum.Tut_PaintBox2: currentTutorial = new Tut_PaintBox2(); break;
				case TutorialsEnum.Tut_Stars: currentTutorial = new Tut_Stars(); break;
				case TutorialsEnum.Tut_Buildings: currentTutorial = new Tut_Buildings(); break;
				case TutorialsEnum.Tut_RotationTest: currentTutorial = new Tut_RotationTest(); break;
				case TutorialsEnum.Tut_Horizon: currentTutorial = new Tut_Horizon(); break;
				case TutorialsEnum.Tut_SolarSystem: currentTutorial = new Tut_SolarSystem(); break;
				case TutorialsEnum.Tut_Ball: currentTutorial = new Tut_Ball(); break;
				case TutorialsEnum.Tut_Marbles: currentTutorial = new Tut_Marbles(); break;
				case TutorialsEnum.Tut_Cans: currentTutorial = new Tut_Cans(); break;
				case TutorialsEnum.Tut_Throw: currentTutorial = new Tut_Throw(); break;
				case TutorialsEnum.Tut_Plane: currentTutorial = new Tut_Plane(); break;
				case TutorialsEnum.Tut_Orbit: currentTutorial = new Tut_Orbit(); break;
				case TutorialsEnum.Tut_StarMap: currentTutorial = new Tut_StarMap(); break;
				case TutorialsEnum.Tut_Paddle: currentTutorial = new Tut_Paddle(); break;
				case TutorialsEnum.Tut_Collisions: currentTutorial = new Tut_Collisions(); break;
				case TutorialsEnum.Tut_TiltBall3D: currentTutorial = new Tut_TiltBall3D(); break;
				case TutorialsEnum.Tut_PaintBoxSlave: currentTutorial = new Tut_PaintBoxSlave(); break;
				case TutorialsEnum.Tut_Dragonfly: currentTutorial = new Tut_Dragonfly(); break;
				case TutorialsEnum.Tut_Zoo: currentTutorial = new Tut_Zoo(); break;
				case TutorialsEnum.Tut_ShadowMap: currentTutorial = new Tut_ShadowMap(); break;
				case TutorialsEnum.Tut_ShadowTexture: currentTutorial = new Tut_ShadowTexture(); break;
				case TutorialsEnum.Tut_GlutTest: currentTutorial = new Tut_GlutTest(); break;
				case TutorialsEnum.Tut_FBO_Test: currentTutorial = new Tut_FBO_Test(); break;
				case TutorialsEnum.Tut_SCube: currentTutorial = new Tut_SCube(); break;
				case TutorialsEnum.Tut_SCubeGLSL: currentTutorial = new Tut_SCubeGLSL(); break;
				case TutorialsEnum.Tut_ColorTransform: currentTutorial = new Tut_ColorTransform(); break;
				case TutorialsEnum.Tut_Dither: currentTutorial = new Tut_Dither(); break;
				case TutorialsEnum.Tut_FlightControls: currentTutorial = new Tut_FlightControls(); break;
				case TutorialsEnum.Tut_SphericalCoordinates: currentTutorial = new Tut_SphericalCoordinates(); break;
				case TutorialsEnum.Tut_Swarm: currentTutorial = new Tut_Swarm(); break;
				case TutorialsEnum.Tut_Skeleton: currentTutorial = new Tut_Skeleton(); break;
				case TutorialsEnum.Tut_Hand: currentTutorial = new Tut_Hand(); break;
			}
		}
	}
}

