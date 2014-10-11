using System;

namespace GlslTutorials
{
	public class ViewScale 
	{
	    float minRadius;		///<The closest the radius to the target point can get.
	    float maxRadius;		///<The farthest the radius to the target point can get.
	    float largeRadiusDelta;	///<The radius change to use when the SHIFT key isn't held while mouse wheel scrolling.
	    float smallRadiusDelta;	///<The radius change to use when the SHIFT key \em is held while mouse wheel scrolling.
	    float largePosOffset;	///<The position offset to use when the SHIFT key isn't held while pressing a movement key.
	    float smallPosOffset;	///<The position offset to use when the SHIFT key \em is held while pressing a movement key.
	    public float rotationScale;	///<The number of degrees to rotate the view per window space pixel the mouse moves when dragging.
	    public ViewScale(float mi, float ma, float la, float sm, float la2, float sm2, float ro)
	    {
	        minRadius = mi;
	        maxRadius = ma;
	        largeRadiusDelta = la;
	        smallRadiusDelta = sm;
	        largePosOffset = la2;
	        smallPosOffset = sm2;
	        rotationScale = ro;
	    }
	}
}

