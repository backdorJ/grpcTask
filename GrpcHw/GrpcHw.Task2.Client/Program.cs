// See https://aka.ms/new-console-template for more information

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcHw.Task2.Client;

using var cts = new CancellationTokenSource();
using var channel = GrpcChannel.ForAddress("https://localhost:7095");
var tokenClient = new TokenService.TokenServiceClient(channel);

var token = tokenClient.GetJwtToken(new GetJwtTokenRequest
{
    Username = "damir",
    Age = 21
}, cancellationToken: cts.Token);

var headers = new Metadata();
headers.Add("Authorization", $"Bearer {token.Token}");

var secretClient = new SecretService.SecretServiceClient(channel);
var secret = secretClient.GetSecret(request: new Empty(), headers: headers, cancellationToken: cts.Token);
Console.WriteLine(secret);