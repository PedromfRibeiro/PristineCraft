using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Event;

public class Event
{
	[Key]
	public int Id { get; set; }
	public string? Description { get; set; }
	public decimal Amount { get; set; }
	public DateTime Date { get; set; }
	public DateTime CreationDate { get; set; } = DateTime.UtcNow;
	public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

	[ForeignKey("Category")]
	public int? CategoryId { get; set; }
	public EventCategory? Category { get; set; }

	[ForeignKey("SubCategory")]
	public int? SubCategoryId { get; set; }
	public EventSubCategory? SubCategory { get; set; }

	[ForeignKey("Owner")]
	public int? OwnerId { get; set; }
	public required User Owner { get; set; }

	[ForeignKey("Group")]
	public int? GroupId { get; set; }
	public Group? Group { get; set; }
	public ICollection<byte[]>? Attachments { get; set; }
}