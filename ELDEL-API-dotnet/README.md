# Introduction 
Eldel API - ASP.NET Core Web API Project. 

# Getting Started
1. Clone the repository.
2. Download [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) and install [ASP.NET Core](https://github.com/aspnet/Home).
3. Add an environment variable `ELDEL_API_ENVIRONMENT`, set it to `stage` or `prod` to copy `appsettings.{environment}.json` during startup.
4. - For Mac:
        * Run Visual studio through a command line terminal (E.g,: ` ~/../../Applications/Visual\ Studio.app/Contents/MacOS/VisualStudio &`).
    - For Windows:
        * Open Visual studio.

# Build and Test
1. `dotnet restore src/ELDEL-API.sln` to restore dependencies (often not required).
2. `dotnet build src/ELDEL-API.sln` to build the ASP.NET Core Web API solution.
3. `dotnet run --project src/ELDEL-API/ELDEL-API.csproj` to run the console app.
