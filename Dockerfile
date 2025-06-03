# Используем официальный .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем всё и публикуем проект
COPY . .
RUN dotnet publish -c Release -o out

# Используем рантайм образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Порт, который Render пробрасывает по умолчанию — 10000
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "ZametkiApp.dll"]
