using System;

namespace GlslTutorials
{
	public class TreeData 
	{
	    public float fXPos;
	    public float fZPos;
	    public float fTrunkHeight;
	    public float fConeHeight;
	
	    public TreeData(float a, float b, float c, float d)
	    {
	        fXPos = a;
	        fZPos = b;
	        fTrunkHeight = c;
	        fConeHeight = d;
	    }
	
	    public static TreeData[] g_forest = new TreeData[]
        {
                new TreeData(-45.0f, -40.0f, 2.0f, 3.0f),
                new TreeData(-42.0f, -35.0f, 2.0f, 3.0f),
                new TreeData(-39.0f, -29.0f, 2.0f, 4.0f),
                new TreeData(-44.0f, -26.0f, 3.0f, 3.0f),
                new TreeData(-40.0f, -22.0f, 2.0f, 4.0f),
                new TreeData(-36.0f, -15.0f, 3.0f, 3.0f),
                new TreeData(-41.0f, -11.0f, 2.0f, 3.0f),
                new TreeData(-37.0f, -6.0f, 3.0f, 3.0f),
                new TreeData(-45.0f, 0.0f, 2.0f, 3.0f),
                new TreeData(-39.0f, 4.0f, 3.0f, 4.0f),
                new TreeData(-36.0f, 8.0f, 2.0f, 3.0f),
                new TreeData(-44.0f, 13.0f, 3.0f, 3.0f),
                new TreeData(-42.0f, 17.0f, 2.0f, 3.0f),
                new TreeData(-38.0f, 23.0f, 3.0f, 4.0f),
                new TreeData(-41.0f, 27.0f, 2.0f, 3.0f),
                new TreeData(-39.0f, 32.0f, 3.0f, 3.0f),
                new TreeData(-44.0f, 37.0f, 3.0f, 4.0f),
                new TreeData(-36.0f, 42.0f, 2.0f, 3.0f),

                new TreeData(-32.0f, -45.0f, 2.0f, 3.0f),
                new TreeData(-30.0f, -42.0f, 2.0f, 4.0f),
                new TreeData(-34.0f, -38.0f, 3.0f, 5.0f),
                new TreeData(-33.0f, -35.0f, 3.0f, 4.0f),
                new TreeData(-29.0f, -28.0f, 2.0f, 3.0f),
                new TreeData(-26.0f, -25.0f, 3.0f, 5.0f),
                new TreeData(-35.0f, -21.0f, 3.0f, 4.0f),
                new TreeData(-31.0f, -17.0f, 3.0f, 3.0f),
                new TreeData(-28.0f, -12.0f, 2.0f, 4.0f),
                new TreeData(-29.0f, -7.0f, 3.0f, 3.0f),
                new TreeData(-26.0f, -1.0f, 2.0f, 4.0f),
                new TreeData(-32.0f, 6.0f, 2.0f, 3.0f),
                new TreeData(-30.0f, 10.0f, 3.0f, 5.0f),
                new TreeData(-33.0f, 14.0f, 2.0f, 4.0f),
                new TreeData(-35.0f, 19.0f, 3.0f, 4.0f),
                new TreeData(-28.0f, 22.0f, 2.0f, 3.0f),
                new TreeData(-33.0f, 26.0f, 3.0f, 3.0f),
                new TreeData(-29.0f, 31.0f, 3.0f, 4.0f),
                new TreeData(-32.0f, 38.0f, 2.0f, 3.0f),
                new TreeData(-27.0f, 41.0f, 3.0f, 4.0f),
                new TreeData(-31.0f, 45.0f, 2.0f, 4.0f),
                new TreeData(-28.0f, 48.0f, 3.0f, 5.0f),

                new TreeData(-25.0f, -48.0f, 2.0f, 3.0f),
                new TreeData(-20.0f, -42.0f, 3.0f, 4.0f),
                new TreeData(-22.0f, -39.0f, 2.0f, 3.0f),
                new TreeData(-19.0f, -34.0f, 2.0f, 3.0f),
                new TreeData(-23.0f, -30.0f, 3.0f, 4.0f),
                new TreeData(-24.0f, -24.0f, 2.0f, 3.0f),
                new TreeData(-16.0f, -21.0f, 2.0f, 3.0f),
                new TreeData(-17.0f, -17.0f, 3.0f, 3.0f),
                new TreeData(-25.0f, -13.0f, 2.0f, 4.0f),
                new TreeData(-23.0f, -8.0f, 2.0f, 3.0f),
                new TreeData(-17.0f, -2.0f, 3.0f, 3.0f),
                new TreeData(-16.0f, 1.0f, 2.0f, 3.0f),
                new TreeData(-19.0f, 4.0f, 3.0f, 3.0f),
                new TreeData(-22.0f, 8.0f, 2.0f, 4.0f),
                new TreeData(-21.0f, 14.0f, 2.0f, 3.0f),
                new TreeData(-16.0f, 19.0f, 2.0f, 3.0f),
                new TreeData(-23.0f, 24.0f, 3.0f, 3.0f),
                new TreeData(-18.0f, 28.0f, 2.0f, 4.0f),
                new TreeData(-24.0f, 31.0f, 2.0f, 3.0f),
                new TreeData(-20.0f, 36.0f, 2.0f, 3.0f),
                new TreeData(-22.0f, 41.0f, 3.0f, 3.0f),
                new TreeData(-21.0f, 45.0f, 2.0f, 3.0f),

                new TreeData(-12.0f, -40.0f, 2.0f, 4.0f),
                new TreeData(-11.0f, -35.0f, 3.0f, 3.0f),
                new TreeData(-10.0f, -29.0f, 1.0f, 3.0f),
                new TreeData(-9.0f, -26.0f, 2.0f, 2.0f),
                new TreeData(-6.0f, -22.0f, 2.0f, 3.0f),
                new TreeData(-15.0f, -15.0f, 1.0f, 3.0f),
                new TreeData(-8.0f, -11.0f, 2.0f, 3.0f),
                new TreeData(-14.0f, -6.0f, 2.0f, 4.0f),
                new TreeData(-12.0f, 0.0f, 2.0f, 3.0f),
                new TreeData(-7.0f, 4.0f, 2.0f, 2.0f),
                new TreeData(-13.0f, 8.0f, 2.0f, 2.0f),
                new TreeData(-9.0f, 13.0f, 1.0f, 3.0f),
                new TreeData(-13.0f, 17.0f, 3.0f, 4.0f),
                new TreeData(-6.0f, 23.0f, 2.0f, 3.0f),
                new TreeData(-12.0f, 27.0f, 1.0f, 2.0f),
                new TreeData(-8.0f, 32.0f, 2.0f, 3.0f),
                new TreeData(-10.0f, 37.0f, 3.0f, 3.0f),
                new TreeData(-11.0f, 42.0f, 2.0f, 2.0f),

                new TreeData(15.0f, 5.0f, 2.0f, 3.0f),
                new TreeData(15.0f, 10.0f, 2.0f, 3.0f),
                new TreeData(15.0f, 15.0f, 2.0f, 3.0f),
                new TreeData(15.0f, 20.0f, 2.0f, 3.0f),
                new TreeData(15.0f, 25.0f, 2.0f, 3.0f),
                new TreeData(15.0f, 30.0f, 2.0f, 3.0f),
                new TreeData(15.0f, 35.0f, 2.0f, 3.0f),
                new TreeData(15.0f, 40.0f, 2.0f, 3.0f),
                new TreeData(15.0f, 45.0f, 2.0f, 3.0f),

                new TreeData(25.0f, 5.0f, 2.0f, 3.0f),
                new TreeData(25.0f, 10.0f, 2.0f, 3.0f),
                new TreeData(25.0f, 15.0f, 2.0f, 3.0f),
                new TreeData(25.0f, 20.0f, 2.0f, 3.0f),
                new TreeData(25.0f, 25.0f, 2.0f, 3.0f),
                new TreeData(25.0f, 30.0f, 2.0f, 3.0f),
                new TreeData(25.0f, 35.0f, 2.0f, 3.0f),
                new TreeData(25.0f, 40.0f, 2.0f, 3.0f),
                new TreeData(25.0f, 45.0f, 2.0f, 3.0f),
        };
	}
}

