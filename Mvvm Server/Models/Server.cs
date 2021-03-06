﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mvvm_Server.Models
{
	/// <summary>
	/// A Tcp Server class which will communicate to its corresponding view-model class.
	/// this mainly functions as an updater to the state of the client which will be connecting
	/// </summary>
	class Server : INotifyPropertyChanged
	{
		public Server()
		{

		}

		#region members
		private TcpListener mServer;
		private const int PORT = 2378;
		private const string IP = "127.0.0.1";
		private string mState = "";

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		public void RaisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// stops the server from listening for connections.
		/// </summary>
		public void Stop()
		{
			mServer.Stop();
		}

		public string State
		{
			get
			{
				return mState;
			}
			set
			{
				if (mState != value)
				{
					mState = value;
					RaisePropertyChanged(mState);
				}
			}
		}

		public async Task RunAsync()
		{
			try
			{
				IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
				// TcpListener server = new TcpListener(port);
				mServer = new TcpListener(iPEndPoint);

				// Start listening for client requests.
				mServer.Start();
				State = "Started";

				// Enter the listening loop. 
				while (true)
				{
					//State = "Waiting for a connection...";
					// wait for a client
					TcpClient client = mServer.AcceptTcpClient();

					//State = "Client Connected";

					// client connected
					var res = await GetClientStateAsync(client);
					Console.WriteLine("String: " + res.ToString());
					State = res;
				}
			}
			catch (SocketException e)
			{
				Console.WriteLine("SocketException: {0}", e.Message);
			}
			finally
			{
				// Stop listening for new clients.
				mServer.Stop();
			}
		}

		/// <summary>
		/// reads the state sent by the client
		/// </summary>
		/// <param name="client">the TcpClient connected</param>
		/// <returns>the state of the client</returns>
		private async Task<string> GetClientStateAsync(TcpClient client)
		{
			return await Task.Run(async () =>
			{
				Console.WriteLine("Reading");
				//State = "Reading...";
				NetworkStream networkStream = client.GetStream();
				int bytes_read = 0;
				int bytes_total_read = 0;
				byte[] buffer = new byte[1024];
				string stateReceived = "";
				//MemoryStream memoryStream = new MemoryStream();

				while ((bytes_read = await networkStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
				{
					stateReceived += buffer.GetStringValue();
					bytes_total_read += bytes_read;
					Console.WriteLine("bytes read: " + bytes_read);
				}
				//State = "Finished reading state";
				Console.WriteLine("State: " + stateReceived);
				return stateReceived.Substring(0, bytes_total_read);
			});
		}
	}
}
