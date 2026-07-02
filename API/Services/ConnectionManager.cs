using System.Collections.Concurrent;
using AdvancedChat.API.Data;
using Microsoft.EntityFrameworkCore;

namespace AdvancedChat.API.Services;

public class ConnectionManager
{
    private readonly ConcurrentDictionary<string, string> _connections = new();

    public void Add(string userId, string connectionId)
    {
        _connections[userId] = connectionId;
    }

    public void Remove(string userId)
    {
        _connections.TryRemove(userId, out _);
    }

    public string? GetConnection(string userId)
    {
        return _connections.TryGetValue(userId, out var connection)
            ? connection
            : null;
    }

    public List<string> GetOnlineUsers()
    {
        return _connections.Keys.ToList();
    }

    public async Task<List<string>> GetOnlineUserFullNamesAsync(ApplicationDbContext context)
    {
        var userIds = _connections.Keys.ToList();
        if (userIds.Count == 0)
            return new List<string>();

        var users = await context.Users
            .Where(u => userIds.Contains(u.Id))
            .Select(u => u.FullName)
            .ToListAsync();

        return users;
    }

    public bool IsOnline(string userId)
    {
        return _connections.ContainsKey(userId);
    }
}
