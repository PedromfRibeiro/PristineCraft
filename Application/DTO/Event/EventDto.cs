using Application.DTO.User;
using Domain.Entities.Event;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Event;

public class EventDto
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
	public required UserDto? Owner { get; set; }
	public GroupDto? Group { get; set; }
	public ICollection<byte[]>? Attachments { get; set; }
}

public class MinimalEventDto
{
	[Key]
	public int Id { get; set; }
	public decimal Amount { get; set; }
	public DateTime CreationDate { get; set; } = DateTime.UtcNow;
	public EventCategory? Category { get; set; }
	public EventSubCategory? SubCategory { get; set; }
	public required UserDto Owner { get; set; }
	public GroupDto? Group { get; set; }
}

public class CreateEventDto
{
	[Key]
	public int Id { get; set; }
	public string? Description { get; set; }
	public decimal Amount { get; set; }
	public DateTime Date { get; set; }
	public DateTime CreationDate { get; set; } = DateTime.UtcNow;
	public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
	public int? SubCategoryId { get; set; }
	public int? GroupId { get; set; }
	public ICollection<byte[]>? Attachments { get; set; }
}