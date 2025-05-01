#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0

ENV TZ="Asia/Tashkent"

# Expose port 80 for incoming HTTP traffic
EXPOSE 80

COPY /app /app
WORKDIR /app
ENTRYPOINT ["dotnet", "SspUis.Job.WebApi.dll"]
