using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class AttributesClass 
	{
	
	    static AttributesClass()
	    {
	        InitializeG_All_AttribeTypes();
	    }
	
	    static AttribType[] g_allAttributeTypes;
	
	    public static void InitializeG_All_AttribeTypes()
	    {
	        g_allAttributeTypes = new AttribType[4];
	        g_allAttributeTypes[0] = new AttribType("float", false,  DrawElementsType.UnsignedInt, 
			                                        VertexAttribPointerType.Float, 4);
	        g_allAttributeTypes[1] = new AttribType("int", false, 	 DrawElementsType.UnsignedInt,
			                                        VertexAttribPointerType.Int, 4);
	        g_allAttributeTypes[2] = new AttribType("short", false,  DrawElementsType.UnsignedShort,
			                                        VertexAttribPointerType.Short, 2); // just using int size
	        g_allAttributeTypes[3] = new AttribType("ushort", false, DrawElementsType.UnsignedShort, 
			                                        VertexAttribPointerType.UnsignedShort, 2);  // just using int size
			// fix add vertex types
	    }
	    /*
	        {"uint",		false,	GL_UNSIGNED_INT,	sizeof(GLuint),		ParseUInts,		WriteUInts},
	        {"norm-int",	true,	GL_INT,				sizeof(GLint),		ParseInts,		WriteInts},
	        {"norm-uint",	true,	GL_UNSIGNED_INT,	sizeof(GLuint),		ParseUInts,		WriteUInts},
	        {"short",		false,	GL_SHORT,			sizeof(GLshort),	ParseShorts,	WriteShorts},
	        {"ushort",		false,	GL_UNSIGNED_SHORT,	sizeof(GLushort),	ParseUShorts,	WriteUShorts},
	        {"norm-short",	true,	GL_SHORT,			sizeof(GLshort),	ParseShorts,	WriteShorts},
	        {"norm-ushort",	true,	GL_UNSIGNED_SHORT,	sizeof(GLushort),	ParseUShorts,	WriteUShorts},
	        {"byte",		false,	GL_BYTE,			sizeof(GLbyte),		ParseBytes,		WriteBytes},
	        {"ubyte",		false,	GL_UNSIGNED_BYTE,	sizeof(GLubyte),	ParseUBytes,	WriteUBytes},
	        {"norm-byte",	true,	GL_BYTE,			sizeof(GLbyte),		ParseBytes,		WriteBytes},
	        {"norm-ubyte",	true,	GL_UNSIGNED_BYTE,	sizeof(GLubyte),	ParseUBytes,	WriteUBytes},
	    };
	    */
	
	    public static AttribType GetAttribType(String strType)
	    {
	        int iArrayCount = g_allAttributeTypes.Length;
	        int pAttrib = -1;
	        for (int i = 0; i < iArrayCount; i++) 
			{
	            if (strType.Equals(g_allAttributeTypes[i].strNameFromFile)) 
	            {
	                pAttrib = i;
	                break;
	            }
	        }
	
	        if (pAttrib == -1)
	            throw new Exception("Unknown 'type' field.");
	
	        return g_allAttributeTypes[pAttrib];
	    }
	
	}
}

