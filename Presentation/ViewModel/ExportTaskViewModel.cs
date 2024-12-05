using System.Collections.ObjectModel;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.Entitys.Enum;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class ExportTaskViewModel : ViewModelBase
{
    public ObservableCollection<SimpleUserDTO> Users { get; set; }
    public ObservableCollection<SimpleUserDTO> SelectedUsers { get; set; }
    public bool AnonymizeUsername { get; set; }
    public bool AnonymizeEmail { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public EFileTypes FileType { get; set; }
    public ETaskModelStatus? TaskStatus { get; set; }
    
    
    
}