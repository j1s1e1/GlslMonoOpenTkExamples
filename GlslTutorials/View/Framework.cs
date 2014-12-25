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
	
	    public static void ForwardMouseWheel(Pole forward, int wheel, int direction, int x, int y)
	    {
	        forward.MouseWheel(wheel, direction, new Point(x, y));
	    }
	
	}
}

