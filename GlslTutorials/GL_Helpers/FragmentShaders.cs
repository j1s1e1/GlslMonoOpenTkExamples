using System;

namespace GlslTutorials
{
	public class FragmentShaders
	{
		public FragmentShaders ()
		{
		}
		
		public static String ColorUniform_frag =
	    "uniform vec4 baseColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_FragColor = baseColor;" +
	    "}";
		
		public static String lms_fragmentShaderCode =
        "uniform vec4 baseColor;" +
        "uniform vec3 lightPos;" +       	// The position of the light in eye space.
        "varying vec3 v_Position;" +		// This will be passed into the fragment shader.
        "varying vec3 v_Normal;" +         	// Interpolated normal for this fragment.
        "void main()" +
		"{" +
            // Will be used for attenuation.
            "float distance = length(lightPos - v_Position);" +

            // Get a lighting direction vector from the light to the vertex.
            "vec3 lightVector = normalize(lightPos - v_Position);" +

            // Calculate the dot product of the light vector and vertex normal. If the normal and light vector are
            // pointing in the same direction then it will get max illumination.
            "float diffuse = max(dot(v_Normal, lightVector), 0.0);" +

            // Add attenuation." +
            "diffuse = diffuse * (1.0 / distance);" +

            // Add ambient lighting"
            "diffuse = diffuse + 0.2;" +

            // Multiply the color by the diffuse illumination level and texture value to get final output color."
            "gl_FragColor = (diffuse * baseColor);" +
    	"}";
		
		public string fragmentShaderCode =
        "precision mediump float;" +
        "uniform vec4 u_Color;" +
        "uniform vec3 u_LightPos;" +       	// The position of the light in eye space.
        "varying vec3 v_Position;" +		// This will be passed into the fragment shader.
        "varying vec3 v_Normal;" +         	// Interpolated normal for this fragment.
        "void main()" +
		"{" +

            // Will be used for attenuation.
            "float distance = length(u_LightPos - v_Position);" +

            // Get a lighting direction vector from the light to the vertex.
            "vec3 lightVector = normalize(u_LightPos - v_Position);" +

            // Calculate the dot product of the light vector and vertex normal. If the normal and light vector are
            // pointing in the same direction then it will get max illumination.
            "float diffuse = max(dot(v_Normal, lightVector), 0.0);" +

            // Add attenuation." +
            "diffuse = diffuse * (1.0 / distance);" +

            // Add ambient lighting"
            "diffuse = diffuse + 0.2;" +

            // Multiply the color by the diffuse illumination level and texture value to get final output color."
            "gl_FragColor = (diffuse * u_Color);" +
    	"}";
		
		public static string ColorMultUniform_frag =
	    "uniform vec4 baseColor;" +
	
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_FragColor = theColor * baseColor;" +
	    "}";
		
		 public static string ColorPassthrough_frag =
	    "varying vec4 theColor;" +
	
	    "void main()" +
	    "{" +
	        "gl_FragColor = theColor;" + // + 
	    "}";
		
		public static String solid_red_frag =
	    "void main()" +
	    "{" +
	        "gl_FragColor =  vec4(1.0, 0.0, 0.0, 1.0);" +
	    "}";
		
		public static String solid_green_frag =
	    "void main()" +
	    "{" +
	        "gl_FragColor =  vec4(0.0, 1.0, 0.0, 1.0);" +
	    "}";
		
		public static String solid_blue_frag =
	    "void main()" +
	    "{" +
	        "gl_FragColor =  vec4(0.0, 0.0, 1.0, 1.0);" +
	    "}";
		
		public static String solid_green_with_normals_frag =
		"uniform vec3 u_LightPos;" +       	// The position of the light in eye space.
        "varying vec3 v_Position;" +		// This will be passed into the fragment shader.
        "varying vec3 v_Normal;" +         	// Interpolated normal for this fragment.
	    "void main()" +
	    "{" +
			// Will be used for attenuation.
            "float distance = length(u_LightPos - v_Position);" +

            // Get a lighting direction vector from the light to the vertex.
            "vec3 lightVector = normalize(u_LightPos - v_Position);" +

            // Calculate the dot product of the light vector and vertex normal. If the normal and light vector are
            // pointing in the same direction then it will get max illumination.
            "float diffuse = max(dot(v_Normal, lightVector), 0.0);" +

            // Add attenuation." +
            "diffuse = diffuse * (1.0 / distance);" +

            // Add ambient lighting"
            "diffuse = diffuse + 0.2;" +

