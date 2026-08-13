FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NuxibaApi.csproj", "./"]
RUN dotnet restore "NuxibaApi.csproj"
COPY . .
RUN dotnet build "NuxibaApi.csproj" -c Release -o /app/build
RUN dotnet publish "NuxibaApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NuxibaApi.dll"]
