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
	    "}";
	}
}

