FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 9090

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GrpcHw.Task3.Server/GrpcHw.Task3.Server.csproj", "GrpcHw.Task3.Server/"]
RUN dotnet restore "GrpcHw.Task3.Server/GrpcHw.Task3.Server.csproj"
COPY . .
WORKDIR "/src/GrpcHw.Task3.Server"
RUN dotnet build "GrpcHw.Task3.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GrpcHw.Task3.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcHw.Task3.Server.dll"]
