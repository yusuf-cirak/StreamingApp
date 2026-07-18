FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY . .
WORKDIR /app/WebAPI

RUN dotnet restore
RUN dotnet publish -c Release --output ../out

# --------------------------------------------------

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build-env /app/out .

# Performance options
ENV DOTNET_TieredPGO=1
ENV DOTNET_TC_QuickJitForLoops=1
ENV DOTNET_ReadyToRun=0

EXPOSE 8080

ENTRYPOINT ["dotnet", "WebAPI.dll"]
