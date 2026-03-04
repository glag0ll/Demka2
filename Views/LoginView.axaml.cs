using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace AvaloniaApplication6.Views;

public class PasswordCharConverter : IValueConverter
{
    public static readonly PasswordCharConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isHidden && isHidden)
        {
            return '*';
        }
        return '\0'; // No character for visible text
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class PasswordVisibilityIconConverter : IValueConverter
{
    public static readonly PasswordVisibilityIconConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isVisible && isVisible)
        {
            return "👁"; // Open eye 
        }
        return "👁‍🗨"; // Eye with stroke (approximate for symbol font)
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }
}
