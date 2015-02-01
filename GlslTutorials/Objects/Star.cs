﻿using System;
using OpenTK;

namespace GlslTutorials
{
	public class Star
	{
		static Random random = new Random();
		LitMatrixSphere2 body;
		float angle;
		Vector3 center;
		Vector3 axis;

		public Star ()
		{
			body = new LitMatrixSphere2(0.05f);
			float x = (float)random.NextDouble() - 0.5f;
			float y = (float)random.NextDouble() - 0.5f;
			float z = (float)random.NextDouble()/2f;
			angle = 360f * (float)random.NextDouble();
			center = new Vector3(x, y, z);
			x = (float)random.NextDouble()/4;
			y = (float)random.NextDouble()/4;
			z = (float)random.NextDouble()/8;
			axis = new Vector3(x, y, z);
		}

		public void Draw()
		{
			Vector3 move = center + Vector3.Transform(Vector3.UnitY, Matrix4.CreateFromAxisAngle(axis, angle));
			angle += 0.1f;
			if (angle > 360f) angle -= 360f;
			body.SetOffset(move);
			body.Draw();
		}
	}
}

