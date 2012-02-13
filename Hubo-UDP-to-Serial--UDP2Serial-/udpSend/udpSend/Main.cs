using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace udpSend
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			IPAddress send_to_address = IPAddress.Parse("192.168.0.103");
			IPEndPoint send_end_point = new IPEndPoint(send_to_address, 11000);
			while(true)
			{
				Console.Write("Send to "+send_to_address.ToString()+": ");
				String toSend = Console.ReadLine();
				byte[] bufferData = System.Text.Encoding.ASCII.GetBytes(toSend.ToCharArray());
				
				sending_socket.SendTo(bufferData , send_end_point);
				Console.WriteLine("Sending to "+send_to_address.ToString()+": "+toSend);
				Console.WriteLine(" ");
			}
			
		}
	}
}
