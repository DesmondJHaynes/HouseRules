using HouseRules.Data;
using HouseRules.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseRules.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChoreController : ControllerBase
{
    private HouseRulesDbContext _dbContext;
    public ChoreController(HouseRulesDbContext context)
    {
        _dbContext = context;
    }


    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_dbContext.Chores);
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetbyId(int id)
    {
        Chore found = _dbContext.Chores
        .Include(c => c.ChoreAssignments)
        .Include(c => c.ChoreCompletions)
        .SingleOrDefault(c => c.Id == id);
        if (found == null)
        {
            return NotFound();
        }
        return Ok(found);
    }

    [Authorize]
    [HttpPost("{id}/complete/")]
    public IActionResult getCompletedbyId(int id, int userId)
    {
        ChoreCompletion completedChore = new ChoreCompletion
        {
            UserProfileId = userId,
            ChoreId = id,
            CompletedOn = DateTime.Now
        };

        _dbContext.ChoreCompletions.Add(completedChore);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult createChore(Chore newChore)
    {
        Chore created = new Chore
        {
            Name = newChore.Name,
            Difficulty = newChore.Difficulty,
            ChoreFrequencyDays = newChore.Difficulty
        };
        _dbContext.Chores.Add(created);
        _dbContext.SaveChanges();
        return Created("api/chore/{id}", created);
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult updateChore(Chore choreObj, int id)
    {
        if (choreObj.Id != id)
        { return BadRequest(); }

        Chore found = _dbContext.Chores.SingleOrDefault(c => c.Id == choreObj.Id);

        if (found == null)
        { return NotFound(); }

        found.Name = choreObj.Name;
        found.Difficulty = choreObj.Difficulty;
        found.ChoreFrequencyDays = choreObj.ChoreFrequencyDays;
        _dbContext.SaveChanges();
        return NoContent();

    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult DeleteChore(int id)
    {
        Chore found = _dbContext.Chores.SingleOrDefault(c => c.Id == id);
        if (found == null)
        { return NotFound(); }
        _dbContext.Chores.Remove(found);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/assign")]
    public IActionResult assignChore(int id, int userId)
    {
        ChoreAssignment newAssign = new ChoreAssignment
        {
            UserProfileId = userId,
            ChoreId = id
        };
        _dbContext.ChoreAssignments.Add(newAssign);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}/unassign")]
    public IActionResult UnassignChore(int id, int userId)
    {
        List<ChoreAssignment> deadList = _dbContext.ChoreAssignments.Where(ca => ca.Id == id && ca.UserProfileId == userId).ToList();

        foreach (ChoreAssignment dead in deadList)
        {
            _dbContext.ChoreAssignments.Remove(dead);
        }
        _dbContext.SaveChanges();
        return Ok(_dbContext.ChoreAssignments);
    }


}