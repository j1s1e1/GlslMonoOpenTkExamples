using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class MatrixStack
	{
	    private Stack<Matrix4> m_stack;
	    private Matrix4 m_currMatrix;
	
	
	    /**
	     \file
	     \brief Contains a \ref module_glutil_matrixstack "matrix stack and associated classes".
	     **/
	
	    ///\addtogroup module_glutil_matrixstack
	    ///@{
	
	    /**
	     \brief Implements a stack for glm::mat4 transformations.
	
	     A matrix stack is a sequence of transforms which you can preserve and restore as needed. The
	     stack has the concept of a "current matrix", which can be retrieved with the Top() function.
	     The top matrix can even be obtained as a float array. The pointer returned will remain valid until
	     this object is destroyed (though its values will change). This is useful for uploading matrices
	     to OpenGL via glUniformMatrix4fv.
	
	     The other functions will right-multiply a transformation matrix with the current matrix, thus
	     changing the current matrix.
	
	     The main power of the matrix stack is the ability to preserve and restore matrices in a stack fashion.
	     The current matrix can be preserved on the stack with Push() and the most recently preserved matrix
	     can be restored with Pop(). You must ensure that you do not Pop() more times than you Push(). Also,
	     while this matrix stack does not have an explicit size limit, if you Push() more times than you Pop(),
	     then you can eventually run out of memory (unless you create and destroy the MatrixStack every frame).
	
	     The best way to manage the stack is to never use the Push() and Pop() methods directly.
	     Instead, use the PushStack object to do all Pushing and Popping. That will ensure that
	     overflows and underflows cannot not happen.
	     **/
	
	    ///Initializes the matrix stack with the identity matrix.
	    public MatrixStack()
	    {
	        m_stack = new Stack<Matrix4>();
	        m_currMatrix = Matrix4.Identity;
	        m_stack.Push(m_currMatrix);
	    }
	
	    ///Initializes the matrix stack with the given matrix.
	    public MatrixStack(Matrix4 initialMatrix)
	    {
	        m_stack = new Stack<Matrix4>();
	        m_currMatrix = initialMatrix;
	        m_stack.Push(m_currMatrix);
	    }
	
	    /**
	     \name Stack Maintanence Functions
	
	     These functions maintain the matrix stack. You must take care not to underflow or overflow the stack.
	     **/
	    ///@{
	
	    ///Preserves the current matrix on the stack.
	    public void Push()
	    {
	        Matrix4 new_matrix = new Matrix4();  // avoid same object
	        new_matrix = m_currMatrix;
			m_stack.Push(new_matrix);
	    }
	
	    ///Restores the most recently preserved matrix.
	    public void Pop()
	    {
	        m_currMatrix = m_stack.Peek();
	        m_stack.Pop();
	    }
	
	    /**
	     \brief Restores the current matrix to the value of the most recently preserved matrix.
	
	     This function does not affect the depth of the matrix stack.
	     **/
	    public void Reset() { m_currMatrix = m_stack.Peek(); }
	
	    ///Retrieve the current matrix.
	    public Matrix4 Top()
	    {
	        return m_currMatrix;
	    }
	    ///@}
	
	    /**
	     \name Rotation Matrix Functions
	
	     These functions right-multiply the current matrix with a rotation matrix of some form.
	     All rotation angles are counter-clockwise for an observer looking down the axis direction.
	     If an observer is facing so that the axis of rotation is pointing directly towards the user,
	     then positive angles will rotate counter-clockwise.
	     **/
	    ///@{
	
	    ///Applies a rotation matrix about the given axis, with the given angle in degrees.
	    public void Rotate(Vector3 axis, float angDegCCW)
	    {
	        Matrix4 rotation = Matrix4.Rotate(axis, (float)Math.PI / 180.0f * angDegCCW);
			m_currMatrix = Matrix4.Mult(m_currMatrix, rotation);
	    }
	
	    ///Applies a rotation matrix about the given axis, with the given angle in radians.
	    public void RotateRadians(Vector3 axis, float angRadCCW)
	    {
	        Matrix4 rotation = Matrix4.Rotate(axis, angRadCCW);
			m_currMatrix = Matrix4.Mult(m_currMatrix, rotation);
	    }
	
	    ///Applies a rotation matrix about the +X axis, with the given angle in degrees.
	    public void RotateX(float angDegCCW)
	    {
	        Rotate(new Vector3(1,0,0), angDegCCW);
	    }
	    ///Applies a rotation matrix about the +Y axis, with the given angle in degrees.
	    public void RotateY(float angDegCCW)
	    {
	        Rotate(new Vector3(0,1,0), angDegCCW);
	    }
	    ///Applies a rotation matrix about the +Z axis, with the given angle in degrees.
	    public void RotateZ(float angDegCCW)
	    {
	        Rotate(new Vector3(0,0,1), angDegCCW);
	    }
	    ///@}
	
	    /**
	     \name Scale Matrix Functions
	
	     These functions right-multiply the current matrix with a scaling matrix of some form.
	     **/
	    ///@{
	
	    ///Applies a scale matrix, with the given glm::vec3 as the axis scales.
	    public void Scale(Vector3 scaleVec)
	    {
			Matrix4 scaleMatrix = Matrix4.CreateScale(scaleVec);
			m_currMatrix = Matrix4.Mult(m_currMatrix, scaleMatrix);
	    }
	    ///Applies a scale matrix, with the given values as the axis scales.
	    public void Scale(float scaleX, float scaleY, float scaleZ)
	    {
	        Scale(new Vector3(scaleX, scaleY, scaleZ));
	    }
	    ///Applies a uniform scale matrix.
	    public void Scale(float uniformScale)
	    {
	        Scale(new Vector3(uniformScale));
	    }
	    ///@}
	
	    /**
	     \name Translation Matrix Functions
	
	     These functions right-multiply the current matrix with a translation matrix of some form.
	     **/
	    ///@{
	
	    ///Applies a translation matrix, with the given glm::vec3 as the offset.
	    public void Translate(Vector3 offsetVec)
	    {
			m_currMatrix = Matrix4.Mult(m_currMatrix, Matrix4.CreateTranslation(offsetVec));
	    }
	    ///Applies a translation matrix, with the given X, Y and Z values as the offset.
	    public void Translate(float transX, float transY, float transZ)
	    {
	        Translate(new Vector3(transX, transY, transZ));
	    }
	    ///@}
	
	    /**
	     \name Camera Matrix Functions
	
	     These functions right-multiply the current matrix with a matrix that transforms from a world space to
	     the camera space expected by the Perspective() or Orthographic() functions.
	     **/
	    ///@{
	
	    /**
	     \brief Applies a matrix that transforms to a camera-space defined by a position, a target in the world, and an up direction.
	
	     \param cameraPos The world-space position of the camera.
	     \param lookatPos The world-space position the camera should be facing. It should not be equal to \a cameraPos.
	     \param upDir The world-space direction vector that should be considered up. The generated matrix will be bad
	     if the up direction is along the same direction as the direction the camera faces (the direction between
	     \a cameraPos and \a lookatPos).
	     **/
	    void LookAt(Vector3 cameraPos, Vector3 lookatPos, Vector3 upDir)
	    {
	        Matrix4 look_at = Matrix4.LookAt(cameraPos, lookatPos, upDir);
			m_currMatrix = Matrix4.Mult(m_currMatrix, look_at);
	    }
	    ///@}
	
	    /**
	     \name Projection Matrix Functions
	
	     These functions right-multiply the current matrix with a projection matrix of some form. These
	     functions all transform positions into the 4D homogeneous space expected by the output of
	     OpenGL vertex shaders. As such, these can be used directly with GLSL shaders.
	
	     The space that these matrices transform from is defined as follows. The pre-projection space,
	     called camera space or eye space, has the camera/eye position at the origin. The camera faces down the
	     -Z axis, so objects with larger negative Z values are farther away. +Y is up and +X is to the right.
	     **/
	    ///@{
	
	    /**
	     \brief Applies a standard, OpenGL-style perspective projection matrix.
	
	     \param degFOV The field of view. This is the angle in degrees between directly forward and the farthest
	     visible point horizontally.
	     \param aspectRatio The ratio of the width of the view area to the height.
	     \param zNear The closest camera-space distance to the camera that can be seen.
	     The projection will be clipped against this value. It cannot be negative or 0.0.
	     \param zFar The farthest camera-space distance from the camera that can be seen.
	     The projection will be clipped against this value. It must be larger than \a zNear.
	     **/
	    public void Perspective(float degFOV, float aspectRatio, float zNear, float zFar)
	    {
	        Matrix4 persp = Matrix4.CreatePerspectiveFieldOfView(
	                (float)Math.PI / 180 * degFOV, aspectRatio, zNear, zFar);
			m_currMatrix = Matrix4.Mult(m_currMatrix, persp);
 	    }
	
	    /**
	     \brief Applies a standard, OpenGL-style orthographic projection matrix.
	
	     \param left The left camera-space position in the X axis that will be captured within the projection.
	     \param right The right camera-space position in the X axis that will be captured within the projection.
	     \param bottom The bottom camera-space position in the Y axis that will be captured within the projection.
	     \param top The top camera-space position in the Y axis that will be captured within the projection.
	     \param zNear The front camera-space position in the Z axis that will be captured within the projection.
	     \param zFar The rear camera-space position in the Z axis that will be captured within the projection.
	     **/
	    void Orthographic(float left, float right, float bottom, float top, float zNear, float zFar)
	    {
	        Matrix4 orth = Matrix4.CreateOrthographic(right - left, top-bottom, zNear, zFar);
			m_currMatrix = Matrix4.Mult(m_currMatrix, orth);
	    }
	
	    void Orthographic(float left, float right, float bottom, float top)
	    {
	        Orthographic(left, right, bottom, top, -1f, 1f);
	    }
	
	    /**
	     \brief Applies an ortho matrix for pixel-accurate reproduction.
	
	     A common use for orthographic projections is to create an ortho matrix that allows for pixel-accurate
	     reproduction of textures. It allows you to provide vertices directly in window space.
	
	     The camera space that this function creates can have the origin at the top-left (with +y going down)
	     or bottom-left (with +y going up). Note that a top-left orientation will have to flip the Y coordinate,
	     which means that the winding order of any triangles are reversed.
	
	     The depth range is arbitrary and up to the user.
	
	     \param size The size of the window space.
	     \param depthRange The near and far depth range. The x coord is zNear, and the y coord is zFar.
	     \param isTopLeft True if this should be top-left orientation, false if it should be bottom-left.
	     **/
	    void PixelPerfectOrtho(Vector2 size, Vector2 depthRange, bool isTopLeft)
	    {
	        Matrix4 orth = Matrix4.CreateOrthographic(size.X, size.Y, depthRange.X, depthRange.Y);
			m_currMatrix = Matrix4.Mult(m_currMatrix, orth);
	    }
	
	    void PixelPerfectOrtho(Vector2 size, Vector2 depthRange)
	    {
	        PixelPerfectOrtho(size, depthRange, true);
	    }
	    ///@}
	
	    /**
	     \name Matrix Application
	
	     These functions right-multiply a user-provided matrix by the current matrix; the result
	     becomes the new current matrix.
	     **/
	    ///@{
	    public void ApplyMatrix(Matrix4 theMatrix)
	    {
			m_currMatrix = Matrix4.Mult(m_currMatrix, theMatrix);
	    }
	    ///@}
	
	    /**
	     \name Matrix Setting
	
	     These functions directly set the value of the current matrix, replacing the old value.
	     Previously preserved matrices on the stack are unaffected.
	     **/
	    ///@{
	
	    ///The given matrix becomes the current matrix.
	    public void SetMatrix(Matrix4 theMatrix)
	    {
	        m_currMatrix = theMatrix;
	    }
	    ///Sets the current matrix to the identity matrix.
	    public void SetIdentity()
	    {
	        m_currMatrix = Matrix4.Identity;
	    }
	    ///@}
	
	}
}

