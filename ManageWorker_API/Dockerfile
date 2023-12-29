FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY published/ ./

ENTRYPOINT ["dotnet", "ManageWorker_API.dll"]