using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DbAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
namespace DbAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly StudentContext _context;
    public StudentController(StudentContext context)
    {
        _context = context;
    }

    

    [HttpGet]
    [Route("getStudent")]
    public List<Student> GetAllStudents() => _context.getStudents();

    [HttpPost]
    [Route("postStudent")]
    public async Task<ActionResult<Student>> PostTodoItem(Student st)
    {
        _context.Students.Add(st);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllStudents), st);
    }

    [HttpPut]
    [Route("putStudent")]
    public async Task<IActionResult> PutTodoItem(Student st)
    {
        
        _context.Entry(st).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("delStudent")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        var todoItem = await _context.Students.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.Students.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}