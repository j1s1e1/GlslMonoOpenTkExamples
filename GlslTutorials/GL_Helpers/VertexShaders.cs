using System;

namespace GlslTutorials
{
	public class VertexShaders
	{
		public VertexShaders ()
		{
		}
		
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

