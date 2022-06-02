#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/KpiV3.WebApi/KpiV3.WebApi.csproj", "src/KpiV3.WebApi/"]
COPY ["src/KpiV3.Domain/KpiV3.Domain.csproj", "src/KpiV3.Domain/"]
COPY ["src/KpiV3.Rop/KpiV3.Rop.csproj", "src/KpiV3.Rop/"]
COPY ["src/KpiV3.Infrastructure/KpiV3.Infrastructure.csproj", "src/KpiV3.Infrastructure/"]
RUN dotnet restore "src/KpiV3.WebApi/KpiV3.WebApi.csproj"
COPY . .
WORKDIR "/src/src/KpiV3.WebApi"
RUN dotnet build "KpiV3.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KpiV3.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KpiV3.WebApi.dll"]