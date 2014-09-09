using OpenTK;
using System;

namespace GlslTutorials
{
	public class LitMatrixSphere
	{
		public LitMatrixSphere (float radius)
		{
		}
		
		public static int mProgram = -1;
	    private int positionAttribute;
	
	    private int cameraToClipMatrixUnif;
	    private static int worldToCameraMatrixUnif;
	    private int modelToWorldMatrixUnif;
	    private int colorUnif;
	
	    static Matrix4 cameraToClip = new Matrix4();
	    static Matrix4 worldToCamera = new Matrix4();
	    Matrix4 modelToWorld = new Matrix4();
		
		public void SetOffset(Vector3 offset)
		{
		}
		
		public void Draw()
		{
		}
	}
}

