using System;
using System.Collections.Generic;
using OpenTK;

namespace GlslTutorials
{
	public class MeshData 
	{
	    public int[] oAttribArraysBuffer;  // switched to array
	    public int[] oIndexBuffer; // switched to array
		public int[]  oVAO;
		
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

		public int 		vertexCount = -1;
		public Vector3	positionMin;
		public Vector3 	positionMax;
	
	    public VAOMap namedVAOs;
	    public Dictionary<String, NamedVaoData> namedVaoData = new Dictionary<String, NamedVaoData>();
	
	    public List<RenderCmd> primatives;
	    public MeshData()
	    {
	        primatives = new List<RenderCmd>();
	        namedVAOs = new VAOMap();
	        oAttribArraysBuffer = new int[1];
	        oIndexBuffer = new int[1];
	        oVAO = new int[1];
	    }
	}

}

