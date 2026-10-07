FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
USER root
RUN apk add --no-cache icu-libs icu-data-full
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DotCruz.Workspaces.Api/DotCruz.Workspaces.Api.csproj", "src/DotCruz.Workspaces.Api/"]
COPY ["src/DotCruz.Workspaces.Application/DotCruz.Workspaces.Application.csproj", "src/DotCruz.Workspaces.Application/"]
COPY ["src/DotCruz.Workspaces.Domain/DotCruz.Workspaces.Domain.csproj", "src/DotCruz.Workspaces.Domain/"]
COPY ["src/DotCruz.Workspaces.Infrastructure/DotCruz.Workspaces.Infrastructure.csproj", "src/DotCruz.Workspaces.Infrastructure/"]
COPY ["src/DotCruz.Workspaces.CrossCutting/DotCruz.Workspaces.CrossCutting.csproj", "src/DotCruz.Workspaces.CrossCutting/"]
RUN dotnet restore "./src/DotCruz.Workspaces.Api/DotCruz.Workspaces.Api.csproj"
COPY . .
WORKDIR "/src/src/DotCruz.Workspaces.Api"
RUN dotnet build "./DotCruz.Workspaces.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DotCruz.Workspaces.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotCruz.Workspaces.Api.dll"]
