FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["Insurance.WebApi/Insurance.WebApi.csproj", "Insurance.WebApi/"]
COPY ["Insurance.Application/Insurance.Application.csproj", "Insurance.Application/"]
COPY ["Insurance.Domain/Insurance.Domain.csproj", "Insurance.Domain/"]
COPY ["Insurance.Infrastructure/Insurance.Infrastructure.csproj", "Insurance.Infrastructure/"]
COPY ["Insurance.Reporting.Infrastructure/Insurance.Reporting.Infrastructure.csproj", "Insurance.Reporting.Infrastructure/"]

RUN dotnet restore "Insurance.WebApi/Insurance.WebApi.csproj"

COPY . .

WORKDIR "/src/Insurance.WebApi"

RUN dotnet build "Insurance.WebApi.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "Insurance.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Insurance.WebApi.dll"]