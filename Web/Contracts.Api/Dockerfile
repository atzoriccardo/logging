#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 3000
ENV ASPNETCORE_URLS=http://*:3000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Web/Contracts.Api/Contracts.Api.csproj", "Web/Contracts.Api/"]
COPY ["Commons/Infrastructure.CorrelationId/Infrastructure.CorrelationId.csproj", "Commons/Infrastructure.CorrelationId/"]
COPY ["Commons/Infrastructure.Logging/Infrastructure.Logging.csproj", "Commons/Infrastructure.Logging/"]
RUN dotnet restore "Web/Contracts.Api/Contracts.Api.csproj"
COPY . .
WORKDIR "/src/Web/Contracts.Api"
RUN dotnet build "Contracts.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Contracts.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vittoria.Contracts.Api.dll"]