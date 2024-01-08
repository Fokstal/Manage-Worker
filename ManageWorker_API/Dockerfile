FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

VOLUME [ "/app/AppData/" ]
VOLUME [ "/app/wwwroot/" ]

COPY bin/Debug/net8.0/ ./

ENTRYPOINT ["dotnet", "ManageWorker_API.dll"]
