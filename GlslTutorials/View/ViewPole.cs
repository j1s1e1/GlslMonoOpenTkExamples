using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	public class ViewPole : ViewProvider 
	{
		ViewData m_currView;

		//Used when rotating.
		bool m_bIsDragging;
		RotateMode m_RotateMode;

		float m_degStarDragSpin;
		Vector2 m_startDragMouseLoc;
		Quaternion m_startDragOrient;

		ViewData m_initialView;
		ViewScale m_viewScale;
		MouseButtons m_actionButton;
		bool m_bRightKeyboardCtrls;

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
			m_currView = initialView;
			m_viewScale = viewScale;
			m_initialView = initialView;
			m_actionButton = actionButton;
			m_bRightKeyboardCtrls = bRightKeyboardCtrls;
			m_bIsDragging = false;
	    }
	
	    ///Generates the world-to-camera matrix for the view.
		public override Matrix4 CalcMatrix()
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
			m_viewScale.rotationScale = rotateScale;
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
			if(!m_bIsDragging)
				m_currView = m_initialView;
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
			if(m_bIsDragging)
				OnDragRotate(position);
	    }
	    public void MouseWheel(int direction, int modifiers, Vector2 position)
	    {
	    }

		public override void MouseButton(int button, int state, int x, int y)
		{
		}
		public override void MouseButton(int button, int state, Point p)
		{
		}

		public override void MouseClick(MouseButtons button, bool isPressed, int modifiers, Point p)
		{
			Vector2 position = new Vector2(p.X, p.Y);
			if(isPressed)
			{
				//Ignore all other button presses when dragging.
				if(!m_bIsDragging)
				{
					if(button == m_actionButton)
					{
						if((modifiers & (int)MouseModifiers.MM_KEY_CTRL) != 0)
							this.BeginDragRotate(position, RotateMode.RM_BIAXIAL);
						else if((modifiers & (int)MouseModifiers.MM_KEY_ALT) != 0)
							this.BeginDragRotate(position, RotateMode.RM_SPIN_VIEW_AXIS);
						else
							this.BeginDragRotate(position, RotateMode.RM_DUAL_AXIS);
					}
				}
			}
			else
			{
				//Ignore all other button releases when not dragging
				if(m_bIsDragging)
				{
					if(button == m_actionButton)
					{
						if(m_RotateMode == RotateMode.RM_DUAL_AXIS ||
							m_RotateMode == RotateMode.RM_SPIN_VIEW_AXIS ||
							m_RotateMode == RotateMode.RM_BIAXIAL)
							this.EndDragRotate(position);
					}
				}
			}
		}

		public override void MouseMove(Point position)
		{
		}
		public override void MouseWheel(int direction, int modifiers, Point p)
		{
			if(direction > 0)
				this.MoveCloser((modifiers & (int)MouseModifiers.MM_KEY_SHIFT) == 0);
			else
				this.MoveAway((modifiers & (int)MouseModifiers.MM_KEY_SHIFT) == 0);
		}

	    public void CharPress(char key)
	    {
			if(m_bRightKeyboardCtrls)
			{
				switch(key)
				{
				case 'i': OffsetTargetPos(TargetOffsetDir.DIR_FORWARD, m_viewScale.largePosOffset); break;
				case 'k': OffsetTargetPos(TargetOffsetDir.DIR_BACKWARD, m_viewScale.largePosOffset); break;
				case 'l': OffsetTargetPos(TargetOffsetDir.DIR_RIGHT, m_viewScale.largePosOffset); break;
				case 'j': OffsetTargetPos(TargetOffsetDir.DIR_LEFT, m_viewScale.largePosOffset); break;
				case 'o': OffsetTargetPos(TargetOffsetDir.DIR_UP, m_viewScale.largePosOffset); break;
				case 'u': OffsetTargetPos(TargetOffsetDir.DIR_DOWN, m_viewScale.largePosOffset); break;

				case 'I': OffsetTargetPos(TargetOffsetDir.DIR_FORWARD, m_viewScale.smallPosOffset); break;
				case 'K': OffsetTargetPos(TargetOffsetDir.DIR_BACKWARD, m_viewScale.smallPosOffset); break;
				case 'L': OffsetTargetPos(TargetOffsetDir.DIR_RIGHT, m_viewScale.smallPosOffset); break;
				case 'J': OffsetTargetPos(TargetOffsetDir.DIR_LEFT, m_viewScale.smallPosOffset); break;
				case 'O': OffsetTargetPos(TargetOffsetDir.DIR_UP, m_viewScale.smallPosOffset); break;
				case 'U': OffsetTargetPos(TargetOffsetDir.DIR_DOWN, m_viewScale.smallPosOffset); break;
				}
			}
			else
			{
				switch(key)
				{
				case 'w': OffsetTargetPos(TargetOffsetDir.DIR_FORWARD, m_viewScale.largePosOffset); break;
				case 's': OffsetTargetPos(TargetOffsetDir.DIR_BACKWARD, m_viewScale.largePosOffset); break;
				case 'd': OffsetTargetPos(TargetOffsetDir.DIR_RIGHT, m_viewScale.largePosOffset); break;
				case 'a': OffsetTargetPos(TargetOffsetDir.DIR_LEFT, m_viewScale.largePosOffset); break;
				case 'e': OffsetTargetPos(TargetOffsetDir.DIR_UP, m_viewScale.largePosOffset); break;
				case 'q': OffsetTargetPos(TargetOffsetDir.DIR_DOWN, m_viewScale.largePosOffset); break;

				case 'W': OffsetTargetPos(TargetOffsetDir.DIR_FORWARD, m_viewScale.smallPosOffset); break;
				case 'S': OffsetTargetPos(TargetOffsetDir.DIR_BACKWARD, m_viewScale.smallPosOffset); break;
				case 'D': OffsetTargetPos(TargetOffsetDir.DIR_RIGHT, m_viewScale.smallPosOffset); break;
				case 'A': OffsetTargetPos(TargetOffsetDir.DIR_LEFT, m_viewScale.smallPosOffset); break;
				case 'E': OffsetTargetPos(TargetOffsetDir.DIR_UP, m_viewScale.smallPosOffset); break;
				case 'Q': OffsetTargetPos(TargetOffsetDir.DIR_DOWN, m_viewScale.smallPosOffset); break;
				}
			}

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

		Vector3[] g_offsets = new Vector3[]
		{
			new Vector3( 0.0f,  1.0f,  0.0f),
			new Vector3( 0.0f, -1.0f,  0.0f),
			new Vector3( 0.0f,  0.0f, -1.0f),
			new Vector3( 0.0f,  0.0f,  1.0f),
			new Vector3( 1.0f,  0.0f,  0.0f),
			new Vector3(-1.0f,  0.0f,  0.0f),
		};
	
	    void OffsetTargetPos(TargetOffsetDir eDir, float worldDistance)
	    {
			Vector3 offsetDir = g_offsets[(int)eDir];
			OffsetTargetPos(offsetDir * worldDistance);
	    }
	    void OffsetTargetPos(Vector3 cameraOffset)
	    {
			Matrix4 currMat = CalcMatrix();
			Quaternion orientation = Quaternion.FromMatrix(new Matrix3(currMat));
			orientation.Conjugate();
			Vector3 worldOffset = Vector3.Transform(cameraOffset, orientation);

			m_currView.targetPos += worldOffset;
	    }
	
	    void ProcessXChange(float iXDiff)
	    {
	        ProcessXChange(iXDiff, false);
	    }
	
		void ProcessXChange(float iXDiff, bool bClearY )
		{
			float degAngleDiff = (iXDiff * m_viewScale.rotationScale);

			//Rotate about the world-space Y axis.
			m_currView.orient = m_startDragOrient * Quaternion.FromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), degAngleDiff);
		}
	
	    void ProcessYChange(float iYDiff)
	    {
	        ProcessYChange(iYDiff, false);
	    }
	
		void ProcessYChange(float iYDiff, bool bClearXZ)
	    {
			float degAngleDiff = (iYDiff * m_viewScale.rotationScale);

			//Rotate about the local-space X axis.
			m_currView.orient = Quaternion.FromAxisAngle(new Vector3(1.0f, 0.0f, 0.0f), degAngleDiff) * m_startDragOrient;
	    }
	    void ProcessXYChange(float iXDiff, float iYDiff)
	    {
			float degXAngleDiff = (iXDiff * m_viewScale.rotationScale);
			float degYAngleDiff = (iYDiff * m_viewScale.rotationScale);

			//Rotate about the world-space Y axis.
			m_currView.orient = m_startDragOrient * Quaternion.FromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), degXAngleDiff);
			//Rotate about the local-space X axis.
			m_currView.orient = Quaternion.FromAxisAngle(new Vector3(1.0f, 0.0f, 0.0f), degYAngleDiff) * m_currView.orient;
	    }
	
	    void ProcessSpinAxis(float iXDiff, float iYDiff)
	    {
			float degSpinDiff = (iXDiff * m_viewScale.rotationScale);
			m_currView.degSpinRotation = degSpinDiff + m_degStarDragSpin;
	    }
	
	    void BeginDragRotate(Vector2 ptStart)
	    {
	        BeginDragRotate(ptStart, RotateMode.RM_DUAL_AXIS);
	    }
	
	    void BeginDragRotate(Vector2 ptStart, RotateMode rotMode)
	    {
			m_RotateMode = rotMode;
			m_startDragMouseLoc = ptStart;
			m_degStarDragSpin = m_currView.degSpinRotation;
			m_startDragOrient = m_currView.orient;
			m_bIsDragging = true;
	    }
	
	    void OnDragRotate(Vector2 ptCurr)
	    {
			Vector2 ptCurrVector = new Vector2(ptCurr.X, ptCurr.Y);
			Vector2 iDiff = ptCurrVector - m_startDragMouseLoc;

			switch(m_RotateMode)
			{
			case RotateMode.RM_DUAL_AXIS:
				ProcessXYChange(iDiff.X, iDiff.Y);
				break;
			case RotateMode.RM_BIAXIAL:
				if(Math.Abs(iDiff.X) > Math.Abs(iDiff.Y))
					ProcessXChange(iDiff.X, true);
				else
					ProcessYChange(iDiff.Y, true);
				break;
			case RotateMode.RM_XZ_AXIS:
				ProcessXChange(iDiff.X);
				break;
			case RotateMode.RM_Y_AXIS:
				ProcessYChange(iDiff.Y);
				break;
			case RotateMode.RM_SPIN_VIEW_AXIS:
				ProcessSpinAxis(iDiff.X, iDiff.Y);
				break;
			default:
				break;
			}
	    }
	
	    void EndDragRotate(Vector2 ptEnd)
	    {
	        EndDragRotate(ptEnd, true);
	    }
	
	    void EndDragRotate(Vector2 ptEnd, bool bKeepResults)
	    {
			if(bKeepResults)
			{
				OnDragRotate(ptEnd);
			}
			else
			{
				m_currView.orient = m_startDragOrient;
			}

			m_bIsDragging = false;
	    }
	
	    void MoveCloser()
	    {
	        MoveCloser(true);
	    }
	
	    void MoveCloser(bool bLargeStep)
	    {
			if(bLargeStep)
				m_currView.radius -= m_viewScale.largeRadiusDelta;
			else
				m_currView.radius -= m_viewScale.smallRadiusDelta;

			if(m_currView.radius < m_viewScale.minRadius)
				m_currView.radius = m_viewScale.minRadius;
	    }
	
	    void MoveAway()
	    {
	        MoveAway(true);
	    }
	
	    void MoveAway(bool bLargeStep)
	    {
			if(bLargeStep)
				m_currView.radius += m_viewScale.largeRadiusDelta;
			else
				m_currView.radius += m_viewScale.smallRadiusDelta;

			if(m_currView.radius > m_viewScale.maxRadius)
				m_currView.radius = m_viewScale.maxRadius;
	    }
	}
}

