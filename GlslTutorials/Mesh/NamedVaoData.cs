using System;

namespace GlslTutorials
{
	public class NamedVaoData 
	{
	    public int[]    oAttribArraysBuffer = new int[1];
		public int[]	oIndexBuffer = new int[1];
	    public int 		positionAttribute = -1;
	    public int 		colorAttribute = -1;
	    public int 		normalAttribute = -1;
	
	    public int 		positionSize = -1;
	    public int 		colorSize = -1;
	    public int 		normalSize = -1;
	
	    public int 		positionOffset = -1;
	    public int 		colorOffset = -1;
	    public int 		normalOffset = -1;
	
	
	    public int 		positionStride = -1;
	    public int 		colorStride = -1;
	    public int 		normalStride  = -1;
	}
}

