using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase.Models;
using WriteAndErase.ViewModels;

namespace WriteAndErase.Views;

public partial class EditOrderView : UserControl
{
    public EditOrderView()
    {
        InitializeComponent();
    }

	public EditOrderView(User? user, Order order)
	{
		InitializeComponent();
		DataContext = new EditOrderViewModel(user, order);
	}
}