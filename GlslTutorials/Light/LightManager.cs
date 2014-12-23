using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;
using LightInterpolator = GlslTutorials.ConstVelLinearInterpolator<GlslTutorials.Vector3IDistance>;
using LightVector = System.Collections.Generic.List<GlslTutorials.LightVectorData>;
using MaxIntensityVector = System.Collections.Generic.List<GlslTutorials.MaxIntensityData>;

namespace GlslTutorials
{
	public class LightManager
	{
		static int NUMBER_OF_LIGHTS = 4;
		int NUMBER_OF_POINT_LIGHTS = NUMBER_OF_LIGHTS - 1;

		static float g_fLightHeight = 10.5f;
		static float g_fLightRadius = 70.0f;

		struct ExtraTimer
		{
			public string name;
			public FrameworkTimer timer;

			public ExtraTimer(string nameIn, FrameworkTimer timerIn)
			{
				name = nameIn;
				timer = timerIn;
			}
		}

		FrameworkTimer m_sunTimer;
		TimedLinearInterpolator<LightVectorData> m_ambientInterpolator;
		TimedLinearInterpolator<LightVectorData> m_backgroundInterpolator;
		TimedLinearInterpolator<LightVectorData> m_sunlightInterpolator;
		TimedLinearInterpolator<FloatIDistance> m_maxIntensityInterpolator;

		List<ConstVelLinearInterpolator<Vector3IDistance> > m_lightPos;
		List<Vector4> m_lightIntensity = new List<Vector4>();
		List<FrameworkTimer> m_lightTimers = new List<FrameworkTimer>();
		List<ExtraTimer> m_extraTimers = new List<ExtraTimer>();

		Vector4 CalcLightPosition(FrameworkTimer timer, float alphaOffset)
		{
			const float fLoopDuration = 5.0f;
			const float fScale = 3.14159f * 2.0f;

			float timeThroughLoop = timer.GetAlpha() + alphaOffset;

			Vector4 ret = new Vector4(0.0f, g_fLightHeight, 0.0f, 1.0f);

			ret.X = (float)Math.Cos(timeThroughLoop * fScale) * g_fLightRadius;
			ret.Z = (float)Math.Sin(timeThroughLoop * fScale) * g_fLightRadius;

			return ret;
		}

		const float g_fHalfLightDistance = 70.0f;
		const float g_fLightAttenuation = 1.0f / (g_fHalfLightDistance * g_fHalfLightDistance);

		float distance(Vector3 lhs, Vector3 rhs)
		{
			return (rhs - lhs).Length;
		}

