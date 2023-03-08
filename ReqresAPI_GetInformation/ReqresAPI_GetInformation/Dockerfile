FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /ReqresAPI_GetInformation

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /ReqresAPI_GetInformation/ReqresAPI_GetInformation
COPY ["ReqresAPI_GetInformation.csproj", "ReqresAPI_GetInformation/"]
RUN dotnet restore "ReqresAPI_GetInformation/ReqresAPI_GetInformation.csproj"
COPY . .
RUN dotnet build "ReqresAPI_GetInformation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReqresAPI_GetInformation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /ReqresAPI_GetInformation
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReqresAPI_GetInformation.dll"]
