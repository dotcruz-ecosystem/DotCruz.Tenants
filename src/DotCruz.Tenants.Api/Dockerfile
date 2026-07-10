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
COPY ["src/DotCruz.Tenants.Api/DotCruz.Tenants.Api.csproj", "src/DotCruz.Tenants.Api/"]
COPY ["src/DotCruz.Tenants.Application/DotCruz.Tenants.Application.csproj", "src/DotCruz.Tenants.Application/"]
COPY ["src/DotCruz.Tenants.Domain/DotCruz.Tenants.Domain.csproj", "src/DotCruz.Tenants.Domain/"]
COPY ["src/DotCruz.Tenants.Infrastructure/DotCruz.Tenants.Infrastructure.csproj", "src/DotCruz.Tenants.Infrastructure/"]
COPY ["src/DotCruz.Tenants.CrossCutting/DotCruz.Tenants.CrossCutting.csproj", "src/DotCruz.Tenants.CrossCutting/"]
RUN dotnet restore "./src/DotCruz.Tenants.Api/DotCruz.Tenants.Api.csproj"
COPY . .
WORKDIR "/src/src/DotCruz.Tenants.Api"
RUN dotnet build "./DotCruz.Tenants.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DotCruz.Tenants.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotCruz.Tenants.Api.dll"]
