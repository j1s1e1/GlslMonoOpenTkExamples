using System;
using OpenTK;

namespace GlslTutorials
{
	public class NodeRef
	{
		public virtual void NodeSetScale(Vector3 scale)
		{
			m_pNode.NodeSetScale(scale);
		}

		public virtual void NodeSetScale(float scale)
		{
			m_pNode.NodeSetScale(new Vector3(scale));
		}

		//Right-multiplies the given orientation to the current one.
		public virtual void NodeRotate(Quaternion orient)
		{
			m_pNode.NodeRotate(orient);
		}

		//Sets the current orientation to the given one.
		public virtual void NodeSetOrient(Quaternion orient)
		{
			m_pNode.NodeSetOrient(orient);
		}

		public virtual Quaternion NodeGetOrient()
		{
			return m_pNode.NodeGetOrient();
		}

		//Adds the offset to the current translation.
		public virtual void NodeOffset(Vector3 offset)
		{
			m_pNode.NodeOffset(offset);
		}

		//Sets the current translation to the given one.
		public virtual void NodeSetTrans(Vector3 offset)
		{
			m_pNode.NodeSetTrans(offset);
		}

		//This object does *NOT* claim ownership of the pointer.
		//You must ensure that it stays around so long as this Scene exists.
		public virtual void SetStateBinder(StateBinder pBinder)
		{
			m_pNode.SetStateBinder(pBinder);
		}

		public virtual int GetProgram()
		{
			return m_pNode.GetProgram();
		}

		public NodeRef(SceneNode pNode)
		{
			m_pNode = pNode;
		}
		SceneNode m_pNode;
	};

}

