using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using SalaryForecast.Core.Models;
using SalaryForecast.Core.ViewModels.StartViewModel;

namespace SalaryForecast.Desktop.Views
{
    public partial class MainWindow : Window
    {
        private static readonly IBrush LightDefaultRowBackground = Brushes.White;
        private static readonly IBrush LightAlternateRowBackground = new SolidColorBrush(Color.FromRgb(200, 232, 255));
        private static readonly IBrush LightInactiveRowBackground = CreateHorizontalGradient(Colors.LightGray, Colors.Gray);
        private static readonly IBrush LightNextSalaryRowBackground = CreateHorizontalGradient(Colors.Lime, Colors.White);
        private static readonly IBrush DarkDefaultRowBackground = new SolidColorBrush(Color.FromRgb(28, 28, 28));
        private static readonly IBrush DarkAlternateRowBackground = new SolidColorBrush(Color.FromRgb(34, 47, 63));
        private static readonly IBrush DarkInactiveRowBackground = CreateHorizontalGradient(
            Color.FromRgb(58, 58, 58),
            Color.FromRgb(88, 88, 88));
        private static readonly IBrush DarkNextSalaryRowBackground = CreateHorizontalGradient(
            Color.FromRgb(97, 157, 57),
            Color.FromRgb(28, 28, 28));
        private bool _isInitialized;

        public MainWindow()
        {
            InitializeComponent();
            Opened += OnOpened;
            ActualThemeVariantChanged += OnActualThemeVariantChanged;
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

        private void OnActualThemeVariantChanged(object? sender, EventArgs e)
        {
            RefreshRowBackgrounds();
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
            var isDarkTheme = row.ActualThemeVariant == ThemeVariant.Dark;

            if (withHighlighting && row.DataContext is Salary salary)
            {
                if (!salary.IsActive)
                {
                    row.Background = isDarkTheme ? DarkInactiveRowBackground : LightInactiveRowBackground;
                    return;
                }

                if (salary.IsNextSalary)
                {
                    row.Background = isDarkTheme ? DarkNextSalaryRowBackground : LightNextSalaryRowBackground;
                    return;
                }
            }

            var groupIndex = Math.Max(row.Index, 0) / 2;
            row.Background = isDarkTheme
                ? groupIndex % 2 == 0 ? DarkDefaultRowBackground : DarkAlternateRowBackground
                : groupIndex % 2 == 0 ? LightDefaultRowBackground : LightAlternateRowBackground;
        }

        private void RefreshRowBackgrounds()
        {
            RefreshGridRows(PastSalaryGrid, withHighlighting: false);
            RefreshGridRows(CurrentSalaryGrid, withHighlighting: true);
        }

        private static void RefreshGridRows(DataGrid dataGrid, bool withHighlighting)
        {
            foreach (var row in dataGrid.GetVisualDescendants().OfType<DataGridRow>())
            {
                ApplyRowBackground(row, withHighlighting);
            }
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
