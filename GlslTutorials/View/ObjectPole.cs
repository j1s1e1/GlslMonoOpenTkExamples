using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlslTutorials
{
	enum MouseModifiers
	{
		MM_KEY_SHIFT =	0x01,	///<One of the shift keys.
		MM_KEY_CTRL =	0x02,	///<One of the control keys.
		MM_KEY_ALT =	0x04,	///<One of the alt keys.
	};

	public class ObjectPole : IPole 
	{
		bool rightMultiply()
		{
			return MatrixStack.rightMultiply;
		}
	    private ObjectData initialPosition;
	    private float rotateScale;
	    private bool isDragging;
			/*
			private ViewPole.RotatingMode rotatingMode;
			private Vec2 prevMousePos;
			private Vec2 startDragMousePos;
			private Quat startDragOrient;
			private ViewPole viewPole;
			*/
	
	    /**
	     \brief Creates an object pole with a given initial position and orientation.
	
	     \param initialData The starting position and orientation of the object in world space.
	     \param rotateScale The number of degrees to rotate the object per window space pixel
	     \param actionButton The mouse button to listen for. All other mouse buttons are ignored.
	     \param pLookatProvider An object that will compute a view matrix. This defines the view space
	     that orientations can be relative to. If it is NULL, then orientations will be relative to the world.
	     **/
	    public ObjectPole(ObjectData initialData, float rotateScale, MouseButtons actionButton, ViewProvider LookatProvider)
	    {
			m_pView = LookatProvider;
			m_po = initialData;
			m_initialPo = initialData;
			m_rotateScale = rotateScale;
			m_actionButton = actionButton;
			m_bIsDragging = false;
	        initialPosition = initialData;
	    }
	
	    ///Generates the local-to-world matrix for this object.
	    public Matrix4 CalcMatrix()
	    {
			Matrix4 translateMat = Matrix4.Identity;
			translateMat.Row3 = new Vector4(m_po.position, 1.0f);
			if (rightMultiply())
			{
				return Matrix4.Mult(translateMat, Matrix4.CreateFromQuaternion(m_po.orientation));
			}
			else
			{
				return Matrix4.Mult(Matrix4.CreateFromQuaternion(m_po.orientation), translateMat);
			}
	    }
	
	    /**
	     \brief Sets the scaling factor for orientation changes.
	
	     The scaling factor is the number of degrees to rotate the object per window space pixel.
	     The scale is the same for all mouse movements.
	     **/
	    void SetRotationScale(float rotateScale)
	    {
	    }
	    ///Gets the current scaling factor for orientation changes.
	    float GetRotationScale()
	    {
	        return m_rotateScale;
	    }
	
	    ///Retrieves the current position and orientation of the object.
	    ObjectData GetPosOrient()
	    {
	        return m_po;
	    }
	
	    ///Resets the object to the initial position/orientation. Will fail if currently dragging.
	    void Reset()
	    {
	    }
	
	    /**
	     \name Input Providers
	
	     These functions provide input, since Poles cannot get input for themselves. See
	     \ref module_glutil_poles "the Pole manual" for details.
	     **/
	    ///@{
	
	    /**
	     \brief Notifies the pole of a mouse button being pressed or released.
	
	     \param button The button being pressed or released.
	     \param isPressed Set to true if \a button is being pressed.
	     \param modifiers A bitfield of MouseModifiers that specifies the modifiers being held down currently.
	     \param position The mouse position at the moment of the mouse click.
	     **/
	    public void MouseClick(MouseButtons button, bool isPressed, int modifiers, Point position)
	    {
			if(isPressed)
			{
				//Ignore button presses when dragging.
				if(!m_bIsDragging)
				{
					if(button == m_actionButton)
					{
						if((modifiers & (int)MouseModifiers.MM_KEY_ALT) != 0)
							m_RotateMode = RotateMode.RM_SPIN;
						else if((modifiers & (int)MouseModifiers.MM_KEY_CTRL) != 0)
							m_RotateMode = RotateMode.RM_BIAXIAL;
						else
							m_RotateMode = RotateMode.RM_DUAL_AXIS;

						m_prevMousePos = new Vector2(position.X, position.Y);
						m_startDragMousePos =  new Vector2(position.X, position.Y);
						m_startDragOrient = m_po.orientation;

						m_bIsDragging = true;
					}
				}
			}
			else
			{
				//Ignore up buttons if not dragging.
				if(m_bIsDragging)
				{
					if(button == m_actionButton)
					{
						MouseMove(position);

						m_bIsDragging = false;
					}
				}
			}
	    }
	
	    ///Notifies the pole that the mouse has moved to the given absolute position.
	    public void MouseMove(Point  position)
	    {
			Vector2 vectorPositoin = new Vector2(position.X, position.Y);
			if(m_bIsDragging)
			{
				Vector2 iDiff = vectorPositoin - m_prevMousePos;

				switch(m_RotateMode)
				{
				case RotateMode.RM_DUAL_AXIS:
					{
						Quaternion rotRight =  Quaternion.FromAxisAngle(Vector3.UnitY, iDiff.X * m_rotateScale);
						Quaternion rotLeft =  Quaternion.FromAxisAngle(Vector3.UnitX, iDiff.Y * m_rotateScale);
						Quaternion rot = Quaternion.Multiply(rotLeft, rotRight);
						rot.Normalize();
						RotateViewDegrees(rot);
					}
					break;
				case RotateMode.RM_BIAXIAL:
					{
						Vector2 iInitDiff = vectorPositoin - m_startDragMousePos;
						Quaternion rot;

						float degAngle;
						if(Math.Abs(iInitDiff.X) > Math.Abs(iInitDiff.Y))
						{
							degAngle = iInitDiff.X * m_rotateScale;
							rot =  Quaternion.FromAxisAngle(Vector3.UnitY, degAngle);
						}
						else
						{
							degAngle = iInitDiff.Y * m_rotateScale;
							rot =  Quaternion.FromAxisAngle(Vector3.UnitX, degAngle);
						}
						RotateViewDegrees(rot, true);
					}
					break;
				case RotateMode.RM_SPIN:
					RotateViewDegrees(Quaternion.FromAxisAngle(Vector3.UnitZ, -iDiff.X * m_rotateScale));
					break;
				}

				m_prevMousePos = vectorPositoin;
			}
	    }
	
	    /**
	     \brief Notifies the pole that the mouse wheel has been rolled up or down.
	
	     \param direction A positive number if the mouse wheel has moved up, otherwise it should be negative.
	     \param modifiers The modifiers currently being held down when the wheel was rolled.
	     \param position The absolute mouse position at the moment the wheel was rolled.
	     **/
	    void MouseWheel(int direction, int modifiers, Vector2 position)
	    {
	    }
	
	    /**
	     \brief Notifies the pole that a character has been entered.
	
	     \param key ASCII keycode.
	     **/
	    void CharPress(char key)
	    {
			// empty in example
	    }
	    ///@}
	
	    ///Returns true if the mouse is currently being dragged.
	    bool IsDragging()
	    {
	        return m_bIsDragging;
	    }
	
	    private enum Axis
	    {
	        AXIS_X,
	        AXIS_Y,
	        AXIS_Z,
	
	        NUM_AXES,
	    };
	
	    enum RotateMode
	    {
	        RM_DUAL_AXIS,
	        RM_BIAXIAL,
	        RM_SPIN,
	    };
	
	    void RotateWorldDegrees(Quaternion rot)
	    {
	        RotateWorldDegrees(rot, false);
	    }
	
	    void RotateWorldDegrees(Quaternion rot, bool bFromInitial)
	    {
			if(!m_bIsDragging)
				bFromInitial = false;
			if (rightMultiply())
			{
				m_po.orientation = ((bFromInitial ? m_startDragOrient : m_po.orientation) * rot).Normalized();
			}
			else
			{
				m_po.orientation = (rot * (bFromInitial ? m_startDragOrient : m_po.orientation)).Normalized();
			}
	    }
	
	    void RotateLocalDegrees(Quaternion rot)
	    {
	        RotateLocalDegrees(rot, false);
	    }
	
	    void RotateLocalDegrees(Quaternion rot, bool bFromInitial)
	    {
			if(!m_bIsDragging)
				bFromInitial = false;
			if (rightMultiply())
			{
				m_po.orientation = ((bFromInitial ? m_startDragOrient : m_po.orientation) * rot).Normalized();
			}
			else
			{
				m_po.orientation = (rot * (bFromInitial ? m_startDragOrient : m_po.orientation)).Normalized();
			}
	    }
	
	    void RotateViewDegrees(Quaternion rot)
	    {
	        RotateViewDegrees(rot, false);
	    }
	
	    void RotateViewDegrees(Quaternion rot, bool bFromInitial)
	    {
			if(!m_bIsDragging)
				bFromInitial = false;

			if(m_pView != null)
			{
				Quaternion viewQuat =  Quaternion.FromMatrix(new Matrix3(m_pView.CalcMatrix()));
				Quaternion invViewQuat = viewQuat;
				invViewQuat.Conjugate();
				// FIXME check multipy order
				Quaternion result = bFromInitial ? m_startDragOrient : m_po.orientation;
				if (rightMultiply())
				{
					result = Quaternion.Multiply(result, viewQuat);
					result = Quaternion.Multiply(result, rot);
					result = Quaternion.Multiply(result, invViewQuat);
				}
				else
				{
					result = Quaternion.Multiply(viewQuat, result);
					result = Quaternion.Multiply(rot, result);
					result = Quaternion.Multiply(invViewQuat, result);
				}
				result.Normalize();

				m_po.orientation = result;
			}
			else
				RotateWorldDegrees(rot, bFromInitial);
	    }
	
	    ViewProvider m_pView;
	    ObjectData m_po;
	    ObjectData m_initialPo;
	
	    float m_rotateScale;
	    MouseButtons m_actionButton;
	
	    //Used when rotating.
	    RotateMode m_RotateMode;
	    bool m_bIsDragging;
	
	    Vector2 m_prevMousePos;
	    Vector2 m_startDragMousePos;
	    Quaternion m_startDragOrient;

		public void MouseButton(int button, int state, int x, int y)
		{
		}
		public void MouseButton(int button, int state, Point p)
		{
		}
			
		public void MouseWheel(int wheel, int direction, Point p)
		{
			// empty in example
		}
	
	}
}

