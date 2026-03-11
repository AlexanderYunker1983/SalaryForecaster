using System;
using Avalonia.Controls;
using SalaryForecast.Core.ViewModels.StartViewModel;

namespace SalaryForecast.Desktop.Views
{
    public partial class MainWindow : Window
    {
        private bool _isInitialized;

        public MainWindow()
        {
            InitializeComponent();
            Opened += OnOpened;
        }

        private async void OnOpened(object? sender, EventArgs e)
        {
            if (_isInitialized) return;
            _isInitialized = true;

            if (DataContext is SalaryForecasterStartViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        }
    }
}
