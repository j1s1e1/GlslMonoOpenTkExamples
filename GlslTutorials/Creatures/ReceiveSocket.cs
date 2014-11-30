using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GlslTutorials
{
	public class ReceiveSocket
	{
		SocketListener socketListener;
		public delegate void ForwardReceivedDataDelegate(byte[] data);
		public ForwardReceivedDataDelegate ForwardReceivedData;
		public ReceiveSocket(ForwardReceivedDataDelegate ForwardReceivedDataIn)
		{
			ForwardReceivedData = ForwardReceivedDataIn;
			socketListener = new SocketListener();
			socketListener.ForwardReceivedData = ForwardData;
			thread = new Thread(RunAsyncSocketListener);
			thread.Start();
		}
		
		~ReceiveSocket()
		{
			if (thread.IsAlive)
			{
				thread.Abort();
			}
		}
		
		public void ForwardData(byte[] data)
		{
			ForwardReceivedData(data);
		}
		
		Thread thread;
		
		private void RunAsyncSocketListener()
		{
			try
			{
				socketListener.StartListening();
			}
			catch
			{
				Thread.ResetAbort();
			}
		}		
	}
}

