using System;

namespace GlslTutorials
{
	public class VertexShaders
	{
	 	public static String PosColorLocalTransform_vert =	// Working Single Mesh Item
		"attribute vec4 position;" +
	    "attribute vec4 color;" +
	
	    "uniform mat4 cameraToClipMatrix;" +
	    "uniform mat4 modelToCameraMatrix;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "vec4 cameraPos = modelToCameraMatrix * position;" +
	        "gl_Position = cameraToClipMatrix * cameraPos;" +
	        "theColor = color;" +
	    "}";
		
		 public static String PosOnlyWorldTransform_vert =	// Working Single Mesh Item
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
		
		 public static string DirAmbVertexLighting_PN_vert =	// Working Single Mesh Item
	    "attribute vec3 position;" +							// ?? Ambient lighting
		"attribute vec4 color;" + // added to make normal 2
	    "attribute vec3 normal;" +
	
	    "uniform vec3 dirToLight;" +
	    "uniform vec4 lightIntensity;" +
	    "uniform vec4 ambientIntensity;" +
	
	    "uniform mat4 modelToCameraMatrix;" +
	    "uniform mat3 normalModelToCameraMatrix;" +
	
	    "struct UniformBlock" +
	    "{" +
	        "mat4 cameraToClipMatrix;" +
	    "};" +
	
	    "uniform UniformBlock Projection;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_Position = Projection.cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
	
	        "vec3 normCamSpace = normalize(normalModelToCameraMatrix * normal);" +
	
	        "float cosAngIncidence = dot(normCamSpace, dirToLight);" +
	        "cosAngIncidence = clamp(cosAngIncidence, 0.0, 1.0);" +
	
	        "theColor = (lightIntensity * cosAngIncidence) + ambientIntensity + 0.01 * color;" +
	    "}";
		
	    public string positionNormal =
        "attribute vec4 a_Position;" +
        "attribute vec3 a_Normal;" +		// Per-vertex normal information we will pass in.
        "varying vec3 v_Normal;" +		// This will be passed into the fragment shader.
        "varying vec3 v_Position;" +		// This will be passed into the fragment shader.
        "void main() {" +
            "v_Position = vec3(a_Position);" +
            "v_Normal = a_Normal;" +
            "gl_Position = a_Position;" +
        "}";
		
		public static string PosColorWorldTransform_vert =	// Working Single Mesh Item
		"attribute vec4 position;" +
	    "attribute vec4 color;" +
	    
	    "uniform	mat4 cameraToClipMatrix;" +
	    "uniform	mat4 worldToCameraMatrix;" +
	    "uniform mat4 modelToWorldMatrix;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "vec4 temp = modelToWorldMatrix * position;" +
	        "temp = worldToCameraMatrix * temp;" +
	        "gl_Position = cameraToClipMatrix * temp;" +
	        "theColor = color;" +
	    "}";
		
		public static string DirVertexLighting_PN_vert =	// ?? Ambient Lighting
		"attribute vec3 position;" +
		"attribute vec4 color;" +		// added for spacing
	    "attribute vec3 normal;" +
	    
	
	    "uniform vec3 dirToLight;" +
	    "uniform vec4 lightIntensity;" +
	
	    "uniform mat4 modelToCameraMatrix;" +
	    "uniform mat3 normalModelToCameraMatrix;" +
	
	    "struct UniformBlock" +
	    "{" +
	        "mat4 cameraToClipMatrix;" +
	    "};" +
	
	    "uniform UniformBlock Projection;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_Position = Projection.cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
	
	        "vec3 normCamSpace = normalize(normalModelToCameraMatrix * normal);" +
	
	        "float cosAngIncidence = dot(normCamSpace, dirToLight);" +
	        "cosAngIncidence = clamp(cosAngIncidence, 0.0, 1.0);" +
	
	        "theColor = lightIntensity * cosAngIncidence + 0.01 * color;" +
	    "}";
		
		 public static string DirVertexLighting_PCN_vert =	// ?? Ambient Lighting
		"attribute vec3 position;" +
		"attribute vec4 diffuseColor;" +
	    "attribute vec3 normal;" +
	
	    "uniform vec3 dirToLight;" +
	    "uniform vec4 lightIntensity;" +
	
	    "uniform mat4 modelToCameraMatrix;" +
	    "uniform mat3 normalModelToCameraMatrix;" +
	
	    "struct UniformBlock" +
	    "{" +
	        "mat4 cameraToClipMatrix;" +
	    "};" +
		
		"uniform UniformBlock Projection;" +

	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_Position = Projection.cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
	
	        "vec3 normCamSpace = normalize(normalModelToCameraMatrix * normal);" +
	
	        "float cosAngIncidence = dot(normCamSpace, dirToLight);" +
	        "cosAngIncidence = clamp(cosAngIncidence, 0.0, 1.0);" +
	
	        "theColor = lightIntensity * diffuseColor * cosAngIncidence;" +
	    "}";
		
		public static string DirAmbVertexLighting_PCN_vert =	// ?? Ambient Lighting
		"attribute vec3 position;" +
		"attribute vec4 diffuseColor;" +
    	"attribute vec3 normal;" +
	
	    "uniform vec3 dirToLight;" +
	    "uniform vec4 lightIntensity;" +
	
	    "uniform mat4 modelToCameraMatrix;" +
	    "uniform mat3 normalModelToCameraMatrix;" +
	
	    "struct UniformBlock" +
	    "{" +
	        "mat4 cameraToClipMatrix;" +
	    "};" +
	
	    "uniform UniformBlock Projection;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_Position = Projection.cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
	
	        "vec3 normCamSpace = normalize(normalModelToCameraMatrix * normal);" +
	
	        "float cosAngIncidence = dot(normCamSpace, dirToLight);" +
	        "cosAngIncidence = clamp(cosAngIncidence, 0.0, 1.0);" +
	
	        "theColor = lightIntensity * diffuseColor * cosAngIncidence;" +
	    "}";

	}
}

