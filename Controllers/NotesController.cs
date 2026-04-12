using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace IsLabApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private static readonly ConcurrentDictionary<int, Note> _notes = new();
    private static int _nextId = 1;

    // GET /api/notes - получить все заметки
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_notes.Values.OrderByDescending(n => n.CreatedAt));
    }

    // GET /api/notes/{id} - получить одну заметку
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        if (_notes.TryGetValue(id, out var note))
            return Ok(note);
        return NotFound();
    }

    // POST /api/notes - создать заметку
    [HttpPost]
    public IActionResult Create([FromBody] CreateNoteRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Title is required");

        var note = new Note
        {
            Id = _nextId++,
            Title = request.Title,
            Text = request.Text ?? "",
            CreatedAt = DateTime.UtcNow
        };
        
        _notes[note.Id] = note;
        return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
    }

    // DELETE /api/notes/{id} - удалить заметку
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (_notes.TryRemove(id, out _))
            return NoContent();
        return NotFound();
    }
}

// Модели данных
public class Note
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Text { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class CreateNoteRequest
{
    public string Title { get; set; } = "";
    public string Text { get; set; } = "";
}