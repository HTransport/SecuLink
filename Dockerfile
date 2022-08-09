FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /SecuLink

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /SecuLink
COPY --from=build-env /SecuLink/out .
ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]