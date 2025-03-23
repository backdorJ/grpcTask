using Microsoft.EntityFrameworkCore;

namespace GrpcHw.Task3.Server.Data;

public class Migrator
{
    private readonly AppDbContext _context;

    public Migrator(AppDbContext context)
    {
        _context = context;
    }

    public async Task MigrateAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}