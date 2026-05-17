FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY . .
WORKDIR /app/WebAPI

RUN dotnet restore
RUN dotnet publish -c Release --output ../out

# --------------------------------------------------

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Install OpenTelemetry .NET Auto Instrumentation
RUN apt-get update && apt-get install -y curl unzip \
    && curl -L https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/latest/download/opentelemetry-dotnet-instrumentation-linux.zip -o otel.zip \
    && mkdir /otel \
    && unzip otel.zip -d /otel \
    && rm otel.zip

COPY --from=build-env /app/out .

# Performance options
ENV DOTNET_TieredPGO=1
ENV DOTNET_TC_QuickJitForLoops=1
ENV DOTNET_ReadyToRun=0

# Enable OpenTelemetry Auto Instrumentation
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={918728DD-259F-4A6A-AC2B-B85E1B658318}
ENV CORECLR_PROFILER_PATH=/otel/linux-x64/OpenTelemetry.AutoInstrumentation.Native.so
ENV DOTNET_ADDITIONAL_DEPS=/otel/AdditionalDeps
ENV DOTNET_SHARED_STORE=/otel/store
ENV DOTNET_STARTUP_HOOKS=/otel/net/OpenTelemetry.AutoInstrumentation.StartupHook.dll

# Service info
ENV OTEL_SERVICE_NAME=webapi
ENV OTEL_EXPORTER_OTLP_ENDPOINT=http://otel-collector:4317
ENV OTEL_TRACES_EXPORTER=otlp
ENV OTEL_METRICS_EXPORTER=otlp
ENV OTEL_LOGS_EXPORTER=otlp

EXPOSE 8080

ENTRYPOINT ["dotnet", "WebAPI.dll"]