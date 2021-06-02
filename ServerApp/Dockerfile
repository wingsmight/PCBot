FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY bin/Release/net5.0/publish/ ServerApp/
WORKDIR /ServerApp
EXPOSE 8000
ENTRYPOINT ["dotnet", "NetCore.Docker.dll"]