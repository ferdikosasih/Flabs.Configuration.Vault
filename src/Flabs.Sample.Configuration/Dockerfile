# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project file to restore dependencies
COPY ["../Flabs.Configuration.Vault/Flabs.Configuration.VaultSharp.Extensions.csproj", "Flabs.Configuration.Vault/"]

## Navigate to the directory where the project file is located
#WORKDIR "/Flabs.Configuration.Vault"
#
## Restore dependencies
#RUN dotnet restore
#
## Copy the rest of the application code
#WORKDIR "/src"
#COPY . .
#
## Build the application
#WORKDIR "/src/Flabs.Sample.Configuration"
#RUN dotnet build "Flabs.Sample.Configuration.csproj" -c Release -o /app/build
#
## Publish Stage
#FROM build AS publish
#RUN dotnet publish "Flabs.Sample.Configuration.csproj" -c Release -o /app/publish
#
## Runtime Stage
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#
## Expose the port your application will run on
#EXPOSE 8080
#
## Set the entry point for your application
#ENTRYPOINT ["dotnet", "Flabs.Sample.Configuration.dll"]
#


##See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["src/Flabs.Sample.Configuration/Flabs.Sample.Configuration.csproj", "src/Flabs.Sample.Configuration/"]
#RUN dotnet restore "./src/Flabs.Sample.Configuration/./Flabs.Sample.Configuration.csproj"
#COPY . .
#WORKDIR "/src/src/Flabs.Sample.Configuration"
#RUN dotnet build "./Flabs.Sample.Configuration.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./Flabs.Sample.Configuration.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Flabs.Sample.Configuration.dll"]