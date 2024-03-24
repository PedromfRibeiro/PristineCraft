using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.User;

public class Message
{
	public int Id { get; set; }
	public string Content { get; set; }
	public DateTime Timestamp { get; set; }

	[ForeignKey("userToId")]
	public User UserTo { get; set; }

	[ForeignKey("userFromId")]
	public User UserFrom { get; set; }
}