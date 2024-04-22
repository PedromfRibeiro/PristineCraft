using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace PristineCraft.Domain.Entities;

public class LogEntry
{
    public LogEntry(int statusCode, string? message = null, string? details = null)
    {
        Id = 0;
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }

    [Key]
    public int Id { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
    public string? Details { get; set; }
    public LogLevel LogLevel { get; set; }
    public DateTime Timestamp { get; set; }
}