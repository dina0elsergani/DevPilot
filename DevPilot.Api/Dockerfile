# Build stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["../DevPilot.Api/DevPilot.Api.csproj", "DevPilot.Api/"]
COPY ["../DevPilot.Application/DevPilot.Application.csproj", "DevPilot.Application/"]
COPY ["../DevPilot.Domain/DevPilot.Domain.csproj", "DevPilot.Domain/"]
COPY ["../DevPilot.Infrastructure/DevPilot.Infrastructure.csproj", "DevPilot.Infrastructure/"]
RUN dotnet restore "DevPilot.Api/DevPilot.Api.csproj"
COPY . .
WORKDIR "/src/DevPilot.Api"
RUN dotnet publish "DevPilot.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DevPilot.Api.dll"] 