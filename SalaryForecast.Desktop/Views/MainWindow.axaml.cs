using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using SalaryForecast.Core.Models;
using SalaryForecast.Core.ViewModels.StartViewModel;

namespace SalaryForecast.Desktop.Views
{
    public partial class MainWindow : Window
    {
        private static readonly IBrush DefaultRowBackground = Brushes.White;
        private static readonly IBrush AlternateRowBackground = new SolidColorBrush(Color.FromRgb(200, 232, 255));
        private static readonly IBrush InactiveRowBackground = CreateHorizontalGradient(Colors.LightGray, Colors.Gray);
        private static readonly IBrush NextSalaryRowBackground = CreateHorizontalGradient(Colors.Lime, Colors.White);
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
                ApplyColumnHeaders(viewModel);
                await viewModel.InitializeAsync();
            }
        }

        private void PastSalaryDataGridLoadingRow(object? sender, DataGridRowEventArgs e)
        {
            ApplyRowBackground(e.Row, withHighlighting: false);
        }

        private void CurrentSalaryDataGridLoadingRow(object? sender, DataGridRowEventArgs e)
        {
            ApplyRowBackground(e.Row, withHighlighting: true);
        }

        private void ApplyColumnHeaders(SalaryForecasterStartViewModel viewModel)
        {
            SetColumnHeader(PastSalaryGrid, 0, viewModel.DateColumnTitle);
            SetColumnHeader(PastSalaryGrid, 1, viewModel.SalaryPartValueColumnTitle);
            SetColumnHeader(PastSalaryGrid, 2, viewModel.SalaryPartPercentColumnTitle);

            SetColumnHeader(CurrentSalaryGrid, 0, viewModel.DateColumnTitle);
            SetColumnHeader(CurrentSalaryGrid, 1, viewModel.SalaryPartValueColumnTitle);
            SetColumnHeader(CurrentSalaryGrid, 2, viewModel.SalaryPartPercentColumnTitle);
            SetColumnHeader(CurrentSalaryGrid, 3, viewModel.OneDayCostColumnTitle);
            SetColumnHeader(CurrentSalaryGrid, 4, viewModel.OneHolidayCostColumnTitle);
        }

        private static void SetColumnHeader(DataGrid dataGrid, int index, string title)
        {
            if (index >= 0 && index < dataGrid.Columns.Count)
            {
                dataGrid.Columns[index].Header = title;
            }
        }

        private static void ApplyRowBackground(DataGridRow row, bool withHighlighting)
        {
            if (withHighlighting && row.DataContext is Salary salary)
            {
                if (!salary.IsActive)
                {
                    row.Background = InactiveRowBackground;
                    return;
                }

                if (salary.IsNextSalary)
                {
                    row.Background = NextSalaryRowBackground;
                    return;
                }
            }

            var groupIndex = Math.Max(row.Index, 0) / 2;
            row.Background = groupIndex % 2 == 0 ? DefaultRowBackground : AlternateRowBackground;
        }

        private static IBrush CreateHorizontalGradient(Color startColor, Color endColor)
        {
            return new LinearGradientBrush
            {
                StartPoint = new RelativePoint(0, 0.5, RelativeUnit.Relative),
                EndPoint = new RelativePoint(1, 0.5, RelativeUnit.Relative),
                GradientStops = new GradientStops
                {
                    new GradientStop(startColor, 0),
                    new GradientStop(endColor, 1)
                }
            };
        }
    }
}
