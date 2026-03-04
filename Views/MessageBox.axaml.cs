using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Threading.Tasks;

namespace AvaloniaApplication6.Views;

public partial class MessageBox : Window
{
    public enum MessageBoxResult
    {
        Ok,
        Yes,
        No
    }

    private MessageBoxResult _result = MessageBoxResult.Ok;

    public MessageBox()
    {
        InitializeComponent();
    }

    public static async Task<MessageBoxResult> Show(Window owner, string message, string title = "Сообщение", bool showYesNo = false)
    {
        var mb = new MessageBox();
        mb.Title = title;
        mb.FindControl<TextBlock>("MessageTextBlock").Text = message;

        if (showYesNo)
        {
            mb.FindControl<Button>("OkButton").IsVisible = false;
            mb.FindControl<Button>("YesButton").IsVisible = true;
            mb.FindControl<Button>("NoButton").IsVisible = true;
        }

        await mb.ShowDialog(owner);
        return mb._result;
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        var yesButton = this.FindControl<Button>("YesButton");
        _result = (yesButton != null && yesButton.IsVisible) ? MessageBoxResult.Yes : MessageBoxResult.Ok;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        _result = MessageBoxResult.No;
        Close();
    }
}
