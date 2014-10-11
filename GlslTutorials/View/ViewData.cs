using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class ViewData 
	{
	    public Vector3 targetPos;	///<The starting target position position.
	    public Quaternion orient;		///<The initial orientation aroudn the target position.
	    public float radius;			///<The initial radius of the camera from the target point.
	    public float degSpinRotation;	///<The initial spin rotation of the "up" axis, relative to \a orient
	    public ViewData(Vector3 t, Quaternion o, float r, float d)
	    {
	        targetPos = t;
	        orient = o;
	        radius = r;
	        degSpinRotation = d;
	
	    }
	}
}

