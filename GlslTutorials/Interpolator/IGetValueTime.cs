using System;

namespace GlslTutorials
{
	public interface IGetValueTime<T>
	{
		T GetValue();
		float GetTime();
	}
}

