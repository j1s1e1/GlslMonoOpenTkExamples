using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public class TimedLinearInterpolator<T> : WeightedLinearInterpolator<T>
	{
		public void SetValues<T_data>(List<T_data> data, bool isLoop = true)
		{
			m_values.Clear();
			
			foreach (T_data newValue in data)
			{
				Data currData = new Data();
				dynamic newValueDyn = newValue;
				currData.data = newValueDyn.GetValue();
				currData.weight = newValueDyn.GetTime();
				m_values.Add(currData);
			}

			//Compute the distances of each segment.
			float m_totalDist = 0.0f;
			for(int iLoop = 1; iLoop < m_values.Count; ++iLoop)
			{
				m_totalDist += Distance(m_values[iLoop - 1].data, m_values[iLoop].data);
				m_values[iLoop].weight = m_totalDist;
			}

			//Compute the alpha value that represents when to use this segment.
			for(int iLoop = 1; iLoop < m_values.Count; ++iLoop)
			{
				m_values[iLoop].weight /= m_totalDist;
			}
		}
	}
}

