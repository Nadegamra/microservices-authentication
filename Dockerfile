#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["Services/Services.Common/Services.Common.csproj", "Services/Services.Common/"]
RUN dotnet restore "Services/Services.Common/Services.Common.csproj"
COPY . .
WORKDIR "/src/Services/Services.Common"
RUN dotnet build "Services.Common.csproj" -c Release -o /app/build

WORKDIR /src
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Infrastructure/Infrastructure.csproj"
COPY . .
WORKDIR "/src/Infrastructure"
RUN dotnet build "Infrastructure.csproj" -c Release -o /app/build

WORKDIR /src
COPY ["Services/Authentication/Authentication.csproj", "Services/Authentication/"]
RUN dotnet restore "Services/Authentication/Authentication.csproj"
COPY . .
WORKDIR "/src/Services/Authentication"
RUN dotnet build "Authentication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authentication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.dll"]