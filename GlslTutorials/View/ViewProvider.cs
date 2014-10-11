using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class ViewProvider : ViewPole 
	{
	    public ViewProvider(ViewData initialView, ViewScale viewScale) : 
			base(initialView, viewScale, MouseButtons.MB_LEFT_BTN, false)
	    {
		}
	       
	
	    public ViewProvider(ViewData initialView, ViewScale viewScale, MouseButtons actionButton) :
			base(initialView, viewScale, actionButton, false)
	    {
	        
	    }
	
	    public ViewProvider(ViewData initialView, ViewScale viewScale, MouseButtons actionButton,
	        bool bRightKeyboardCtrls):
			base(initialView, viewScale, actionButton, bRightKeyboardCtrls)
	    {
	       
	    }
	}
}

