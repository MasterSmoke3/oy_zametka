# Устанавливаем SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем весь проект и собираем
COPY . ./
RUN dotnet publish -c Release -o out

# Финальный runtime образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Указываем порт и стартовую команду
EXPOSE 80
ENTRYPOINT ["dotnet", "ZametkiApp.dll"]
