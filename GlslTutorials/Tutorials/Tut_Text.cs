using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Tut_Text : TutorialBase
	{
 		static List<TextClass> text;
		
		bool staticText = true;
		bool reverseRotation = true;
	
	    private void SetupText()
	    {
			TextClass new_text;
	        text = new List<TextClass>();
	        for (int i = 0; i < 2; i++)
	        {

	            if (i == 1)
	            {
	                new_text = new TextClass("ABC123", 1f, 0.1f, staticText, reverseRotation);
	            }
	            else
	            {
	                new_text = new TextClass("56789", 0.5f, 0.2f);
	            }
	
	            new_text.SetOffset(new Vector3(-0.5f, -0.5f+ i * 0.4f, 0.5f - i));
	            text.Add(new_text);
	        }
	        for (int i = 0; i < 2; i++)
	        {
	            if (i == 1)
	            {
	                new_text = new TextClass("867", 1f, 0.06f);
	            }
	            else
	            {
	                new_text = new TextClass("5309", 1f, 0.06f);
	            }
	
	            new_text.SetOffset(new Vector3(0f, -0.25f + i * 0.3f, 0.0f));
	            text.Add(new_text);
	        }
			
			new_text = new TextClass("ABCDEFGHIJKLM", 0.5f, 0.05f, staticText, reverseRotation);
			new_text.SetOffset(new Vector3(0f, 0.5f, 0.0f));
			text.Add(new_text);
			
			new_text = new TextClass("NOPQRSTUVWXYZ", 0.5f, 0.05f, staticText, reverseRotation);
			new_text.SetOffset(new Vector3(0f, 0.4f, 0.0f));
			text.Add(new_text);
	    }
	
	    protected override void init()
	    {
	        SetupText();
			SetupDepthAndCull();
	    }
	
	    //Called to update the display.
	    //You should call glutSwapBuffers after all of your rendering to display what you rendered.
	    //If you need continuous updates of the screen, call glutPostRedisplay() at the end of the function.
	    public override void display()
	    {
	        ClearDisplay();
	        foreach (TextClass t in text )
	        {
	            t.Draw();
	        }
	    }
		
		public override String keyboard(Keys keyCode, int x, int y)
	    {
	        StringBuilder result = new StringBuilder();
	        switch (keyCode) 
			{
				case Keys.D1:
					Shape.RotateWorld(Vector3.UnitX, 5f);
					break;
				case Keys.D2:
					Shape.RotateWorld(Vector3.UnitX, -5f);
					break;
				case Keys.D3:
					Shape.RotateWorld(Vector3.UnitY, 5f);
					break;
				case Keys.D4:
					Shape.RotateWorld(Vector3.UnitY, -5f);
					break;
				case Keys.D5:
					Shape.RotateWorld(Vector3.UnitZ, 5f);
					break;
				case Keys.D6:
					Shape.RotateWorld(Vector3.UnitZ, -5f);
					break;				
				case Keys.N:
					Shape.worldToCamera.M42 = Shape.worldToCamera.M42 + 0.01f;
					break;
				case Keys.S:
					Shape.worldToCamera.M42 = Shape.worldToCamera.M42 - 0.01f;
				    break;
				case Keys.E:
					Shape.worldToCamera.M41 = Shape.worldToCamera.M41 + 0.01f;
					break;
				case Keys.W:
					Shape.worldToCamera.M41 = Shape.worldToCamera.M41 - 0.01f;	
					break;
				case Keys.I:
					foreach (TextClass t in text )
			        {
			            result.AppendLine(t.CheckRotations(new Vector3(0f, 0f, 1f)));
			        }
					break;
	            case Keys.Space:
	                break;
	        }
	        result.AppendLine(keyCode.ToString());
	        reshape();
	        display();
	        return result.ToString();
	    }

	}
}

