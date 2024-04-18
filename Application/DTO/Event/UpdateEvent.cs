using Application.DTO.User;
using Domain.Entities.Event;

namespace Application.DTO.Event;

public class UpdateEvent
{
	public int Id { get; set; }
	public string? Description { get; set; }
	public decimal Amount { get; set; }
	public DateTime Date { get; set; }
	public DateTime CreationDate { get; set; } = DateTime.UtcNow;
	public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
	public EventCategory? Category { get; set; }
	public EventSubCategory? SubCategory { get; set; }
	public required UserDto Owner { get; set; }
	public GroupDto? Group { get; set; }
	public ICollection<byte[]>? Attachments { get; set; }
}