using System;
using System.Drawing;

namespace GlslTutorials
{
	public interface IPole 
	{	
		void MouseButton(int button, int state, int x, int y);
		void MouseButton(int button, int state, Point p);
		void MouseClick(MouseButtons eButton, bool isPressed, int modifiers, Point p);
		void MouseMove(Point position);
		void MouseWheel(int wheel, int direction, Point p);
	}
}

