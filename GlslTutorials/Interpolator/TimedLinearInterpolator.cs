using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public class TimedLinearInterpolator<T> : WeightedLinearInterpolator<T> where T : IDistance<T>
	{
		public void SetValues<T_data>(List<T_data> dataSet, bool isLoop = true)  where T_data : IGetValueTime<T>
		{
			m_values = new List<Data>();
			
			for(int i = 0; i < dataSet.Count; i++)
			{
				Data currData = new Data();
				currData.data = dataSet[i].GetValue();
				currData.weight = dataSet[i].GetTime();
				m_values.Add(currData);
			}

			//Compute the distances of each segment.
			float m_totalDist = 0.0f;
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
	}
}

