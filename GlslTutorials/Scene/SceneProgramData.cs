using System;

namespace GlslTutorials
{
	public struct SceneProgramData
	{
		public int theProgram;

		public int modelToCameraMatrixUnif;
		public int normalModelToCameraMatrixUnif;
		public LightBlock lightBlock;
		public MaterialBlock materialBlock;

		public int cameraToClipMatrixUnif;
	};
}

