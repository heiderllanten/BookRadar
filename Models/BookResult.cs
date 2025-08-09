namespace BookRadar.Models;

public class BookResult
{
    public string Title { get; set; } = string.Empty;
    public int? FirstPublishYear { get; set; }
    public string? Publisher { get; set; }
}