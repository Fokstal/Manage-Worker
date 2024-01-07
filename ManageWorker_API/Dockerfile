FROM mcr.microsoft.com/dotnet/aspnet:8.0

EXPOSE 5000

WORKDIR /app

COPY bin/Debug/net8.0/ ./

ENTRYPOINT ["dotnet", "ManageWorker_API.dll"]