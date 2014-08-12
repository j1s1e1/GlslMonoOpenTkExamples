using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GlslTutorials
{
	public partial class GlsTutorialsClass : Form
	{
		public GlsTutorialsClass()
		{
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
		
		private void glControlMousehandler(object sender, System.Windows.Forms.MouseEventArgs e)
        {
			if (currentTutorial != null)
			{
				currentTutorial.TouchEvent(e.X, e.Y);
			}
            AddMessage("Mouse Event ");
        }
		
		bool loaded = false;
        private void glControlLoad (object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor (Color.SkyBlue);
        }

        private void glControlPaint(object sender, PaintEventArgs e)
        {
			if (currentTutorial != null)
			{
				currentTutorial.display();
			}
        }
		
		 int x = 0;
		
		static TutorialBase currentTutorial;

        private void glControlKeyDown (object sender, KeyEventArgs e)
        {
			if (currentTutorial != null)
			{
				currentTutorial.keypress(e.KeyCode, 0, 0);
			}
        }

        private void glControlKeyUp (object sender, KeyEventArgs e)
        {
			if (currentTutorial != null)
			{
				currentTutorial.keypress(e.KeyCode, 0, 0);
			}
        }
		
	}
}

