using System;

namespace GlslTutorials
{
	public class PushStack : IDisposable
	{
	    private MatrixStack m_stack;
	
	    ///Pushes the given MatrixStack.
	    public PushStack(MatrixStack stack)
	    {
	        m_stack = stack;
	        m_stack.Push();
	    }
	
	    ///Pops the MatrixStack that the constructor was given.
	    public void close()
	    {
	        m_stack.Pop();
	    }
		
		public void Dispose()
		{
		 	m_stack.Pop();
		}
		
	
	    /**
	     \brief Resets the current matrix of the MatrixStack to the value that was pushed in the constructor.
	
	     This does not alter the stack depth. It just resets the matrix.
	     **/
	    void ResetStack()
	    {
	        m_stack.Reset();
	    }
	
	    // ?? PushStack(const PushStack &);
	    // ?? PushStack &operator=(const PushStack&);
	
	}
}

