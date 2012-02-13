using System;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using System.Threading;



namespace udp2serial
{
	class MainClass
	{
		
		public static void Main (string[] args)
		{
			pingUDP pingClass = new pingUDP();
			Thread trd = new Thread( new ThreadStart(pingClass.theLoop));
			trd.Interrupt();
			trd.Start();
			
			while(true)
			{
				if(Console.ReadKey().KeyChar == 'q')
				{
					trd.Interrupt();
					Environment.Exit(1);
				}
				Thread.Sleep(1);
			}
			
		}
		
	}
	
	class pingUDP
	{
		public void theLoop()
		{
			int listenPort = 11000;
			UdpClient listener = new UdpClient(listenPort);
			IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
			SerialPort portCOM1 = new SerialPort();
			try{
				portCOM1 = new SerialPort("/dev/ttyS0", 115200, Parity.None, 8, StopBits.One);
				portCOM1.Open();
			}
			catch(Exception ee)
			{
				Console.WriteLine("Not connected to COM1");
				Console.WriteLine(ee.Message.ToString());
			}
			string received_data;
			byte[] received_byte_array;
			
			bool done = false;
			
			while(!done)
			{
				Console.WriteLine("Waiting For Data - press (q) to quit");
				received_byte_array = listener.Receive(ref groupEP);
				received_data = "";
				for(int i = 0; i < received_byte_array.Length; i++)
				{
					received_data = received_data+received_byte_array[i]+" ";
				}
				Console.WriteLine("Recived Data From {0}: {1}", groupEP.ToString(), received_data);
				try{
					portCOM1.Write(received_byte_array,0,received_byte_array.Length);		// write what was recived on UDP to com 1
					Console.WriteLine("Wrote data to COM1 @ 115200 8N1");
				}
				catch(Exception ee)
				{
					Console.WriteLine(ee.Message.ToString());
					try{
						portCOM1.Close();
						portCOM1.DiscardOutBuffer();
						portCOM1.DiscardInBuffer();
						
						listener.Close();
						
						listener = new UdpClient(listenPort);
				 		groupEP = new IPEndPoint(IPAddress.Any, listenPort);
					}
					catch(Exception eee)
					{
						Console.WriteLine(eee.Message.ToString());
					}
					
				}
						
			}
		}
	}
	
			                                         
			                                         
}
