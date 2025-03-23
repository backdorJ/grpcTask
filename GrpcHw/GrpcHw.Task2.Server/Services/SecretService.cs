using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace GrpcHw.Task2.Server.Services;

[Authorize]
public class SecretService : Server.SecretService.SecretServiceBase
{
    public override Task<GetSecretResponse> GetSecret(Empty request, ServerCallContext context)
        => Task.FromResult(new GetSecretResponse { Secret = "Ты пидор" });
}