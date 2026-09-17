namespace DPBack.Application.Contracts;

public record OrderHistoryElementResponse
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public string? AuthorId { get; set; }
    public DateTime ChangedAt { get; set; }
}