            // Multiply the color by the diffuse illumination level to get final output color."
            "gl_FragColor = (diffuse * vec4(0.0, 1.0, 0.0, 1.0));" +
	    "}";
		
		public static String FragmentLighting =

	    "uniform vec3 modelSpaceLightPos;" +

	    "uniform vec4 lightIntensity;" +
	    "uniform vec4 ambientIntensity;" +
				
		"varying vec4 diffuseColor;" +
	    "varying vec3 vertexNormal;" +
	    "varying vec3 modelSpacePosition;" +

	    "void main()" +
	    "{" +
		    "vec3 lightDir = normalize(modelSpaceLightPos - modelSpacePosition);" +
	
		    "float cosAngIncidence = dot(normalize(vertexNormal), lightDir);" +
		    "cosAngIncidence = clamp(cosAngIncidence, 0, 1);" +
		
		    "gl_FragColor = (diffuseColor * lightIntensity * cosAngIncidence) + (diffuseColor * ambientIntensity);" +
			//"gl_FragColor = diffuseColor;" + //TEST
	    "}";
		
		public static String TextureGaussian =
		
		"varying vec3 vertexNormal;" +
		"varying vec3 cameraSpacePosition;" +
		
		"uniform Material" +
		"{" +
			"vec4 diffuseColor;" +
			"vec4 specularColor;" +
			"float specularShininess;" +	//Not used in this shader
		"} Mtl;" +
		
		"struct PerLight" +
		"{" +
			"vec4 cameraSpaceLightPos;" +
			"vec4 lightIntensity;" +
		"};" +
		
		"const int numberOfLights = 2;" +
		
		"uniform Light" +
		"{" +
			"vec4 ambientIntensity;" +
			"float lightAttenuation;" +
			"PerLight lights[numberOfLights];" +
		"} Lgt;" +
		
		"uniform sampler1D gaussianTexture;" +
		
		"float CalcAttenuation(in vec3 cameraSpacePosition," +
			"in vec3 cameraSpaceLightPos," +
			"out vec3 lightDirection)" +
		"{" +
			"vec3 lightDifference =  cameraSpaceLightPos - cameraSpacePosition;" +
			"float lightDistanceSqr = dot(lightDifference, lightDifference);" +
			"lightDirection = lightDifference * inversesqrt(lightDistanceSqr);" +
			
			"return (1 / ( 1.0 + Lgt.lightAttenuation * lightDistanceSqr));" +
		"}" +
		
		"vec4 ComputeLighting(in PerLight lightData, in vec3 cameraSpacePosition," +
			"in vec3 cameraSpaceNormal)" +
		"{" +
			"vec3 lightDir;" +
			"vec4 lightIntensity;" +
			"if(lightData.cameraSpaceLightPos.w == 0.0)" +
			"{" +
				"lightDir = vec3(lightData.cameraSpaceLightPos);" +
				"lightIntensity = lightData.lightIntensity;" +
			"}" +
			"else" +
			"{" +
				"float atten = CalcAttenuation(cameraSpacePosition," +
					"lightData.cameraSpaceLightPos.xyz, lightDir);" +
				"lightIntensity = atten * lightData.lightIntensity;" +
			"}" +
			
			"vec3 surfaceNormal = normalize(cameraSpaceNormal);" +
			"float cosAngIncidence = dot(surfaceNormal, lightDir);" +
			"cosAngIncidence = cosAngIncidence < 0.0001 ? 0.0 : cosAngIncidence;" +
			
			"vec3 viewDirection = normalize(-cameraSpacePosition);" +
			
			"vec3 halfAngle = normalize(lightDir + viewDirection);" +
			"float texCoord = dot(halfAngle, surfaceNormal);" +
			"float gaussianTerm = texture(gaussianTexture, texCoord).r;" +
		
			"gaussianTerm = cosAngIncidence != 0.0 ? gaussianTerm : 0.0;" +
			
			"vec4 lighting = Mtl.diffuseColor * lightIntensity * cosAngIncidence;" +
			"lighting += Mtl.specularColor * lightIntensity * gaussianTerm;" +
			
			"return lighting;" +
		"}" +
		
		"void main()" +
		"{" +
			"vec4 accumLighting = Mtl.diffuseColor * Lgt.ambientIntensity;" +
			"for(int light = 0; light < numberOfLights; light++)" +
			"{" +
				"accumLighting += ComputeLighting(Lgt.lights[light]," +
					"cameraSpacePosition, vertexNormal);" +
			"}" +
			
			"gl_FragColor = sqrt(accumLighting);" + //2.0 gamma correction
		"}";
		
		
		public static String ShaderGaussian =
			
