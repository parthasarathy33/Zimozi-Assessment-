# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["TaskManagementAPI/TaskManagementAPI.csproj", "TaskManagementAPI/"]
RUN dotnet restore "TaskManagementAPI/TaskManagementAPI.csproj"

# Copy the rest of the code
COPY . .

# Build the application
WORKDIR "/src/TaskManagementAPI"
RUN dotnet build "TaskManagementAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "TaskManagementAPI.csproj" -c Release -o /app/publish

# Use the ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .

# Expose the port the app runs on
EXPOSE 80
EXPOSE 443

# Set the entry point
ENTRYPOINT ["dotnet", "TaskManagementAPI.dll"] 