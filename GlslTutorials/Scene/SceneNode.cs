using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class SceneNode
	{
		public SceneNode(SceneMesh pMesh, SceneProgram pProg, Vector3 nodePos, List<TextureBinding> texBindings)
		{ 
			m_pMesh = pMesh;
			m_pProg = pProg;
			m_texBindings = texBindings;
			m_nodeTm = new Transform();
			m_nodeTm.m_trans = nodePos;
		}

		public void NodeSetScale(Vector3 scale )
		{
			m_nodeTm.m_scale = scale;
		}

		public void NodeRotate( Quaternion orient )
		{
			m_nodeTm.m_orient = m_nodeTm.m_orient * orient;
		}

		public void NodeSetOrient( Quaternion orient )
		{
			m_nodeTm.m_orient = orient;
		}

		public Quaternion NodeGetOrient()
		{
			return m_nodeTm.m_orient;
		}

		public void SetNodeOrient(Quaternion nodeOrient)
		{
			m_nodeTm.m_orient =  nodeOrient.Normalized();
		}

		public void SetNodeScale(Vector3 nodeScale)
		{
			m_nodeTm.m_scale = nodeScale;
		}

		public void Render(List<int> samplers, Matrix4 baseMat)
		{
			baseMat = Matrix4.Mult(baseMat, m_nodeTm.GetMatrix());
			Matrix4 objMat = baseMat * m_objTm.GetMatrix();

			m_pProg.UseProgram();
			GL.UniformMatrix4(m_pProg.GetMatrixLoc(), false, ref objMat);

			if(m_pProg.GetNormalMatLoc() != -1)
			{
				Matrix3 normMat = new Matrix3(Matrix4.Transpose(Matrix4.Invert(objMat)));
				GL.UniformMatrix3(m_pProg.GetNormalMatLoc(), false, ref normMat);
			}

			//std::for_each(m_binders.begin(), m_binders.end(), BindBinder(m_pProg->GetProgram()));
			foreach(StateBinder sb in m_binders)
			{
				BindBinder bindBinder = new BindBinder(m_pProg.GetProgram());

				bindBinder.StateBinder(sb); 
			}
			for(int texIx = 0; texIx < m_texBindings.Count; ++texIx)
			{
				TextureBinding binding = m_texBindings[texIx];
				GL.ActiveTexture(TextureUnit.Texture0 + binding.texUnit);
				GL.BindTexture(binding.pTex.GetTextureType(), binding.pTex.GetTexture());
				GL.BindSampler(binding.texUnit, samplers[(int)binding.sampler]);
			}

			m_pMesh.Render();

			for(int texIx = 0; texIx < m_texBindings.Count; ++texIx)
			{
				TextureBinding binding = m_texBindings[texIx];
				GL.ActiveTexture(TextureUnit.Texture0  + binding.texUnit);
				GL.BindTexture(binding.pTex.GetTextureType(), 0);
				GL.BindSampler(binding.texUnit, 0);
			}

			foreach(StateBinder sb in m_binders)
			{
				UnbindBinder unbindBinder = new UnbindBinder(m_pProg.GetProgram());
				unbindBinder.StateBinder(sb);
			}
			GL.UseProgram(0);
		}

		public void NodeOffset(Vector3 offset)
		{
			m_nodeTm.m_trans += offset;
		}

		public void NodeSetTrans(Vector3 offset)
		{
			m_nodeTm.m_trans = offset;
		}

		public void SetStateBinder(StateBinder pBinder)
		{
			m_binders.Add(pBinder);
		}

		public int GetProgram()
		{
			return m_pProg.GetProgram();
		}
			
		private SceneMesh m_pMesh;		//Unmanaged. We are deleted first, so these should always be real values.
		private SceneProgram m_pProg;	//Unmanaged. We are deleted first, so these should always be real values.

		private List<StateBinder> m_binders = new List<StateBinder>();	//Unmanaged. These live beyond us.
		private List<TextureBinding> m_texBindings = new List<TextureBinding>();

		private Transform m_nodeTm = new Transform();
		private Transform m_objTm = new Transform();
	}

}

