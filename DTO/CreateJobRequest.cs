namespace JobTracker.Api.DTO;

public class CreateJobRequest
{
    public string CompanyName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Notes { get; set; }
    public string? Url { get; set; }
}
