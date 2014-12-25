using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Scene
	{
		//One for the ground, and one for each of the 5 objects.
		int MATERIAL_COUNT = 6;

		Mesh m_pTerrainMesh;
		Mesh m_pCubeMesh;
		Mesh m_pTetraMesh;
		Mesh m_pCylMesh;
		Mesh m_pSphereMesh;

		public delegate SceneProgramData GetProgramDelegate(LightingProgramTypes eType);
		static public GetProgramDelegate GetProgramFromTutorial;

		List<MaterialBlock> materials;

		SceneProgramData GetProgram(LightingProgramTypes eType)
		{
			return GetProgramFromTutorial(eType);
		}

		static void GetMaterials( List<MaterialBlock> materials )
		{
			//Ground
			materials.Add(new MaterialBlock(Vector4.One, new Vector4(0.5f, 0.5f, 0.5f, 1.0f), 0.6f));

			//Tetrahedron
			materials.Add(new MaterialBlock(Vector4.One * 0.5f, new Vector4(0.5f, 0.5f, 0.5f, 1.0f), 0.05f));

			//Monolith
			materials.Add(new MaterialBlock(Vector4.One * 0.05f, new Vector4(0.95f, 0.95f, 0.95f, 1.0f), 0.4f));

			//Cube
			materials.Add(new MaterialBlock(Vector4.One * 0.5f, new Vector4(0.3f, 0.3f, 0.3f, 1.0f), 0.1f));

			//Cylinder
			materials.Add(new MaterialBlock(Vector4.One * 0.5f, new Vector4(0.0f, 0.0f, 0.0f, 1.0f), 0.6f));

			//Sphere
			materials.Add(new MaterialBlock(new Vector4(0.63f, 0.60f, 0.02f, 1.0f), 
				new Vector4(0.22f, 0.20f, 0.0f, 1.0f), 0.3f));
		}

		public Scene()
		{
			string XmlFilesDirectory = GlsTutorialsClass.ProjectDirectory + @"/XmlFilesForMeshes";

			Stream ground =  File.OpenRead(XmlFilesDirectory + @"/ground.xml");
			m_pTerrainMesh = new Mesh(ground);

			Stream unitcube =  File.OpenRead(XmlFilesDirectory + @"/unitcubehdr.xml");
			m_pCubeMesh = new Mesh(unitcube);

			Stream unittetrahedron =  File.OpenRead(XmlFilesDirectory + @"/unittetrahedron.xml");
			m_pTetraMesh = new Mesh(unittetrahedron);

			Stream unitcylinder =  File.OpenRead(XmlFilesDirectory + @"/unitcylinder.xml");
			m_pCylMesh = new Mesh(unitcylinder);

			Stream unitsphere =  File.OpenRead(XmlFilesDirectory + @"/unitsphere12.xml");
			m_pSphereMesh = new Mesh(unitsphere);

			////Align the size of each MaterialBlock to the uniform buffer alignment.
			//int uniformBufferAlignSize = 0;
			//glGetIntegerv(GL_UNIFORM_BUFFER_OFFSET_ALIGNMENT, &uniformBufferAlignSize);

			//m_sizeMaterialBlock = sizeof(MaterialBlock);
			//m_sizeMaterialBlock += uniformBufferAlignSize -
			//	(m_sizeMaterialBlock % uniformBufferAlignSize);

			//int sizeMaterialUniformBuffer = m_sizeMaterialBlock * MATERIAL_COUNT;

			materials = new List<MaterialBlock>();
			GetMaterials(materials);
			//assert(materials.size() == MATERIAL_COUNT);

			//List<byte> mtlBuffer;
			//mtlBuffer.resize(sizeMaterialUniformBuffer, 0);

			//GLubyte *bufferPtr = &mtlBuffer[0];

			//for(size_t mtl = 0; mtl < materials.size(); ++mtl)
			//	memcpy(bufferPtr + (mtl * m_sizeMaterialBlock), &materials[mtl], sizeof(MaterialBlock));

			//glGenBuffers(1, &m_materialUniformBuffer);
			//glBindBuffer(GL_UNIFORM_BUFFER, m_materialUniformBuffer);
			//glBufferData(GL_UNIFORM_BUFFER, sizeMaterialUniformBuffer, bufferPtr, GL_STATIC_DRAW);
			//glBindBuffer(GL_UNIFORM_BUFFER, 0);
		}

		public void Draw(MatrixStack modelMatrix, int materialBlockIndex, float alphaTetra )
		{
			//Render the ground plane.
			using ( PushStack pushstack = new PushStack(modelMatrix))
			{
				modelMatrix.RotateX(-90);

				DrawObject(m_pTerrainMesh, GetProgram(LightingProgramTypes.LP_VERT_COLOR_DIFFUSE), materialBlockIndex, 0,
					modelMatrix);
			}

			//Render the tetrahedron object.
			using ( PushStack pushstack = new PushStack(modelMatrix))
			{
				modelMatrix.Translate(75.0f, 5.0f, 75.0f);
				modelMatrix.RotateY(360.0f * alphaTetra);
				modelMatrix.Scale(10.0f, 10.0f, 10.0f);
				modelMatrix.Translate(0.0f, (float)Math.Sqrt(2.0f), 0.0f);
				modelMatrix.Rotate(new Vector3(-0.707f, 0.0f, -0.707f), 54.735f);

				DrawObject(m_pTetraMesh, "lit-color", 
					GetProgram(LightingProgramTypes.LP_VERT_COLOR_DIFFUSE_SPECULAR), 
					materialBlockIndex, 1, modelMatrix);
			}

			//Render the monolith object.
			using ( PushStack pushstack = new PushStack(modelMatrix))
			{
				modelMatrix.Translate(88.0f, 5.0f, -80.0f);
				modelMatrix.Scale(4.0f, 4.0f, 4.0f);
				modelMatrix.Scale(4.0f, 9.0f, 1.0f);
				modelMatrix.Translate(0.0f, 0.5f, 0.0f);

				DrawObject(m_pCubeMesh, "lit", GetProgram(LightingProgramTypes.LP_MTL_COLOR_DIFFUSE_SPECULAR),
					materialBlockIndex, 2, modelMatrix);
			}

			//Render the cube object.
			using ( PushStack pushstack = new PushStack(modelMatrix))
			{
				modelMatrix.Translate(-52.5f, 14.0f, 65.0f);
				modelMatrix.RotateZ(50.0f);
				modelMatrix.RotateY(-10.0f);
				modelMatrix.Scale(20.0f, 20.0f, 20.0f);

				DrawObject(m_pCubeMesh, "lit-color", GetProgram(LightingProgramTypes.LP_VERT_COLOR_DIFFUSE_SPECULAR),
					materialBlockIndex, 3, modelMatrix);
			}

			//Render the cylinder.
			using ( PushStack pushstack = new PushStack(modelMatrix))
			{
				modelMatrix.Translate(-7.0f, 30.0f, -14.0f);
				modelMatrix.Scale(15.0f, 55.0f, 15.0f);
				modelMatrix.Translate(0.0f, 0.5f, 0.0f);

				DrawObject(m_pCylMesh, "lit-color", GetProgram(LightingProgramTypes.LP_VERT_COLOR_DIFFUSE_SPECULAR),
					materialBlockIndex, 4, modelMatrix);
			}

			//Render the sphere.
			using ( PushStack pushstack = new PushStack(modelMatrix))
			{
				modelMatrix.Translate(-83.0f, 14.0f, -77.0f);
				modelMatrix.Scale(20.0f, 20.0f, 20.0f);

				DrawObject(m_pSphereMesh, "lit", GetProgram(LightingProgramTypes.LP_MTL_COLOR_DIFFUSE_SPECULAR),
					materialBlockIndex, 5, modelMatrix);
			}
		}

		public void DrawObject(Mesh pMesh, SceneProgramData prog, int materialBlockIndex, int mtlIx,
			MatrixStack modelMatrix )
		{
			Matrix3 normMatrix = new Matrix3(modelMatrix.Top());
			normMatrix = Matrix3.Transpose(normMatrix.Inverted());

			GL.UseProgram(prog.theProgram);
			Matrix4 mm =  modelMatrix.Top();

			GL.UniformMatrix4(prog.modelToCameraMatrixUnif, false, ref mm);

			GL.UniformMatrix3(prog.normalModelToCameraMatrixUnif, false, ref normMatrix);

			// Apply Material
			materials[mtlIx].SetUniforms(prog.theProgram);
			materials[mtlIx].UpdateInternal();

			pMesh.Render();
			GL.UseProgram(0);
		}

		public void DrawObject(Mesh pMesh, string meshName, SceneProgramData prog, int materialBlockIndex, int mtlIx,
			MatrixStack modelMatrix)
		{
			Matrix3 normMatrix = new Matrix3(modelMatrix.Top());
			normMatrix = Matrix3.Transpose(normMatrix.Inverted());

			GL.UseProgram(prog.theProgram);
			// Apply Material
			materials[mtlIx].SetUniforms(prog.theProgram);
			materials[mtlIx].UpdateInternal();

			Matrix4 mm = modelMatrix.Top();
			GL.UniformMatrix4(prog.modelToCameraMatrixUnif, false, ref mm);

			GL.UniformMatrix3(prog.normalModelToCameraMatrixUnif, false, ref normMatrix);
			pMesh.Render(meshName);
			GL.UseProgram(0);
		}

		public Mesh GetSphereMesh()
		{
			return m_pSphereMesh;
		}

		public Mesh GetCubeMesh() 
		{
			return m_pCubeMesh;
		}
	}
}

