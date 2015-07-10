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

		public static String worldTransformObjectPositionColor =
		"attribute vec4 position;" +

		"uniform mat4 cameraToClipMatrix;" +
		"uniform mat4 worldToCameraMatrix;" +
		"uniform mat4 modelToWorldMatrix;" +

		"varying vec4 objectPosition;" +

		"void main()" +
		"{" +
			"objectPosition = vec4(abs(position));" +
			"objectPosition = clamp(objectPosition, 0.0, 1.0);" +
			"vec4 temp = modelToWorldMatrix *  position;" +
			"temp = worldToCameraMatrix * temp;" +
			"gl_Position = cameraToClipMatrix * temp;" +
		" }";
		
		public static string lms_vertexShaderCode =
        "attribute vec4 position;" +
        "attribute vec3 normal;" +		// Per-vertex normal information we will pass in.

		"uniform mat4 cameraToClipMatrix;" +
	    "uniform mat4 worldToCameraMatrix;" +
	    "uniform mat4 modelToWorldMatrix;" +
				
		"varying vec3 v_Normal;" +		// This will be passed into the fragment shader.
        "varying vec3 v_Position;" +	// This will be passed into the fragment shader.
        "void main()" +
		"{" +
			"vec4 temp = modelToWorldMatrix * position;" +
	        "temp = worldToCameraMatrix * temp;" +
	        "temp = cameraToClipMatrix * temp;" +
        	"v_Position = vec3(temp);" +
            "v_Normal = normal;" +
            "gl_Position = temp;" +
        "}";
		
		 public static string DirAmbVertexLighting_PN_vert =	// Working Single Mesh Item
	    "attribute vec3 position;" +							// ?? Ambient lighting works for cylinder
		"attribute vec4 color;" + // added to make normal 2		// if normal matrixes are used
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
			//"theColor = vec4(1.0, 0.0, 0.0, 1.0);" +	
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
		
		public static string DirVertexLighting_PN_vert =	// ?? Ambient Lighting works for cylinder
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
	
	        "theColor = lightIntensity * cosAngIncidence + 0.001 * color;" +
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
		
		public static string DirAmbVertexLighting_PCN_vert =	// ?? Ambient Lighting works for cylinder
		"attribute vec3 position;" +
		"attribute vec4 diffuseColor;" +
    	"attribute vec3 normal;" +
	
	    "uniform vec3 dirToLight;" +
	    "uniform vec4 lightIntensity;" +
	
	    "uniform mat4 modelToCameraMatrix;" +
	    "uniform mat3 normalModelToCameraMatrix;" +
	
	    "uniform mat4 cameraToClipMatrix;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_Position = cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
			//"gl_Position = modelToCameraMatrix * (cameraToClipMatrix * vec4(position, 1.0));" +
			//"gl_Position =	(modelToCameraMatrix * vec4(position, 1.0));" +
			//"gl_Position =	(cameraToClipMatrix * vec4(position, 1.0));" +
			//"gl_Position =	vec4(position, 1.0);" +
	        "vec3 normCamSpace = normalize(normalModelToCameraMatrix * normal);" +
	
	        "float cosAngIncidence = dot(normCamSpace, dirToLight);" +
	        "cosAngIncidence = clamp(cosAngIncidence, 0.0, 1.0);" +
	
	        "theColor = lightIntensity * diffuseColor * cosAngIncidence;" +
	    "}";
		
		public static string ModelPosVertexLighting_PN =
		
		"attribute vec3 position;" +
		"attribute vec4 color;" + // added to keep positions
		"attribute vec3 normal;" +

		"uniform vec3 modelSpaceLightPos;" +
		"uniform vec4 lightIntensity;" +
		"uniform vec4 ambientIntensity;" +

		"uniform mat4 modelToCameraMatrix;" +
		
	    "uniform mat4 cameraToClipMatrix;" +
					
		"varying vec4 theColor;" +
		
		"void main()" +
		"{" +
			"gl_Position = cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
		
			"vec3 dirToLight = normalize(modelSpaceLightPos - position);" +
			
			"float cosAngIncidence = dot(normal, dirToLight);" +
			"cosAngIncidence = clamp(cosAngIncidence, 0, 1);" +
			
			"theColor = (lightIntensity * cosAngIncidence) + ambientIntensity + 0.001 * color;" +
		"}";
		
		public static string ModelPosVertexLighting_PCN = 
		"attribute vec3 position;" +
		"attribute vec4 inDiffuseColor;" +
		"attribute vec3 normal;" +

		"uniform vec3 modelSpaceLightPos;" +
		"uniform vec4 lightIntensity;" +
		"uniform vec4 ambientIntensity;" +
		"uniform mat4 modelToCameraMatrix;" +
				
	    "uniform mat4 cameraToClipMatrix;" +
				
		"varying vec4 theColor;" +

		"void main()" +
		"{" +
			"gl_Position = cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +

			"vec3 dirToLight = normalize(modelSpaceLightPos - position);" +
	
			"float cosAngIncidence = dot( normal, dirToLight);" +
			"cosAngIncidence = clamp(cosAngIncidence, 0, 1);" +
	
			"theColor = (lightIntensity * cosAngIncidence * inDiffuseColor) + (ambientIntensity * inDiffuseColor);" +
		"}";
		
		public static string FragmentLighting_PN = 

		"attribute vec3 position;" +
		"attribute vec4 color;" + // dummy to hold positions
		"attribute vec3 normal;" +

		"uniform mat4 modelToCameraMatrix;" +

		"uniform mat4 cameraToClipMatrix;" +
				
		"varying vec4 diffuseColor;" +
		"varying vec3 vertexNormal;" +
		"varying vec3 modelSpacePosition;" +

		"void main()" +
		"{" +
			"gl_Position = cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
			//"gl_Position =  vec4(position, 1.0);" + //TEST
			"vertexNormal = normal;" +
			"modelSpacePosition = position;" +
			"diffuseColor = vec4(1.0);" +
		"}";
		
		public static string FragmentLighting_PCN =

		"attribute vec3 position;" +
		"attribute vec4 inDiffuseColor;" +
		"attribute vec3 normal;" +

		"uniform mat4 modelToCameraMatrix;" +

		"uniform mat4 cameraToClipMatrix;" +
				
		"varying vec4 diffuseColor;" +
		"varying vec3 vertexNormal;" +
		"varying vec3 modelSpacePosition;" +

		"void main()" +
		"{" +
			"gl_Position =  cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
	
			"modelSpacePosition = position;" +
			"vertexNormal = normal;" +
			"diffuseColor = inDiffuseColor;" +
		"}";

		public static string FragLightAtten_PN =

		"attribute  vec3 position;" +
		"attribute  vec4 color;" + // added for spacing
		"attribute vec3 normal;" +

		"varying vec4 diffuseColor;" +
		"varying vec3 vertexNormal;" +

		"uniform mat4 modelToCameraMatrix;" +
		"uniform mat3 normalModelToCameraMatrix;" +

		"uniform mat4 cameraToClipMatrix;" +

		"void main()" +
		"{" +
			"gl_Position = cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
			"vertexNormal = normalModelToCameraMatrix * normal;" +
			"diffuseColor = vec4(1.0);" +
		"}";

		public static string FragLightAtten_PCN =

		"attribute vec3 position;" +
		"attribute vec4 inDiffuseColor;" +
		"attribute vec3 normal;" +

		"varying vec4 diffuseColor;" +
		"varying vec3 vertexNormal;" +

		"uniform mat4 modelToCameraMatrix;" +
		"uniform mat3 normalModelToCameraMatrix;" +

		"uniform mat4 cameraToClipMatrix;" +

		"void main()" +
		"{" +
			"gl_Position = cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +

			"vertexNormal = normalModelToCameraMatrix * normal;" +
			"diffuseColor = inDiffuseColor;" +
		"}";
		
		public static string PosTransform =
		
		"attribute vec3 position;" +

		"uniform mat4 modelToCameraMatrix;" +

		"uniform mat4 cameraToClipMatrix;" +

		"void main()" +
		"{" +
			"gl_Position = cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
		"}";
		
		public static string DirVertexLighting_PCN =

		"attribute vec3 position;" +
		"attribute vec4 color;" +
		"attribute vec3 normal;" +
				
				
		"uniform vec3 dirToLight;" +
		"uniform vec4 lightIntensity;" +

		"uniform mat4 modelToCameraMatrix;" +
		"uniform mat3 normalModelToCameraMatrix;" +
		
		"uniform mat4 cameraToClipMatrix;" +
				
		"varying vec4 theColor;" +

		"void main()" +
		"{" +
			"gl_Position = cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +

			"vec3 normCamSpace = normalize(normalModelToCameraMatrix * normal);" +
	
			"float cosAngIncidence = dot(normCamSpace, dirToLight);" +
			"cosAngIncidence = clamp(cosAngIncidence, 0.0, 1.0);" +
	
			//"theColor = lightIntensity * color;" + // works
			//"theColor = vec4(normal, 1.0f);" + // works
			"theColor = vec4(normCamSpace, 1.0f);" + // works\
			"theColor = cosAngIncidence * vec4(normCamSpace, 1.0f);" +  // works for some faces
			
			"theColor = lightIntensity * cosAngIncidence * vec4(normCamSpace, 1.0f);" +  // works for some faces
				
			//"theColor = lightIntensity  * cosAngIncidence * color;" +
		"}";
		
		public static string BasicTexture_PN =
		
		"attribute vec3 position;" +
		"attribute vec3 normal;" +

		"varying vec3 vertexNormal;" +
		"varying vec3 cameraSpacePosition;" +

		"uniform mat4 cameraToClipMatrix;" +

		"uniform mat4 modelToCameraMatrix;" +
		"uniform mat3 normalModelToCameraMatrix;" +
		
		"void main()" +
		"{" +
			"vec4 tempCamPosition = (modelToCameraMatrix * vec4(position, 1.0));" +
			"gl_Position = cameraToClipMatrix * tempCamPosition;" +
		
			"vertexNormal = normalize(normalModelToCameraMatrix * normal);" +
			"cameraSpacePosition = vec3(tempCamPosition);" +
		"}";
		
		public static string PNT =
		
		"attribute vec3 position;" +
		"attribute vec3 dummy1;" +
		"attribute vec3 normal;" +
		"attribute vec3 dummy3;" +	
		"attribute vec3 dummy4;" +
		"attribute vec2 texCoord;" +

		"varying vec2 colorCoord;" +
		"varying vec3 cameraSpacePosition;" +
		"varying vec3 cameraSpaceNormal;" +

		"uniform mat4 cameraToClipMatrix;" +
		"uniform mat4 modelToCameraMatrix;" +

		"void main()" +
		"{" +
			"cameraSpacePosition = dummy1 + dummy3 + dummy4;" +
			"cameraSpacePosition = (modelToCameraMatrix * vec4(position, 1.0)).xyz;" +
			"gl_Position = cameraToClipMatrix * vec4(cameraSpacePosition, 1.0);" +
			//Assume the modelToCameraMatrix contains no scaling.
			"cameraSpaceNormal = (modelToCameraMatrix * vec4(normal, 0)).xyz;" +
			"colorCoord = texCoord;" +
		"}";
		
		public static string SimpleTexture =
		
		"attribute vec4 position;" +
		"attribute vec2 texCoord;" +

		"varying vec2 colorCoord;" +

		"void main()" +
		"{" +
			"gl_Position = position;" +
			"colorCoord = texCoord;" +
		"}";

	
	public static string MatrixTexture =
        "attribute vec4 position;" +
		"attribute vec3 normal;" +	
		"attribute vec2 texCoord;" +
				
		"uniform mat4 cameraToClipMatrix;" +
	    "uniform mat4 worldToCameraMatrix;" +
	    "uniform mat4 modelToWorldMatrix;" +
				
		"varying vec3 v_Normal;" +
        "varying vec3 v_Position;" +
		"varying vec2 colorCoord;" +
				
        "void main()" +
		"{" +
			"vec4 temp = modelToWorldMatrix * position;" +
	        "temp = worldToCameraMatrix * temp;" +
	        "temp = cameraToClipMatrix * temp;" +
        	"v_Position = vec3(temp);" +
            "v_Normal = normal;" +
            "gl_Position = temp;" +
			"colorCoord = texCoord;" +
        "}";

		public static string HDR_PCN =

		"attribute vec3 position;" +
		"attribute vec4 inDiffuseColor;" +
		"attribute vec3 normal;" +

		"varying vec4 diffuseColor;" +
		"varying vec3 vertexNormal;" +
		"varying vec3 cameraSpacePosition;" +

		"uniform mat4 cameraToClipMatrix;" +

		"uniform mat4 modelToCameraMatrix;" +
		"uniform mat3 normalModelToCameraMatrix;" +

		"void main()" +
		"{" +
			"vec4 tempCamPosition = (modelToCameraMatrix * vec4(position, 1.0));" +
			"gl_Position = cameraToClipMatrix * tempCamPosition;" +

			"vertexNormal = normalize(normalModelToCameraMatrix * normal);" +
			"diffuseColor = inDiffuseColor;" +
			"cameraSpacePosition = vec3(tempCamPosition);" +
		"}";

		public static string HDR_PCN2 =

		"attribute vec4 position;" +
		"attribute vec4 color;" +
		"attribute vec3 normal;" +

		"varying vec4 diffuseColor;" +
		"varying vec3 vertexNormal;" +
		"varying vec3 cameraSpacePosition;" +

		"uniform mat4 cameraToClipMatrix;" +
		"uniform mat4 worldToCameraMatrix;" +
		"uniform mat4 modelToWorldMatrix;" +

		"uniform mat3 normalModelToCameraMatrix;" +

		"void main()" +
		"{" +
			"vec4 temp = modelToWorldMatrix * position;" +
			"temp = worldToCameraMatrix * temp;" +
			"gl_Position = cameraToClipMatrix * temp;" +

			//"vec4 tempCamPosition = (modelToCameraMatrix * vec4(position, 1.0));" +
			"vec4 tempCamPosition = position;" +
			"vertexNormal = normalize(normalModelToCameraMatrix * normal);" +
			"diffuseColor = vec4(1.0, 1.0, 0.0, 1.0);" +
			"cameraSpacePosition = vec3(tempCamPosition);" +
		"}";


		public static string PT =
		"attribute vec3 position;" +
		"attribute vec2 texCoord;" +

		"varying vec2 colorCoord;" +

		"uniform mat4 cameraToClipMatrix;" +

		"uniform mat4 modelToCameraMatrix;" +

		"void main()" +
		"{" +
			"gl_Position = cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
			"colorCoord = texCoord;" +
		"}";

		public static String unlit =
		"attribute vec3 position;" +

		"uniform mat4 cameraToClipMatrix;" +

		"uniform mat4 modelToCameraMatrix;" +

		"void main()" +
		"{" +
			"gl_Position = cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
			//"gl_Position = cameraToClipMatrix *  vec4(position, 1.0);" +
			//"gl_Position = vec4(position, 1.0);" +
		"}";

		public static String littexture =

		"attribute vec3 position;" +
		"attribute vec3 normal;" +
		"attribute vec2 texCoord;" +

		"varying vec2 colorCoord;" +
		"varying vec3 cameraSpacePosition;" +
		"varying vec3 cameraSpaceNormal;" +

		"uniform mat4 cameraToClipMatrix;" +

		"uniform mat4 modelToCameraMatrix;" +
		"uniform mat3 normalModelToCameraMatrix;" +

		"void main()" +
		"{" +
			"cameraSpacePosition = (modelToCameraMatrix * vec4(position, 1.0)).xyz;" +
			"gl_Position = cameraToClipMatrix * vec4(cameraSpacePosition, 1.0);" +
			"cameraSpaceNormal = normalize(normalModelToCameraMatrix * normal);" +

			"colorCoord = texCoord;" +
		"}";

		public static String colored =

		"attribute vec3 position;" +
		"attribute  vec4 color;" +

		"varying vec4 objectColor;" +

		"uniform mat4 cameraToClipMatrix;" +

		"uniform mat4 modelToCameraMatrix;" +

		"void main()" +
		"{" +
			"gl_Position = cameraToClipMatrix * (modelToCameraMatrix * vec4(position, 1.0));" +
			"objectColor = color;" +
		"}";

		public static String projlight =

		"attribute vec3 position;" +
		"attribute vec3 dummy1;" +  // preserve spacing
		"attribute vec3 normal;" +
		"attribute vec3 dummy3;" +  // preserve spacing
		"attribute vec3 dummy4;" +  // preserve spacing
		"attribute vec2 texCoord;" +

		"varying vec2 colorCoord;" +
		"varying vec3 cameraSpacePosition;" +
		"varying vec3 cameraSpaceNormal;" +
		"varying vec4 lightProjPosition;" +

		"uniform mat4 cameraToClipMatrix;" +

		"uniform mat4 modelToCameraMatrix;" +
		"uniform mat3 normalModelToCameraMatrix;" +
		"uniform mat4 cameraToLightProjMatrix;" +

		"void main()" +
		"{" +
			"cameraSpacePosition = dummy1 + dummy3 + dummy4;" +
			"cameraSpacePosition = (modelToCameraMatrix * vec4(position, 1.0)).xyz + 0.0001 * cameraSpacePosition;" +
			"gl_Position = cameraToClipMatrix * vec4(cameraSpacePosition, 1.0);" +
			"cameraSpaceNormal = normalize(normalModelToCameraMatrix * normal);" +
			"lightProjPosition = cameraToLightProjMatrix * vec4(cameraSpacePosition, 1.0);" +
			//"lightProjPosition = vec4(position, 1.0);" +

			"colorCoord = texCoord;" +
			//"gl_Position = modelToCameraMatrix * vec4(position, 1.0);" +
			//"gl_Position = vec4(position, 1.0);" +
			"}"; // projlight

		public static String shadowMapPcf =
		
		"varying vec4 ShadowCoord;" +

		"void main()" +
		"{" +
			"ShadowCoord= gl_TextureMatrix[7] * gl_Vertex;" +
			"gl_Position = ftransform();" +
			"gl_FrontColor = vec4(1.0, 0.0, 0.0, 1.0);" + // gl_Color;" +
		"}";

		public static String ShadowMap =
			"varying vec4 ShadowCoord;" +
			"void main()" +
			"{" +
				"ShadowCoord= gl_TextureMatrix[7] * gl_Vertex;" +
				"gl_Position = ftransform();" +
				"gl_FrontColor = gl_Color;" +
			"}";

		public static String sCube = 
			"#version 120\n" +
			"attribute vec4 position;" +

			"uniform mat4 cameraToClipMatrix;" +
			"uniform mat4 modelToCameraMatrix;" +
			"uniform vec4 offset;" +

			"void main()" +
			"{" +
				"vec4 cameraPos = modelToCameraMatrix * position;" +
				"cameraPos = cameraPos + offset;" +
				"gl_Position = cameraToClipMatrix * cameraPos;" +
			"}";
		
	}
}

