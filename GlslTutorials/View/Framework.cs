using System;
using System.Drawing;

namespace GlslTutorials
{
	public class Framework 
	{
		public static void ForwardMouseMotion<T>(T forward, int x, int y) where T : IPole
	    {
	        forward.MouseMove(new Point(x, y));
	    }
	
		public static void ForwardMouseButton<T>(T forward, int button, int state, int x, int y) where T : IPole
	    {
			//int modifiers = calc_glut_modifiers();
			int modifiers = 0;

			bool buttonDown = true;  // FIXME

			Point mouseLoc = new Point(x, y);



			MouseButtons eButton = (MouseButtons)(-1);

			switch (button)
			{
			case (int) System.Windows.Forms.MouseButtons.Left: 
				eButton = MouseButtons.MB_LEFT_BTN;
				break;
			case (int) System.Windows.Forms.MouseButtons.Right:
				eButton = MouseButtons.MB_RIGHT_BTN;
				break;
			case (int) System.Windows.Forms.MouseButtons.Middle: 
				eButton = MouseButtons.MB_MIDDLE_BTN;
				break;
			}

			forward.MouseClick(eButton, buttonDown, modifiers, mouseLoc);
			forward.MouseButton(button, state, mouseLoc);
	    }
	
		public static void ForwardMouseWheel<T>(T forward, int wheel, int direction, int x, int y) where T : IPole
	    {
	        forward.MouseWheel(wheel, direction, new Point(x, y));
	    }
	
	}
}

