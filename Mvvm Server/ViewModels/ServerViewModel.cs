using Mvvm_Server.Models;
using serv_view_model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mvvm_Server.ViewModels
{
	class ServerViewModel : INotifyPropertyChanged
	{
		#region members
		private Server mServer;
		private Thread mServerThread;
		private ICommand mBtnStartServer;
		public string mState = "Null";

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		public ServerViewModel()
		{
			mServer = new Server();
			mServerThread = new Thread(async () => await mServer.RunAsync());
			mBtnStartServer = new RelayCommand(() => mServerThread.Start(), null);
			mServer.PropertyChanged += MServer_PropertyChanged;
		}

		private void MServer_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			State = propertyName;
		}

		public void RaisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void OnWindow_Closing(object sender, CancelEventArgs e)
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
					RaisePropertyChanged("State");
				}
			}
		}

		public ICommand BtnStartServer
		{
			get
			{
				return mBtnStartServer;
			}
		}
	}
}
