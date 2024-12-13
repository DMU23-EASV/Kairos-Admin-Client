namespace WPF_MVVM_TEMPLATE.Entitys.DTOs;

public class ExportableTaskDTO
{
    public string? Name { get; set; }
    public string? Owner { get; set; }
    public string Email { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? StartKilometers { get; set; }
    public int? EndKilometers { get; set; }
    
    
}