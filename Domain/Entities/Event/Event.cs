using PristineCraft.Domain.Entities.User;
using System.ComponentModel.DataAnnotations;

namespace PristineCraft.Domain.Entities.Event;

public class Event
{
    [Key]
    public int Id { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public EventCategory? Category { get; set; }
    public EventSubCategory? SubCategory { get; set; }
    public required AppUser Owner { get; set; }
    public Group? Group { get; set; }
    public ICollection<byte[]>? Attachments { get; set; }
}