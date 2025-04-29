namespace JobTracker.Api.Models;

public class JobApplication
{
    public Guid Id { get; init; }
    public string CompanyName { get; set; }
    public string  Title { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string Status { get; set; } = "Sent";
    public string? Notes { get; set; }
    public string? Url { get; set; }
}
