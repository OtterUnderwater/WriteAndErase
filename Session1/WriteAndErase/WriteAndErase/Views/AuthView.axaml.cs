using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase.ViewModels;

namespace WriteAndErase.Views;

public partial class AuthView : UserControl
{
    public AuthView()
    {
        InitializeComponent();
        DataContext = new AuthViewModel();
	}
}