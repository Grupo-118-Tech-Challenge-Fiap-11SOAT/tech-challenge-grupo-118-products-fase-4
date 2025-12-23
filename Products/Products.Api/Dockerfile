FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Products/Products.Api/Products.Api.csproj", "Products/Products.Api/"]
COPY ["Products/Products.Application/Products.Application.csproj", "Products/Products.Application/"]
COPY ["Products/Products.Domain/Products.Domain.csproj", "Products/Products.Domain/"]
COPY ["Products/Products.Infra/Products.Infra.csproj", "Products/Products.Infra/"]
RUN dotnet restore "Products/Products.Api/Products.Api.csproj"

COPY Products/ .

WORKDIR "/src/Products.Api"

RUN dotnet build "Products.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Products.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

ENV \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

RUN apk add --no-cache \
    icu-data-full \
    icu-libs \
    curl

COPY --from=publish /app/publish .

USER $APP_UID

ENTRYPOINT ["dotnet", "Products.Api.dll"]
