using AdvancedChat.API.Data;
using API.DTOs;
using API.Hubs;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AdvancedChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext;


    public RoomsController(
    ApplicationDbContext context,
    IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = await _context.Rooms
            .Select(r => new
            {
                r.Id,
                r.Name,
                r.CreatedAt
            })
            .ToListAsync();

        return Ok(rooms);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDTO dto)
    {
        // Both clients POST a JSON body: { "roomName": "..." }.
        // The previous signature `CreateRoom(string roomName)` had no
        // [FromBody]/[FromForm] attribute, so ASP.NET Core tried to bind
        // it from the query string / route instead of the JSON body,
        // meaning roomName was always null and every request failed
        // with "Room name is required."
        var roomName = dto?.RoomName;

        if (string.IsNullOrWhiteSpace(roomName))
            return BadRequest("Room name is required.");

        if (await _context.Rooms.AnyAsync(r => r.Name == roomName))
            return BadRequest("Room already exists.");

        var room = new Room
        {
            Name = roomName,
            CreatedById = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
            CreatedAt = DateTime.UtcNow
        };

        _context.Rooms.Add(room);

        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync(
            "RoomCreated",
            room.Id,
            room.Name);

        return Ok(room);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
            return NotFound();

        _context.Rooms.Remove(room);

        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync(
            "RoomDeleted",
            room.Id);

        return Ok();
    }
}