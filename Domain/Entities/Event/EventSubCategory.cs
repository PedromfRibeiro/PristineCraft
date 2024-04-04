using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Event;

public class EventSubCategory
{
	[Key]
	public int Id { get; set; }
	public required string Name { get; set; }
	public EventCategory? Category { get; set; }
}