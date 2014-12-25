using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class ViewPole : Pole 
	{
	    protected ViewData position;
	    protected ViewData initialPosition;
	    protected float rotateScale;
	    protected bool isDragging;
	
	    public ViewPole(ViewData initialView, ViewScale viewScale) :
			this(initialView, viewScale, MouseButtons.MB_LEFT_BTN, false)
	    {
	        
	    }
	
	    public ViewPole(ViewData initialView, ViewScale viewScale, MouseButtons actionButton) :
			 this(initialView, viewScale, actionButton, false)
	    {
	       
	    }
	
	    public ViewPole(ViewData initialView, ViewScale viewScale, MouseButtons actionButton,
	                    bool bRightKeyboardCtrls)
	    {
	        position = initialView;
	        initialPosition = initialView;
			m_currView = initialView;
	    }
	
	    ///Generates the world-to-camera matrix for the view.
	    public Matrix4 CalcMatrix()
	    {
 	        Matrix4 theMat = Matrix4.Identity;
	
	        //Remember: these transforms are in reverse order.
	
	        //In this space, we are facing in the correct direction. Which means that the camera point
	        //is directly behind us by the radius number of units.
	        Matrix4 translation = Matrix4.CreateTranslation(new Vector3(0.0f, 0.0f, -m_currView.radius));
	
	        theMat = Matrix4.Mult(theMat, translation);
	
	        //Rotate the world to look in the right direction..
	        Quaternion fullRotation =
				Quaternion.Mult(new Quaternion(new Vector3(0.0f, 0.0f, 1.0f), m_currView.radSpinRotation),
	                m_currView.orient);
	        theMat = Matrix4.Mult(theMat, Matrix4.CreateFromQuaternion(fullRotation));
	
	        //Translate the world by the negation of the lookat point, placing the origin at the
	        //lookat point.
	        translation  = Matrix4.CreateTranslation(Vector3.Multiply(m_currView.targetPos, -1f));
	        theMat = Matrix4.Mult(theMat, translation);
	
	        return theMat;
	    }
	
	    /**
	     \brief Sets the scaling factor for orientation changes.
	
	     The scaling factor is the number of degrees to rotate the view per window space pixel.
	     The scale is the same for all mouse movements.
	     **/
	    void SetRotationScale(float rotateScale)
	    {
	    }
	
	    ///Gets the current scaling factor for orientation changes.
	    float GetRotationScale()
	    {
	        return m_viewScale.rotationScale;
	    }
	
	    ///Retrieves the current viewing information.
	    public ViewData GetView()
	    {
	        return m_currView;
	    }
	
	    ///Resets the view to the initial view. Will fail if currently dragging.
	    public void Reset()
	    {
			MessageBox.Show("Viewpole reseet not implemented.");
	    }
	
	    /**
	     \name Input Providers
	
	     These functions provide input, since Poles cannot get input for themselves. See
	     \ref module_glutil_poles "the Pole manual" for details.
	     **/
	    ///@{
	    public void MouseClick(MouseButtons button, bool isPressed, int modifiers, Vector2 position)
	    {
	    }
	    public void MouseMove(Vector2 position)
	    {
	    }
	    public void MouseWheel(int direction, int modifiers, Vector2 position)
	    {
	    }
	    public void CharPress(char key)
	    {
	    }
	
	    ///@}
	
	    ///Returns true if the mouse is being dragged.
	    bool IsDragging()
	    {
	        return m_bIsDragging;
	    }
	
	    private enum TargetOffsetDir
	    {
	        DIR_UP,
	        DIR_DOWN,
	        DIR_FORWARD,
	        DIR_BACKWARD,
	        DIR_RIGHT,
	        DIR_LEFT,
	    };
	
	    void OffsetTargetPos(TargetOffsetDir eDir, float worldDistance)
	    {
	    }
	    void OffsetTargetPos(Vector3 cameraOffset)
	    {
	    }
	
	    ViewData m_currView;
	    ViewScale m_viewScale;
	
	    ViewData m_initialView;
	    MouseButtons m_actionButton;
	    bool m_bRightKeyboardCtrls;
	
	    //Used when rotating.
	    bool m_bIsDragging;
	    RotateMode m_RotateMode;
	
	    float m_degStarDragSpin;
	    Vector2 m_startDragMouseLoc;
	    Quaternion m_startDragOrient;
	
	    void ProcessXChange(int iXDiff)
	    {
	        ProcessXChange(iXDiff, false);
	    }
	
	    void ProcessXChange(int iXDiff, bool bClearY)
	    {
	    }
	
	    void ProcessYChange(int iYDiff)
	    {
	        ProcessYChange(iYDiff, false);
	    }
	
	    void ProcessYChange(int iYDiff, bool bClearXZ)
	    {
	    }
	    void ProcessXYChange(int iXDiff, int iYDiff)
	    {
	    }
	
	    void ProcessSpinAxis(int iXDiff, int iYDiff)
	    {
	    }
	
	    void BeginDragRotate(Vector2 ptStart)
	    {
	        BeginDragRotate(ptStart, RotateMode.RM_DUAL_AXIS);
	    }
	
	    void BeginDragRotate(Vector2 ptStart, RotateMode rotMode)
	    {
	    }
	
	    void OnDragRotate(Vector2 ptCurr)
	    {
	    }
	
	    void EndDragRotate(Vector2 ptEnd)
	    {
	        EndDragRotate(ptEnd, true);
	    }
	
	    void EndDragRotate(Vector2 ptEnd, bool bKeepResults)
	    {
	    }
	
	    void MoveCloser()
	    {
	        MoveCloser(true);
	    }
	
	    void MoveCloser(bool bLargeStep)
	    {
	    }
	
	    void MoveAway()
	    {
	        MoveAway(true);
	    }
	
	    void MoveAway(bool bLargeStep)
	    {
	    }
	}
}

