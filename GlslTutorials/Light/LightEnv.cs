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
		float m_fLightAttenuation;
		ConstVelLinearInterpolator<Vector3> LightInterpolator;
		
		TimedLinearInterpolator<Vector4> m_ambientInterpolator;
		TimedLinearInterpolator<Vector4> m_backgroundInterpolator;
		TimedLinearInterpolator<Vector4> m_sunlightInterpolator;
		TimedLinearInterpolator<float> m_maxIntensityInterpolator;
		
		FrameworkTimer m_sunTimer;
		List<FrameworkTimer> m_lightTimers = new List<FrameworkTimer>();
		
		List<ConstVelLinearInterpolator<Vector3>> m_lightPos = new List<ConstVelLinearInterpolator<Vector3>>();
		List<Vector4> m_lightIntensity = new List<Vector4>();
		
		const int MAX_NUMBER_OF_LIGHTS = 4;
		
		public float first;
		List<MaxIntensityData> MaxIntensityVector = new List<MaxIntensityData>();

		List<LightData> LightVector = new List<LightData>();
		
		float GetValue(MaxIntensityData data) 
		{
			return data.first;
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
			XmlDocument docenvFilename;
		
			try
			{
				docenvFilename = new XmlDocument();
				docenvFilename.Load(envFilename);
			}
			catch(Exception ex)
			{
				MessageBox.Show(envFilename + ": Parse error in light environment file.");
				return;
			}
			
			XmlElement root = docenvFilename.DocumentElement;
		
			//XmlNode pRootNode = doc.first_node("lightenv");
			//PARSE_THROW(pRootNode, ("lightenv node not found in light environment file: " + envFilename));
		
			XmlNodeList attributeNodes = root.GetElementsByTagName("attribute");
			
			XmlNode sunNode = root.SelectSingleNode("sun");
			float time = 0f; 
		    if (attributeNodes.Count > 0) 
			{
            	for (int i = 0; i < attributeNodes.Count; i++) 
				{
                    XmlNode xmlNode = attributeNodes[i];
					switch (xmlNode.Name)
					{
						case "atten": m_fLightAttenuation = float.Parse(xmlNode.Value); break;
						case "sun":
						{	
							XmlNodeList sunAttributeNodes = sunNode.ChildNodes;
							if (sunAttributeNodes.Count > 0) 
							{
				            	for (i = 0; i < sunAttributeNodes.Count; i++)
				                {
				                    XmlNode currentSunNode = attributeNodes[i];
									switch (currentSunNode.Name)
									{
										case "time": time = float.Parse(currentSunNode.Value); break;
									}
				                }
				            }
							break;
						}
					}
                }
            }
			
			//m_fLightAttenuation = rapidxml::get_attrib_float(root, "atten", m_fLightAttenuation);
			m_fLightAttenuation = 1.0f / (m_fLightAttenuation * m_fLightAttenuation);

			//PARSE_THROW(pSunNode, "lightenv node must have a first child that is called `sun`.");
			
			m_sunTimer = new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, time);
		
			List<LightData> ambient = new List<LightData>();
			List<LightData> light = new List<LightData>();
			List<LightData> background = new List<LightData>();
			List<MaxIntensityData> maxIntensity = new List<MaxIntensityData>();
		
			
			XmlNodeList pKeyNodes = root.GetElementsByTagName("key");
	        if (pKeyNodes.Count > 0) 
			{
	        	for (int i=0;  i < pKeyNodes.Count; i++) 
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
		
			XmlNodeList pLightNodes = ((XmlElement)sunNode).GetElementsByTagName("light");
			List<Vector3> posValues = new List<Vector3>();
			
			if (pLightNodes.Count > 0) 
			{
	        	for (int i=0;  i < pLightNodes.Count; i++) 
				{
					XmlElement pLightNode = (XmlElement)pLightNodes.Item(i);
					if(m_lightPos.Count + 1 == MAX_NUMBER_OF_LIGHTS)
					{
						MessageBox.Show("Too many lights specified.");
						return;
					}
			
					m_lightTimers.Add(new FrameworkTimer(FrameworkTimer.Type.TT_LOOP, 
					                                 float.Parse(pLightNode.GetAttribute("time"))));
			
					m_lightIntensity.Add(ParseVec4(pLightNode.GetAttribute("intensity")));
					
					XmlNodeList pKeyNodes2 = pLightNode.GetElementsByTagName("key");
			
					foreach (XmlNode x in pKeyNodes2)
					{
						posValues.Add(ParseVec3(x.Value));
					}
				}
			}
		
			if(posValues.Count == 0)
			{
				MessageBox.Show("'light' elements must have at least one 'key' element child.");
			}
	
			m_lightPos.Add(new ConstVelLinearInterpolator<Vector3>());
			m_lightPos[m_lightPos.Count - 1].SetValues(posValues);
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
			return m_sunlightInterpolator.Interpolate(m_sunTimer.GetAlpha()) /
				m_maxIntensityInterpolator.Interpolate(m_sunTimer.GetAlpha());
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
				m_maxIntensityInterpolator.Interpolate(m_sunTimer.GetAlpha());
		}
		
		public Vector3 GetPointLightWorldPos( int pointLightIx )
		{
			return m_lightPos[pointLightIx].Interpolate(m_lightTimers[pointLightIx].GetAlpha());
		}
		
		public LightBlock GetLightBlock(Matrix4 worldToCamera, int numberOfLights = 4)
		{
			LightBlock lightData = new LightBlock(numberOfLights);
			lightData.ambientIntensity = m_ambientInterpolator.Interpolate(m_sunTimer.GetAlpha());
			lightData.lightAttenuation = m_fLightAttenuation;
			lightData.maxIntensity = m_maxIntensityInterpolator.Interpolate(m_sunTimer.GetAlpha());
		
			lightData.lights[0].cameraSpaceLightPos =  Vector4.Transform(GetSunlightDirection(), worldToCamera);
			lightData.lights[0].lightIntensity = m_sunlightInterpolator.Interpolate(m_sunTimer.GetAlpha());
		
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
			return m_backgroundInterpolator.Interpolate(m_sunTimer.GetAlpha());
		}
	
		float GetMaxIntensity()
		{
			return m_maxIntensityInterpolator.Interpolate(m_sunTimer.GetAlpha());
		}
	
		Vector4 GetSunlightIntensity()
		{
			return m_sunlightInterpolator.Interpolate(m_sunTimer.GetAlpha());
		}
	
		float GetElapsedTime()
		{
			return m_sunTimer.GetProgression();
		}
	}
}

