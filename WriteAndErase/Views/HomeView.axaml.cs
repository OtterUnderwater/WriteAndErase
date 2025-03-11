using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase.Models;
using WriteAndErase.ViewModels;

namespace WriteAndErase.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
	}

	public HomeView(User? user)
	{
		InitializeComponent();
		DataContext = new HomeViewModel(user);
	}
}