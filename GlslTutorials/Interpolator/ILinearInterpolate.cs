using System;

namespace GlslTutorials
{
	public interface ILinearInterpolate<T>
	{
		T LinearInterpolate(T a, T b, float alpha);
	}
}

