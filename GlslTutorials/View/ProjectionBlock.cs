using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class ProjectionBlock 
	{
	    public Matrix4 cameraToClipMatrix;
	    static public int byteLength()
	    {
	        return 16 * 4;
	    }
	
	    public float[] ToFloat()
	    {
	        return new float[]{ 
			 cameraToClipMatrix.M11, cameraToClipMatrix.M12, cameraToClipMatrix.M13, cameraToClipMatrix.M14,
			 cameraToClipMatrix.M21, cameraToClipMatrix.M22, cameraToClipMatrix.M23, cameraToClipMatrix.M24,
			 cameraToClipMatrix.M31, cameraToClipMatrix.M32, cameraToClipMatrix.M33, cameraToClipMatrix.M34,
			 cameraToClipMatrix.M41, cameraToClipMatrix.M42, cameraToClipMatrix.M43, cameraToClipMatrix.M44};
		}
	}
}

