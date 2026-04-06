FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["MarketplaceService.API/MarketplaceService.API.csproj", "MarketplaceService.API/"]
COPY ["MarketplaceService.Infrastructure/MarketplaceService.Infrastructure.csproj", "MarketplaceService.Infrastructure/"]
COPY ["MarketplaceService.Application/MarketplaceService.Application.csproj", "MarketplaceService.Application/"]
COPY ["MarketplaceService.Domain/MarketplaceService.Domain.csproj", "MarketplaceService.Domain/"]

RUN dotnet restore "MarketplaceService.API/MarketplaceService.API.csproj"

COPY . .
RUN dotnet publish "MarketplaceService.API/MarketplaceService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MarketplaceService.API.dll"]