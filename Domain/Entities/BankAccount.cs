namespace Domain.Entities;

public class BankAccount
{
	[System.ComponentModel.DataAnnotations.Key]
	public int Id { get; set; }

	public required string Name { get; set; }
	public string? Description { get; set; }
	public string Icon { get; set; } = "bi-bank";
	public User? Proprietary { get; set; }
}