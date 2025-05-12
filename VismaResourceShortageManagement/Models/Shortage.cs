using System;
namespace VismaResourceShortageManagement.Models
{
public class Shortage
{
    public required string Title { get; set; }
    public required string Name { get; set; }
    public required string Room { get; set; }
    public required string Category { get; set; }
    public required int Priority { get; set; }
    public required DateTime CreatedOn { get; set; }
}
}