using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace GlslTutorials
{
	public class Bone
	{
		List<Shape> shapes;
		Vector3 sphere1position;
		Vector3 sphere2position;
		Bone parent;
		List<Bone> children = new List<Bone>();

		public Bone()
		{
			shapes = new List<Shape>();
			shapes.Add(new LitMatrixSphere2(0.1f));
			shapes.Add(new Octahedron(new Vector3(1f, 1f, 1f), Colors.RED_COLOR));
			shapes.Add(new LitMatrixSphere2(0.1f));
			shapes[0].SetYOffset(0.45f);
			shapes[2].SetYOffset(-0.45f);
			sphere1position = shapes[0].GetOffset();
			sphere2position = shapes[2].GetOffset();
		}

		public void Draw()
		{
			foreach(Shape s in shapes)
			{
				s.Draw();
			}
		}

		public void Rotate(Vector3 axis, float angleDeg)
		{
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(axis, (float)Math.PI / 180.0f * angleDeg);
			foreach(Shape s in shapes)
			{
				s.RotateShape(sphere1position, rotation);
			}
			sphere2position = sphere2position - sphere1position;
			sphere2position = Vector3.TransformVector(sphere2position, rotation);
			sphere2position = sphere2position + sphere1position;
			if (children.Count != 0)
			{
				foreach (Bone child in children)
				{
					child.Move(Vector3.Subtract(sphere2position, child.GetSphere1Position()));
				}
			}
		}

		public void Rotate(Matrix4 rotation)
		{
			foreach(Shape s in shapes)
			{
				s.RotateShape(sphere1position, rotation);
			}
			sphere2position = sphere2position - sphere1position;
			sphere2position = Vector3.TransformVector(sphere2position, rotation);
			sphere2position = sphere2position + sphere1position;
			if (children.Count != 0)
			{
				foreach (Bone child in children)
				{
					child.Move(Vector3.Subtract(sphere2position, child.GetSphere1Position()));
				}
			}
		}

		public void Scale(Vector3 scale)
		{
			foreach(Shape s in shapes)
			{
				s.Scale(scale);
			}
			sphere1position.X = sphere1position.X * scale.X;
			sphere1position.Y = sphere1position.Y * scale.Y;
			sphere1position.Z = sphere1position.Z * scale.Z;
			sphere2position.X = sphere2position.X * scale.X;
			sphere2position.Y = sphere2position.Y * scale.Y;
			sphere2position.Z = sphere2position.Z * scale.Z;
			if (children.Count != 0)
			{
				foreach (Bone child in children)
				{
					child.Move(Vector3.Subtract(sphere2position, child.GetSphere1Position()));
				}
			}
		}

		public void Move(Vector3 offset)
		{
			foreach(Shape s in shapes)
			{
				s.Move(offset);
			}
			sphere1position = Vector3.Add(sphere1position, offset);
			sphere2position = Vector3.Add(sphere2position, offset);
			if (children.Count != 0)
			{
				foreach (Bone child in children)
				{
					child.Move(Vector3.Subtract(sphere2position, child.GetSphere1Position()));
				}
			}
		}

		public Vector3 GetSphere1Position()
		{
			return sphere1position;
		}

		public void SetParent(Bone parentIn)
		{
			parent = parentIn;
		}

		public void AddChild(Bone childIn)
		{
			childIn.Move(Vector3.Subtract(sphere2position, childIn.GetSphere1Position()));
			childIn.SetParent(this);
			children.Add(childIn);
		}

		public string GetBoneInfo()
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine("sphere1position = " + sphere1position.ToString());
			result.AppendLine("sphere2position = " + sphere2position.ToString());
			return result.ToString();
		}

		public Bone GetParent()
		{
			return parent;
		}

		public void SetProgram(int program)
		{
			foreach(Shape s in shapes)
			{
				s.SetProgram(program);
			}
		}
	}
}

