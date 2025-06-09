# # Stage 1: Build the application
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src

# # Copy solution and project files to restore dependencies
# COPY *.sln .
# COPY src/TodoApi/TodoApi.csproj ./src/TodoApi/

# RUN dotnet restore *.sln

# # Copy the rest of the soruce code and publish application
# COPY . .
# WORKDIR /src/src/TodoApi
# RUN dotnet publish -c Release -o /app/publish

# # -----------------------------------------------------

# # Stage 2: Create the final,small runtime image
# FROM mcr.microsoft.com/dotnet/sdk:8.0
# WORKDIR /app
# COPY --from=build /app/publish .


# # Define the internal port the API will listen to
# ENV ASPNETCORE_URLS=http://+:8080

# # I dont know if this is the correct way to do so,
# # but this is needed to be able to run the db migrations
# # on the .sh script later
# COPY src/TodoApi/.config/dotnet-tools.json ./.config/
# RUN dotnet tool restore

# COPY src/TodoApi/entrypoint.sh .
# RUN chmod +x ./entrypoint.sh

# ENTRYPOINT [ "./entrypoint.sh" ]
# CMD [ "dotnet", "TodoApi.dll" ]



# -------------------------------------------------------------------------------------

# FROM mcr.microsoft.com/dotnet/sdk:8.0
# WORKDIR /app

# COPY . .

# WORKDIR /app/src/TodoApi

# RUN dotnet tool restore
# RUN dotnet restore ../../crunchloop-project.sln

# RUN apt-get update && apt-get install -y netcat-openbsd

# ENV ASPNETCORE_URLS=http://+:8080

# RUN chmod +x ./entrypoint.sh

# ENTRYPOINT ["./entrypoint.sh"]

# CMD ["dotnet", "run", "--no-launch-profile"]

# ---------------------------------------------------------------------------------------

FROM mcr.microsoft.com/devcontainers/dotnet:8.0
WORKDIR /app
    
COPY src/TodoApi/*.csproj ./src/TodoApi/
WORKDIR /app/src/TodoApi
RUN dotnet restore --verbosity detailed

WORKDIR /app
COPY src/TodoApi/ src/TodoApi/


RUN apt-get update && apt-get install -y netcat-openbsd
ENV ASPNETCORE_URLS=http://+:8080

WORKDIR /app/src/TodoApi
RUN dotnet tool restore
RUN chmod +x ./entrypoint.sh
    
ENTRYPOINT ["./entrypoint.sh"]
    
CMD ["dotnet", "run", "--no-launch-profile"]

# ---------------------------------------------------------------------------------------
    

# # s1
# FROM mcr.microsoft.com/devcontainers/dotnet:8.0
# WORKDIR /app

# COPY src/TodoApi/*.csproj ./src/TodoApi/
# WORKDIR /app/src/TodoApi
# RUN dotnet restore --verbosity detailed

# # s2

# COPY src/TodoApi/ src/TodoApi

# RUN dotnet tool restore

# RUN apt-get update && apt-get install -y netcat-openbsd

# ENV ASPNETCORE_URLS=http://+:8080

# RUN chmod +x ./entrypoint.sh

# ENTRYPOINT ["./entrypoint.sh"]

# CMD ["dotnet", "run", "--no-launch-profile"]