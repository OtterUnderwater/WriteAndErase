using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WriteAndErase.Models;
using WriteAndErase.Views;

namespace WriteAndErase.ViewModels
{
	public partial class EditProductViewModel : ViewModelBase
	{
		private User? _currentUser;

		[ObservableProperty]
		private bool _isNewProduct = true;

		[ObservableProperty]
		private string _title = "Добавление товара";

		[ObservableProperty]
		private Product _product = new();

		[ObservableProperty]
		private List<Category> _categories = new();

		[ObservableProperty]
		private List<Manufacturer> _manufacturers = new();

		[ObservableProperty]
		private List<Supplier> _suppliers = new();

		[ObservableProperty]
		private List<Unit> _units = new();
		private void Launch()
		{
			Categories = MainWindowViewModel.DB.Categories.ToList();
			Manufacturers = MainWindowViewModel.DB.Manufacturers.ToList();
			Suppliers = MainWindowViewModel.DB.Suppliers.ToList();
			Units = MainWindowViewModel.DB.Units.ToList();
		}
		public EditProductViewModel(User? user)
		{
			_currentUser = user;
			Launch();
		}
		public EditProductViewModel(User? user, string article)
		{
			Title = "Редактирование товара";
			IsNewProduct = false;
			_currentUser = user;
			Product = MainWindowViewModel.DB.Products
				.Include(it => it.IdCategoryNavigation)
				.Include(it => it.IdManufacturerNavigation)
				.Include(it => it.IdSupplierNavigation)
				.Include(it => it.IdUnitNavigation)
				.First(it => it.ArticleNumber == article);
			Launch();
		}

		[RelayCommand]
		public async Task Save()
		{
			try
			{
				if (IsNewProduct)
				{
					MainWindowViewModel.DB.Products.Add(Product);
					MainWindowViewModel.DB.SaveChanges();
					await MessageBoxManager.GetMessageBoxStandard("Добавление", "Товар успешно добавлен", ButtonEnum.Ok).ShowAsync();
				}
				else
				{
					MainWindowViewModel.DB.Products.Update(Product);
					MainWindowViewModel.DB.SaveChanges();
					await MessageBoxManager.GetMessageBoxStandard("Редактирование", "Данные о товаре изменены", ButtonEnum.Ok).ShowAsync();
				}
				Exit();
			}
			catch 
			{
				await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Проверьте правильность заполнения полей и попробуйте еще раз", ButtonEnum.Ok).ShowAsync();
			}
		}

		[RelayCommand]
		public void Exit() => MainWindowViewModel.Instance!.UserControl = new ListProductsView(_currentUser);
	}
}
