using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tut_Skeleton : TutorialBase
	{
		List<Bone> bones;

		int boneToUpdate = 0;

		protected override void init()
		{
			bones = new List<Bone>();
			Bone bone = new Bone();
			bone.Scale(new Vector3(0.2f, 1f, 0.2f));
			bones.Add(bone);
			bone = new Bone();
			bone.Scale(new Vector3(0.2f, 0.5f, 0.2f));
			bone.Move(new Vector3(0.3f, 0f, 0f));
			bones[0].AddChild(bone);
			bones.Add(bone);
			SetupDepthAndCull();
		}


		public override void display()
		{
			ClearDisplay();
			foreach(Bone b in bones)
			{
				b.Draw();
			}
		}

		private void Rotate(Vector3 v, float angle)
		{
			bones[boneToUpdate].Rotate(v, angle);
		}

		private void Scale(Vector3 v)
		{
			bones[boneToUpdate].Scale(v);
		}

		private void Move(Vector3 v)
		{
			bones[boneToUpdate].Move(v);
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			switch (keyCode)
			{
			case Keys.B:
				boneToUpdate++;
				if (boneToUpdate > (bones.Count - 1))
				{
					boneToUpdate = 0;
				}
				result.AppendLine("boneToUpdate = " + boneToUpdate.ToString());
				break;
			case Keys.D1:  Rotate(Vector3.UnitX, 5f);   break;
			case Keys.D2:  Rotate(Vector3.UnitX, -5f);  break;
			case Keys.D3:  Rotate(Vector3.UnitY, 5f);   break;
			case Keys.D4:  Rotate(Vector3.UnitY, -5f);  break;
			case Keys.D5:  Rotate(Vector3.UnitZ, 5f);   break;
			case Keys.D6:  Rotate(Vector3.UnitZ, -5f);  break;
			case Keys.D7:  Scale(new Vector3(0.9f, 0.9f, 0.9f));  break;
			case Keys.D8:  Scale(new Vector3(1.1f, 1.1f, 1.1f));  break;
			case Keys.D9:  Move(new Vector3(0.1f, 0.1f, 0.1f));  break;
			case Keys.D0:  Move(new Vector3(-0.1f, -0.1f, -0.1f));  break;
			case Keys.I:
				foreach(Bone b in bones)
				{
					result.Append(b.GetBoneInfo());
				}
				break;
			}
			result.AppendLine(keyCode.ToString());
			return result.ToString();
		}
	}
}

