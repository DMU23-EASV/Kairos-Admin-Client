using WPF_MVVM_TEMPLATE.Entitys.Enum;

namespace WPF_MVVM_TEMPLATE.Entitys;

public class TaskModel
{
    public ObjectId Id { get; set; }
    public string? Name { get; set; }
    public string? Owner { get; set; }
    public string? Comment { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? StartKilometers { get; set; }
    public ETaskModelStatus? ModelStatus { get; set; }
    public int? EndKilometers { get; set; }
    
}