		public LightManager()
		{
			m_ambientInterpolator = new TimedLinearInterpolator<LightVectorData>();
			m_backgroundInterpolator = new TimedLinearInterpolator<LightVectorData>();
			m_sunlightInterpolator = new TimedLinearInterpolator<LightVectorData>();
			m_maxIntensityInterpolator = new TimedLinearInterpolator<FloatIDistance>();
			m_sunTimer = new FrameworkTimer(FrameworkTimer.Type.TT_LOOP,  30.0f);
			m_ambientInterpolator = new TimedLinearInterpolator<LightVectorData>();
			m_lightTimers = new List<FrameworkTimer>();
			m_lightPos = new List<ConstVelLinearInterpolator<Vector3IDistance>>();
			m_lightPos.Add(new LightInterpolator());
			m_lightPos.Add(new LightInterpolator());
			m_lightPos.Add(new LightInterpolator());

			m_lightIntensity = new List<Vector4>();
			for (int i = 0; i < NUMBER_OF_POINT_LIGHTS; i++)
			{
				m_lightIntensity.Add(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
			}

			List<Vector3IDistance> posValues = new List<Vector3IDistance>();

			posValues.Add(new Vector3IDistance(-50.0f, 30.0f, 70.0f));
			posValues.Add(new Vector3IDistance(-70.0f, 30.0f, 50.0f));
			posValues.Add(new Vector3IDistance(-70.0f, 30.0f, -50.0f));
			posValues.Add(new Vector3IDistance(-50.0f, 30.0f, -70.0f));
			posValues.Add(new Vector3IDistance(50.0f, 30.0f, -70.0f));
			posValues.Add(new Vector3IDistance(70.0f, 30.0f, -50.0f));
			posValues.Add(new Vector3IDistance(70.0f, 30.0f, 50.0f));
			posValues.Add(new Vector3IDistance(50.0f, 30.0f, 70.0f));
			m_lightPos[0].SetValues(posValues);
			m_lightTimers.Add(new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, 15.0f));

			//Right-side light.
			posValues.Clear();
			posValues.Add(new Vector3IDistance(100.0f, 6.0f, 75.0f));
			posValues.Add(new Vector3IDistance(90.0f, 8.0f, 90.0f));
			posValues.Add(new Vector3IDistance(75.0f, 10.0f, 100.0f));
			posValues.Add(new Vector3IDistance(60.0f, 12.0f, 90.0f));
			posValues.Add(new Vector3IDistance(50.0f, 14.0f, 75.0f));
			posValues.Add(new Vector3IDistance(60.0f, 16.0f, 60.0f));
			posValues.Add(new Vector3IDistance(75.0f, 18.0f, 50.0f));
			posValues.Add(new Vector3IDistance(90.0f, 20.0f, 60.0f));
			posValues.Add(new Vector3IDistance(100.0f, 22.0f, 75.0f));
			posValues.Add(new Vector3IDistance(90.0f, 24.0f, 90.0f));
			posValues.Add(new Vector3IDistance(75.0f, 26.0f, 100.0f));
			posValues.Add(new Vector3IDistance(60.0f, 28.0f, 90.0f));
			posValues.Add(new Vector3IDistance(50.0f, 30.0f, 75.0f));

			posValues.Add(new Vector3IDistance(105.0f, 9.0f, -70.0f));
			posValues.Add(new Vector3IDistance(105.0f, 10.0f, -90.0f));
			posValues.Add(new Vector3IDistance(72.0f, 20.0f, -90.0f));
			posValues.Add(new Vector3IDistance(72.0f, 22.0f, -70.0f));
			posValues.Add(new Vector3IDistance(105.0f, 32.0f, -70.0f));
			posValues.Add(new Vector3IDistance(105.0f, 34.0f, -90.0f));
			posValues.Add(new Vector3IDistance(72.0f, 44.0f, -90.0f));

			m_lightPos[1].SetValues(posValues);
			m_lightTimers.Add(new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, 25.0f));

			//Left-side light.
			posValues.Clear();
			posValues.Add(new Vector3IDistance(-7.0f, 35.0f, 1.0f));
			posValues.Add(new Vector3IDistance(8.0f, 40.0f, -14.0f));
			posValues.Add(new Vector3IDistance(-7.0f, 45.0f, -29.0f));
			posValues.Add(new Vector3IDistance(-22.0f, 50.0f, -14.0f));
			posValues.Add(new Vector3IDistance(-7.0f, 55.0f, 1.0f));
			posValues.Add(new Vector3IDistance(8.0f, 60.0f, -14.0f));
			posValues.Add(new Vector3IDistance(-7.0f, 65.0f, -29.0f));

			posValues.Add(new Vector3IDistance(-83.0f, 30.0f, -92.0f));
			posValues.Add(new Vector3IDistance(-98.0f, 27.0f, -77.0f));
			posValues.Add(new Vector3IDistance(-83.0f, 24.0f, -62.0f));
			posValues.Add(new Vector3IDistance(-68.0f, 21.0f, -77.0f));
			posValues.Add(new Vector3IDistance(-83.0f, 18.0f, -92.0f));
			posValues.Add(new Vector3IDistance(-98.0f, 15.0f, -77.0f));

			posValues.Add(new Vector3IDistance(-50.0f, 8.0f, 25.0f));
			posValues.Add(new Vector3IDistance(-59.5f, 4.0f, 65.0f));
			posValues.Add(new Vector3IDistance(-59.5f, 4.0f, 78.0f));
			posValues.Add(new Vector3IDistance(-45.0f, 4.0f, 82.0f));
			posValues.Add(new Vector3IDistance(-40.0f, 4.0f, 50.0f));
			posValues.Add(new Vector3IDistance(-70.0f, 20.0f, 40.0f));
			posValues.Add(new Vector3IDistance(-60.0f, 20.0f, 90.0f));
			posValues.Add(new Vector3IDistance(-40.0f, 25.0f, 90.0f));

