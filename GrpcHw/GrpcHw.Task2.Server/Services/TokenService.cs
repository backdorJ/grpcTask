using Grpc.Core;
using GrpcHw.Task2.Server.Utils;

namespace GrpcHw.Task2.Server.Services;

public class TokenService : Server.TokenService.TokenServiceBase
{
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public TokenService(JwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public override Task<GetJwtTokenResponse> GetJwtToken(GetJwtTokenRequest request, ServerCallContext context)
    {
        var token = _jwtTokenGenerator.GetJwtToken(request.Username, request.Age);
        
        
        return Task.FromResult(new GetJwtTokenResponse()
        {
            Token = token,
        });
    }
}