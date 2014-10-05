using System;
using System.Collections.Generic;

namespace GlslTutorials
{
	public class AttributeCollection
	{
	    public Dictionary<String, List<int>> collection;
	
	    public AttributeCollection()
	    {
	        collection = new Dictionary<String, List<int>>();
	    }
	
	    public void Add(String s, List<int> ints) 
		{
	        collection.Add(s, ints);
	    }
	
	    public int Count()
	    {
	        return collection.Count;
	    }
		
		public List<string> GetKeys()
	    {
			List<string> keys = new List<string>();			
		    foreach (string key in collection.Keys)
		    {
		       keys.Add(key);
		    }
	        return keys;
	    }
	
	    public List<int> getValue(String key)
	    {
	        return collection[key];
	    }
	}
}

