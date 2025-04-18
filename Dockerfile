FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
LABEL maintainer="DigitalTrade"
WORKDIR /src
COPY . .
RUN dotnet restore ./DigitalTrade.Payment.Host/DigitalTrade.Payment.Host.csproj && \
    dotnet publish ./DigitalTrade.Payment.Host/DigitalTrade.Payment.Host.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /src
COPY --from=build-env /src/out .
ENTRYPOINT ["dotnet", "DigitalTrade.Payment.Host.dll"]