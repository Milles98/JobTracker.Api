namespace JobTracker.Api.DTO;

public class UpdateJobRequest
{
    public string Status { get; set; } = default!;
    public string? Notes { get; set; }
    public string? Url { get; set; }
}
