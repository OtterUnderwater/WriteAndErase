using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Linq;
using WriteAndErase.Models;
using WriteAndErase.Views;
using CommunityToolkit.Mvvm.Input;
using WriteAndErase.Function;
using Avalonia;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Threading.Tasks;

namespace WriteAndErase.ViewModels
{
	public partial class AuthViewModel : ViewModelBase
	{
		#region [ Поля ]
		[ObservableProperty]
		private string _password = "";

		[ObservableProperty]
		private string _login = "";

		[ObservableProperty]
		private Canvas? _captcha = null;

		[ObservableProperty]
		private string _tbcaptcha = "";

		[ObservableProperty]
		private bool _isDisabled = true;

		[ObservableProperty]
		public bool isVisibleCaptcha = false;

		string _textCaptcha = "";
		#endregion
 
		public AuthViewModel()
		{
			_textCaptcha = "";
			Login = "";
			Password = "";
			Tbcaptcha = "";
			Captcha = null;
			IsDisabled = true;
			timer.Interval = new TimeSpan(0, 0, 10);
			timer.Tick += new EventHandler(EventTimer);
		}

		[RelayCommand]
		public async Task GoHomeAuth()
		{
			User? user = MainWindowViewModel.DB.Users.FirstOrDefault(u => u.Password == Password && u.Login == Login);		
			// Капчи нет
			if (Captcha == null)
			{
				if (user != null) MainWindowViewModel.Instance!.UserControl = new ListProductsView(user);
				else CreateCaptha();
			}
			else
			{
				if (user != null && Tbcaptcha == _textCaptcha) MainWindowViewModel.Instance!.UserControl = new ListProductsView(user);
				else 
				{
					Captcha = null;
					IsDisabled = false;
					IsVisibleCaptcha = false;
					Login = "";
					Password = "";
					Tbcaptcha = "";
					timer.Start(); // запуск таймера
					await MessageBoxManager.GetMessageBoxStandard("Доступ временно ограничен", "Попробуйте еще раз через 10 секунд", ButtonEnum.Ok).ShowAsync();			
				}
			}
		}

		[RelayCommand]
		public void GoHome() => MainWindowViewModel.Instance!.UserControl = new ListProductsView(null);

		/// <summary>
		/// Генерация капчи и отображение
		/// </summary>
		private void CreateCaptha()
		{
			_textCaptcha = "";
			RandomElements rndEl = new RandomElements();
			Random rnd = new Random();
			Canvas canvas = new Canvas()
			{
				Width = 400,
				Height = 100,
				Background = rndEl.GetRandomColor()
			};
			for (int i = 0; i < 4; i++)
			{
				TextBlock text = new TextBlock()
				{
					Text = rndEl.GetRandomText(),
					FontSize = 50,
					Foreground = Brushes.Black,
					Padding = rndEl.GetRandomThickness(i + 1),
					FontWeight = rnd.Next(2) == 0 ? FontWeight.Bold : FontWeight.Normal,
					FontStyle = rnd.Next(2) == 0 ? FontStyle.Italic : FontStyle.Normal
				};
				_textCaptcha += text.Text;
				canvas.Children.Add(text);
			}
			for (int i = 0; i < 10; i++)
			{
				Line line = new Line()
				{
					StartPoint = new Point(rnd.Next(400), rnd.Next(100)),
					EndPoint = new Point(rnd.Next(400), rnd.Next(100)),
					Stroke = rndEl.GetRandomColor(),
					StrokeThickness = 3
				};
				canvas.Children.Add(line);
			}
			Captcha = canvas;
			IsVisibleCaptcha = true;
		}

		#region [ Таймер ]
		DispatcherTimer timer = new DispatcherTimer();
		/// <summary>
		/// Cобытие, которое происходит после истечения времени таймера
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EventTimer(object sender, EventArgs e)
		{
			IsDisabled = true;
			CreateCaptha();
			timer.Stop();
		}
		#endregion	
	}
}