using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GlslTutorials
{
	public partial class GlsTutorialsClass : Form
	{
		static public string ProjectDirectory;
		public GlsTutorialsClass()
		{
			ProjectDirectory =  Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
			//ProjectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			InitializeComponent();
			this.TestSelection.DataSource = MenuClass.FillTestlist();
		}
		
		private void button1_Click(object sender, EventArgs e)
		{
            messages.Text = "Button 1 Click";
		}
		
		private void button2_Click(object sender, EventArgs e)
		{
            AddMessage("Button 2 Click");
		}
		
		private void button3_Click(object sender, EventArgs e)
		{
			SetupViewport (TestSelection.Text);
		}
		
		private void AddMessage(string message)
		{
			 messages.Text =  messages.Text + message + "\n";
		}
		
		
        private void SetupViewport (string selectedValue)
        {
			TutorialsEnum currentTest = (TutorialsEnum)Enum.Parse(typeof(TutorialsEnum), selectedValue);
            this.GlControl.Size = new System.Drawing.Size (512, 512);
            int w = GlControl.Width;
            int h = GlControl.Height;
			TutorialBase.GlControl  = GlControl;
            GL.MatrixMode (MatrixMode.Projection);
            GL.LoadIdentity ();
            GL.Ortho (0, w, 0, h, -1000, 1000); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport (0, 0, w, h); // Use all of the glControl painting area
			if (currentTutorial != null)
			{
				currentTutorial.Dispose();
			}
			MenuClass.StartTutorial(ref currentTutorial, currentTest);
			if (currentTutorial != null)
			{
				currentTutorial.Setup();
			}
        }

		int mouseState = 0;

		static int GLUT_ACTIVE_SHIFT = 0x0001;
		static int GLUT_ACTIVE_CTRL  = 0x0002;
		static int GLUT_ACTIVE_ALT   = 0x0004;

		private void glControlMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			mouseState = 0;
			if ((ModifierKeys & Keys.Control) == Keys.Control) mouseState &= GLUT_ACTIVE_CTRL;
			if ((ModifierKeys & Keys.Shift) == Keys.Shift) mouseState &= GLUT_ACTIVE_SHIFT;
			if ((ModifierKeys & Keys.Alt) == Keys.Alt) mouseState &= GLUT_ACTIVE_ALT;
			if (currentTutorial != null)
			{
				currentTutorial.MouseButton((int)e.Button, mouseState,  e.X, e.Y);
			}
		}

		private void glControlMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			mouseState = -1;
			currentTutorial.MouseButton((int)e.Button, mouseState,  e.X, e.Y);
		}

		private void glControlMousehandler(object sender, System.Windows.Forms.MouseEventArgs e)
        {
			if (currentTutorial != null)
			{
				currentTutorial.TouchEvent(e.X, e.Y);
			}
        }

		private void glControlMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (currentTutorial != null)
			{
				currentTutorial.MouseMotion(e.X, e.Y);
			}
		}
		
		private void glControlLoad (object sender, EventArgs e)
        {
            GL.ClearColor (Color.SkyBlue);
        }

        private void glControlPaint(object sender, PaintEventArgs e)
        {
			if (currentTutorial != null)
			{
				currentTutorial.display();
			}
        }

		static TutorialBase currentTutorial;

        private void glControlKeyDown (object sender, KeyEventArgs e)
        {
			if (currentTutorial != null)
			{
				messages.Clear();
				AddMessage(currentTutorial.keypress(e.KeyCode, 0, 0));
			}
        }

        private void glControlKeyUp (object sender, KeyEventArgs e)
        {
			if (currentTutorial != null)
			{
			}
        }
		
	}
}

