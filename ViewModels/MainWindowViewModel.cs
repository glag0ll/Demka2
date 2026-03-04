using CommunityToolkit.Mvvm.ComponentModel;
using AvaloniaApplication6.Models;

namespace AvaloniaApplication6.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _currentPage;

        [ObservableProperty]
        private Employee? _currentEmployee;

        public MainWindowViewModel()
        {
            _currentPage = new LoginViewModel(this);
        }

        public void NavigateToOrders(Employee? employee = null)
        {
            if (employee != null)
            {
                CurrentEmployee = employee;
            }
            
            if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow.CanResize = true;
                desktop.MainWindow.SizeToContent = Avalonia.Controls.SizeToContent.Manual;
                desktop.MainWindow.Width = 1200;
                desktop.MainWindow.Height = 800;
                desktop.MainWindow.WindowState = Avalonia.Controls.WindowState.Normal;
            }
            CurrentPage = new OrdersViewModel(this, CurrentEmployee);
        }

        public void NavigateToOrderEdit(Order? order = null)
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow.Width = 600;
                desktop.MainWindow.Height = 700;
                desktop.MainWindow.CanResize = true;
            }
            CurrentPage = new OrderEditViewModel(this, order);
        }

        public void NavigateToLogin()
        {
            CurrentEmployee = null;
            if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow.CanResize = false;
                desktop.MainWindow.SizeToContent = Avalonia.Controls.SizeToContent.WidthAndHeight;
            }
            CurrentPage = new LoginViewModel(this);
        }
    }
}
