using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase.Models;
using WriteAndErase.ViewModels;

namespace WriteAndErase.Views;

public partial class ListOrdersView : UserControl
{
    public ListOrdersView()
    {
        InitializeComponent();
    }

	public ListOrdersView(User? user)
	{
		InitializeComponent();
		DataContext = new ListOrdersViewModel(user);
	}
}