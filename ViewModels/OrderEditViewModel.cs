using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AvaloniaApplication6.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace AvaloniaApplication6.ViewModels;

public partial class OrderEditViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindow;
    private readonly Order? _order;

    [ObservableProperty]
    private string _code = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DateCreateDateTime))]
    private DateOnly _dateCreate = DateOnly.FromDateTime(DateTime.Now);

    public DateTime? DateCreateDateTime
    {
        get => _dateCreate.ToDateTime(TimeOnly.MinValue);
        set
        {
            if (value.HasValue)
            {
                DateCreate = DateOnly.FromDateTime(value.Value);
            }
        }
    }

    [ObservableProperty]
    private TimeOnly _timeOrder = TimeOnly.FromDateTime(DateTime.Now);

    [ObservableProperty]
    private Client? _selectedClient;

    [ObservableProperty]
    private Orderstatus? _selectedStatus;

    [ObservableProperty]
    private string _rentalTime = "";

    [ObservableProperty]
    private string _employeeId = "";

    [ObservableProperty]
    private ObservableCollection<Service> _allServices = new();

    [ObservableProperty]
    private ObservableCollection<Service> _selectedServices = new();

    [ObservableProperty]
    private ObservableCollection<Client> _clients = new();

    [ObservableProperty]
    private ObservableCollection<Orderstatus> _statuses = new();

    [ObservableProperty]
    private string _title = "Новый заказ";

    public OrderEditViewModel(MainWindowViewModel mainWindow, Order? order = null)
    {
        _mainWindow = mainWindow;
        _order = order;
        
        if (_order != null)
        {
            Title = "Редактирование заказа";
            Code = _order.Code;
            DateCreate = _order.Datecreate;
            TimeOrder = _order.Timeorder;
            RentalTime = _order.Rentaltime;
            EmployeeId = _order.Employeeid;
        }

        LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        using var db = new ApplicationDbContext();
        var clientsList = await db.Clients.ToListAsync();
        var statusesList = await db.Orderstatuses.ToListAsync();
        var servicesList = await db.Services.ToListAsync();

        Clients = new ObservableCollection<Client>(clientsList);
        Statuses = new ObservableCollection<Orderstatus>(statusesList);
        AllServices = new ObservableCollection<Service>(servicesList);

        if (_order != null)
        {
            var existingOrder = await db.Orders
                .Include(o => o.Services)
                .FirstOrDefaultAsync(o => o.Id == _order.Id);

            if (existingOrder != null)
            {
                SelectedClient = Clients.FirstOrDefault(c => c.Code == existingOrder.Codeclient);
                SelectedStatus = Statuses.FirstOrDefault(s => s.Id == existingOrder.Statusid);
                SelectedServices = new ObservableCollection<Service>(existingOrder.Services);
            }
        }
        else
        {
            SelectedStatus = Statuses.FirstOrDefault();
            var lastOrder = await db.Orders.OrderByDescending(o => o.Id).FirstOrDefaultAsync();
            if (lastOrder != null)
            {
                Code = (lastOrder.Id + 1).ToString();
            }
            else
            {
                Code = "1";
            }
            
            var firstEmployee = await db.Employees.FirstOrDefaultAsync();
            if (firstEmployee != null)
            {
                EmployeeId = firstEmployee.Id;
            }
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedClient == null || SelectedStatus == null || string.IsNullOrWhiteSpace(Code))
        {
            return;
        }

        using var db = new ApplicationDbContext();
        if (_order == null)
        {
            var newOrder = new Order
            {
                Code = Code,
                Datecreate = DateCreate,
                Timeorder = TimeOrder,
                Codeclient = SelectedClient.Code,
                Statusid = SelectedStatus.Id,
                Rentaltime = RentalTime,
                Employeeid = EmployeeId
            };
            
            foreach (var service in SelectedServices)
            {
                db.Entry(service).State = EntityState.Unchanged;
                newOrder.Services.Add(service);
            }

            db.Orders.Add(newOrder);
        }
        else
        {
            var existingOrder = await db.Orders
                .Include(o => o.Services)
                .FirstOrDefaultAsync(o => o.Id == _order.Id);

            if (existingOrder != null)
            {
                existingOrder.Code = Code;
                existingOrder.Datecreate = DateCreate;
                existingOrder.Timeorder = TimeOrder;
                existingOrder.Codeclient = SelectedClient.Code;
                existingOrder.Statusid = SelectedStatus.Id;
                existingOrder.Rentaltime = RentalTime;
                existingOrder.Employeeid = EmployeeId;

                existingOrder.Services.Clear();
                foreach (var service in SelectedServices)
                {
                    db.Entry(service).State = EntityState.Unchanged;
                    existingOrder.Services.Add(service);
                }
            }
        }

        await db.SaveChangesAsync();
        _mainWindow.NavigateToOrders();
    }

    [RelayCommand]
    private void Cancel()
    {
        _mainWindow.NavigateToOrders();
    }
}
