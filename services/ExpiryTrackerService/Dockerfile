FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ExpiryTrackerService.Worker/ExpiryTrackerService.Worker.csproj", "ExpiryTrackerService.Worker/"]
COPY ["ExpiryTrackerService.Infrastructure/ExpiryTrackerService.Infrastructure.csproj", "ExpiryTrackerService.Infrastructure/"]
COPY ["ExpiryTrackerService.Application/ExpiryTrackerService.Application.csproj", "ExpiryTrackerService.Application/"]
COPY ["ExpiryTrackerService.Domain/ExpiryTrackerService.Domain.csproj", "ExpiryTrackerService.Domain/"]

RUN dotnet restore "ExpiryTrackerService.Worker/ExpiryTrackerService.Worker.csproj"

COPY . .
RUN dotnet publish "ExpiryTrackerService.Worker/ExpiryTrackerService.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ExpiryTrackerService.Worker.dll"]