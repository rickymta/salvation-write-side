using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Salvation.WriteSide.Commons;
using Salvation.WriteSide.Models.Context;
using Salvation.WriteSide.Models.Entities;
using Salvation.WriteSide.Services.MessageServices.Abstractions;

namespace Salvation.WriteSide.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MainTableController : ControllerBase
{
    private readonly SalvationDBContext _context;
    private readonly IKafkaProducer _kafkaProducer;

    public MainTableController(SalvationDBContext context, IKafkaProducer kafkaProducer)
    {
        _context = context;
        _kafkaProducer = kafkaProducer;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MainTable>>> GetTodoItems()
    {
        try
        {
            return await _context.MainTables.ToListAsync();
        }
        catch (Exception ex)
        {
            LogProvider.Error("MainTableController", ex);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult<MainTable>> PostTodoItem(MainTable item)
    {
        try
        {
            _context.MainTables.Add(item);
            await _context.SaveChangesAsync();
            await _kafkaProducer.ProduceAsync("todolist", item);
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.TaskID }, item);
        }
        catch (Exception ex)
        {
            LogProvider.Error("MainTableController", ex);
            return BadRequest();
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MainTable>> GetTodoItem(int id)
    {
        try
        {
            var item = await _context.MainTables.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        catch (Exception ex)
        {
            LogProvider.Error("MainTableController", ex);
            return BadRequest();
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMainTable(Guid id, MainTable item)
    {
        try
        {
            if (id != item.TaskID)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                await _kafkaProducer.ProduceAsync("todolist", item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MainTables(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            LogProvider.Error("MainTableController", ex);
            return BadRequest();
        }
    }

    private bool MainTables(Guid id)
    {
        return _context.MainTables.Any(e => e.TaskID == id);
    }
}
