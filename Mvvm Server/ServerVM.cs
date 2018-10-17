using serv_view_model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mvvm_Server
{
	class ServerVM : INotifyPropertyChanged
	{
		public string Temperature { get; set; }
		public string ServerStatus { get; set; }
		public string State
		{
			get
			{
				return mState;
			}
			set
			{
				mState = value;
				RaisePropertyChanged("State");
			}
		}
		private Thread mServerThread;
		private ICommand mBtnStartCommand;
		public string mState = "Null";

		public event PropertyChangedEventHandler PropertyChanged;

		public ServerVM()
		{
			ServerStatus = "Press Start server button to start the TCP/IP server";
			mServerThread = new Thread(async () => await StartServerAsync());
			mBtnStartCommand = new RelayCommand(UpdateControlExecute, CanUpdateControlExecute);
		}



		public void RaisePropertyChanged(string property)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}

		void UpdateControlExecute()
		{
			if (!mServerThread.IsAlive)
			{
				mServerThread.Start();
			}
		}

		private async Task StartServerAsync()
		{
			TcpListener server = null;
			try
			{
				// Set the TcpListener on port 13000.
				Int32 port = 8089;
				IPAddress localAddr = IPAddress.Parse("127.0.0.1");

				// TcpListener server = new TcpListener(port);
				server = new TcpListener(localAddr, port);

				// Start listening for client requests.
				server.Start();
				State = "Starting";
				Thread.Sleep(1500);

				// Buffer for reading data
				Byte[] bytes = new Byte[256];
				String data = null;

				// Enter the listening loop. 
				while (true)
				{
					ServerStatus = "Waiting for a connection... ";
					State = "Waiting...";

					// Perform a blocking call to accept requests. 
					// You could also user server.AcceptSocket() here.
					TcpClient client = server.AcceptTcpClient();
					ServerStatus = "Connected!";

					data = null;

					// Get a stream object for reading and writing
					NetworkStream stream = client.GetStream();

					int i;

					// Loop to receive all the data sent by the client. 
					while ((i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
					{
						// Translate data bytes to a ASCII string.
						data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
						Temperature = data;

						// Process the data sent by the client.
						//data = data.ToUpper();

						byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

						// Send back a response.
						stream.Write(msg, 0, msg.Length);
						ServerStatus = "Sent: " + data;
					}

					// Shutdown and end connection
					client.Close();
				}
			}
			catch (SocketException e)
			{
				Debug.WriteLine("SocketException: {0}", e);
			}
			finally
			{
				// Stop listening for new clients.
				server.Stop();
			}
		}

		bool CanUpdateControlExecute()
		{
			return true;
		}

		public string STATEHA
		{
			get
			{
				return "";
			}
		}

		public ICommand StartServerButton
		{
			get
			{
				return mBtnStartCommand;
			}
		}
	}
}
