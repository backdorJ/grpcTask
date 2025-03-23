using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcHw.Task3.Server.Data;
using GrpcHw.Task3.Server.Data.Entities;
using GrpcHw.Task3.Server.Utils;
using Microsoft.EntityFrameworkCore;

namespace GrpcHw.Task3.Server.Services;

public class AuthService : Server.AuthService.AuthServiceBase
{
    private readonly IValidator<RegisterRequest> _registerValidator;
    
    private readonly HashService _hashService;
    private readonly AppDbContext _appDbContext;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IValidator<RegisterRequest> registerValidator,
        HashService hashService,
        AppDbContext appDbContext, JwtTokenGenerator jwtTokenGenerator)
    {
        _registerValidator = registerValidator;
        _hashService = hashService;
        _appDbContext = appDbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var user = await _appDbContext.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email, context.CancellationToken);
        
        if (user == null)
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

        var token = _jwtTokenGenerator.GetJwtToken(user.Id.ToString(), user.Email);

        return new LoginResponse
        {
            Token = token,
        };
    }

    public override async Task<Empty> Register(RegisterRequest request, ServerCallContext context)
    {
        var response = await _registerValidator.ValidateAsync(request, context.CancellationToken);
        
        if (!response.IsValid)
            throw new RpcException(Status.DefaultCancelled, string.Join(",", response.Errors));
        
        var user = new User
        {
            Email = request.Email,
            HashPassword = _hashService.HashPassword(request.Password)
        };

        await _appDbContext.Users.AddAsync(user, context.CancellationToken); 
        await _appDbContext.SaveChangesAsync(context.CancellationToken);
        
        return new Empty();
    }
}