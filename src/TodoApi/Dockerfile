# Stage 1: Build adn publish the api
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files to restore dependencies
COPY . .
RUN dotnet restore ./src/TodoApi/TodoApi.csproj

# Publish the api
RUN dotnet publish ./src/TodoApi/TodoApi.csproj -c Release -o /app/publish

# --------------

# Stage 2: Running the api in a small ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Instyall netcat to check the database connection
RUN apt-get update && apt-get install -y netcat-traditional


# Copy only the compiled output
COPY --from=build /app/publish .

# Copy the entrypoint script
COPY ./src/TodoApi/entrypoint.sh .
RUN chmod +x ./entrypoint.sh

# Set the entrypoint
ENTRYPOINT ["./entrypoint.sh"]
CMD ["dotnet", "TodoApi.dll"]

# ---------------------------------------------------------------------------------------

# FROM mcr.microsoft.com/devcontainers/dotnet:8.0
# WORKDIR /app
    
# COPY src/TodoApi/*.csproj ./src/TodoApi/
# WORKDIR /app/src/TodoApi
# RUN dotnet restore --verbosity detailed

# WORKDIR /app
# COPY src/TodoApi/ src/TodoApi/


# RUN apt-get update && apt-get install -y netcat-openbsd
# ENV ASPNETCORE_URLS=http://+:8080

# WORKDIR /app/src/TodoApi
# RUN dotnet tool restore
# RUN chmod +x ./entrypoint.sh
    
# ENTRYPOINT ["./entrypoint.sh"]
    
# CMD ["dotnet", "run", "--no-launch-profile"]