			m_lightPos[2].SetValues(posValues);
			m_lightTimers.Add(new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, 15.0f));
		}

		Vector4 GetValue(LightVectorData data) {return data.first;}
		float GetTime(LightVectorData data) {return data.second;}
		float GetValue(MaxIntensityData data) {return data.first.GetValue();}
		float GetTime(MaxIntensityData data) {return data.second;}

		public void UpdateTime()
		{
			m_sunTimer.Update();
			foreach(FrameworkTimer ft in m_lightTimers)
			{
				ft.Update();
			}
			foreach(ExtraTimer et in m_extraTimers)
			{
				et.timer.Update();
			}
		}

		public void SetPause(TimerTypes eTimer, bool pause)
		{
			if(eTimer == TimerTypes.TIMER_ALL || eTimer == TimerTypes.TIMER_LIGHTS)
			{
				foreach(FrameworkTimer ft in m_lightTimers)
				{
					ft.SetPause(true);
				}
				foreach(ExtraTimer et  in m_extraTimers)
				{
					et.timer.SetPause(true);
				}
			}

			if(eTimer == TimerTypes.TIMER_ALL || eTimer == TimerTypes.TIMER_SUN)
			{
				m_sunTimer.TogglePause();
			}
		}

		public void TogglePause( TimerTypes eTimer )
		{
			SetPause(eTimer, !IsPaused(eTimer));
		}

		public bool IsPaused( TimerTypes eTimer )
		{
			if(eTimer == TimerTypes.TIMER_ALL || eTimer == TimerTypes.TIMER_SUN)
				return m_sunTimer.IsPaused();

			return m_lightTimers[0].IsPaused();
		}

		public void RewindTime(TimerTypes eTimer, float secRewind )
		{
			if(eTimer == TimerTypes.TIMER_ALL || eTimer == TimerTypes.TIMER_SUN)
				m_sunTimer.Rewind(secRewind);

			if(eTimer == TimerTypes.TIMER_ALL || eTimer == TimerTypes.TIMER_LIGHTS)
			{
				foreach(FrameworkTimer ft in m_lightTimers)
				{
					ft.Rewind(secRewind);
				}
				foreach(ExtraTimer et in m_extraTimers)
				{
					et.timer.Rewind(secRewind);
				}
			}
		}

		public void FastForwardTime(TimerTypes eTimer,  float secFF )
		{
			if(eTimer == TimerTypes.TIMER_ALL || eTimer == TimerTypes.TIMER_SUN)
				m_sunTimer.Fastforward(secFF);

			if(eTimer == TimerTypes.TIMER_ALL || eTimer == TimerTypes.TIMER_LIGHTS)
			{
				foreach(FrameworkTimer ft in m_lightTimers)
				{
					ft.Fastforward(secFF);
				}
				foreach(ExtraTimer et in m_extraTimers)
				{
					et.timer.Fastforward(secFF);
				}
			}
		}

		public LightBlock GetLightInformation( Matrix4 worldToCameraMat )
		{
			LightBlock lightData = new LightBlock();

			lightData.ambientIntensity = m_ambientInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
			lightData.lightAttenuation = g_fLightAttenuation;

			lightData.lights[0].cameraSpaceLightPos =
				Vector4.Transform(GetSunlightDirection(), worldToCameraMat);
			lightData.lights[0].lightIntensity = m_sunlightInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();

			for(int light = 0; light < NUMBER_OF_POINT_LIGHTS; light++)
			{
				Vector4 worldLightPos =
					new Vector4(m_lightPos[light].Interpolate(m_lightTimers[light].GetAlpha()).GetValue(), 1.0f);
				Vector4 lightPosCameraSpace = Vector4.Transform(worldLightPos, worldToCameraMat);

				lightData.lights[light + 1].cameraSpaceLightPos = lightPosCameraSpace;
				lightData.lights[light + 1].lightIntensity = m_lightIntensity[light];
			}

			return lightData;
		}
			
		public LightBlock GetLightInformationHDR(Matrix4 worldToCameraMat ) 
		{
			LightBlock lightData = new LightBlock(NUMBER_OF_LIGHTS);

			lightData.ambientIntensity = m_ambientInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
			lightData.lightAttenuation = g_fLightAttenuation;
			lightData.maxIntensity = m_maxIntensityInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();

			lightData.lights[0].cameraSpaceLightPos = 
				Vector4.Transform(GetSunlightDirection(), worldToCameraMat);
			lightData.lights[0].lightIntensity = m_sunlightInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();

			for(int light = 0; light < NUMBER_OF_POINT_LIGHTS; light++)
			{
				Vector4 worldLightPos =
					new Vector4(m_lightPos[light].Interpolate(m_lightTimers[light].GetAlpha()).GetValue(), 1.0f);
				Vector4 lightPosCameraSpace = Vector4.Transform(worldLightPos, worldToCameraMat);

				lightData.lights[light + 1].cameraSpaceLightPos = lightPosCameraSpace;
				lightData.lights[light + 1].lightIntensity = m_lightIntensity[light];
			}

			return lightData;
		}

		public LightBlock GetLightInformationGamma(Matrix4 worldToCameraMat )
		{
			LightBlock lightData  = GetLightInformationHDR(worldToCameraMat);
			return lightData;
		}

		public Matrix4 Rotate(Matrix4 input, Vector3 rotationAxis, float angleDeg)
		{
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angleDeg);
			return Matrix4.Mult(rotation, input);
		}

		public Vector4 GetSunlightDirection()
		{
			float angle = 2.0f * 3.14159f * m_sunTimer.GetAlpha();
			Vector4 sunDirection = Vector4.Zero;
			sunDirection[0] = (float)Math.Sin(angle);
			sunDirection[1] = (float)Math.Cos(angle);

			//Keep the sun from being perfectly centered overhead.
			sunDirection = Vector4.Transform(sunDirection, 
				Rotate(Matrix4.Identity, new Vector3(0.0f, 1.0f, 0.0f), 5.0f));

			return sunDirection;
		}

		public Vector4 GetSunlightIntensity()
		{
			return m_sunlightInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
		}

		public int GetNumberOfPointLights()
		{
			return m_lightPos.Count;
		}

		public Vector3 GetWorldLightPosition( int lightIx )
		{
			return m_lightPos[lightIx].Interpolate(m_lightTimers[lightIx].GetAlpha()).GetValue();
		}

		public void SetPointLightIntensity( int iLightIx, Vector4 intensity )
		{
			m_lightIntensity[iLightIx] = intensity;
		}

		public Vector4 GetPointLightIntensity( int iLightIx )
		{
			return m_lightIntensity[iLightIx];
		}

		public void CreateTimer(string timerName,
			FrameworkTimer.Type eType, float fDuration )
		{
			m_extraTimers.Add(new ExtraTimer(timerName, new FrameworkTimer(eType, fDuration)));
		}

		public float GetTimerValue(string timerName)
		{
			foreach (ExtraTimer et in m_extraTimers)
			{
				if (et.name == timerName)
				{
					return et.timer.GetAlpha();
				}
			}
			return -1f;
		}

		public Vector4 GetBackgroundColor()
		{
			try
			{
				return m_backgroundInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error interpolating background " + ex.ToString());
				return Vector4.Zero;
			}
		}

		public float GetMaxIntensity()
		{
			return m_maxIntensityInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
		}

		public float GetSunTime()
		{
			return m_sunTimer.GetAlpha();
		}

		public void SetSunlightValues(SunlightValue[] values)
		{
			LightVector ambient = new LightVector();
			LightVector light = new LightVector();
			LightVector background = new LightVector();

			foreach(SunlightValue sv in values)
			{
				ambient.Add(new LightVectorData(sv.ambient, sv.normTime));
				light.Add(new LightVectorData(sv.sunlightIntensity, sv.normTime));
				background.Add(new LightVectorData(sv.backgroundColor, sv.normTime));
			}

			m_ambientInterpolator.SetValues(ambient);
			m_sunlightInterpolator.SetValues(light);
			m_backgroundInterpolator.SetValues(background);

			MaxIntensityVector maxIntensity = new MaxIntensityVector();
			maxIntensity.Add(new MaxIntensityData(1.0f, 0.0f));
			m_maxIntensityInterpolator.SetValues(maxIntensity, false);
		}

	}
}

