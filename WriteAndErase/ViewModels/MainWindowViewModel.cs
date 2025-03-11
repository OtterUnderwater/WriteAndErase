using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using WriteAndErase.Models;
using WriteAndErase.Views;

namespace WriteAndErase.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
	{
		public static MainWindowViewModel? Instance;
		public MainWindowViewModel()
		{
			Instance = this;
		}

		[ObservableProperty]
		private UserControl _userControl = new AuthView();

		[ObservableProperty]
		private UserControl _ucChildren;

		public static YpContext DB = new YpContext();
	}
}
