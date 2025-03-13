using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase.Models;
using WriteAndErase.ViewModels;

namespace WriteAndErase.Views;

public partial class EditProductView : UserControl
{
    public EditProductView()
    {
        InitializeComponent();
    }

	public EditProductView(User? user)
	{
		InitializeComponent();
		DataContext = new EditProductViewModel(user);
	}

	public EditProductView(User? user, string article)
	{
		InitializeComponent();
		DataContext = new EditProductViewModel(user, article);
	}
}