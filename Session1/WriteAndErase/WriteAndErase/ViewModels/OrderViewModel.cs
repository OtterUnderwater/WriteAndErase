using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WriteAndErase.Models;
using WriteAndErase.Views;

namespace WriteAndErase.ViewModels
{	public partial class OrderViewModel : ViewModelBase
	{
		#region [ Поля и свойства ]
		User? _currentUser;

		[ObservableProperty]
		private double _fullCost = 0;

		[ObservableProperty]
		private double _fullDiscount = 0;

		[ObservableProperty]
		private string _name = "Гость";

		[ObservableProperty]
		Order _order;

		[ObservableProperty]
		List<OrdersProduct>? _ordersProducts;

		[ObservableProperty]
		List<PickUpPoint> _pickUpPoint;
		#endregion

		public OrderViewModel(User? user, List<OrdersProduct> ordersProducts)
		{
			_currentUser = user;
			if (user != null) Name = user.Surname + " " + user.Name + " " + user.Patronymic;
			OrdersProducts = ordersProducts;
			PickUpPoint = MainWindowViewModel.DB.PickUpPoints.ToList();
			Order = MainWindowViewModel.DB.Orders.Include(it => it.IdPickUpPointNavigation)
				.First(it => it.Id == ordersProducts[0].OrderId);
			UpdateCost();
		}

		private void UpdateCost()
		{
			FullCost = 0;
			FullDiscount = 0;
			double cost = 0;
			if (OrdersProducts != null)
			{
				OrdersProducts.ForEach(it =>
				{
					double costProd = it.ProductArticleNumberNavigation.Cost;
					double currentDiscount = (double) it.ProductArticleNumberNavigation.CurrentDiscount;
					cost += (it.Count * costProd);
					double costOneProd = (costProd / 100) * (100 - currentDiscount);
					FullCost += it.Count * costOneProd;
				});
				if (cost > 0) FullDiscount = (1.0 - (FullCost / cost)) * 100;
			}
		}
		
		[RelayCommand]
		public void AddUnit(OrdersProduct product)
		{
			foreach (OrdersProduct item in OrdersProducts)
				if (item.ProductArticleNumber == product.ProductArticleNumber)
					item.Count++;
			OrdersProducts = new List<OrdersProduct>(OrdersProducts);
			UpdateCost();
		}
		
		[RelayCommand]
		public void DeleteUnit(OrdersProduct product)
		{
			if (product.Count == 1)
			{
				if (OrdersProducts.Count == 1) OrdersProducts = null;
				else
				{
					OrdersProducts.Remove(product);
					OrdersProducts = new List<OrdersProduct>(OrdersProducts);
				}
			}
			else
			{
				foreach (OrdersProduct item in OrdersProducts)
					if (item.ProductArticleNumber == product.ProductArticleNumber)
						item.Count--;
				OrdersProducts = new List<OrdersProduct>(OrdersProducts);
			}
			UpdateCost();
		}

		/// <summary>
		/// Создание заказа
		/// </summary>
		[RelayCommand]
		public void SaveOrder()
		{
			try
			{
				int count = 0;
				foreach (var item in OrdersProducts)
				{
					if (item.ProductArticleNumberNavigation.QuantityInStock > 3) count++;
				}
				DateOnly dateOrder = new DateOnly(Order.DateOrder!.Value.Year,
					Order.DateOrder!.Value.Month, Order.DateOrder!.Value.Day);
				//срок доставки – 3 дня
				if (OrdersProducts.Count == count) Order.DateDelivery = dateOrder.AddDays(3);
				//иначе 6 дней
				else Order.DateDelivery = dateOrder.AddDays(6);
				Random rnd = new Random();
				//Код генерируется случайным образом (100-999)
				Order.Code = rnd.Next(100, 1000);
				MainWindowViewModel.DB.Orders.Update(Order);
				MainWindowViewModel.DB.OrdersProducts.AddRange(OrdersProducts);
				MainWindowViewModel.DB.SaveChanges();
				Exit();
			}
			catch
			{
				Exit();
			}
		}

		[RelayCommand]
		public void Exit() => MainWindowViewModel.Instance!.UserControl = new ListProductsView(_currentUser);	
	}
}