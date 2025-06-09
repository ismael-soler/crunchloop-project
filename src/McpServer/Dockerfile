# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src 

# Copy all project and solution files needed to restore dependencies.
COPY src/McpServer/McpServer.csproj ./src/McpServer/
RUN dotnet restore src/McpServer/McpServer.csproj

# Copy the rest of the source code and publish the McpServer project
COPY . .
WORKDIR /src/src/McpServer
RUN dotnet publish -c Release -o /app/publish

# ------------------------------------------------------------------

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "McpServer.dll"]