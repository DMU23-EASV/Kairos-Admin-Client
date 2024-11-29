﻿using System.Windows;
using System.Windows.Controls;
using WPF_MVVM_TEMPLATE.Presentation.ViewModel;

namespace WPF_MVVM_TEMPLATE.Presentation.View;

public partial class NavigationDrawer : UserControl
{
    public NavigationDrawer()
    {
        InitializeComponent();
    }

    private void SettingsNav_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<SettingsViewModel>();
    }

    private void LoginNav_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<LoginViewModel>();
    }

    private void HomeNav_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<HomeViewModel>();
    }
}