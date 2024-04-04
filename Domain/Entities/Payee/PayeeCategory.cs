﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PayeeCategory
{
	[Key]
	public int Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public ICollection<Payee>? Category { get; set; }
}