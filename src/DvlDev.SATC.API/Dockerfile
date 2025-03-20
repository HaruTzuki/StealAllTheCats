FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust
WORKDIR /src
COPY ["DvlDev.SATC.API/DvlDev.SATC.API.csproj", "DvlDev.SATC.API/"]
COPY ["DvlDev.SATC.Application/DvlDev.SATC.Application.csproj", "DvlDev.SATC.Application/"]
COPY ["DvlDev.SATC.Contracts/DvlDev.SATC.Contracts.csproj", "DvlDev.SATC.Contracts/"]
RUN dotnet restore "DvlDev.SATC.API/DvlDev.SATC.API.csproj"
COPY . .
WORKDIR "/src/DvlDev.SATC.API"
RUN dotnet build "DvlDev.SATC.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DvlDev.SATC.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DvlDev.SATC.API.dll"]
