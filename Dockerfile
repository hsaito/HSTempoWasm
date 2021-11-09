FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj ./aspnetapp/

# copy everything else and build app
COPY . ./aspnetapp/

WORKDIR /source/aspnetapp
RUN dotnet workload install wasm-tools
RUN dotnet publish -c Release -o /app

# final stage/image
FROM nginx:latest
WORKDIR /usr/share/nginx/html
#RUN rm -rf *
COPY --from=build /app/wwwroot ./

EXPOSE 80
EXPOSE 443