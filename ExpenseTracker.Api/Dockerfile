FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ExpenseTracker.Api/ExpenseTracker.Api.csproj", "ExpenseTracker.Api/"]
COPY ["ExpenseTracker.Application/ExpenseTracker.Application.csproj", "ExpenseTracker.Application/"]
COPY ["ExpenseTracker.Infrastructure/ExpenseTracker.Infrastructure.csproj", "ExpenseTracker.Infrastructure/"]
COPY ["ExpenseTracker.Domain/ExpenseTracker.Domain.csproj", "ExpenseTracker.Domain/"]
RUN dotnet restore "ExpenseTracker.Api/ExpenseTracker.Api.csproj"
COPY . .
WORKDIR "/src/ExpenseTracker.Api"
RUN dotnet build "./ExpenseTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ExpenseTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpenseTracker.Api.dll"]
