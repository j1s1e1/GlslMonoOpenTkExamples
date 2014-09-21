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
		
		public static string lms_vertexShaderCode =
        "attribute vec4 a_Position;" +
        //"attribute vec3 a_Normal;" +		// Per-vertex normal information we will pass in.
        //"varying out vec3 v_Normal;" +		// This will be passed into the fragment shader.
        //"varying out vec3 v_Position;" +		// This will be passed into the fragment shader.
        "void main()" +
		"{" +
        	//"v_Position = vec3(a_Position);" +
            //"v_Normal = a_Normal;" +
            "gl_Position = a_Position;" +
        "}";
	}
}

