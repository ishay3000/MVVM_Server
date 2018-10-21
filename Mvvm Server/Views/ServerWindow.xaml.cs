using Mvvm_Server;
using Mvvm_Server.Models;
using Mvvm_Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace serv_view_model
{
	/// <summary>
	/// Interaction logic for ServerWindow.xaml
	/// </summary>
	public partial class ServerWindow : Window
	{
		private ServerViewModel mServerVM;
		private VoiceSynthesizer synthesizer;

		public ServerWindow()
		{
			InitializeComponent();
			mServerVM = new ServerViewModel();
			this.DataContext = mServerVM;
			this.Closing += mServerVM.OnWindow_Closing;

			synthesizer = new VoiceSynthesizer();
			mServerVM.mServer.PropertyChanged += synthesizer.MServer_PropertyChanged;
		}
	}
}