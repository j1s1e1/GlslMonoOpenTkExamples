using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class MeshPrimitiveType 
	{
	    public String strPrimitiveName;
	    public PrimitiveType eGLPrimType;
	    public MeshPrimitiveType()
	    {
	    }
	    public MeshPrimitiveType(String name, PrimitiveType mode)
	    {
	        strPrimitiveName = name;
	        eGLPrimType = mode;
	    }
	}
}

