using AdvancedChat.API.Data;
using AdvancedChat.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ConnectionManager _connectionManager;

    public UsersController(
        ApplicationDbContext context,
        ConnectionManager connectionManager)
    {
        _context = context;
        _connectionManager = connectionManager;
    }

    [HttpGet("online")]
    public IActionResult GetOnlineUsers()
    {
        return Ok(_connectionManager.GetOnlineUsers());
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users
            .Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email
            })
            .ToListAsync();

        return Ok(users);
    }
}