		"#version 140" +
		"varying vec3 vertexNormal;" +
		"varying vec3 cameraSpacePosition;" +
				
		"uniform Material" +
		"{" +
			"vec4 diffuseColor;" +
			"vec4 specularColor;" +
			"float specularShininess;" +
		"} Mtl;" +
		
		"struct PerLight" +
		"{" +
			"vec4 cameraSpaceLightPos;" +
			"vec4 lightIntensity;" +
		"};" +
				
		"const int numberOfLights = 2;" +
		
		"uniform Light" +
		"{" +
			"vec4 ambientIntensity;" +
			"float lightAttenuation;" +
			"PerLight lights[numberOfLights];" +
		"} Lgt;" +				
		
		
		"float CalcAttenuation(in vec3 cameraSpacePosition," +
			"in vec3 cameraSpaceLightPos," +
			"out vec3 lightDirection)" +
		"{" +
			"vec3 lightDifference =  cameraSpaceLightPos - cameraSpacePosition;" +
			"float lightDistanceSqr = dot(lightDifference, lightDifference);" +
			"lightDirection = lightDifference * inversesqrt(lightDistanceSqr);" +
			
			"return (1 / ( 1.0 + Lgt.lightAttenuation * lightDistanceSqr));" +
		"}" +				
				
		"vec4 ComputeLighting(in PerLight lightData, in vec3 cameraSpacePosition," +
			"in vec3 cameraSpaceNormal)" +
		"{" +
			"vec3 lightDir;" +
			"vec4 lightIntensity;" +
			"if(lightData.cameraSpaceLightPos.w == 0.0)" +
			"{" +
				"lightDir = vec3(lightData.cameraSpaceLightPos);" +
				"lightIntensity = lightData.lightIntensity;" +
			"}" +		
			"else" +
			"{" +
				"float atten = CalcAttenuation(cameraSpacePosition," +
					"lightData.cameraSpaceLightPos.xyz, lightDir);" +
				"lightIntensity = atten * lightData.lightIntensity;" +
			"}" +
			
			"vec3 surfaceNormal = normalize(cameraSpaceNormal);" +
			"float cosAngIncidence = dot(surfaceNormal, lightDir);" +
			"cosAngIncidence = cosAngIncidence < 0.0001 ? 0.0 : cosAngIncidence;" +
			
			"vec3 viewDirection = normalize(-cameraSpacePosition);" +

			"vec3 halfAngle = normalize(lightDir + viewDirection);" +
			
			"float angleNormalHalf = acos(dot(halfAngle, surfaceNormal));" +
			"float exponent = angleNormalHalf / Mtl.specularShininess;" +
			"exponent = -(exponent * exponent);" +
			"float gaussianTerm = exp(exponent);" +
		
			"gaussianTerm = cosAngIncidence != 0.0 ? gaussianTerm : 0.0;" +
			
			"vec4 lighting = Mtl.diffuseColor * lightIntensity * cosAngIncidence;" +
			"lighting += Mtl.specularColor * lightIntensity * gaussianTerm;" +
			
			"return lighting;" +
		"}" +				
		
		"void main()" +
		"{" +
			"vec4 accumLighting = Mtl.diffuseColor * Lgt.ambientIntensity;" +
			"for(int light = 0; light < numberOfLights; light++)" +
			"{" +
				"accumLighting += ComputeLighting(Lgt.lights[light]," +
					"cameraSpacePosition, vertexNormal);" +
			"}" +
			
			"gl_FragColor = sqrt(accumLighting);" + //2.0 gamma correction
		"}";
		
		
		public static String TextureGaussianOrig =
		
		"varying vec3 vertexNormal;" +
		"varying vec3 cameraSpacePosition;" +
		
		"uniform Material" +
		"{" +
			"vec4 diffuseColor;" +
			"vec4 specularColor;" +
			"float specularShininess;" +	//Not used in this shader
		"} Mtl;" +
		
		"struct PerLight" +
		"{" +
			"vec4 cameraSpaceLightPos;" +
			"vec4 lightIntensity;" +
		"};" +
		
		"const int numberOfLights = 2;" +
		
		"uniform Light" +
		"{" +
			"vec4 ambientIntensity;" +
			"float lightAttenuation;" +
			"PerLight lights[numberOfLights];" +
		"} Lgt;" +
		
		"uniform sampler1D gaussianTexture;" +
		
		"float CalcAttenuation(in vec3 cameraSpacePosition," +
			"in vec3 cameraSpaceLightPos," +
			"out vec3 lightDirection)" +
		"{" +
			"vec3 lightDifference =  cameraSpaceLightPos - cameraSpacePosition;" +
			"float lightDistanceSqr = dot(lightDifference, lightDifference);" +
			"lightDirection = lightDifference * inversesqrt(lightDistanceSqr);" +
			
