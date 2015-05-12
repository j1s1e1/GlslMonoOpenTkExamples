using System;
using System.Globalization;
using System.Text;
using OpenTK;

namespace GlslTutorials
{
	public class Planet
	{
		// crit_velocity = sqrt( 2*G*m1 / dist )

		TextureSphere ts;
		Vector3 axis = new Vector3(0f, 0f, 1f);
		float angleStep = 1f;

		bool orbitPosition = false;
		Vector3 orbitCenter;
		Vector3 currentPositoin;
		Vector3 currentVelocity;
		float gravityConstant;

		bool timedPosition = false;

		Vector4 longitude;
		Vector4 semiMajorAxis;
		Vector4 eccentricity;
		Vector4 inclination;
		Vector4 arguementPerihelion;
		Vector4 longitudeAscendingNode;

		float currentDate;
		float julianCenturies;
		String name;

		static bool logOn = false;

		public Planet (String nameIn, float radiusIn, string textureIn = "")
		{
			name = nameIn;
			ts = new TextureSphere(radiusIn, textureIn);
		}

		public void SetAngleStep(float f)
		{
			angleStep = f;
		}

		public void UpdatePosition()
		{
			if (orbitPosition)
			{
				if (timedPosition)
				{
					TimedOrbit();
				}
				else
				{
					GravityOrbit();
				}

			}
			else 
			{
				ts.RotateShape(axis, angleStep);
				//if (logOn) Log.i(name, ts.getOffset().toString());
			}
		}

		public void Draw()
		{
			ts.Draw();
		}

		public void SetUpOrbit(Vector3 orbitCenterIn, Vector3 currentPositoinIn, Vector3 currentVelocityIn,
			float gravityConstantIn)
		{
			orbitPosition = true;
			orbitCenter = orbitCenterIn;
			currentPositoin = currentPositoinIn;
			currentVelocity = currentVelocityIn;
			gravityConstant = gravityConstantIn;
		}

		public void SetProgram(int program)
		{
			ts.SetProgram(program);
		}

		private string TimedOrbit()
		{
			StringBuilder result = new StringBuilder();
			currentDate++;
			julianCenturies = (currentDate - 2415020.0f) / 36525f;
			Vector4 tVector = new Vector4(1, julianCenturies, (float)Math.Pow(julianCenturies, 2),
				(float)Math.Pow(julianCenturies, 3));
			float L = Vector4.Dot(longitude, tVector);
			float a = Vector4.Dot(semiMajorAxis, tVector);
			float e = Vector4.Dot(eccentricity, tVector);
			float i = Vector4.Dot(inclination, tVector);
			float w = Vector4.Dot(arguementPerihelion, tVector);
			float W = Vector4.Dot(longitudeAscendingNode, tVector);
			if (logOn)
			{
				result.AppendLine("longitude " + L.ToString());
				result.AppendLine("semiMajorAxis " + a.ToString());
				result.AppendLine("eccentricity " + e.ToString());
				result.AppendLine("inclination " + i.ToString());
				result.AppendLine("arguementPerihelion " + w.ToString());
				result.AppendLine("longitudeAscendingNode " + W.ToString());
			}
			return result.ToString();
		}

		private string GravityOrbit()
		{
			StringBuilder result = new StringBuilder();
			Vector3 distance = Vector3.Multiply((currentPositoin - orbitCenter), -1f);
			Vector3 acceleration = Vector3.Multiply(distance.Normalized(), gravityConstant / (float)Math.Pow(distance.Length, 2));
			currentVelocity = Vector3.Add(currentVelocity, acceleration);
			currentPositoin = Vector3.Add(currentPositoin, currentVelocity);
			if (logOn) result.AppendLine(name + currentPositoin.ToString());
			ts.SetOffset(currentPositoin);
			return result.ToString();
		}

		public void Move(Vector3 v)
		{
			ts.Move(v);
		}

		public void setProgram(int program)
		{
			ts.SetProgram(program);
		}

		public Vector3 GetLocation()
		{
			return ts.GetOffset();
		}

		public int GetProgram()
		{
			return ts.GetProgram();
		}

		public void SetLightScale(float f)
		{
			ts.SetLightScale(f);
		}

		private string SetupTimedOrbit()
		{
			StringBuilder result = new StringBuilder();
			GregorianCalendar gc = new GregorianCalendar();
			currentDate = gc.GetDayOfYear(DateTime.Now);
			result.AppendLine("DateTest" + currentDate.ToString());
			timedPosition = true;
			orbitPosition = true;
			return result.ToString();
		}

		public void SetMercury()
		{
			longitude = new Vector4(178.179078f, 149474.07078f, 0.0003011f, 0f);
			semiMajorAxis = new Vector4(0.3870986f, 0f, 0f, 0f);
			eccentricity = new Vector4(0.20561421f, 0.00002046f, -0.000000030f, 0f);
			inclination = new Vector4(7.002881f, 0.0018608f, -0.0000183f, 0f);
			arguementPerihelion = new Vector4(28.753753f, 0.3702806f, 0.0001208f, 0f);
			longitudeAscendingNode = new Vector4(47.145944f, 1.1852083f, 0.0001739f, 0f);
			SetupTimedOrbit();
		}


		public void SetPlanetVenus()
		{
			longitude = new Vector4(342.767053f, 58519.21191f, 0.0003097f, 0f);
			semiMajorAxis = new Vector4(0.7233316f, 0f, 0f, 0f);
			eccentricity = new Vector4(0.00682069f, -0.00004774f, 0.000000091f, 0f);
			inclination = new Vector4(3.393631f, 0.0010058f, -0.0000010f, 0f);
			arguementPerihelion = new Vector4(54.384186f, 0.5081861f, -0.0013864f, 0f);
			longitudeAscendingNode = new Vector4(75.779647f, 0.89985f, 0.00041f, 0f);
			SetupTimedOrbit();
		}

		public void setPlanetEarth()
		{
			longitude = new Vector4();
			semiMajorAxis = new Vector4();
			eccentricity = new Vector4();
			inclination = new Vector4();
			arguementPerihelion = new Vector4();
			longitudeAscendingNode = new Vector4();
		}

		public static string sunFileName = "suncyl1.jpg";
		public static string mercuryFileName = "mercurymap.jpg";
		public static string venusFileName = "Venus_Magellan_C3-MDIR_ClrTopo_Global_Mosaic_1024.jpg";
		public static string earthFileName = "PathfinderMap.jpg";
		public static string marsFileName = "Mars_Viking_MDIM21_ClrMosaic_global_1024.jpg";
		public static string jupiterFileName = "jup0vss1.jpg";
		public static string saturnFileName = "saturnmap.jpg";
		public static string uranusFileName = "uranusmap.jpg";
		public static string neptuneFileName = "neptunemap.jpg";
		public static string plutoFileName = "plutomap1k.jpg";


	}
}

