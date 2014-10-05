using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Attribute {
	    public Attribute()
	    {
	        iAttribIx = int.MaxValue;
	        iSize = -1;
	        bIsIntegral = false;
	    }
	
	    public Attribute(XmlElement attribElem)
	    {
	        int iAttributeIndex = -1;
	        String attribute = attribElem.GetAttribute("index");
	        iAttributeIndex = Int32.Parse(attribute);
	        if (!((0 <= iAttributeIndex) && (iAttributeIndex < 16)))
	            throw new Exception("Attribute index must be between 0 and 16.");
	        iAttribIx = (int)iAttributeIndex;
	
	        int iVectorSize = -1;
	        String VectorSize = attribElem.GetAttribute("size");
	        iVectorSize = Int32.Parse(VectorSize);
	        if (!((1 <= iVectorSize) && (iVectorSize < 5)))
	            throw new Exception("Attribute size must be between 1 and 4.");
	        iSize = iVectorSize;
	
	        pAttribType = AttributesClass.GetAttribType(attribElem.GetAttribute("type"));
	
	
	        bIsIntegral = false;
	        String pIntegralAttrib = attribElem.GetAttribute("integral");
	
	        if (!pIntegralAttrib.Equals(""))
	        {
	            String strIntegral = pIntegralAttrib;
	            if (strIntegral.Equals("true"))
	                bIsIntegral = true;
	            else if (strIntegral.Equals("false"))
	                bIsIntegral = false;
	            else
	                throw new Exception("Incorrect 'integral' value for the 'attribute'.");
	
	            if (pAttribType.bNormalized)
	                throw new Exception("Attribute cannot be both 'integral' and a normalized 'type'.");
	
	            //if (pAttribType.eGLType == GLES20.GL_FLOAT)
	            //    throw new Exception("Attribute cannot be both 'integral' and a floating-point 'type'.");
	        }
	
	        //Parse text
	        XmlNodeList nodes = attribElem.ChildNodes;
	        int i = 0;
	        StringBuilder sb = new StringBuilder();
	        for (XmlNode pChild = nodes[i]; i < nodes.Count; i++ )
	        {
	            String next_value = pChild.Value;
	            sb.Append(next_value);
	        }
	
	        dataArray = pAttribType.ParseFunc(sb);
	
	        if (dataArray.Count == 0)
	            throw new Exception("The attribute must have an array of values.");
	        if (dataArray.Count  % iSize != 0)
	            throw new Exception("The attribute's data must be a multiple of its size in elements.");
	
	    }
	
	    public Attribute(Attribute rhs)
	    {
	        iAttribIx = rhs.iAttribIx;
	        pAttribType = rhs.pAttribType;
	        iSize = rhs.iSize;
	        bIsIntegral = rhs.bIsIntegral;
	        dataArray = rhs.dataArray;
	    }
	
	    public int NumElements()
	    {
	        return dataArray.Count  / iSize;
	    }
	
	    public int CalcByteSize()
	    {
	        return dataArray.Count * pAttribType.iNumBytes;
	    }
	
	    public void FillBoundBufferObject(int iOffset)
	    {
	        pAttribType.WriteToBuffer(BufferTarget.ArrayBuffer, dataArray, iOffset);
	    }
	
	    public void SetupAttributeArray(int iOffset)
	    {
	        switch (iAttribIx)
	        {
	            case 0: break; // position
	            case 1: break; // color
	            case 2: break; // normal
	        }
	        GL.EnableVertexAttribArray(iAttribIx);
	        if (bIsIntegral)
	        {
	            //FIX_THIS GLES20.glVertexAttribIPointer(iAttribIx, iSize, pAttribType.eGLType, 0, iOffset);
	        }
	        else
	        {
	            GL.VertexAttribPointer(iAttribIx, iSize,
	                    pAttribType.eGLTypeDrawVertex, pAttribType.bNormalized, 0, iOffset);
	        }
	    }
	
	    public int iAttribIx;
	    public AttribType pAttribType;
	    public int iSize;
	    bool bIsIntegral;
	    List<AttribData> dataArray;
	}
}

