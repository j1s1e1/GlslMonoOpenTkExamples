using System;
using System.Drawing;

namespace GlslTutorials
{
	public class Framework 
	{
	    public static void ForwardMouseMotion(Pole forward, int x, int y)
	    {
	        forward.MouseMove(new Point(x, y));
	    }
	
	    public static void ForwardMouseButton(Pole forward, int button, int state, int x, int y)
	    {
	        forward.MouseButton(button, state, new Point(x, y));
	    }
	
	    public static void ForwardMouseWheel(Pole forward, int wheel, int direction, int x, int y)
	    {
	        forward.MouseWheel(wheel, direction, new Point(x, y));
	    }
	
	}
}

