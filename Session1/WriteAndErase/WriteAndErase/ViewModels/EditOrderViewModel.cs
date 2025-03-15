using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteAndErase.Models;
using WriteAndErase.Views;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WriteAndErase.ViewModels
{
	public partial class EditOrderViewModel : ViewModelBase
	{
		#region [ Поля и свойства ]
		User? _currentUser;

		[ObservableProperty]
		List<Status> _statuses;

		[ObservableProperty]
		Order _order;
		#endregion
		
		public EditOrderViewModel(User? user, Order order)
		{
			_currentUser = user;
			Statuses = MainWindowViewModel.DB.Statuses.ToList();
			Order = order;
		}

		/// <summary>
		/// Изменения статуса заказа и даты доставки
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		public async Task Save()
		{
			try
			{
				MainWindowViewModel.DB.Orders.Update(Order);
				MainWindowViewModel.DB.SaveChanges();
				await MessageBoxManager.GetMessageBoxStandard("Сохранено", "Данные заказа были изменены", ButtonEnum.Ok).ShowAsync();
				Exit();
			}
			catch
			{
				await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Проверьте правильность заполнения полей и попробуйте еще раз", ButtonEnum.Ok).ShowAsync();
			}
		}

		[RelayCommand]
		public void Exit() => MainWindowViewModel.Instance!.UserControl = new ListOrdersView(_currentUser);
	}
}
