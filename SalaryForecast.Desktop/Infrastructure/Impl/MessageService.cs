using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class MessageService : IMessageService
    {
        public async Task ShowErrorAsync(string title, string message)
        {
            var dialog = CreateDialog(title, message);

            var okButton = new Button
            {
                Content = "OK",
                HorizontalAlignment = HorizontalAlignment.Right,
                MinWidth = 90
            };
            okButton.Click += (_, _) => dialog.Close();

            dialog.Content = new StackPanel
            {
                Margin = new Thickness(16),
                Spacing = 16,
                Children =
                {
                    new TextBlock
                    {
                        Text = message,
                        TextWrapping = TextWrapping.Wrap
                    },
                    okButton
                }
            };

            await ShowDialogAsync(dialog);
        }

        public async Task<bool> ShowConfirmationAsync(string title, string message)
        {
            var dialog = CreateDialog(title, message);

            var yesButton = new Button
            {
                Content = "Да",
                MinWidth = 90
            };
            yesButton.Click += (_, _) => dialog.Close(true);

            var noButton = new Button
            {
                Content = "Нет",
                MinWidth = 90
            };
            noButton.Click += (_, _) => dialog.Close(false);

            dialog.Content = new StackPanel
            {
                Margin = new Thickness(16),
                Spacing = 16,
                Children =
                {
                    CreateMessageTextBlock(message),
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Spacing = 8,
                        Children =
                        {
                            yesButton,
                            noButton
                        }
                    }
                }
            };

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
                desktop.MainWindow != null)
            {
                return await dialog.ShowDialog<bool>(desktop.MainWindow);
            }

            dialog.Show();
            return false;
        }

        private static TextBlock CreateMessageTextBlock(string message)
        {
            return new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap
            };
        }

        private static Window CreateDialog(string title, string message)
        {
            return new Window
            {
                Title = title,
                Width = 420,
                CanResize = false,
                SizeToContent = SizeToContent.Height,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = new StackPanel
                {
                    Margin = new Thickness(16),
                    Spacing = 16,
                    Children =
                    {
                        CreateMessageTextBlock(message)
                    }
                }
            };
        }

        private static async Task ShowDialogAsync(Window dialog)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
                desktop.MainWindow != null)
            {
                await dialog.ShowDialog(desktop.MainWindow);
                return;
            }

            dialog.Show();
        }
    }
}
