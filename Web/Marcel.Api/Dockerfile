#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Web/Marcel.Api/Marcel.Api.csproj", "Web/Marcel.Api/"]
COPY ["DAL/Marcel.DbModels/Marcel.DbModels.csproj", "DAL/Marcel.DbModels/"]
COPY ["DAL/Marcel.Access/Marcel.Access.csproj", "DAL/Marcel.Access/"]
RUN dotnet restore "Web/Marcel.Api/Marcel.Api.csproj"
COPY . .
WORKDIR "/src/Web/Marcel.Api"
RUN dotnet build "Marcel.Api.csproj" -c Production -o /app/build

FROM build AS publish
RUN dotnet publish "Marcel.Api.csproj" -c Production -r linux-x64 -o  /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Marcel.Api.dll"]