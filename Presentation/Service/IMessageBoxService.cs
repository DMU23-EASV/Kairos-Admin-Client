using System.Windows;

namespace WPF_MVVM_TEMPLATE.Presentation.Service;

public interface IMessageBoxService {
    
    public void ShowMessageInfo(string message, string caption, MessageBoxButton button, MessageBoxImage icon);

    public MessageBoxResult ShowMessageConfirm(string message, string caption, MessageBoxButton button,
         MessageBoxImage icon);
    
}