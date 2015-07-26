using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class Tut_Hand : TutorialBase
	{
		List<Bone> bones;

		int boneToUpdate = 0;

		enum handBonesEnum
		{
			ARM,
			METACARPAL1,
			METACARPAL2,
			METACARPAL3,
			METACARPAL4,
			METACARPAL5,
			PROXIMALPHALANGE1,
			PROXIMALPHALANGE2,
			PROXIMALPHALANGE3,
			PROXIMALPHALANGE4,
			PROXIMALPHALANGE5,
			MIDDLEPHALANGE1,
			MIDDLEPHALANGE2,
			MIDDLEPHALANGE3,
			MIDDLEPHALANGE4,
			DISTALPHALANGE1,
			DISTALPHALANGE2,
			DISTALPHALANGE3,
			DISTALPHALANGE4,
			DISTALPHALANGE5,
		}

		protected override void init()
		{
			bones = new List<Bone>();
			Bone arm = new Bone();
			arm.Scale(new Vector3(0.05f, 0.5f, 0.05f));
			arm.Move(new Vector3(0.0f, 0.5f, 0.0f));
			bones.Add(arm);

			Bone metacarpal = new Bone();
			metacarpal.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			metacarpal.Rotate(Vector3.UnitZ, -40f);
			bones[(int)handBonesEnum.ARM].AddChild(metacarpal);
			bones.Add(metacarpal);

			metacarpal = new Bone();
			metacarpal.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			metacarpal.Rotate(Vector3.UnitZ, -20f);
			bones[(int)handBonesEnum.ARM].AddChild(metacarpal);
			bones.Add(metacarpal);

			metacarpal = new Bone();
			metacarpal.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			metacarpal.Rotate(Vector3.UnitZ, -0f);
			bones[(int)handBonesEnum.ARM].AddChild(metacarpal);
			bones.Add(metacarpal);

			metacarpal = new Bone();
			metacarpal.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			metacarpal.Rotate(Vector3.UnitZ, 20f);
			bones[(int)handBonesEnum.ARM].AddChild(metacarpal);
			bones.Add(metacarpal);

			metacarpal = new Bone();
			metacarpal.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			metacarpal.Rotate(Vector3.UnitZ, 40f);
			bones[(int)handBonesEnum.ARM].AddChild(metacarpal);
			bones.Add(metacarpal);

			Bone proximalPhalange = new Bone();
			proximalPhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			proximalPhalange.Rotate(Vector3.UnitZ, -40f);
			bones[(int)handBonesEnum.METACARPAL1].AddChild(proximalPhalange);
			bones.Add(proximalPhalange);

			proximalPhalange = new Bone();
			proximalPhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			proximalPhalange.Rotate(Vector3.UnitZ, -20f);
			bones[(int)handBonesEnum.METACARPAL2].AddChild(proximalPhalange);
			bones.Add(proximalPhalange);

			proximalPhalange = new Bone();
			proximalPhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			proximalPhalange.Rotate(Vector3.UnitZ, 0f);
			bones[(int)handBonesEnum.METACARPAL3].AddChild(proximalPhalange);
			bones.Add(proximalPhalange);

			proximalPhalange = new Bone();
			proximalPhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			proximalPhalange.Rotate(Vector3.UnitZ, 20f);
			bones[(int)handBonesEnum.METACARPAL4].AddChild(proximalPhalange);
			bones.Add(proximalPhalange);

			proximalPhalange = new Bone();
			proximalPhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			proximalPhalange.Rotate(Vector3.UnitZ, 40f);
			bones[(int)handBonesEnum.METACARPAL5].AddChild(proximalPhalange);
			bones.Add(proximalPhalange);

			Bone middlePhalange = new Bone();
			middlePhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			middlePhalange.Rotate(Vector3.UnitZ, -40f);
			bones[(int)handBonesEnum.PROXIMALPHALANGE1].AddChild(middlePhalange);
			bones.Add(middlePhalange);

			middlePhalange = new Bone();
			middlePhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			middlePhalange.Rotate(Vector3.UnitZ, -20f);
			bones[(int)handBonesEnum.PROXIMALPHALANGE2].AddChild(middlePhalange);
			bones.Add(middlePhalange);

			middlePhalange = new Bone();
			middlePhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			middlePhalange.Rotate(Vector3.UnitZ, 0f);
			bones[(int)handBonesEnum.PROXIMALPHALANGE3].AddChild(middlePhalange);
			bones.Add(middlePhalange);

			middlePhalange = new Bone();
			middlePhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			middlePhalange.Rotate(Vector3.UnitZ, 20f);
			bones[(int)handBonesEnum.PROXIMALPHALANGE4].AddChild(middlePhalange);
			bones.Add(middlePhalange);

			Bone distalPhalange = new Bone();
			distalPhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			distalPhalange.Rotate(Vector3.UnitZ, -40f);
			bones[(int)handBonesEnum.MIDDLEPHALANGE1].AddChild(distalPhalange);
			bones.Add(distalPhalange);

			distalPhalange = new Bone();
			distalPhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			distalPhalange.Rotate(Vector3.UnitZ, -20f);
			bones[(int)handBonesEnum.MIDDLEPHALANGE2].AddChild(distalPhalange);
			bones.Add(distalPhalange);

			distalPhalange = new Bone();
			distalPhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			distalPhalange.Rotate(Vector3.UnitZ, 0f);
			bones[(int)handBonesEnum.MIDDLEPHALANGE3].AddChild(distalPhalange);
			bones.Add(distalPhalange);

			distalPhalange = new Bone();
			distalPhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			distalPhalange.Rotate(Vector3.UnitZ, 20f);
			bones[(int)handBonesEnum.MIDDLEPHALANGE4].AddChild(distalPhalange);
			bones.Add(distalPhalange);

			distalPhalange = new Bone();
			distalPhalange.Scale(new Vector3(0.05f, 0.2f, 0.05f));
			distalPhalange.Rotate(Vector3.UnitZ, 40f);
			bones[(int)handBonesEnum.PROXIMALPHALANGE5].AddChild(distalPhalange);
			bones.Add(distalPhalange);

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

