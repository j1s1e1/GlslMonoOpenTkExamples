using System;

namespace GlslTutorials
{
	public class ProgramSet
	{
		public String name;
		public String vertexShader;
		public String fragmentShader;
		public ProgramSet (string nameIn, string vertexShaderIn, string fragmentShaderIn)
		{
			name = nameIn;
			vertexShader = vertexShaderIn;
			fragmentShader = fragmentShaderIn;
		}
	}
}

