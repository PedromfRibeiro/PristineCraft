using System.ComponentModel.DataAnnotations;

namespace PristineCraft.Domain.Entities.Payee;

public class Payee
{
    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public required PayeeCategory Category { get; set; }
}