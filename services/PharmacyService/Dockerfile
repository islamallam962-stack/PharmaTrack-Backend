FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["PharmacyService.API/PharmacyService.API.csproj", "PharmacyService.API/"]
COPY ["PharmacyService.Infrastructure/PharmacyService.Infrastructure.csproj", "PharmacyService.Infrastructure/"]
COPY ["PharmacyService.Application/PharmacyService.Application.csproj", "PharmacyService.Application/"]
COPY ["PharmacyService.Domain/PharmacyService.Domain.csproj", "PharmacyService.Domain/"]

RUN dotnet restore "PharmacyService.API/PharmacyService.API.csproj"

COPY . .
RUN dotnet publish "PharmacyService.API/PharmacyService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PharmacyService.API.dll"]