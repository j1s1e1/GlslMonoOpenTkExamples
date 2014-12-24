using System;
using OpenTK;

namespace GlslTutorials
{
	public class Transform
	{
		public Transform()
		{
			m_orient = new Quaternion(1.0f, 0.0f, 0.0f, 0.0f);
			m_scale = new Vector3(1.0f, 1.0f, 1.0f);
			m_trans = new Vector3(0.0f, 0.0f, 0.0f);
		}

		public Matrix4 GetMatrix()
		{
			// FIXME check matrix orders
			Matrix4 ret;
			ret = Matrix4.CreateTranslation(m_trans);
			ret = Matrix4.Mult(ret, Matrix4.CreateFromQuaternion(m_orient));
			ret = Matrix4.Mult(ret, Matrix4.CreateScale(m_scale));
			return ret;
		}

		public Quaternion m_orient;
		public Vector3 m_scale;
		public Vector3 m_trans;
	};
}

