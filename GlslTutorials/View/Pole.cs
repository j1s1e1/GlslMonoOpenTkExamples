using System;
using System.Drawing;

namespace GlslTutorials
{
	public class Pole 
	{
	    public Pole()
	    {
	    }
	
		public virtual void MouseButton(int button, int state, int x, int y)
	    {
	        MouseButton(button, state, new Point(x,y));
	    }
	
		public virtual void MouseButton(int button, int state, Point p)
	    {

	    }
	
		public virtual void MouseClick(MouseButtons eButton, bool glut_down, int modifiers, Point p)
	    {
	    }
	
	    public virtual void MouseMove(Point position)
	    {

	    }
	
		public virtual void MouseWheel(int wheel, int direction, Point p)
	    {
	    }
	
		public virtual void ForwardMouseMotion(Pole forward, int x, int y)
	    {
	        forward.MouseMove(new Point(x, y));
	    }
	}
}

