using System;
using System.Windows.Forms;

namespace GlslTutorials
{
	public class FrameworkTimer : Timer
	{
		public enum Type
		{
			TT_LOOP,
			TT_SINGLE,
			TT_INFINITE,
	
			NUM_TIMER_TYPES,
		};
		
		private Type m_eType;
		private float m_secDuration;

		private bool m_hasUpdated;
		private bool m_isPaused;

		private float m_absPrevTime;
		private float m_secAccumTime;
		
		public FrameworkTimer(Type eType, float fDuration)
		{
			m_eType = eType;
			m_secDuration = fDuration;
			m_hasUpdated = false;
			m_isPaused = false;
			m_absPrevTime = 0.0f;
			m_secAccumTime = 0.0f;
			//if(m_eType != Type.TT_INFINITE)
			//	Assert(m_secDuration > 0.0f);
		}
	
		void Reset()
		{
			m_hasUpdated = false;
			m_secAccumTime = 0.0f;
		}
	
		public bool TogglePause()
		{
			m_isPaused = !m_isPaused;
			return m_isPaused;
		}
	
		bool IsPaused()
		{
			return m_isPaused;
		}
	
		public void SetPause( bool pause )
		{
			m_isPaused = pause;
		}
	
		public bool Update()
		{
			float absCurrTime = TutorialBase.GetElapsedTime() / 1000.0f;
			if(!m_hasUpdated)
			{
				m_absPrevTime = absCurrTime;
				m_hasUpdated = true;
			}
	
			if(m_isPaused)
			{
				m_absPrevTime = absCurrTime;
				return false;
			}
	
			float fDeltaTime = absCurrTime - m_absPrevTime;
			m_secAccumTime += fDeltaTime;
	
			m_absPrevTime = absCurrTime;
			if(m_eType == Type.TT_SINGLE)
				return m_secAccumTime > m_secDuration;
	
			return false;
		}
	
		public void Rewind(float secRewind)
		{
			m_secAccumTime -= secRewind;
			if(m_secAccumTime < 0.0f)
				m_secAccumTime = 0.0f;
		}
	
		public void Fastforward( float secFF )
		{
			m_secAccumTime += secFF;
		}
		
		private float Clamp(float input, float min, float max)
		{
			float result = input;
			if (input < min) result = min;
			if (input > max) result = max;
			return result;
		}
		
		private float FmodF(float a, float b)
		{
			return a % b;
		}
	
		public float GetAlpha()
		{
			float result = -1f;
			switch(m_eType)
			{
				case Type.TT_LOOP: result = FmodF(m_secAccumTime, m_secDuration) / m_secDuration; break;
				case Type.TT_SINGLE: result = Clamp(m_secAccumTime / m_secDuration, 0.0f, 1.0f); break;
			}
			return result;
		}
	
		public float GetProgression()
		{
			float result = -1f;
			switch(m_eType)
			{
				case Type.TT_LOOP: result = FmodF(m_secAccumTime, m_secDuration); break;
				case Type.TT_SINGLE: result = Clamp(m_secAccumTime, 0.0f, m_secDuration); break;
			}
			return result;	//Garbage.
		}
	
		float GetTimeSinceStart()
		{
			return m_secAccumTime;
		}

	}
}

