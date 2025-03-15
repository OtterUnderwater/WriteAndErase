using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteAndErase.Models;
using WriteAndErase.Views;

namespace WriteAndErase.ViewModels
{
	public partial class ListOrdersViewModel : ViewModelBase
	{
		#region [ Поля и свойства ]
		User? _currentUser;
		public int SelectedSortedType
		{
			get => _selectedSortedType;
			set
			{
				SetProperty(ref _selectedSortedType, value);
				Filter();
			}
		}
		private int _selectedSortedType = 0;

		public int SelectedFilterType
		{
			get => _selectedFilterType;
			set
			{
				SetProperty(ref _selectedFilterType, value);
				Filter();
			}
		}
		private int _selectedFilterType = 0;

		[ObservableProperty]
		private List<string> _listSortedType = new()
		{
			"Без сортировки",
			"По возрастанию стоимости",
			"По убыванию стоимости",
		};
		
		[ObservableProperty]
		private List<string> _listFilterType = new()
		{
			"Без фильтра",
			"от 0 до 10%",
			"от 11 до 14%",
			"15% и более",
		};

		[ObservableProperty]
		private List<Order> _orders = new();

		[ObservableProperty]
		private List<Order> _ordersPreview = new();
		#endregion
		public Order SelectedOrder
		{
			get => _selectedOrder;
			set
			{
				SetProperty(ref _selectedOrder, value);
				MainWindowViewModel.Instance!.UserControl = new EditOrderView(_currentUser, SelectedOrder);
			}
		}
		private Order _selectedOrder;

		private void Filter()
		{
			OrdersPreview = Orders;
			if (_selectedSortedType != 0)
			{
				if (_selectedSortedType == 1) OrdersPreview = OrdersPreview.OrderBy(it => it.Cost).ToList();
				if (_selectedSortedType == 2) OrdersPreview = OrdersPreview.OrderByDescending(it => it.Cost).ToList();
			}
			if (_selectedFilterType != 0)
			{
				if (_selectedFilterType == 2) OrdersPreview = OrdersPreview
						.Where(it => it.Discount >= 0 && it.Discount <= 10).ToList();
				if (_selectedFilterType == 2) OrdersPreview = OrdersPreview
						.Where(it => it.Discount >= 11 && it.Discount <= 14).ToList();
				if (_selectedFilterType == 2) OrdersPreview = OrdersPreview.Where(it => it.Discount >= 15).ToList();
			}
		}

		public ListOrdersViewModel(User? user)
		{
			_currentUser = user;
			Orders = MainWindowViewModel.DB.Orders
				.Include(it => it.IdClientNavigation)
				.Include(it => it.IdPickUpPointNavigation)
				.Include(it => it.IdStatusNavigation)
				.ToList();
			Orders.ForEach(it => {
				it.FIO = (it.IdClientNavigation == null) ? "Гость" :
				(it.IdClientNavigation.Surname + " " + it.IdClientNavigation.Name + " " + it.IdClientNavigation.Patronymic);
				// Цвет по умолчанию
				it.Color = "#ffffff";
				int count = 0;
				double cost = 0;
				List<OrdersProduct> ordersProducts = MainWindowViewModel.DB.OrdersProducts
				.Where(op => op.OrderId == it.Id).Include(it => it.ProductArticleNumberNavigation).ToList();
				foreach (OrdersProduct product in ordersProducts)
				{
					double costProd = product.ProductArticleNumberNavigation.Cost;
					double currentDiscount = (double)product.ProductArticleNumberNavigation.CurrentDiscount;
					cost += (product.Count * costProd);
					double costOneProd = (costProd / 100) * (100 - currentDiscount);
					it.Cost += product.Count * costOneProd;
					if (cost > 0)
						it.Discount = (1.0 - (it.Cost / cost)) * 100;
					if (product.ProductArticleNumberNavigation.QuantityInStock > 3)
						count++;

					else if (product.ProductArticleNumberNavigation.QuantityInStock == 0)
						it.Color = "#ff8c00"; // Xотя бы одного товара нет на складе
				}
				//Все товары в заказе есть на складе в наличии более 3 позиций
				if (it.Color == "#ffffff" && ordersProducts.Count == count) it.Color = "#20b2aa";
			});
			Filter();
		}

		[RelayCommand]
		public void Exit() => MainWindowViewModel.Instance!.UserControl = new ListProductsView(_currentUser);
	}
}
