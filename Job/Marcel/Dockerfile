#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Job/Marcel/Marcel.csproj", "Job/Marcel/"]
COPY ["Infrastructure/Marcel.Browser/Marcel.Browser.csproj", "Infrastructure/Marcel.Browser/"]
COPY ["Infrastructure/Marcel.Common/Marcel.Common.csproj", "Infrastructure/Marcel.Common/"]
COPY ["DAL/Marcel.Access/Marcel.Access.csproj", "DAL/Marcel.Access/"]
COPY ["DAL/Marcel.DbModels/Marcel.DbModels.csproj", "DAL/Marcel.DbModels/"]
COPY ["BL/Marcel.Pure.Co.UK/Marcel.Pure.Co.UK.csproj", "BL/Marcel.Pure.Co.UK/"]
RUN dotnet restore "Job/Marcel/Marcel.csproj"
COPY . .
WORKDIR "/src/Job/Marcel"
RUN dotnet build "Marcel.csproj" -c Production -o /app/build

FROM build AS publish
RUN dotnet publish "Marcel.csproj" -c Production -r linux-x64 -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Marcel.dll"]