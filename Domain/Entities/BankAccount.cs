using PristineCraft.Domain.Entities.User;

namespace PristineCraft.Domain.Entities;

public class BankAccount
{
    [System.ComponentModel.DataAnnotations.Key]
    public int Id { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public string Icon { get; set; } = "bi-bank";
    public AppUser? Proprietary { get; set; }
}