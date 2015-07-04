using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Diagnostics;
using System.Windows.Forms;
using OpenTK;

namespace GlslTutorials
{
	public class LightEnv
	{
		public float m_fLightAttenuation;

		TimedLinearInterpolator<Vector4IDistance> m_ambientInterpolator = new TimedLinearInterpolator<Vector4IDistance>();
		TimedLinearInterpolator<Vector4IDistance> m_backgroundInterpolator = new TimedLinearInterpolator<Vector4IDistance>();
		TimedLinearInterpolator<Vector4IDistance> m_sunlightInterpolator = new TimedLinearInterpolator<Vector4IDistance>();
		TimedLinearInterpolator<FloatIDistance> m_maxIntensityInterpolator = new TimedLinearInterpolator<FloatIDistance>();
		
		FrameworkTimer m_sunTimer;
		List<FrameworkTimer> m_lightTimers = new List<FrameworkTimer>();
		
		List<ConstVelLinearInterpolator<Vector3IDistance>> m_lightPos = new List<ConstVelLinearInterpolator<Vector3IDistance>>();
		List<Vector4> m_lightIntensity = new List<Vector4>();
		
		const int MAX_NUMBER_OF_LIGHTS = 4;
		
		public float first;

		float GetValue(MaxIntensityData data) 
		{
			return data.GetFloat();
		}
		float GetTime(MaxIntensityData data) 
		{
			return data.second;
		}
		
		float distance(Vector3 lhs, Vector3 rhs)
		{
			return (rhs - lhs).Length;
		}
		
		void ThrowAttrib(XmlNode attrib, string msg)
		{
			string name = attrib.Name;
			throw new Exception("Attribute " + name + " " + msg);
		}
		
		Vector4 ParseVec4(string strVec4)
		{
			List<float> newFloats = strVec4.Split(' ').Select(s => Convert.ToSingle(s)).ToList();
			Vector4 ret = new Vector4(newFloats[0], newFloats[1], newFloats[2], newFloats[3]);
			return ret;
		}
		
		Vector3 ParseVec3(string strVec3)
		{
			List<float> newFloats = strVec3.Split(' ').Select(s => Convert.ToSingle(s)).ToList();
			Vector3 ret = new Vector3(newFloats[0], newFloats[1], newFloats[2]);
			return ret;
		}
				
		public LightEnv(Stream envFilename, float m_fLightAttenuation = 40.0f)
		{	
			int i;
			XmlElement pLightNode;
			XmlDocument docenvFilename;
		
			try
			{
				docenvFilename = new XmlDocument();
				docenvFilename.Load(envFilename);
			}
			catch
			{
				MessageBox.Show(envFilename + ": Parse error in light environment file.");
				return;
			}
			
			XmlElement root = docenvFilename.DocumentElement;
		
			float time = float.Parse(root.GetAttribute("atten"));; 
			XmlElement sunNode = (XmlElement)root.SelectSingleNode("sun");

   		    time = float.Parse(((XmlElement)sunNode).GetAttribute("time"));

			string attenString = ((XmlElement)sunNode).GetAttribute("atten");
			if (attenString != "") m_fLightAttenuation = float.Parse(attenString);
			m_fLightAttenuation = 1.0f / (m_fLightAttenuation * m_fLightAttenuation);

			//PARSE_THROW(pSunNode, "lightenv node must have a first child that is called `sun`.");
			
			m_sunTimer = new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, time / 4f); // speed up
		
			List<LightData> ambient = new List<LightData>();
			List<LightData> light = new List<LightData>();
			List<LightData> background = new List<LightData>();
			List<MaxIntensityData> maxIntensity = new List<MaxIntensityData>();
		
			
			XmlNodeList pKeyNodes = sunNode.SelectNodes("key");
	        if (pKeyNodes.Count > 0) 
			{
	        	for (i=0;  i < pKeyNodes.Count; i++) 
				{
	            	XmlElement pKeyNode = (XmlElement)pKeyNodes.Item(i);
	                float keyTime = float.Parse(pKeyNode.GetAttribute("time"));
					//Convert from hours to normalized time.
					keyTime = keyTime / 24.0f;
			
					ambient.Add(new LightData(ParseVec4(pKeyNode.GetAttribute("ambient")), keyTime));
			
					light.Add(new LightData(ParseVec4(pKeyNode.GetAttribute("intensity")), keyTime));
			
					background.Add(new LightData(ParseVec4(pKeyNode.GetAttribute("background")), keyTime));
			
					maxIntensity.Add(new MaxIntensityData(float.Parse(pKeyNode.GetAttribute("max-intensity")), keyTime));
		     	}
	        }
		
