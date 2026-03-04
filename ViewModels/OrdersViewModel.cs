using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AvaloniaApplication6.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using AvaloniaApplication6.Views;

namespace AvaloniaApplication6.ViewModels;

public partial class OrdersViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindow;

    [ObservableProperty]
    private Employee? _currentEmployee;

    [ObservableProperty]
    private Bitmap? _employeePhoto;

    [ObservableProperty]
    private ObservableCollection<Order> _orders = new();

    [ObservableProperty]
    private string _searchText = "";

    partial void OnSearchTextChanged(string value) => ApplyFilters();

    [ObservableProperty]
    private string _selectedSort = "Без сортировки";

    partial void OnSelectedSortChanged(string value) => ApplyFilters();

    public List<string> SortOptions { get; } = new() { "Без сортировки", "По дате (возр)", "По дате (убыв)" };

    [ObservableProperty]
    private Orderstatus? _selectedStatus;

    partial void OnSelectedStatusChanged(Orderstatus? value) => ApplyFilters();

    [ObservableProperty]
    private ObservableCollection<Orderstatus> _statuses = new();

    public OrdersViewModel(MainWindowViewModel mainWindow, Employee? employee = null)
    {
        _mainWindow = mainWindow;
        CurrentEmployee = employee;
        SetEmployeePhoto();
        LoadDataAsync();
    }

    private void SetEmployeePhoto()
    {
        // Получаем имя текущей сборки динамически
        string? assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        // Загружаем логотип по умолчанию
        try
        {
            EmployeePhoto = new Bitmap(AssetLoader.Open(new Uri($"avares://{assemblyName}/Assets/logo.png")));
        }
        catch { }

        if (CurrentEmployee == null) return;

        // Извлекаем фамилию (первое слово из Fio)
        string lastName = CurrentEmployee.Fio.Split(' ')[0];
        
        // Список возможных расширений в Assets
        string[] extensions = { ".jpeg", ".jpg", ".png" };
        
        foreach (var ext in extensions)
        {
            try
            {
                var uri = new Uri($"avares://{assemblyName}/Assets/{lastName}{ext}");
                if (AssetLoader.Exists(uri))
                {
                    EmployeePhoto = new Bitmap(AssetLoader.Open(uri));
                    return;
                }
            }
            catch { }
        }
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        SearchText = "";
        SelectedSort = "Без сортировки";
        
        using var db = new ApplicationDbContext();
        var ordersList = await db.Orders
            .Include(o => o.CodeclientNavigation)
            .Include(o => o.Status)
            .Include(o => o.Employee)
            .Include(o => o.Services)
            .ToListAsync();

        var statusesList = await db.Orderstatuses.ToListAsync();
        statusesList.Insert(0, new Orderstatus { Id = 0, Name = "Все статусы" });

        Orders = new ObservableCollection<Order>(ordersList);
        Statuses = new ObservableCollection<Orderstatus>(statusesList);
        SelectedStatus = Statuses[0];
    }

    [RelayCommand]
    private void Logout()
    {
        _mainWindow.NavigateToLogin();
    }

    [RelayCommand]
    private void Search()
    {
        ApplyFilters();
    }

    private async void ApplyFilters()
    {
        using var db = new ApplicationDbContext();
        var query = db.Orders
            .Include(o => o.CodeclientNavigation)
            .Include(o => o.Status)
            .Include(o => o.Employee)
            .Include(o => o.Services)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            query = query.Where(o => o.Code.Contains(SearchText) || 
                                    o.CodeclientNavigation.Family.Contains(SearchText) || 
                                    o.CodeclientNavigation.Name.Contains(SearchText) || 
                                    o.CodeclientNavigation.Patronymic.Contains(SearchText));
        }

        if (SelectedStatus != null && SelectedStatus.Id != 0)
        {
            query = query.Where(o => o.Statusid == SelectedStatus.Id);
        }

        if (SelectedSort == "По дате (возр)")
        {
            query = query.OrderBy(o => o.Datecreate).ThenBy(o => o.Timeorder);
        }
        else if (SelectedSort == "По дате (убыв)")
        {
            query = query.OrderByDescending(o => o.Datecreate).ThenByDescending(o => o.Timeorder);
        }

        var list = await query.ToListAsync();
        Orders = new ObservableCollection<Order>(list);
    }

    [RelayCommand]
    private async Task DeleteOrder(Order order)
    {
        if (order == null) return;

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var result = await MessageBox.Show(desktop.MainWindow, 
                $"Вы действительно хотите удалить заказ №{order.Code}?", 
                "Подтверждение удаления", 
                true);

            if (result == MessageBox.MessageBoxResult.Yes)
            {
                using var db = new ApplicationDbContext();
                db.Orders.Remove(order);
                await db.SaveChangesAsync();
                await LoadDataAsync();
            }
        }
    }

    [RelayCommand]
    private void AddOrder()
    {
        _mainWindow.NavigateToOrderEdit();
    }

    [RelayCommand]
    private void EditOrder(Order order)
    {
        if (order == null) return;
        _mainWindow.NavigateToOrderEdit(order);
    }
}
