using System;

namespace GlslTutorials
{
	public partial class GlsTutorialsClass
	{
		private void SetupGlControl()
		{
			this.GlControl = new OpenTK.GLControl();
			this.GlControl.Context.SwapInterval = 1;
			this.GlControl.Context.VSync = true;
			this.GlControl.Location = new System.Drawing.Point(400, 50);
            this.GlControl.Size = new System.Drawing.Size (512, 512);
			this.GlControl.MouseDown += new System.Windows.Forms.MouseEventHandler(glControlMouseDown);
			this.GlControl.MouseUp += new System.Windows.Forms.MouseEventHandler(glControlMouseUp);
			this.GlControl.MouseClick += new System.Windows.Forms.MouseEventHandler(glControlMousehandler);
			this.GlControl.MouseMove += new System.Windows.Forms.MouseEventHandler(glControlMouseMove);
            this.GlControl.Paint += glControlPaint;
            this.GlControl.Load += glControlLoad;
            this.GlControl.KeyDown += glControlKeyDown;
            this.GlControl.KeyUp += glControlKeyUp;
			this.Controls.Add(this.GlControl);
		}
		
		private void InitializeComponent()
        {
			SetupGlControl();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.messages = new System.Windows.Forms.TextBox();
			this.TestSelection = new System.Windows.Forms.ComboBox();
            // 
            // button1
            // 
            this.button1.Name = "button1";
            this.button1.Location = new System.Drawing.Point(50, 48);
            this.button1.TabIndex = 0;
            this.button1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Name = "button2";
            this.button2.Location = new System.Drawing.Point(150, 48);
            this.button2.TabIndex = 1;
            this.button2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Name = "button3";
            this.button3.Location = new System.Drawing.Point(250, 48);
            this.button3.TabIndex = 2;
            this.button3.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // messages
            // 
            this.messages.Name = "messages";
            this.messages.ForeColor = System.Drawing.SystemColors.WindowText;
            this.messages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.messages.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.messages.Location = new System.Drawing.Point(64, 136);
            this.messages.TabIndex = 3;
            this.messages.Size = new System.Drawing.Size(300, 150);
            this.messages.BackColor = System.Drawing.SystemColors.Window;
            this.messages.Multiline = true;
            this.messages.Text = "";
			//
            // Test Selection Combo Box
            //
            this.TestSelection.Location = new System.Drawing.Point (50, 10);
            this.TestSelection.Size = new System.Drawing.Size (300, 25);
            // 
            // TestClass
            // 
            this.Name = "TestForm";
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.messages);
			this.Controls.Add(this.TestSelection);
            this.Text = "TestForm";
			
        }
	
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox messages;
		private System.Windows.Forms.ComboBox TestSelection;
		private OpenTK.GLControl GlControl;
	}
}

