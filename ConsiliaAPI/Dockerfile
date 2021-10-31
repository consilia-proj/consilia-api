FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80
ENV ASPNETCORE_URLS=http://*:80

# Development Version 
# ARG COMMIT_HASH_ARG
# ARG BRANCH_ARG

# Set environment variables
# ENV COMMIT_HASH=$COMMIT_HASH_ARG
# ENV BRANCH=$BRANCH_ARG

RUN apt-get update
RUN apt-get -y install libgdiplus
RUN apt-get -qq -y install libgdiplus

ENTRYPOINT ["dotnet", "ConsiliaAPI.dll"]
