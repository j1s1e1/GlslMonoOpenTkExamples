using System;

namespace GlslTutorials
{
	public interface IDistance<T>
	{
		float Distance(T a, T b);
	}
}

