using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public class WeightedLinearInterpolator<T> where T : IDistance<T>, ILinearInterpolate<T>
	{		
		protected class Data
		{
			public T data;
			public float weight;
		};

		protected List<Data> m_values = new List<Data>();
		
		int NumSegments() 
		{
			return m_values.Count - 1;
		}

		public T Interpolate(float fAlpha)
		{
			if(m_values.Count == 0)
				return default(T);
			if(m_values.Count == 1)
				return m_values[0].data;

			//Find which segment we are within.
			int segment = 1;
			for(; segment < m_values.Count; ++segment)
			{
				if(fAlpha < m_values[segment].weight)
					break;
			}

			if(segment == m_values.Count)
				return m_values[m_values.Count-1].data;

			float sectionAlpha = fAlpha - m_values[segment - 1].weight;
			sectionAlpha /= m_values[segment].weight - m_values[segment - 1].weight;

			float invSecAlpha = 1.0f - sectionAlpha;
			
			return LinearInterpolate(segment - 1, segment, sectionAlpha);
		}
		
		protected float Distance(int a, int b)
		{
			return m_values[a].data.Distance(m_values[a].data, m_values[b].data);
		}
		
		protected T LinearInterpolate(int a, int b, float sectionAlpha)
		{
			return m_values[a].data.LinearInterpolate(m_values[a].data, m_values[b].data, sectionAlpha);
		}
	}
}