			if(ambient.Count == 0)
			{
				MessageBox.Show("'sun' element must have at least one 'key' element child.");
				return;
			}
			
			m_ambientInterpolator.SetValues(ambient);
			m_sunlightInterpolator.SetValues(light);
			m_backgroundInterpolator.SetValues(background);
			m_maxIntensityInterpolator.SetValues(maxIntensity);
		
			XmlNodeList pLightNodes = root.SelectNodes("light");
			List<Vector3> posValues = new List<Vector3>();
			
			if (pLightNodes.Count > 0) 
			{
	        	for (i=0;  i < pLightNodes.Count; i++) 
				{
					pLightNode = (XmlElement)pLightNodes.Item(i);
					if(m_lightPos.Count + 1 == MAX_NUMBER_OF_LIGHTS)
					{
						MessageBox.Show("Too many lights specified.");
						return;
					}
			
					m_lightTimers.Add(new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, 
					                                 float.Parse(pLightNode.GetAttribute("time"))));
			
					m_lightIntensity.Add(ParseVec4(pLightNode.GetAttribute("intensity")));
					
					XmlNodeList pKeyNodes2 = pLightNode.SelectNodes("key");
			
					if (pKeyNodes2.Count > 0)
					{
						for (i=0;  i < pKeyNodes2.Count; i++) 
						{
							pLightNode = (XmlElement)pKeyNodes2.Item(i);
							posValues.Add(ParseVec3(pLightNode.InnerText));
						}
					}
				}
			}
		
			if(posValues.Count == 0)
			{
				MessageBox.Show("'light' elements must have at least one 'key' element child.");
			}
	
