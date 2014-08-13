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
	}
}

