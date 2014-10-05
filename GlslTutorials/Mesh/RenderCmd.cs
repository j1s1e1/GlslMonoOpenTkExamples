using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class RenderCmd 
	{
	    public bool bIsIndexedCmd;
	    public PrimitiveType ePrimType;
	    public int start;
	    public int elemCount;
	    public DrawElementsType eIndexDataType;	//Only if bIsIndexedCmd is true.
	    public int primRestart;		//Only if bIsIndexedCmd is true.
	
	    public void Render()
	    {
	        try
	        {
	            if (bIsIndexedCmd)
	              	GL.DrawElements(ePrimType, elemCount, eIndexDataType, start);
	            else
	              	GL.DrawArrays(ePrimType, start, elemCount);
	        }
	        catch (Exception ex)
	        {
	            throw new Exception("Error rendering mesh: " + ex.ToString());
	        }
	    }
	}

}

