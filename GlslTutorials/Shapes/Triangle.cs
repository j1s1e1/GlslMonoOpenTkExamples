using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Triangle
	{
		public Triangle()
		{
			//this(new Vector3(1f, 0f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f));
		}
		
		public Triangle(Vector3 aIn, Vector3 bIn, Vector3 cIn)
		{
			a = aIn;
			b = bIn;
			c = cIn;
		}
		
		Vector3 a;
		Vector3 b;
		Vector3 c;
		
		public List<Triangle> Divide()
		{
			List<Triangle> result = new List<Triangle>();
			Vector3 midpointOne = (a + b)/2;
			Vector3 midpointTwo = (b + c)/2;
			Vector3 midpointThree = (c + a)/2;
			result.Add(new Triangle(a, midpointOne, midpointThree));
			result.Add(new Triangle(midpointOne, midpointTwo, midpointThree));
			result.Add(new Triangle(midpointThree, midpointTwo, c));
			result.Add(new Triangle(midpointOne, b, midpointTwo));
			return result;
		}
		
		public float[] GetFloats()
		{
			float[] result = new float[9];
			Array.Copy(a.ToFloat(), 0, result, 0, 3);
			Array.Copy(b.ToFloat(), 0, result, 3, 3);
			Array.Copy(c.ToFloat(), 0, result, 6, 3);
			return result;
		}
		
		public Triangle Clone()
		{
			Triangle result = new Triangle();
			result.a = a.Clone();
			result.b = b.Clone();
			result.c = c.Clone();
			return result;
		}
		
		public Vector3[] GetVertices()
		{
			return new Vector3[] {a, b, c};
		}
	}
}

