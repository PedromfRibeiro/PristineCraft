using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Domain.Entities.Event;

public class EventSubCategory
{
	[Key]
	public int Id { get; set; }
	public required string Name { get; set; }
	public int CategoryId { get; set; }

	public EventCategory? Category { get; set; }
}