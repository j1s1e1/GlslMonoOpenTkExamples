using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Leg
	{
		List<Bone> bones;

		int crawlCountOffset = 0;
		
		public Leg (int boneCount)
		{
			bones = new List<Bone>();
			for (int i = 0; i < boneCount; i++)
			{
				Bone bone = new Bone();
				if (i > 0)
				{
					bones[i-1].AddChild(bone);
				}
				bones.Add(bone);
			}
		}

		public void Scale(Vector3 v)
		{
			foreach (Bone b in bones)
			{
				b.Scale(v);
			}
		}

		public void Move(Vector3 v)
		{
			bones[0].Move(v);
		}

		public void RotateAngles(Vector3[] axis, float[] angles)
		{
			for (int i = 0; i < bones.Count; i++)
			{
				if (i < axis.Length) bones[i].Rotate(axis[i], angles[i]);
			}
		}

		public void Draw()
		{
			foreach (Bone b in bones)
			{
				b.Draw();
			}
		}

		public void SetProgram(int program)
		{
			foreach (Bone b in bones)
			{
				b.SetProgram(program);
			}
		}

		public void SetCrawlCountOffset(int crawlCountOffsetIn)
		{
			crawlCountOffset = crawlCountOffsetIn;
		}

		public void Crawl(int crawlCount)
		{
			switch ((crawlCount + crawlCountOffset) % 8)
			{
			case 0:	
				RotateAngles(new Vector3[]{Vector3.UnitX, Vector3.UnitX, Vector3.UnitX}, new float[]{2f, 2f, 0f}); 
				break;
			case 1:	
				RotateAngles(new Vector3[]{Vector3.UnitX, Vector3.UnitX, Vector3.UnitX}, new float[]{3f, 3f, 0f}); 
				break;
			case 2:	
				RotateAngles(new Vector3[]{Vector3.UnitX, Vector3.UnitX, Vector3.UnitX}, new float[]{-2f, -2f, 0f}); 
				break;
			case 3:	
				RotateAngles(new Vector3[]{Vector3.UnitX, Vector3.UnitX, Vector3.UnitX}, new float[]{-3f, -3f, 0f}); 
				break;
			case 4:	
				RotateAngles(new Vector3[]{Vector3.UnitX, Vector3.UnitX, Vector3.UnitX}, new float[]{0f, 0f, 0f}); 
				break;
			case 5:	
				RotateAngles(new Vector3[]{Vector3.UnitX, Vector3.UnitX, Vector3.UnitX}, new float[]{0f, 0f, 0f}); 
				break;
			case 6:	
				RotateAngles(new Vector3[]{Vector3.UnitX, Vector3.UnitX, Vector3.UnitX}, new float[]{0f, 0f, 0f}); 
				break;
			case 7:	
				RotateAngles(new Vector3[]{Vector3.UnitX, Vector3.UnitX, Vector3.UnitX}, new float[]{0f, 0f, 0f}); 
				break;
			}
		}
	}
}

