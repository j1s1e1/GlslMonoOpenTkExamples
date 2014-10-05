using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public class AttribIndexMap 
	{
	    Dictionary<int, int> map;
	
	    public AttribIndexMap()
	    {
	        map = new Dictionary<int, int>();
	    }
	
	    public void Add(int i1, int i2)
	    {
	        map.Add(i1, i2);
	    }
	}
}

