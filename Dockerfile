# Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env


#EXPOSE $PORT

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

# Run the app on container startup
# Use your project name for the second parameter
# e.g. MyProject.dll
#ENTRYPOINT [ "dotnet", "spbu.dll" ]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet spbu.dll