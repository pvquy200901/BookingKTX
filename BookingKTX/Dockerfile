FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src

COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /publish
COPY --from=build-env /publish .
ENTRYPOINT ["dotnet", "BookingTKX.dll"]