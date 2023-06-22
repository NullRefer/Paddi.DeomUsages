FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5123

ENV ASPNETCORE_URLS=http://+:5123

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/services/Paddi.DemoUsages.ApiDemo/Paddi.DemoUsages.ApiDemo.csproj", "src/services/Paddi.DemoUsages.ApiDemo/"]
RUN dotnet restore "src/services/Paddi.DemoUsages.ApiDemo/Paddi.DemoUsages.ApiDemo.csproj"
COPY . .
WORKDIR "/src/src/services/Paddi.DemoUsages.ApiDemo"
RUN dotnet build "Paddi.DemoUsages.ApiDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Paddi.DemoUsages.ApiDemo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Paddi.DemoUsages.ApiDemo.dll"]
