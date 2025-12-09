namespace SalaryForecast.Desktop.Views.SalarySettingsView
{
    /// <summary>
    /// Interaction logic for SalarySettingsView.xaml
    /// </summary>
    public partial class SalarySettingsView
    {
        public SalarySettingsView()
        {
            InitializeComponent();
            this.Closing += OnClosing;
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RecurringPaymentsGrid.CommitEdit();
            RecurringPaymentsGrid.CommitEdit(System.Windows.Controls.DataGridEditingUnit.Row, true);
        }
    }
}
