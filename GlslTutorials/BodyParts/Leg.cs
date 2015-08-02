using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class Leg
	{
		List<Bone> bones;

		int crawlCountOffset = 0;

		static Leg()
		{
			SetCrawlRotations();
		}
		
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

		public void RotateAngles(Matrix4[] rotations)
		{
			for (int i = 0; i < bones.Count; i++)
			{
				if (i < rotations.Length) bones[i].Rotate(rotations[i]);
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

		static Matrix4[][] crawlRotations;

		private static Matrix4[] CreateRotationsX(float[] anglesDeg)
		{
			Matrix4[] result = new Matrix4[anglesDeg.Length];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = Matrix4.CreateRotationX((float)Math.PI / 180.0f * anglesDeg[i]);
			}
			return result;
		}

		private static void SetCrawlRotations()
		{
			crawlRotations = new Matrix4[8][];
			crawlRotations[0] = CreateRotationsX(new float[]{2f, 2f, 0f});
			crawlRotations[1] = CreateRotationsX(new float[]{3f, 3f, 0f});
			crawlRotations[2] = CreateRotationsX(new float[]{-2f, -2f, 0f});
			crawlRotations[3] = CreateRotationsX(new float[]{-3f, -3f, 0f});
			crawlRotations[4] = CreateRotationsX(new float[]{0f, 0f, 0f});
			crawlRotations[5] = CreateRotationsX(new float[]{0f, 0f, 0f});
			crawlRotations[6] = CreateRotationsX(new float[]{0f, 0f, 0f});
			crawlRotations[7] = CreateRotationsX(new float[]{0f, 0f, 0f});

		}

		public void Crawl(int crawlCount)
		{
			RotateAngles(crawlRotations[(crawlCount + crawlCountOffset) % 8]);
		}
	}
}

