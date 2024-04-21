namespace PristineCraft.Application.Common.Models;

public class FilterOptions
{
    public required string field { get; set; }
    public required string operators { get; set; }
    public required string value { get; set; }
}