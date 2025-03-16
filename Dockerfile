# Build Stage - Using .NET SDK 8.0 to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Install Entity Framework CLI globally
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy project files and restore dependencies
COPY *.sln ./
COPY *.csproj ./
RUN dotnet restore

# Copy everything and build the application
COPY . ./
RUN dotnet publish ToDoMVC.csproj -c Release -o /app/publish

# Runtime Stage - Using .NET 8 Runtime for deployment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# Run EF Core migrations (if dotnet-ef is configured)
RUN dotnet ef database update || echo "Skipping migration"

# Expose port 8080
EXPOSE 8080

# Run the application
CMD ["dotnet", "ToDoMVC.dll"]