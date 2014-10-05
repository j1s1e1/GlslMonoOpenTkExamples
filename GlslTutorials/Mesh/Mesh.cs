using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Mesh
	{
	    static Mesh()
	    {
	        InitializeGallPrimativeTypes();
	    }

	    private MeshData m_pData;
	
	    static MeshPrimitiveType[] g_allPrimitiveTypes;
	
	    static private void InitializeGallPrimativeTypes()
	    {
	        g_allPrimitiveTypes = new MeshPrimitiveType[7];
	        g_allPrimitiveTypes[0] = new MeshPrimitiveType("triangles", PrimitiveType.Triangles);
	        g_allPrimitiveTypes[1] = new MeshPrimitiveType("tri-strip", PrimitiveType.TriangleStrip);
	        g_allPrimitiveTypes[2] = new MeshPrimitiveType("tri-fan", PrimitiveType.TriangleFan);
	        g_allPrimitiveTypes[3] = new MeshPrimitiveType("lines", PrimitiveType.Lines);
	        g_allPrimitiveTypes[4] = new MeshPrimitiveType("line-strip", PrimitiveType.LineStrip);
	        g_allPrimitiveTypes[5] = new MeshPrimitiveType("line-loop", PrimitiveType.LineLoop);
	        g_allPrimitiveTypes[6] = new MeshPrimitiveType("points", PrimitiveType.Points);
	    }
		
	    RenderCmd ProcessRenderCmd(XmlElement cmdElem)
	    {
	        RenderCmd cmd = new RenderCmd();
	
	        String strCmdName = cmdElem.GetAttribute("cmd");
	        int iArrayCount = g_allPrimitiveTypes.Length;
	        MeshPrimitiveType pPrim = new MeshPrimitiveType();
	        bool match = false;
	        foreach (MeshPrimitiveType pt in g_allPrimitiveTypes)
	        {
	            if (pt.strPrimitiveName.Equals(strCmdName))
	            {
	                match = true;
	                pPrim = pt;
	            }
	        }
	
	        if(match == false)
	            throw new Exception("Unknown 'cmd' field.");
	
	        cmd.ePrimType = pPrim.eGLPrimType;
	
	        String strElemName = cmdElem.Name;
	        if(strElemName.Equals("indices"))
	        {
	            cmd.bIsIndexedCmd = true;
	            try
	            {
	                String test = cmdElem.GetAttribute("prim-restart");
	                cmd.primRestart = Int32.Parse(cmdElem.GetAttribute("prim-restart"));
	            }
	            catch (Exception ex)
	            {
	                cmd.primRestart = -1;
	            }
	        }
	        else if(strElemName.Equals("arrays"))
	        {
	            cmd.bIsIndexedCmd = false;
	            cmd.start = Int32.Parse(cmdElem.GetAttribute("start"));
	            if(cmd.start < 0)
	                throw new Exception("`array` 'start' index must be between 0 or greater.");
	
	            cmd.elemCount = Int32.Parse(cmdElem.GetAttribute("count"));
	            if(cmd.elemCount <= 0)
	                throw new Exception("`array` 'count' must be between 0 or greater.");
	        }
	        else
	            throw new Exception("Bad command element " + strElemName + ". Must be 'indices' or 'arrays'.");
	
	        return cmd;
	    }
	
	    public Mesh(Stream inputStream) 
		{
	        int i;
	        m_pData = new MeshData();
	        List<Attribute> attribs = new List<Attribute>();
	        AttribIndexMap attribIndexMap = new AttribIndexMap();
	
	        List<IndexData> indexData = new List<IndexData>();
	
	        AttributeCollection namedVaoList = new AttributeCollection();
	
	        {
				XmlDocument myXMLDoc = new XmlDocument();
				myXMLDoc.Load(inputStream);
				
				XmlElement root = myXMLDoc.DocumentElement;

	            XmlNodeList attributeNodes = root.GetElementsByTagName("attribute");
	            if (attributeNodes.Count > 0) {
	                for (i = 0; i < attributeNodes.Count; i++) {
	                    {
	                        XmlNode vaoNode = attributeNodes[i];
	                        attribs.Add(new Attribute((XmlElement) vaoNode));
	                        attribIndexMap.Add(attribs[attribs.Count - 1].iAttribIx, attribs.Count - 1);
	                    }
	                }
	
	                XmlNodeList vaoNodes = root.GetElementsByTagName("vao");
	                if (vaoNodes.Count > 0) {
	                    for (i=0;  i < vaoNodes.Count; i++) {
	                        XmlNode vaoNode = vaoNodes.Item(i);
	                        ProcessVAO((XmlElement) vaoNode, namedVaoList);
	                    }
	                }
	
	                XmlNodeList iNodes = root.GetElementsByTagName("indices");
	                if (iNodes.Count > 0) {
	                    for (i = 0; i < iNodes.Count;i++ ) {
	                        XmlNode iNode = iNodes.Item(i);
	                        m_pData.primatives.Add(ProcessRenderCmd((XmlElement) iNode));
	                        indexData.Add(new IndexData((XmlElement) iNode));
	                    }
	                }
	
	                XmlNodeList aNodes = root.GetElementsByTagName("arrays");
	                if (aNodes.Count > 0) {
	                    for (i = 0; i < aNodes.Count;i++ ) {
	                        XmlNode iNode = aNodes.Item(i);
	                        m_pData.primatives.Add(ProcessRenderCmd((XmlElement) iNode));
	                    }
	                }
	
	                //Figure out how big of a buffer object for the attribute data we need.
	                int iAttrbBufferSize = 0;
	                List<int> attribStartLocs = new List<int>();
	                int iNumElements = 0;
	                for (int iLoop = 0; iLoop < attribs.Count; iLoop++) {
	                    iAttrbBufferSize = ((iAttrbBufferSize % 16) != 0) ?
	                            (iAttrbBufferSize + (16 - iAttrbBufferSize % 16)) : iAttrbBufferSize;
	
	                    attribStartLocs.Add(iAttrbBufferSize);
	
	                    iAttrbBufferSize += attribs[iLoop].CalcByteSize();
	
	                    if (iNumElements != 0) {
	                        if (iNumElements != attribs[iLoop].NumElements()) {
	                            throw new Exception("Some of the attribute arrays have different element counts. "
	                                    + iNumElements.ToString() + "  " + attribs[iLoop].NumElements().ToString());
	                        }
	                    } else
	                        iNumElements = attribs[iLoop].NumElements();
	                }
	
	                //Create the "Everything" VAO.
	                GL.GenVertexArrays(1, m_pData.oVAO);
	                GL.BindVertexArray(m_pData.oVAO[0]);
	
	                //Create the buffer object.
	                GL.GenBuffers(1, m_pData.oAttribArraysBuffer);
	                // Changing these only works if other group changes as well.
	                GL.BindBuffer(BufferTarget.ArrayBuffer, m_pData.oAttribArraysBuffer[0]);
	                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)iAttrbBufferSize,
	                        (IntPtr)0,BufferUsageHint.StaticDraw);
	
	                //Fill in our data and set up the attribute arrays.
	                for (int iLoop = 0; iLoop < attribs.Count; iLoop++) {
	                    switch (iLoop)
	                    {
	                        case 0:
	                            m_pData.positionAttribute = 0;
	                            m_pData.positionSize = attribs[iLoop].iSize;
	                            m_pData.positionOffset = attribStartLocs[iLoop];
	                            m_pData.positionStride = m_pData.positionSize * attribs[iLoop].pAttribType.iNumBytes;
	                            break;
	                        case 1:
	                            m_pData.colorAttribute = 1;
	                            m_pData.colorSize = attribs[iLoop].iSize;
	                            m_pData.colorOffset = attribStartLocs[iLoop];
	                            m_pData.colorStride = m_pData.colorSize * attribs[iLoop].pAttribType.iNumBytes;
	                            break;
	                        case 2:
	                            m_pData.normalAttribute = 2;
	                            m_pData.normalSize = attribs[iLoop].iSize;
	                            m_pData.normalOffset = attribStartLocs[iLoop];
	                            m_pData.normalStride = m_pData.normalSize * attribs[iLoop].pAttribType.iNumBytes;
	                            break;
	                    }
	                    attribs[iLoop].FillBoundBufferObject(attribStartLocs[iLoop]);
	                    attribs[iLoop].SetupAttributeArray(attribStartLocs[iLoop]);
	                }
	
	                GL.BindVertexArray(0);
	                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	
	                //Fill the named VAOs.
	                List<String> keys = namedVaoList.GetKeys();
	
	                foreach (String key in keys)
	                {
	                    NamedVaoData namedVaoData = new NamedVaoData();
	                    List<int> namedVao = namedVaoList.collection[key];
	                    int[] vao = new int[1];
	                    vao[0] = 1; // using array buffer, just selecting items
	                    GL.GenVertexArrays(1, vao);
	                    GL.BindVertexArray(vao[0]);
	                    int bufferSize = 0;
	
	                    for (int iAttribIx = 0; iAttribIx < namedVao.Count; iAttribIx++) {
	                        int iAttrib = namedVao[iAttribIx];
	                        int iAttribOffset = -1;
	                        for (int iCount = 0; iCount < attribs.Count; iCount++) {
	                            bufferSize = bufferSize + attribs[iCount].iSize * attribs[iCount].pAttribType.iNumBytes;
	                            switch (iCount)
	                            {
	                                case 0:
	                                    namedVaoData.positionAttribute = 0;
	                                    namedVaoData.positionSize = attribs[iCount].iSize;
	                                    namedVaoData.positionOffset = attribStartLocs[iCount];
	                                    namedVaoData.positionStride = m_pData.positionSize * attribs[iCount].pAttribType.iNumBytes;
	                                    break;
	                                case 1:
	                                    namedVaoData.colorAttribute = 1;
	                                    namedVaoData.colorSize = attribs[iCount].iSize;
	                                    namedVaoData.colorOffset = attribStartLocs[iCount];
	                                    namedVaoData.colorStride = m_pData.colorSize * attribs[iCount].pAttribType.iNumBytes;
	                                    break;
	                                case 2:
	                                    namedVaoData.normalAttribute = 2;
	                                    namedVaoData.normalSize = attribs[iCount].iSize;
	                                    namedVaoData.normalOffset = attribStartLocs[iCount];
	                                    namedVaoData.normalStride = m_pData.normalSize * attribs[iCount].pAttribType.iNumBytes;
	                                    break;
	                            }
	                            if (attribs[iCount].iAttribIx == iAttrib) {
	                                iAttribOffset = iCount;
	                                break;
	                            }
	                        }
	                        // skip this use other buffer attribs.get(iAttribOffset).SetupAttributeArray(attribStartLocs.get(iAttribOffset));
	                    }
	
	
	                    m_pData.namedVAOs.Add(key, vao[0]);
	                    m_pData.namedVaoData.Add(key, namedVaoData);
	                    GL.BindVertexArray(0);
	                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	                }
	
	                //Get the Size of our index buffer data.
	                int iIndexBufferSize = 0;
	                List<int> indexStartLocs = new List<int>();
	                for (int iLoop = 0; iLoop < indexData.Count; iLoop++) {
	                    iIndexBufferSize = ((iIndexBufferSize % 16) != 0) ?
	                            (iIndexBufferSize + (16 - iIndexBufferSize % 16)) : iIndexBufferSize;
	
	                    indexStartLocs.Add(iIndexBufferSize);
	                    IndexData currData = indexData[iLoop];
	
	                    iIndexBufferSize += currData.CalcByteSize();
	                }
	
	                //Create the index buffer object.
	                if (iIndexBufferSize != 0) {
	                    GL.BindVertexArray(m_pData.oVAO[0]);
	
	                    GL.GenBuffers(1, m_pData.oIndexBuffer);
	                    // Changing these only works if other group changes as well.
	                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_pData.oIndexBuffer[0]);
	                    GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)iIndexBufferSize,
	                            (IntPtr)iIndexBufferSize, BufferUsageHint.StaticDraw);
	
	                    //Fill with data.
	                    for (int iLoop = 0; iLoop < indexData.Count; iLoop++) {
	                        IndexData currData = indexData[iLoop];
	                        currData.FillBoundBufferObject(indexStartLocs[iLoop]);
	                    }
	
	                    //Fill in indexed rendering commands.
	                    int iCurrIndexed = 0;
	                    for (int iLoop = 0; iLoop < m_pData.primatives.Count; iLoop++) {
	                        RenderCmd prim = m_pData.primatives[iLoop];
	                        if (prim.bIsIndexedCmd) {
	                            prim.start = indexStartLocs[iCurrIndexed];
	                            prim.elemCount = indexData[iCurrIndexed].dataArray.Count;
	                            prim.eIndexDataType = indexData[iCurrIndexed].pAttribType.eGLTypeDrawElement;
	                            iCurrIndexed++;
	                            m_pData.primatives[iLoop] = prim;
	                        }
	                    }
	
	                    List<String> vaoKeys = new List<String>(m_pData.namedVAOs.getKeys());
	                    foreach (String key in vaoKeys) 
						{
	                        GL.BindVertexArray(m_pData.namedVAOs.Value(key));
	                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_pData.oIndexBuffer[0]);
	                    }
	
	                    GL.BindVertexArray(0);
	                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	                } else {
	                    throw new Exception("Empty Index Buffer for VAO");
	                }
	            }
	        }
	    }
		
	    void ProcessVAO(XmlElement vaoElem, AttributeCollection attributes)
	    {
	        List<int> source_attributes = new List<int>();
	       bool sources_found = false;
	
	        XmlNodeList XmlNodeList = vaoElem.GetElementsByTagName("source");
	        if (XmlNodeList.Count > 0)
	        {
	            sources_found = true;
	            for (int i = 0; i < XmlNodeList.Count; i++ )
	            {
	                XmlNode pSource = XmlNodeList.Item(i);
	                XmlAttributeCollection  element_attributes = pSource.Attributes;
	                int j = 0;
	                for (XmlNode attrib = element_attributes[j]; j < element_attributes.Count; j++) {
	                    source_attributes.Add(Int32.Parse(attrib.Value));
	                }
	            }
	            if (sources_found) {
	                String vao_name = vaoElem.GetAttribute("name");
	                attributes.Add(vaoElem.GetAttribute("name"), source_attributes);
	            } else {
	                throw new Exception("No sources found for vao");
	            }
	        }
	    }
	
	
	
	    public void Render()
	    {
	        if (m_pData.oVAO[0] == 0)
	        if (m_pData.oIndexBuffer[0] == 0)
	            return;
	
	        GL.BindVertexArray(m_pData.oVAO[0]);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, m_pData.oAttribArraysBuffer[0]);
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_pData.oIndexBuffer[0]);
	        if (m_pData.positionAttribute != -1)
	        {
	            GL.EnableVertexAttribArray(m_pData.positionAttribute);
	            GL.VertexAttribPointer(m_pData.positionAttribute, m_pData.positionSize, 
				        VertexAttribPointerType.Float, false, m_pData.positionStride, m_pData.positionOffset);
	        }
	        if (m_pData.colorAttribute != -1)
	        {
	            GL.EnableVertexAttribArray(m_pData.colorAttribute);
	            GL.VertexAttribPointer(m_pData.colorAttribute, m_pData.colorSize,
	                   VertexAttribPointerType.Float, false, m_pData.colorStride, m_pData.colorOffset);
	
	        }
	        if (m_pData.normalAttribute != -1)
	        {
	            GL.EnableVertexAttribArray(m_pData.normalAttribute);
	            GL.VertexAttribPointer(m_pData.normalAttribute, m_pData.normalSize,
	                    VertexAttribPointerType.Float, false, m_pData.normalStride, m_pData.normalOffset);
	        }
	
	
	        foreach (RenderCmd renderCmd in m_pData.primatives)
	        {
	            renderCmd.Render();
	        }
	        if (m_pData.positionAttribute != -1) GL.DisableVertexAttribArray(m_pData.positionAttribute);
	        if (m_pData.colorAttribute != -1) GL.DisableVertexAttribArray(m_pData.colorAttribute);
	        if (m_pData.normalAttribute != -1) GL.DisableVertexAttribArray(m_pData.normalAttribute);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	        GL.BindVertexArray(0);
	    }
	
	    public void Render(String strMeshName)
	    {
	        int theIt = m_pData.namedVAOs.Value(strMeshName);
	        if (theIt == 0)
	            return;
	
	        if (m_pData.oAttribArraysBuffer[0] == 0)
	            return;
	
	        GL.BindVertexArray(theIt);
	
	        NamedVaoData namedVaoData = m_pData.namedVaoData[strMeshName];
	
	        GL.BindBuffer(BufferTarget.ArrayBuffer, m_pData.oAttribArraysBuffer[0]);
	        if ( m_pData.oIndexBuffer[0] != 0) GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_pData.oIndexBuffer[0]);
	        GL.BindBuffer(BufferTarget.ElementArrayBuffer, namedVaoData.oIndexBuffer[0]);
	        if (namedVaoData.positionAttribute != -1)
	        {
	            GL.EnableVertexAttribArray(namedVaoData.positionAttribute);
	            GL.VertexAttribPointer(namedVaoData.positionAttribute, namedVaoData.positionSize,
	                    VertexAttribPointerType.Float, false, namedVaoData.positionStride, namedVaoData.positionOffset);
	        }
	        if (namedVaoData.colorAttribute != -1)
	        {
	            GL.EnableVertexAttribArray(namedVaoData.colorAttribute);
	            GL.VertexAttribPointer(namedVaoData.colorAttribute, namedVaoData.colorSize,
	                    VertexAttribPointerType.Float, false, namedVaoData.colorStride, namedVaoData.colorOffset);
	
	        }
	        if (namedVaoData.normalAttribute != -1)
	        {
	            GL.EnableVertexAttribArray(namedVaoData.normalAttribute);
	            GL.VertexAttribPointer(namedVaoData.normalAttribute, namedVaoData.normalSize,
	                    VertexAttribPointerType.Float, false, namedVaoData.normalStride, namedVaoData.normalOffset);
	        }
	
	        foreach (RenderCmd renderCmd in m_pData.primatives)
	        {
	            renderCmd.Render();
	        }
	        GL.BindVertexArray(0);
	        if (namedVaoData.positionAttribute != -1) GL.DisableVertexAttribArray(namedVaoData.positionAttribute);
	        if (namedVaoData.colorAttribute != -1) GL.DisableVertexAttribArray(namedVaoData.colorAttribute);
	        if (namedVaoData.normalAttribute != -1) GL.DisableVertexAttribArray(namedVaoData.normalAttribute);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	    }
		
		public void DeleteObjects()
        {
	        GL.DeleteBuffers(1, m_pData.oAttribArraysBuffer);
	        m_pData.oAttribArraysBuffer[0] = 0;
	        GL.DeleteBuffers(1, m_pData.oIndexBuffer);
	        m_pData.oIndexBuffer[0] = 0;
	        GL.DeleteBuffers(1, m_pData.oVAO);
	        m_pData.oVAO[0] = 0;
	
	        List<String> keys = new List<String>(m_pData.namedVAOs.getKeys());
	        int[] current_value  = new int[1];
	        foreach (String key in keys)
	        {
	            current_value[0]  =  m_pData.namedVAOs.Value(key);
	            GL.DeleteVertexArrays(1, current_value);
	            m_pData.namedVAOs.Remove(key);
	        }
		}
    }
}

