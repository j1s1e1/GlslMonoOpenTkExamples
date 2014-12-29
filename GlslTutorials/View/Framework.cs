using System;
using System.Drawing;

namespace GlslTutorials
{
	public class Framework 
	{
		static int GLUT_ACTIVE_SHIFT = 0x0001;
		static int GLUT_ACTIVE_CTRL  = 0x0002;
		static int GLUT_ACTIVE_ALT   = 0x0004;

		public static void ForwardMouseMotion<T>(T forward, int x, int y) where T : IPole
	    {
	        forward.MouseMove(new Point(x, y));
	    }

		static int glutGetModifiers(int state)
		{

			return 0;
		}

		static int calc_glut_modifiers(int state)
		{
			int ret = 0;

			int modifiers = glutGetModifiers(state);
			if((modifiers & GLUT_ACTIVE_SHIFT) != 0)
				ret |= GLUT_ACTIVE_SHIFT;
			if((modifiers & GLUT_ACTIVE_CTRL) != 0)
				ret |= GLUT_ACTIVE_CTRL;
			if((modifiers & GLUT_ACTIVE_ALT) != 0)
				ret |= GLUT_ACTIVE_ALT;

			return ret;
		}
	
		public static void ForwardMouseButton<T>(T forward, int button, int state, int x, int y) where T : IPole
	    {
			int modifiers = calc_glut_modifiers(state);

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
			default:
				return;
			}

			bool buttonDown = true;

			if (state == -1)
			{
				buttonDown = false;
				forward.MouseClick(eButton, buttonDown, modifiers, mouseLoc);
			}
			else
			{
				forward.MouseClick(eButton, buttonDown, modifiers, mouseLoc);
				forward.MouseButton(button, state, mouseLoc);
			}
	    }
	
		public static void ForwardMouseWheel<T>(T forward, int wheel, int direction, int x, int y) where T : IPole
	    {
	        forward.MouseWheel(wheel, direction, new Point(x, y));
	    }
	
	}
}

