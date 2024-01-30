FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set the working directory inside the container
WORKDIR /app

# Copy the remaining source code and build the application
COPY . .
WORKDIR /app/WebAPI
RUN dotnet restore

RUN dotnet publish -c Release --output ../out

# Create a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

ENV DOTNET_TieredPGO 1 
ENV DOTNET_TC_QuickJitForLoops 1 
ENV DOTNET_ReadyToRun 0

EXPOSE 8080
# Define the entry point for your application
ENTRYPOINT ["dotnet", "WebAPI.dll"]