using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PristineCraft.Domain.Entities.User;

public class Message
{
    [Key]
    public int Id { get; set; }
    public required string Content { get; set; }
    public required DateTime Timestamp { get; set; }

    [ForeignKey("Sender")]
    public int SenderId { get; set; }
    public required AppUser Sender { get; set; }

    [ForeignKey("Receiver")]
    public int ReceiverId { get; set; }
    public required AppUser Receiver { get; set; }
}