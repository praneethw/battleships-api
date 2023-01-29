FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Ofx.Battleships.Api/Ofx.Battleships.Api.csproj", "Ofx.Battleships.Api/"]
COPY ["src/Ofx.Battleships.Domain/Ofx.Battleships.Domain.csproj", "Ofx.Battleships.Domain/"]
COPY ["src/Ofx.Battleships.Infrastructure/Ofx.Battleships.Infrastructure.csproj", "Ofx.Battleships.Infrastructure/"]
RUN dotnet restore "Ofx.Battleships.Api/Ofx.Battleships.Api.csproj"

COPY "src/" .
RUN dotnet build "Ofx.Battleships.Api/Ofx.Battleships.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ofx.Battleships.Api/Ofx.Battleships.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ofx.Battleships.Api.dll"]
