# Используем официальный .NET SDK образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем остальное и публикуем
COPY . ./
RUN dotnet publish -c Release -o out

# Используем рантайм образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Запускаем приложение
ENTRYPOINT ["dotnet", "ZametkiApp.dll"]
