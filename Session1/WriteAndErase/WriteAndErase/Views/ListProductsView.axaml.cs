using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase.Models;
using WriteAndErase.ViewModels;

namespace WriteAndErase.Views;

public partial class ListProductsView : UserControl
{
    public ListProductsView()
    {
        InitializeComponent();
	}
	public ListProductsView(User? user)
	{
		InitializeComponent();
		DataContext = new ListProductsViewModel(user);
	}
}