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
        "uniform vec4 u_Color;" +
        //"uniform vec3 u_LightPos;" +       	// The position of the light in eye space.
        //"varying in vec3 v_Position;" +		// This will be passed into the fragment shader.
        //"varying in vec3 v_Normal;" +         	// Interpolated normal for this fragment.
        "void main()" +
		"{" +
            // Will be used for attenuation.
            //"float distance = length(u_LightPos - v_Position);" +

            // Get a lighting direction vector from the light to the vertex.
            //"vec3 lightVector = normalize(u_LightPos - v_Position);" +

            // Calculate the dot product of the light vector and vertex normal. If the normal and light vector are
            // pointing in the same direction then it will get max illumination.
            //"float diffuse = max(dot(v_Normal, lightVector), 0.0);" +

            // Add attenuation." +
            //"diffuse = diffuse * (1.0 / distance);" +

            // Add ambient lighting"
            //"diffuse = diffuse + 0.2;" +

            // Multiply the color by the diffuse illumination level and texture value to get final output color."
            "gl_FragColor =  u_Color;" + //(diffuse * u_Color);" +
    	"}";
	}
}

