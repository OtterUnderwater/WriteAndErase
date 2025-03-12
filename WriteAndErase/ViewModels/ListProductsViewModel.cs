using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using WriteAndErase.Models;

namespace WriteAndErase.ViewModels
{
	public partial class ListProductsViewModel : ViewModelBase
	{
		#region [ Поля и свойства ]
		private User? _user;
		private Order? _order;

		[ObservableProperty]
		private List<Product> _products = new();

		[ObservableProperty]
		private List<Product> _productsPreview = new();

		[ObservableProperty]
		private bool _isVisibleButton = false;

		[ObservableProperty]
		private Product _selectedProduct;

		[ObservableProperty]
		private List<OrdersProduct> _ordersProducts = new();

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
			"Все диапазоны",
			"от 0 до 9,99%",
			"от 10 до 14,99%",
			"15% и более",
		};

		[ObservableProperty]
		private string _messageCountProduct = "";

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

		public string Search
		{
			get => _search;
			set
			{
				SetProperty(ref _search, value);
				Filter();
			}
		}
		private string _search = "";
		#endregion

		public ListProductsViewModel(User? user)
		{
			_user = user;
			Products = MainWindowViewModel.DB.Products
				.Include(it => it.IdCategoryNavigation)
				.Include(it => it.IdManufacturerNavigation)
				.Include(it => it.IdSupplierNavigation)
				.Include(it => it.IdUnitNavigation)
				.ToList();
			ProductsPreview = Products;
			MessageCountProduct = $"{ProductsPreview.Count} из {ProductsPreview.Count}";
		}

		[RelayCommand]
		public void AddProduct()
		{
			bool flagNoProduct = true;
			if (_order == null) CreateOrder();
			foreach (var product in OrdersProducts)
			{
				if (product.ProductArticleNumber == SelectedProduct.ArticleNumber)
				{
					product.Count++;
					flagNoProduct = false;
				}
			}
			if (flagNoProduct)
			{
				OrdersProduct ordersProduct = new OrdersProduct
				{
					OrderId = _order!.Id,
					ProductArticleNumber = SelectedProduct.ArticleNumber,
					Count = 1,
					Order = _order!,
					ProductArticleNumberNavigation = SelectedProduct,
				};
				OrdersProducts.Add(ordersProduct);
			}
			if (OrdersProducts.Count() > 0) IsVisibleButton = true;
		}

		private void CreateOrder()
		{
			_order = new Order();
			DateTime date = DateTime.Now;
			_order.DateOrder = new DateOnly(date.Year, date.Month, date.Day);
			_order.IdClient = (_user != null) ? _user.Id : null;
			_order.IdStatus = 1;
			MainWindowViewModel.DB.Orders.Add(_order);
			MainWindowViewModel.DB.SaveChanges();
		}

		void Filter()
		{
			ProductsPreview = Products;
			if (_selectedFilterType != 0)
			{
				if (_selectedFilterType == 1)
					ProductsPreview = ProductsPreview
						.Where(it => it.CurrentDiscount >= 0 && it.CurrentDiscount < 10)
						.ToList();

				if (_selectedFilterType == 2)
					ProductsPreview = ProductsPreview
						.Where(it => it.CurrentDiscount >= 10 && it.CurrentDiscount < 14)
						.ToList();

				if (_selectedFilterType == 3)
					ProductsPreview = ProductsPreview
						.Where(it => it.CurrentDiscount >= 15)
						.ToList();
			}
			if (_selectedSortedType != 0)
			{
				if (_selectedSortedType == 1) ProductsPreview = ProductsPreview.OrderBy(it => it.Cost).ToList();
				if (_selectedSortedType == 2) ProductsPreview = ProductsPreview.OrderByDescending(it => it.Cost).ToList();
			}
			if (Search != "")
			{
				ProductsPreview = ProductsPreview.Where(it => it.Name.ToLower().Contains(_search.ToLower())).ToList();
			}
			MessageCountProduct = $"{ProductsPreview.Count} из {Products.Count}";
		}	
	}
}