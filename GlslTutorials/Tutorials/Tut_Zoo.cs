using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Zoo : TutorialBase
	{
		List<Animal> animals;
		Exhibit exhibit;
		int sphericalProgram;
		int sphericalTextureProgram;
		float yRotation = 0;

		protected override void init()
		{
			sphericalProgram = Programs.AddProgram(VertexShaders.spherical_lms, FragmentShaders.lms_fragmentShaderCode);
			sphericalTextureProgram = Programs.AddProgram(VertexShaders.SphericalMatrixTexture, FragmentShaders.MatrixTextureTest);
			animals = new List<Animal>();
			animals.Add(new Butterfly());
			exhibit = new Exhibit();
			GL.Enable(EnableCap.DepthTest);
		}

		public override void display()
		{
			ClearDisplay();
			exhibit.Draw();
			foreach(Animal a in animals)
			{
				a.Draw();
			}
		}

		enum AnimalsEnum
		{
			CAT,
			DOG,
			DRAGONFLY,
			LADYBUG,
			FIREFLY,
			SCORPION,
			BUTTERFLY,
			NUMBER_ANIMALS,
		}

		enum ExhibitsEnum
		{
			DEFAULT,
			CAGE,
			GRASS,
			RIVER,
			NUMBER_EXHIBITS,
		}

		AnimalsEnum currentAnimal = AnimalsEnum.BUTTERFLY;

		ExhibitsEnum currentExhibit = ExhibitsEnum.DEFAULT;

		bool moveExhibit = false;

		private void Move(Vector3 v)
		{
			if (moveExhibit)
			{
				exhibit.Move(v);
			}
			else
			{
				foreach(Animal a in animals)
				{
					a.Move(v);
				}
			}
		}

		public override String keyboard(Keys keyCode, int x, int y)
		{
			StringBuilder result = new StringBuilder();
			switch (keyCode) 
			{
			case Keys.D1:
				break;
			case Keys.NumPad8:
				Move(new Vector3(0.0f, 0.1f, 0.0f));
				break;
			case Keys.NumPad2:
				Move(new Vector3(0.0f, -0.1f, 0.0f));
				break;
			case Keys.NumPad4:
				Move(new Vector3(-0.1f, 0.0f, 0.0f));
				break;
			case Keys.NumPad6:
				Move(new Vector3(0.1f, 0.0f, 0.0f));
				break;
			case Keys.NumPad7:
				Move(new Vector3(0.0f, 0.0f, 0.1f));
				break;
			case Keys.NumPad3:
				Move(new Vector3(0.0f, 0.0f, -0.1f));
				break;
			case Keys.A:
				switch (currentAnimal)
				{
				case AnimalsEnum.CAT: animals.Add(new Cat()); break;
				case AnimalsEnum.DOG: animals.Add(new Dog()); break;
				case AnimalsEnum.DRAGONFLY: animals.Add(new Dragonfly3d()); break;
				case AnimalsEnum.LADYBUG: animals.Add(new LadyBug3d()); break;	
				case AnimalsEnum.FIREFLY: animals.Add(new FireFly3d()); break;	
				case AnimalsEnum.SCORPION: animals.Add(new Scorpion()); break;	
				case AnimalsEnum.BUTTERFLY: animals.Add(new Butterfly()); break;	
				}
				break;
			case Keys.I:
				result.Append(Programs.DumpShaders());
				break;
			case Keys.N:
				int a = (int)currentAnimal + 1;
				if(a >= (int)AnimalsEnum.NUMBER_ANIMALS) a = 0;
				currentAnimal = (AnimalsEnum) a;
				animals = new List<Animal>();
				switch (currentAnimal)
				{
				case AnimalsEnum.CAT: animals.Add(new Cat()); break;
				case AnimalsEnum.DOG: animals.Add(new Dog()); break;
				case AnimalsEnum.DRAGONFLY: animals.Add(new Dragonfly3d()); break;
				case AnimalsEnum.LADYBUG: animals.Add(new LadyBug3d()); break;	
				case AnimalsEnum.FIREFLY: animals.Add(new FireFly3d()); break;	
				case AnimalsEnum.SCORPION: animals.Add(new Scorpion()); break;
				case AnimalsEnum.BUTTERFLY: animals.Add(new Butterfly()); break;
				}
				break;
			case Keys.E:
				int e = (int)currentExhibit + 1;
				if(e >= (int)ExhibitsEnum.NUMBER_EXHIBITS) e = 0;
				currentExhibit = (ExhibitsEnum) e;
				switch (currentExhibit)
				{
				case ExhibitsEnum.DEFAULT: exhibit = new Exhibit(); break;
				case ExhibitsEnum.CAGE: exhibit = new Cage(); break;
				case ExhibitsEnum.GRASS: exhibit = new Grass(); break;
				case ExhibitsEnum.RIVER: exhibit = new River(); break;
				}
				break;
			case Keys.M:
				if (moveExhibit)
				{
					moveExhibit = false;
				}
				else
				{
					moveExhibit = true;
				}
				break;
			case Keys.S:
				foreach (Animal animal in animals)
				{
					if (animal is Butterfly)
					{
						animal.SetProgram(sphericalTextureProgram);
						animal.SetSphericalMovement(sphericalTextureProgram, 1f, 0f);
					}
					else
					{
						animal.SetProgram(sphericalProgram);
						animal.SetSphericalMovement(sphericalProgram, 1f, 0f);
					}
				}
				break;
			case Keys.R:
				yRotation += 5f;
				Shape.SetWorldToCameraRotation(0f, yRotation, 0f);
				break;
			case Keys.T:
				foreach (Animal animal in animals)
				{
					if (animal.GetAutoMove())
					{
						animal.ClearAutoMove();
					}
					else
					{
						animal.SetAutoMove();
					}
				}
				break;
			case Keys.Add:
				foreach (Animal animal in animals)
				{
					animal.ChangeRadius(0.1f);
				}
				break;
			case Keys.Subtract:
				foreach (Animal animal in animals)
				{
					animal.ChangeRadius(-0.1f);
				}
				break;
			}   
			return result.ToString();
		}
	}
}

