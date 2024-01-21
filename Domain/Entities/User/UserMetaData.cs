namespace Domain.Entities.User;
public class UserMetaData
{
	public int Id { get; set; }
	public string? Parameter { get; set; }
	public string? Value { get; set; }
	public bool Valid { get; set; } = true;
	public int UserId { get; set; }
	public required User User { get; set; }
}