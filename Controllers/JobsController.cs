using JobTracker.Api.Data;
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
}