			m_lightPos.Add(new ConstVelLinearInterpolator<Vector3IDistance>());
			List<Vector3IDistance> posValuesIDistance = new List<Vector3IDistance>();
			foreach (Vector3 v in posValues)
			{
				posValuesIDistance.Add(new Vector3IDistance(v));
			}
			m_lightPos[m_lightPos.Count - 1].SetValues(posValuesIDistance);
		}
		
		public Vector4 GetSunlightDirection()
		{
			float angle = 2.0f * 3.14159f * m_sunTimer.GetAlpha();
			Vector4 sunDirection = Vector4.Zero;
			sunDirection[0] = (float)Math.Sin(angle);
			sunDirection[1] = (float)Math.Cos(angle);
		
			//Keep the sun from being perfectly centered overhead.
			float angleDeg = 5f;
			Vector3 rotationAxis = Vector3.UnitY;
			Matrix4 rotation = Matrix4.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 180.0f * angleDeg);
			sunDirection =  Vector4.Transform(sunDirection, rotation);
			return sunDirection;
		}
		
		public Vector4 GetSunlightScaledIntensity()
		{
			return m_sunlightInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue() /
				m_maxIntensityInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
		}
		
		public int GetNumLights()
		{
			return 1 + m_lightPos.Count();
		}
		
		public int GetNumPointLights()
		{
			return m_lightPos.Count();
		}
		
		public Vector4 GetPointLightIntensity(int pointLightIx)
		{
			return m_lightIntensity[pointLightIx];
		}
		
		public Vector4 GetPointLightScaledIntensity(int pointLightIx)
		{
			return m_lightIntensity[pointLightIx] /
				m_maxIntensityInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
		}
		
		public Vector3 GetPointLightWorldPos( int pointLightIx )
		{
			return m_lightPos[pointLightIx].Interpolate(m_lightTimers[pointLightIx].GetAlpha()).GetValue();
		}
		
		public LightBlock GetLightBlock(Matrix4 worldToCamera, int numberOfLights = 4)
		{
			LightBlock lightData = new LightBlock(numberOfLights);
			lightData.ambientIntensity = m_ambientInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
			lightData.lightAttenuation = m_fLightAttenuation;
			lightData.maxIntensity = m_maxIntensityInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
		
			lightData.lights[0].cameraSpaceLightPos =  Vector4.Transform(GetSunlightDirection(), worldToCamera);
			lightData.lights[0].lightIntensity = m_sunlightInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
		
			for(int light = 0; light < m_lightPos.Count; light++)
			{
				Vector4 worldLightPos = new Vector4(GetPointLightWorldPos(light), 1.0f);
				Vector4 lightPosCameraSpace = Vector4.Transform(worldLightPos, worldToCamera);
		
				lightData.lights[light + 1].cameraSpaceLightPos = lightPosCameraSpace;
				lightData.lights[light + 1].lightIntensity = m_lightIntensity[light];
			}
		
			return lightData;
		}
		
		/*
		struct UpdateTimer
		{
			void operator()(FrameworkTimer timer) {timer.Update();}
			void operator()(std::pair<const std::string, Framework::Timer> &timeData)
			{timeData.second.Update();}
		};
	
		struct PauseTimer
		{
			PauseTimer(bool _pause) : pause(_pause) {}
			void operator()(Framework::Timer &timer) {timer.SetPause(pause);}
			void operator()(std::pair<const std::string, Framework::Timer> &timeData)
			{timeData.second.SetPause(pause);}
	
			bool pause;
		};
	
		struct RewindTimer
		{
			RewindTimer(float _secRewind) : secRewind(_secRewind) {}
	
			void operator()(Framework::Timer &timer) {timer.Rewind(secRewind);}
			void operator()(std::pair<const std::string, Framework::Timer> &timeData)
			{timeData.second.Rewind(secRewind);}
	
			float secRewind;
		};
	
		struct FFTimer
		{
			FFTimer(float _secFF) : secFF(_secFF) {}
	
			void operator()(Framework::Timer &timer) {timer.Fastforward(secFF);}
			void operator()(std::pair<const std::string, Framework::Timer> &timeData)
			{timeData.second.Fastforward(secFF);}
	
			float secFF;
		};
		*/
		
		public void UpdateTime()
		{
			m_sunTimer.Update();
			foreach(FrameworkTimer ft in m_lightTimers)
			{
				ft.Update();
			}
		}
		
		public void TogglePause()
		{
			bool isPaused = m_sunTimer.TogglePause();
			foreach(FrameworkTimer ft in m_lightTimers)
			{
				ft.SetPause(isPaused);
			}
		}
		
		public void SetPause(bool pause)
		{
			m_sunTimer.SetPause(pause);
			foreach(FrameworkTimer ft in m_lightTimers)
			{
				ft.SetPause(pause);
			}
		}
		
		public void RewindTime(float secRewind)
		{
			m_sunTimer.Rewind(secRewind);
			foreach(FrameworkTimer ft in m_lightTimers)
			{
				ft.Rewind(secRewind);
			}			
		}
		
		public void FastForwardTime(float secFF)
		{
			m_sunTimer.Fastforward(secFF);
			foreach(FrameworkTimer ft in m_lightTimers)
			{
				ft.Fastforward(secFF);
			}
		}
		
		public Vector4 GetBackgroundColor()
		{
			float fAlpha = m_sunTimer.GetAlpha();
			return m_backgroundInterpolator.Interpolate(fAlpha).GetValue();
		}
	
		float GetMaxIntensity()
		{
			return m_maxIntensityInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
		}
	
		Vector4 GetSunlightIntensity()
		{
			return m_sunlightInterpolator.Interpolate(m_sunTimer.GetAlpha()).GetValue();
		}
	
		float GetElapsedTime()
		{
			return m_sunTimer.GetProgression();
		}
	}
}