			"return (1 / ( 1.0 + Lgt.lightAttenuation * lightDistanceSqr));" +
		"}" +
		
		"vec4 ComputeLighting(in PerLight lightData, in vec3 cameraSpacePosition," +
			"in vec3 cameraSpaceNormal)" +
		"{" +
			"vec3 lightDir;" +
			"vec4 lightIntensity;" +
			"if(lightData.cameraSpaceLightPos.w == 0.0)" +
			"{" +
				"lightDir = vec3(lightData.cameraSpaceLightPos);" +
				"lightIntensity = lightData.lightIntensity;" +
			"}" +
			"else" +
			"{" +
				"float atten = CalcAttenuation(cameraSpacePosition," +
					"lightData.cameraSpaceLightPos.xyz, lightDir);" +
				"lightIntensity = atten * lightData.lightIntensity;" +
			"}" +
			
			"vec3 surfaceNormal = normalize(cameraSpaceNormal);" +
			"float cosAngIncidence = dot(surfaceNormal, lightDir);" +
			"cosAngIncidence = cosAngIncidence < 0.0001 ? 0.0 : cosAngIncidence;" +
			
			"vec3 viewDirection = normalize(-cameraSpacePosition);" +
			
			"vec3 halfAngle = normalize(lightDir + viewDirection);" +
			"float texCoord = dot(halfAngle, surfaceNormal);" +
			"float gaussianTerm = texture(gaussianTexture, texCoord).r;" +
		
			"gaussianTerm = cosAngIncidence != 0.0 ? gaussianTerm : 0.0;" +
			
			"vec4 lighting = Mtl.diffuseColor * lightIntensity * cosAngIncidence;" +
			"lighting += Mtl.specularColor * lightIntensity * gaussianTerm;" +
			
			"return lighting;" +
		"}" +
		
		"void main()" +
		"{" +
			"vec4 accumLighting = Mtl.diffuseColor * Lgt.ambientIntensity;" +
			"for(int light = 0; light < numberOfLights; light++)" +
			"{" +
				"accumLighting += ComputeLighting(Lgt.lights[light]," +
					"cameraSpacePosition, vertexNormal);" +
			"}" +
			
			"gl_FragColor = sqrt(accumLighting);" + //2.0 gamma correction
		"}";
		
		
		public static String ShaderGaussianOrig =

		"varying vec3 vertexNormal;" +
		"varying vec3 cameraSpacePosition;" +
				
		"uniform Material" +
		"{" +
			"vec4 diffuseColor;" +
			"vec4 specularColor;" +
			"float specularShininess;" +
		"} Mtl;" +
		
		"struct PerLight" +
		"{" +
			"vec4 cameraSpaceLightPos;" +
			"vec4 lightIntensity;" +
		"};" +
				
		"const int numberOfLights = 2;" +
		
		"uniform Light" +
		"{" +
			"vec4 ambientIntensity;" +
			"float lightAttenuation;" +
			"PerLight lights[numberOfLights];" +
		"} Lgt;" +				
		
		
		"float CalcAttenuation(in vec3 cameraSpacePosition," +
			"in vec3 cameraSpaceLightPos," +
			"out vec3 lightDirection)" +
		"{" +
			"vec3 lightDifference =  cameraSpaceLightPos - cameraSpacePosition;" +
			"float lightDistanceSqr = dot(lightDifference, lightDifference);" +
			"lightDirection = lightDifference * inversesqrt(lightDistanceSqr);" +
			
			"return (1 / ( 1.0 + Lgt.lightAttenuation * lightDistanceSqr));" +
		"}" +				
				
		"vec4 ComputeLighting(in PerLight lightData, in vec3 cameraSpacePosition," +
			"in vec3 cameraSpaceNormal)" +
		"{" +
			"vec3 lightDir;" +
			"vec4 lightIntensity;" +
			"if(lightData.cameraSpaceLightPos.w == 0.0)" +
			"{" +
				"lightDir = vec3(lightData.cameraSpaceLightPos);" +
				"lightIntensity = lightData.lightIntensity;" +
			"}" +		
			"else" +
			"{" +
				"float atten = CalcAttenuation(cameraSpacePosition," +
					"lightData.cameraSpaceLightPos.xyz, lightDir);" +
				"lightIntensity = atten * lightData.lightIntensity;" +
			"}" +
			
