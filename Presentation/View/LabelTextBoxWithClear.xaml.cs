using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_MVVM_TEMPLATE.Presentation.View;

public partial class LabelTextBoxWithClear : UserControl
{
    public string LabelContent
    {
        get { return (string)GetValue(LabelContentProperty); }
        set { SetValue(LabelContentProperty, value); }
    }

    public static readonly DependencyProperty LabelContentProperty =
        DependencyProperty.Register(nameof(LabelContent), typeof(string), typeof(LabelTextBoxWithClear), new PropertyMetadata(string.Empty));

    public string TextBoxContent
    {
        get { return (string)GetValue(TextBoxContentProperty); }
        set
        {
            SetValue(TextBoxContentProperty, value);
            Console.WriteLine($"TextBoxContent Updated: {value}");
        }
    }

    public static readonly DependencyProperty TextBoxContentProperty =
        DependencyProperty.Register(nameof(TextBoxContent), typeof(string), typeof(LabelTextBoxWithClear), new PropertyMetadata(string.Empty));

    //public ICommand ClearCommand { get; set; }

    public LabelTextBoxWithClear()
    {
        InitializeComponent();

        // Initialize the ClearCommand for the button
        /*
        ClearCommand = new CommandBase(
            executeAction: _ => TextBoxContent = "",
            canExecuteAction: _ => !string.IsNullOrEmpty(TextBoxContent));*/
    }
}