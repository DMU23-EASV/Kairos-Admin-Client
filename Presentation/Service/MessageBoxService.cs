using System.Windows;


namespace WPF_MVVM_TEMPLATE.Presentation.Service;

public class MessageBoxService : IMessageBoxService
{
    // Static field to hold the singleton instance of the MessageBoxService
    private static MessageBoxService? _instance;
    
    // Private constructor to prevent instantiation from outside the class
    private MessageBoxService(){}

    
    /// <summary>
    /// Property to access the singleton instance of the ViewModelController.
    /// If the instance is null, it initializes a new instance.
    /// </summary>
    public static MessageBoxService Instance
    {
        get
        {
            if (_instance == null )
            {
                _instance = new MessageBoxService();
            }
            
            return _instance;
        }
    }
    
    
    /// <summary>
    /// Displays a message box just to inform user
    /// </summary>
    /// <param name="message"> the message to be displayed </param>
    /// <param name="caption"> the name of caption </param>
    /// <param name="button"> the type of button(s) (e.g. OK, YesNo) </param>
    /// <param name="icon"> the type of icon to be displayed (e.g. Information, Warning) </param>
    public void ShowMessageInfo(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        MessageBox.Show(message, caption, button, icon);
    }


    /// <summary>
    /// Displays a message box that requires a confirmation from user
    /// </summary>
    /// <param name="message"> the message/question to be displayed </param>
    /// <param name="caption"> the name of caption </param>
    /// <param name="button"> the type of button(s) (e.g. OK, YesNo) </param>
    /// <param name="icon"> the type of icon to be displayed (e.g. Information, Warning) </param>
    /// <returns> the users confirm result </returns>
    public MessageBoxResult ShowMessageConfirm(string message, string caption, MessageBoxButton button,
        MessageBoxImage icon)
    {
        return MessageBox.Show(message, caption, button, icon);
    }
    
    
}