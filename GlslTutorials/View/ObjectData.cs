using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class ObjectData 
	{
	    public Vector3 position;			///<The world-space position of the object.
	    public Quaternion orientation;		///<The world-space orientation of the object.
	    public ObjectData(Vector3 p, Quaternion o)
	    {
	        position = p;
	        orientation = o;
	    }
	}
}

