using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class IndexData 
	{
	    public IndexData(XmlElement indexElem)
	    {
	        String strType = indexElem.GetAttribute("type");
	
	        if(!strType.Equals("uint") && !strType.Equals("ushort") && !strType.Equals("ubyte"))
	            throw new Exception("Improper 'type' attribute value on 'index' element.");
	
	        pAttribType = AttributesClass.GetAttribType(strType);
	
	        //Parse text
	        XmlNodeList nodes = indexElem.ChildNodes;
	
	        StringBuilder sb = new StringBuilder();
	        for (int i = 0; i < nodes.Count; i++ )
	        {
	            XmlNode pChild = nodes[i];
	            String next_value = pChild.Value;
	
	            sb.Append(next_value);
	        }
	
	        dataArray = pAttribType.ParseFunc(sb);
	
	        if(dataArray.Count == 0)
	            throw new Exception("The index element must have an array of values.");
	    }
	
	    public IndexData()
	    {
	    }
	
	    public IndexData(IndexData rhs)
	    {
	        pAttribType = rhs.pAttribType;
	        dataArray = rhs.dataArray;
	    }
	
	    public int CalcByteSize()
	    {
	        return dataArray.Count * pAttribType.iNumBytes;
	    }
	
	    public void FillBoundBufferObject(int iOffset)
	    {
	        pAttribType.WriteToBuffer(BufferTarget.ElementArrayBuffer, dataArray, iOffset);
	    }
	
	    public AttribType pAttribType;
	    public List<AttribData> dataArray;
	}

}

