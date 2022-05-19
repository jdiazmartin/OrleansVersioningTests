#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN groupadd -r app && \
    useradd --uid 101 -r -g app -d /app -s /sbin/nologin app && \
    mkdir -p /app && \
    chown app:app /app
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["src/Silo.Dashboard/Silo.Dashboard.csproj", "src/Silo.Dashboard/"]
RUN dotnet restore "src/Silo.Dashboard/Silo.Dashboard.csproj"
COPY . .
WORKDIR "/src/src/Silo.Dashboard"
RUN dotnet build "Silo.Dashboard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Silo.Dashboard.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080

ENTRYPOINT ["dotnet", "Peza.Webhooks.Silo.Dashboard.dll"]
