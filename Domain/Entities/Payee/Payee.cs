using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Payee
{
	[Key]
	public int Id { get; set; }

	public required string Name { get; set; }
	public string? Description { get; set; }
	public required PayeeCategory Category { get; set; }
}