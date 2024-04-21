using System.ComponentModel.DataAnnotations;

namespace PristineCraft.Domain.Entities.Event;

public class EventCategory
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public EventSubCategory? SubCategory { get; set; }
}