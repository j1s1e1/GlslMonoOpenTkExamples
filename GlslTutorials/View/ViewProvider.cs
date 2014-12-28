using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public abstract class ViewProvider : IPole
	{
		public abstract Matrix4 CalcMatrix();
		public abstract void MouseButton(int button, int state, int x, int y);
		public abstract void MouseButton(int button, int state, Point p);
		public abstract void MouseClick(MouseButtons eButton, bool glut_down, int modifiers, Point p);
		public abstract void MouseMove(Point position);
		public abstract void MouseWheel(int wheel, int direction, Point p);
	}
}

