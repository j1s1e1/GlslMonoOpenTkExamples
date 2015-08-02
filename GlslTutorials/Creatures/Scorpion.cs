using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Scorpion : BugClass3d
	{
		List<Leg> legs;
		Leg tail;
		LitMatrixSphere2 body;
		int crawlCount = 0;

		private void AddLeg(Vector3 offset, Vector3[] rotationAxes, float[] rotationAngles, int crawlCountOffset)
		{
			Leg leg = new Leg(3);
			leg.Scale(new Vector3(0.05f, 0.1f, 0.05f));
			leg.RotateAngles(rotationAxes, rotationAngles);
			leg.Move(offset);
			leg.SetCrawlCountOffset(crawlCountOffset);
			legs.Add(leg);
		}

		public Scorpion(int x_in = 0, int y_in = 0, int z_in = 0): base(x_in, y_in, z_in)
		{
			Vector3[] rotationAxes = new Vector3[]{Vector3.UnitX, Vector3.UnitX, Vector3.UnitX};
			float[] rotationAngles = new float[]{-135f, -45f, 0f};
			legs = new List<Leg>();
			AddLeg(new Vector3(-0.2f, 0f, 0.6f), rotationAxes, rotationAngles, 0);
			AddLeg(new Vector3(-0.1f, 0f, 0.6f), rotationAxes, rotationAngles, 4);
			AddLeg(new Vector3(0.1f, 0f, 0.6f), rotationAxes, rotationAngles, 0);
			AddLeg(new Vector3(0.2f, 0f, 0.6f), rotationAxes, rotationAngles, 4);

			rotationAngles = new float[]{135f, 45f, 0f};
			AddLeg(new Vector3(-0.2f, 0f, 0.4f), rotationAxes, rotationAngles, 4);
			AddLeg(new Vector3(-0.1f, 0f, 0.4f), rotationAxes, rotationAngles, 0);
			AddLeg(new Vector3(0.1f, 0f, 0.4f), rotationAxes, rotationAngles, 4);
			AddLeg(new Vector3(0.2f, 0f, 0.4f), rotationAxes, rotationAngles, 0);

			tail = new Leg(5);
			tail.Scale(new Vector3(0.05f, 0.1f, 0.05f));
			tail.Move(new Vector3(-0.2f, 0.1f, 0.5f));
			rotationAxes = new Vector3[]{Vector3.UnitZ, Vector3.UnitZ, Vector3.UnitZ, Vector3.UnitZ, Vector3.UnitZ};
			rotationAngles = new float[]{-180f, -210f, -240f, -270f, -300f};
			tail.RotateAngles(rotationAxes, rotationAngles);

			body = new LitMatrixSphere2(0.1f);
			body.Scale(new Vector3(2.0f, 0.55f, 1.0f));
			body.Move(new Vector3(0f, 0.1f, 0.5f));
			movement = new BugMovement3D(new Vector3(0.02f, 0.02f, 0.02f));
			movement.SetLimits(new Vector3(-0.6f, -0.6f, -0.6f), new Vector3(0.6f, -0.4f, 0.6f));
		}

		public override void Draw()
		{
			foreach (Leg l in legs)
			{
				l.Draw();
			}
			tail.Draw();
			body.Draw();
			if (autoMove)
			{				
				crawlCount++;
				if (crawlCount > 31) 
				{
					crawlCount = 0;
				}
				foreach (Leg l in legs)
				{
					l.Move(Vector3.Subtract(position, lastPosition));
					l.Crawl(crawlCount/4);
				}
				tail.Move(Vector3.Subtract(position, lastPosition));
				tail.Crawl(crawlCount/4);
				body.Move(Vector3.Subtract(position, lastPosition));
				Move();
			}
		}
			
		public override void SetProgram(int program)
		{
			base.SetProgram(program);
			foreach(Leg l in legs)
			{
				l.SetProgram(program);
			}
			tail.SetProgram(program);
			body.SetProgram(program);
		}
	}
}

