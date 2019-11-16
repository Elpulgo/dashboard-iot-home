# base dotnet core 3 image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS dotnetbuilder
WORKDIR /src
COPY DashboardIotHome.csproj DashboardIotHome/DashboardIotHome.csproj
RUN dotnet restore DashboardIotHome/DashboardIotHome.csproj
COPY . ./DashboardIotHome
WORKDIR /src/DashboardIotHome
RUN dotnet build DashboardIotHome.csproj -c Release -o /app

FROM dotnetbuilder AS publish
RUN dotnet publish "DashboardIotHome.csproj" -c Release -o /app
WORKDIR /app

# base node image for frontend
FROM node:12.13 as nodebuilder
WORKDIR /app
# install and cache app dependencies
COPY ./iot-dashboard/package.json ./package.json
COPY ./iot-dashboard/ /app
RUN npm install
RUN npm run build

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=nodebuilder /app/build ./wwwroot
ENTRYPOINT ["dotnet", "DashboardIotHome.dll"]
