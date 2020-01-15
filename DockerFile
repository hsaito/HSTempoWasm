# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:latest AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj ./aspnetapp/

# copy everything else and build app
COPY . ./aspnetapp/

WORKDIR /source/aspnetapp
RUN dotnet publish -c Release -o /app

# final stage/image
FROM nginx:latest
WORKDIR /usr/share/nginx/html
#RUN rm -rf *
COPY --from=build /app/HSTempoWasm/dist ./

EXPOSE 80
EXPOSE 443