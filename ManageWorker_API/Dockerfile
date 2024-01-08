# Используйте официальный образ .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Установите рабочую директорию в /app
WORKDIR /app

# Создайте тома для папок AppData и wwwroot
VOLUME [ "/app/AppData/" ]
VOLUME [ "/app/wwwroot/" ]

# Копируйте сборку приложения в рабочую директорию
COPY bin/Debug/net8.0/ ./

# Запустите приложение
ENTRYPOINT ["dotnet", "ManageWorker_API.dll"]
