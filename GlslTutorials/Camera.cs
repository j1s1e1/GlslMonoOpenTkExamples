using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class Camera {
	    //In spherical coordinates.
	    public static Vector3 g_sphereCamRelPos = new Vector3(67.5f, -46.0f, 150.0f);
	
	    public static Vector3 g_camTarget = new Vector3(0.0f, 0.4f, 0.0f);
	
	    public static float Clamp(float value, float min, float max)
	    {
	        if (value < min)
	            return min;
	
	        if (value > max)
	            return max;
	
	        return value;
	    }
	
	    public static void Move(float x, float y, float z)
	    {
	        g_sphereCamRelPos = Vector3.Add(g_sphereCamRelPos , new Vector3(x, y, z));
	        g_sphereCamRelPos.Y = Clamp(g_sphereCamRelPos.Y, -78.75f, -1.0f);
	        g_sphereCamRelPos.Z = g_sphereCamRelPos.Z > 5.0f ? g_sphereCamRelPos.Z : 5.0f;
	    }
	
	    public static void MoveTarget(float x, float y, float z)
	    {
	        g_camTarget = Vector3.Add(g_camTarget, new Vector3(x, y, z));
	        g_camTarget.Y = g_camTarget.Y > 0.0f ? g_camTarget.Y : 0.0f;
	    }
	
	    public static String GetTargetString()
	    {
	        return String.Format("\nTarget: {0:f} {1:f} {2:f}", g_camTarget.X, g_camTarget.Y,
	                g_camTarget.Z);
	    }
	
	    public static String GetPositionString()
	    {
	        return String.Format("\nPosition: {0:f} {1:f} {2:f}", g_sphereCamRelPos.X, g_sphereCamRelPos.Y,
	                g_sphereCamRelPos.Z);
	    }
	
	    public static Vector3 ResolveCamPosition()
	    {
	        MatrixStack tempMat;
	
	        float phi = (float)(Math.PI / 180 * g_sphereCamRelPos.X);
	        float theta = (float)(Math.PI / 180 * g_sphereCamRelPos.Y + 90.0f);
	
	        float fSinTheta = (float)Math.Sin(theta);
	        float fCosTheta = (float)Math.Cos(theta);
	        float fCosPhi = (float)Math.Cos(phi);
	        float fSinPhi = (float)Math.Sin(phi);
	
	        Vector3 dirToCamera = new Vector3(fSinTheta * fCosPhi, fCosTheta, fSinTheta * fSinPhi);
	        return Vector3.Multiply(dirToCamera, Vector3.Add(new Vector3(0, 0, g_sphereCamRelPos.Z) , g_camTarget));
	    }
	
	    public static Matrix4 CalcLookAtMatrix(Vector3 cameraPt, Vector3 lookPt, Vector3 upPt)
	    {
	        Vector3 lookDir = Vector3.Subtract(lookPt, cameraPt);
	        lookDir.Normalize();
	        Vector3 upDir = upPt;
	        upDir.Normalize();
	
	        Vector3 rightDir = Vector3.Cross(lookDir, upDir);
	        rightDir.Normalize();
	        Vector3 perpUpDir = Vector3.Cross(rightDir, lookDir);
	
	        Matrix4 rotMat = Matrix4.Identity;
	        rotMat.Row0 = new Vector4(rightDir, 0.0f);
	        rotMat.Row1 = new Vector4(perpUpDir, 0.0f);
	        rotMat.Row2 = new Vector4(Vector3.Multiply(lookDir, -1f), 0.0f);
	
	        rotMat.Transpose();
	
	        Matrix4 transMat = Matrix4.Identity;
	        transMat.Row3 = new Vector4(Vector3.Multiply(cameraPt, -1f), 1.0f);
	
	        return Matrix4.Mult(rotMat, transMat);
	    }
	
	    public static Matrix4 GetLookAtMatrix()
	    {
	        return CalcLookAtMatrix(ResolveCamPosition(), g_camTarget, new Vector3(0.0f, 1.0f, 0.0f));
	    }
	}
}

