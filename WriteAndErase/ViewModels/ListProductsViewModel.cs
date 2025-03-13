using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WriteAndErase.Models;
using WriteAndErase.Views;

namespace WriteAndErase.ViewModels
{
	public partial class ListProductsViewModel : ViewModelBase
	{
		#region [ Поля и свойства ]
		private User? _user;
		private Order? _order;

		[ObservableProperty]
		private string _name = "Гость";

		[ObservableProperty]
		private List<Product> _products = new();

		[ObservableProperty]
		private List<Product> _productsPreview = new();

		[ObservableProperty]
		private bool _isVisibleAdmin = false;
		[ObservableProperty]

		private bool _isVisibleManager = false;

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
			if (_user != null)
			{
				Name = _user.Surname + " " + _user.Name + " " + _user.Patronymic;
				Role role = MainWindowViewModel.DB.Roles.First(it => it.Id == user!.RoleId);
				if (role.Name == "Менеджер") IsVisibleManager = true;
				else if (role.Name == "Администратор")
				{
					IsVisibleManager = true;
					IsVisibleAdmin = true;
				}
			}
			Update();
			MessageCountProduct = $"{ProductsPreview.Count} из {ProductsPreview.Count}";
		}
		private void Update()
		{
			Products = MainWindowViewModel.DB.Products
				.Include(it => it.IdCategoryNavigation)
				.Include(it => it.IdManufacturerNavigation)
				.Include(it => it.IdSupplierNavigation)
				.Include(it => it.IdUnitNavigation)
				.ToList();
			ProductsPreview = Products;
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
		private void Filter()
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
		
		[RelayCommand]
		public void AddProductToOrder()
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
					ProductArticleNumberNavigation =
					 MainWindowViewModel.DB.Products
					 .Include(it => it.IdCategoryNavigation)
					 .Include(it => it.IdManufacturerNavigation)
					 .Include(it => it.IdSupplierNavigation)
					 .Include(it => it.IdUnitNavigation)
					 .First(it => it.ArticleNumber == SelectedProduct.ArticleNumber),
				};
				OrdersProducts.Add(ordersProduct);
			}
			if (OrdersProducts.Count() > 0) IsVisibleButton = true;
		}

		[RelayCommand]
		public async Task DeleteProduct(Product product)
		{
			try
			{
				ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Удаление", "Вы уверены, что хотите удалить товар?", ButtonEnum.YesNo).ShowAsync();
				if (result == ButtonResult.Yes)
				{
					MainWindowViewModel.DB.Products.Remove(product);
					MainWindowViewModel.DB.SaveChanges();
					Update();
				}
			}
			catch
			{
				await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Не удалось удалить товар", ButtonEnum.Ok).ShowAsync();
			}
		}

		[RelayCommand]
		public void EditProduct(Product product) => MainWindowViewModel.Instance!.UserControl = new EditProductView(_user, product.ArticleNumber);

		[RelayCommand]
		public void AddProduct() => MainWindowViewModel.Instance!.UserControl = new EditProductView(_user);

		[RelayCommand]
		public void ViewOrder() => MainWindowViewModel.Instance!.UserControl = new OrderView(_user, _ordersProducts);

		[RelayCommand]
		public void ViewOrders() => MainWindowViewModel.Instance!.UserControl = new ListOrdersView(_user);

		[RelayCommand]
		public void GoAuth() => MainWindowViewModel.Instance!.UserControl = new AuthView();
	}
}