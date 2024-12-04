using System.Collections.ObjectModel;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class ExportTaskViewModel : ViewModelBase
{
    public ObservableCollection<SimpleUserDTO> Users { get; set; }
    public ObservableCollection<SimpleUserDTO> SelectedUsers { get; set; }
    public bool AnonymizeUsername, AnonymizeEmail;
}