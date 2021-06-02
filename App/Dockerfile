FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY bin/Release/net5.0/publish/ App/
EXPOSE 8880
WORKDIR /App
ENTRYPOINT ["dotnet", "NetCore.Docker.dll"]