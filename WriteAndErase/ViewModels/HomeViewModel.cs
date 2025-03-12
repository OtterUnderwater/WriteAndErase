using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using WriteAndErase.Models;
using WriteAndErase.Views;

namespace WriteAndErase.ViewModels
{
	public partial class HomeViewModel : ViewModelBase
	{
		public UserControl UCChildren => MainWindowViewModel.Instance!.UcChildren;
		
		[ObservableProperty]
		private string _name = "�����";

		public HomeViewModel(User? user)
		{
			MainWindowViewModel.Instance!.UcChildren = new ListProductsView(user);
			if (user != null) Name = user.Surname + " " + user.Name + " " + user.Patronymic;
		}

		[RelayCommand]
		public void GoAuth() => MainWindowViewModel.Instance!.UserControl = new AuthView();

		//ListProductsView
		//���������������� � �������������� ������ �����
		//������������� ������, ������������ ����� � ������� ������� ��� ���� ����� ������.
		//�������� ����� ������������� ������, ����������� � ������������� ������;
		//������������� ����� ���������/�������������/������� ������, ������������� � ������������� ������.
	}
}