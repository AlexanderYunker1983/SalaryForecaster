using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Desktop.Views.StartView
{
    /// <summary>
    /// Interaction logic for SalaryForecasterStartView.xaml
    /// </summary>
    public partial class SalaryForecasterStartView
    {
        public SalaryForecasterStartView()
        {
            InitializeComponent();
        }
        
        private void DataGridLoadingRow(object sender, DataGridRowEventArgs e)
        {
            var ee = (DataGrid)sender;
            if (ee?.ItemContainerGenerator == null) return;

            FillBackgrounds(ee, true);
        }

        private void FillBackgrounds(DataGrid ee, bool withHighlighting)
        {
            var counter = -1;
            for (var index = 0; index < ee.Items.Count; index++)
            {
                if ((index + 1) % 2 != 0) counter++;
                try
                {
                    var row = (DataGridRow)ee.ItemContainerGenerator.ContainerFromIndex(index);
                    if (row != null)
                    {
                        if (withHighlighting)
                        {
                            var a = row.DataContext;
                            if (a is Salary salary)
                            {
                                var colorChanged = false;
                                if (salary.IsNextSalary)
                                {
                                    row.Background = new LinearGradientBrush(Colors.Lime, Colors.White,
                                        new Point(0, 0.5), new Point(1.0, 0.5));
                                    colorChanged = true;
                                }

                                if (salary.WarningEnabled)
                                {
                                    if (!colorChanged)
                                    {
                                        row.Background = new SolidColorBrush(Colors.Red);
                                        colorChanged = true;
                                    }
                                    else
                                    {
                                        row.Background = new LinearGradientBrush(Colors.Lime, Colors.Red,
                                            new Point(0, 0.5), new Point(1.0, 0.5));
                                    }
                                }

                                if (colorChanged) continue;
                            }
                        }

                        row.Background = counter % 2 == 0
                            ? new SolidColorBrush(Colors.White)
                            : new SolidColorBrush(Color.FromArgb(255, 200, 232, 255));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void DataGridLoadingRowWithoutHighlighting(object sender, DataGridRowEventArgs e)
        {
            var ee = (DataGrid)sender;
            if (ee?.ItemContainerGenerator == null) return;

            FillBackgrounds(ee, false);
        }
    }
}
