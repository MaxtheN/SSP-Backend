FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

ENV TZ="Asia/Tashkent"

EXPOSE 80

COPY /app /app
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*
    
ENTRYPOINT ["dotnet", "SspUis.My.WebApi.dll"]
