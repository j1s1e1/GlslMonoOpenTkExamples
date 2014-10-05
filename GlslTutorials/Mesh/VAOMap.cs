using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public class VAOMap 
	{
	    Dictionary<string, int> map;
	
	    public VAOMap()
	    {
	        map = new Dictionary<string, int>();
	    }
	
	    public void Add(String key, int i)
	    {
	        map.Add(key, i);
	    }
		
	    public int Value(String key)
	    {
	        return map[key];
	    }
	
	    public void Remove(String key)
	    {
	        map.Remove(key);
	    }
	    public List<string> getKeys()
	    {
	        List<string> keys = new List<string>();
			foreach (string key in map.Keys)
		    {
		       keys.Add(key);
		    }
	        return keys;
	    }
	}
}

