FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /WorkWithJson

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /WorkWithJson/WorkWithJson
COPY ["WorkWithJson.csproj", "WorkWithJson/"]
RUN dotnet restore "WorkWithJson/WorkWithJson.csproj"
COPY . .
RUN dotnet build "WorkWithJson.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkWithJson.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /WorkWithJson
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkWithJson.dll"]
