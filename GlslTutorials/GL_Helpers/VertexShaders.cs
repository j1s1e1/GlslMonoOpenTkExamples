using System;

namespace GlslTutorials
{
	public class VertexShaders
	{
	 	public static String PosColorLocalTransform_vert =
	    "attribute vec4 color;" +
	    "attribute vec4 position;" +
	
	    "uniform mat4 cameraToClipMatrix;" +
	    "uniform mat4 modelToCameraMatrix;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "vec4 cameraPos = modelToCameraMatrix * position;" +
	        "gl_Position = cameraToClipMatrix * cameraPos;" +
	        "theColor = color;" +
	    "}";
		
		 public static String PosOnlyWorldTransform_vert =
	    "attribute vec4 position;" +
	
	    "uniform mat4 cameraToClipMatrix;" +
	    "uniform mat4 worldToCameraMatrix;" +
	    "uniform mat4 modelToWorldMatrix;" +
	
	    "void main()" +
	    "{" +
	        "vec4 temp = modelToWorldMatrix *  position;" +
	        "temp = worldToCameraMatrix * temp;" +
	        "gl_Position = cameraToClipMatrix * temp;" +
	    " }";
	}
}

