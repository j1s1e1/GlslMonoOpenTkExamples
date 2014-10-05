using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class AttribType
	{
	    public String strNameFromFile;
	    public bool bNormalized;
	    public DrawElementsType eGLTypeDrawElement;	// this changes, so must be cast
		public VertexAttribPointerType eGLTypeDrawVertex;
	    public int iNumBytes;
	
	    public AttribType(String fn_in, bool bn_in, DrawElementsType egl_de_in, 
		                  VertexAttribPointerType egl_dv_in, int inum_in)
	    {
	        strNameFromFile = fn_in;
	        bNormalized = bn_in;
	        eGLTypeDrawElement = egl_de_in;
			eGLTypeDrawVertex = egl_dv_in;
	        iNumBytes = inum_in;
	    }
	
	    public List<AttribData> ParseFunc(StringBuilder sb)
	    {
	        List<AttribData> attrib_data = new List<AttribData>();
	        String[] items = sb.ToString().Split('\n');
	        for (int i = 0; i < items.Length; i++) 
			{
	            String[] values = items[i].Split(' ');
	            for (int j = 0; j < values.Length; j++) 
				{
	                try 
					{
	                    AttribData ad = new AttribData();
						if (eGLTypeDrawVertex == VertexAttribPointerType.Float)
						{
							switch (eGLTypeDrawVertex)
		                    {
								case VertexAttribPointerType.Float:
		                            ad.fValue = float.Parse(values[j]);
		                            break;
		                        default:  throw new Exception("Unidentified type");
		                    }
						}
						else
						{
		                    switch (eGLTypeDrawElement)
		                    {
		                        case DrawElementsType.UnsignedInt:
		                            ad.uiValue = UInt32.Parse(values[j]);
		                            break;
		                        case DrawElementsType.UnsignedShort:
		                            ad.usValue = UInt16.Parse(values[j]);
		                            break;
		                        case DrawElementsType.UnsignedByte:
		                            ad.ubValue = byte.Parse(values[j]);
		                            break;
		                        default:  throw new Exception("Unidentified type");
		                    }
						}
	                    attrib_data.Add(ad);
	                } catch (Exception ex) {
	                }
	            }
	
	        }
	        return attrib_data;
	    }
	
	
	    public void WriteToBuffer(BufferTarget eBuffer, List<AttribData> attribData, int iOffset)
	    {
	        int i = 0;
			if (eBuffer == BufferTarget.ArrayBuffer)
			{
				switch (eGLTypeDrawVertex)
		        {
					case VertexAttribPointerType.Float:
		                float[] floatBuffer = new float[attribData.Count];
		                foreach (AttribData a in attribData)
		                {
		                    floatBuffer[i++]  = a.fValue;
		                }
		                GL.BufferSubData(eBuffer, (IntPtr)iOffset, (IntPtr)(floatBuffer.Length * 4), floatBuffer);
		                break;
		            default:  throw new Exception("Unidentified type");
		        }
			}
			else
			{
		        switch (eGLTypeDrawElement)
		        {
		            case DrawElementsType.UnsignedInt:
		                uint[] uintBuffer = new uint[attribData.Count];
		                foreach (AttribData a in attribData)
		                {
		                    uintBuffer[i++]  = a.uiValue;
		                }
		                GL.BufferSubData(eBuffer, (IntPtr)iOffset, (IntPtr)(uintBuffer.Length * 4), uintBuffer);
		                break;
		            case DrawElementsType.UnsignedShort:
		                ushort[] ushortBuffer = new ushort[attribData.Count];
		                foreach (AttribData a in attribData)
		                {
		                    ushortBuffer[i++]  = a.usValue;
		                }
		                GL.BufferSubData(eBuffer, (IntPtr)iOffset, (IntPtr)(ushortBuffer.Length * 2), ushortBuffer);
		                break;
		            case DrawElementsType.UnsignedByte:
		                byte[] byteBuffer = new byte[attribData.Count];
		                foreach (AttribData a in attribData)
		                {
		                    byteBuffer[i++]  = a.ubValue;
		                }
		                GL.BufferSubData(eBuffer, (IntPtr)iOffset, (IntPtr)(byteBuffer.Length * 1), byteBuffer);  break;
		            default:  throw new Exception("Unidentified type");
		        }
			}
	    }
	}
}

