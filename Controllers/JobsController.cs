using JobTracker.Api.Data;
using JobTracker.Api.DTO;
using JobTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobApplication>>> GetAll()
    {
        var jobs = await db.JobApplications.ToListAsync();
        return Ok(jobs);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobApplication>> GetById(Guid id)
    {
        var job = await db.JobApplications.FindAsync(id);
        if (job is null) return NotFound();
        return Ok(job);
    }

    [HttpPost]
    public async Task<ActionResult<JobApplication>> Create(CreateJobRequest request)
    {
        var newJob = new JobApplication
        {
            Id = Guid.NewGuid(),
            CompanyName = request.CompanyName,
            Title = request.Title,
            ApplicationDate = DateTime.UtcNow,
            Status = "Skickat",
            Notes = request.Notes
        };

        db.JobApplications.Add(newJob);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = newJob.Id }, newJob);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateJobRequest request)
    {
        var job = await db.JobApplications.FindAsync(id);
        if (job is null) return NotFound();

        job.Status = request.Status;
        job.Notes = request.Notes;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var job = await db.JobApplications.FindAsync(id);
        if (job is null) return NotFound();

        db.JobApplications.Remove(job);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
