#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Flex.Web/Flex.Web.csproj", "src/Flex.Web/"]
COPY ["src/Flex.Application/Flex.Application.csproj", "src/Flex.Application/"]
COPY ["src/Flex.Application.Contracts/Flex.Application.Contracts.csproj", "src/Flex.Application.Contracts/"]
COPY ["src/Flex.Dapper/Flex.Dapper.csproj", "src/Flex.Dapper/"]
COPY ["src/Flex.Core/Flex.Core.csproj", "src/Flex.Core/"]
COPY ["src/Flex.Domain/Flex.Domain.csproj", "src/Flex.Domain/"]
COPY ["src/Flex.SqlSugar/Flex.SqlSugarFactory.csproj", "src/Flex.SqlSugar/"]
COPY ["src/Flex.EFSql/Flex.EFSql.csproj", "src/Flex.EFSql/"]
RUN dotnet restore "./src/Flex.Web/./Flex.Web.csproj"
COPY . .
WORKDIR "/src/src/Flex.Web"
RUN dotnet build "./Flex.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Flex.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Flex.Web.dll"]