			"vec3 surfaceNormal = normalize(cameraSpaceNormal);" +
			"float cosAngIncidence = dot(surfaceNormal, lightDir);" +
			"cosAngIncidence = cosAngIncidence < 0.0001 ? 0.0 : cosAngIncidence;" +
			
			"vec3 viewDirection = normalize(-cameraSpacePosition);" +

			"vec3 halfAngle = normalize(lightDir + viewDirection);" +
			
			"float angleNormalHalf = acos(dot(halfAngle, surfaceNormal));" +
			"float exponent = angleNormalHalf / Mtl.specularShininess;" +
			"exponent = -(exponent * exponent);" +
			"float gaussianTerm = exp(exponent);" +
		
			"gaussianTerm = cosAngIncidence != 0.0 ? gaussianTerm : 0.0;" +
			
			"vec4 lighting = Mtl.diffuseColor * lightIntensity * cosAngIncidence;" +
			"lighting += Mtl.specularColor * lightIntensity * gaussianTerm;" +
			
			"return lighting;" +
		"}" +				
		
		"void main()" +
		"{" +
			"vec4 accumLighting = Mtl.diffuseColor * Lgt.ambientIntensity;" +
			"for(int light = 0; light < numberOfLights; light++)" +
			"{" +
				"accumLighting += ComputeLighting(Lgt.lights[light]," +
					"cameraSpacePosition, vertexNormal);" +
			"}" +
			
			"gl_FragColor = sqrt(accumLighting);" + //2.0 gamma correction
		"}";
		
		public static String litTexture =

		"varying vec2 colorCoord;" +
		"varying vec3 cameraSpacePosition;" +
		"varying vec3 cameraSpaceNormal;" +

		"struct PerLight" +
		"{" +
			"vec4 cameraSpaceLightPos;" +
			"vec4 lightIntensity;" +
		"};" +
		
		"uniform Light" +
		"{" +
			"vec4 ambientIntensity;" +
			"float lightAttenuation;" +
			"float maxIntensity;" +
			"PerLight lights[4];" +
		"} Lgt;" +
		
		"uniform int numberOfLights;" +
		
		"float CalcAttenuation(in vec3 cameraSpacePosition, in vec3 cameraSpaceLightPos, out vec3 lightDirection)" +
		"{" +
			"vec3 lightDifference =  cameraSpaceLightPos - cameraSpacePosition;" +
			"float lightDistanceSqr = dot(lightDifference, lightDifference);" +
			"lightDirection = lightDifference * inversesqrt(lightDistanceSqr);" +
			
			"return (1.0 / ( 1.0 + Lgt.lightAttenuation * lightDistanceSqr));" +
		"}" +
		
		"vec4 ComputeLighting(in vec4 diffuseColor, in PerLight lightData)" +
		"{" +
			"vec3 lightDir;" +
			"vec4 lightIntensity;" +
			"if(lightData.cameraSpaceLightPos.w == 0.0)" +
			"{" +
				"lightDir = vec3(lightData.cameraSpaceLightPos);" +
				"lightIntensity = lightData.lightIntensity;" +
			"}" +
			"else" +
			"{" +
				"float atten = CalcAttenuation(cameraSpacePosition," +
					"lightData.cameraSpaceLightPos.xyz, lightDir);" +
				"lightIntensity = atten * lightData.lightIntensity;" +
			"}" +
			
			"vec3 surfaceNormal = normalize(cameraSpaceNormal);" +
			"float cosAngIncidence = dot(surfaceNormal, lightDir);" +
			"cosAngIncidence = cosAngIncidence < 0.0001 ? 0.0 : cosAngIncidence;" +
			
			"vec4 lighting = diffuseColor * lightIntensity * cosAngIncidence;" +
			
			"return lighting;" +
		"}" +
		
		"uniform sampler2D diffuseColorTex;" +
		
		"void main()" +
		"{" +
			"vec4 diffuseColor = texture(diffuseColorTex, colorCoord);" +
			
			"vec4 accumLighting = diffuseColor * Lgt.ambientIntensity;" +
			"for(int light = 0; light < numberOfLights; light++)" +
			"{" +
				"accumLighting += ComputeLighting(diffuseColor, Lgt.lights[light]);" +
			"}" +
			
			"gl_FragColor = accumLighting / Lgt.maxIntensity;" +
		
		"}";
		
		public static String SimpleTexture =
	
		"varying vec2 colorCoord;" +
				
		"uniform sampler2D diffuseColorTex;" +
		
		"void main()" +
		"{" +			
			"gl_FragColor = texture2D(diffuseColorTex, colorCoord);" +
		"}";


	}
}

