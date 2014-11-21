using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public class ConstVelLinearInterpolator<T> : WeightedLinearInterpolator<T>  where T : IDistance<T>, ILinearInterpolate<T>
	{
		float m_totalDist;
		
		public ConstVelLinearInterpolator()
		{
			m_totalDist = 0.0f;
		}

		public void SetValues(List<T> data, bool isLoop = true)
		{
			m_values.Clear();
			
			foreach (T newValue in data)
			{
				Data currData = new Data();
				currData.data = newValue;
				currData.weight = 0.0f;
				m_values.Add(currData);
			}

			//Compute the distances of each segment.
			m_totalDist = 0.0f;
			for(int iLoop = 1; iLoop < m_values.Count; ++iLoop)
			{
				m_totalDist += Distance(iLoop - 1, iLoop);
				m_values[iLoop].weight = m_totalDist;
			}

			//Compute the alpha value that represents when to use this segment.
			for(int iLoop = 1; iLoop < m_values.Count; ++iLoop)
			{
				m_values[iLoop].weight /= m_totalDist;
			}
		}

		float Distance()
		{
			return m_totalDist;
		}

	}
}

