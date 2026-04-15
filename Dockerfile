# Этап 1: Сборка (build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY IsLabApp.csproj .
RUN dotnet restore

# Копируем весь код и собираем
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Этап 2: Запуск (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Копируем опубликованное приложение из этапа build
COPY --from=build /app/publish .

# Переменные окружения по умолчанию
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

# Открываем порт 8080
EXPOSE 8080

# Точка входа
ENTRYPOINT ["dotnet", "IsLabApp.dll"]
