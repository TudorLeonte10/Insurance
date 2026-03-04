FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["Insurance.Reporting.Worker/Insurance.Reporting.Worker.csproj", "Insurance.Reporting.Worker/"]
COPY ["Insurance.Application/Insurance.Application.csproj", "Insurance.Application/"]
COPY ["Insurance.Infrastructure/Insurance.Infrastructure.csproj", "Insurance.Infrastructure/"]
COPY ["Insurance.Reporting.Infrastructure/Insurance.Reporting.Infrastructure.csproj", "Insurance.Reporting.Infrastructure/"]

RUN dotnet restore "Insurance.Reporting.Worker/Insurance.Reporting.Worker.csproj"

COPY . .

WORKDIR "/src/Insurance.Reporting.Worker"

RUN dotnet build "Insurance.Reporting.Worker.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "Insurance.Reporting.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Insurance.Reporting.Worker.dll"]