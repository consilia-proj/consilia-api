FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
COPY root.crt ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
COPY root.crt ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80
ENV ASPNETCORE_URLS=http://*:80
COPY root.crt ./

# Get env vars from docker args
ARG DB_USERNAME_ARG
ARG DB_PASSWORD_ARG
ARG DB_ADDRESS_ARG
ARG DB_PORT_ARG
ARG DB_NAME_ARG
ARG GMAPS_API_KEY_ARG

# Set environment variables
ENV DB_USERNAME=$DB_USERNAME_ARG
ENV DB_PASSWORD=$DB_PASSWORD_ARG
ENV DB_ADDRESS=$DB_ADDRESS_ARG
ENV DB_PORT=$DB_PORT_ARG
ENV DB_NAME=$DB_NAME_ARG
ENV GMAPS_API_KEY=$GMAPS_API_KEY_ARG

# Needed for pfp functionality
RUN apt-get update
RUN apt-get -y install libgdiplus
RUN apt-get -qq -y install libgdiplus

ENTRYPOINT ["dotnet", "ConsiliaAPI.dll"]
