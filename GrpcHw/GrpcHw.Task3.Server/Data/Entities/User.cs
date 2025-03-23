namespace GrpcHw.Task3.Server.Data.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string HashPassword { get; set; }
}