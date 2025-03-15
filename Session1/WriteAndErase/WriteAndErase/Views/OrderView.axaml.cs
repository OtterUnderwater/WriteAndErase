using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using WriteAndErase.Models;
using WriteAndErase.ViewModels;

namespace WriteAndErase.Views;

public partial class OrderView : UserControl
{
    public OrderView()
    {
        InitializeComponent();
    }
	public OrderView(User? user, List<OrdersProduct> ordersProducts)
	{
		InitializeComponent();
		DataContext = new OrderViewModel(user, ordersProducts);
	}
}