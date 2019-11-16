# base dotnet core 3 image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS dotnetbuilder
WORKDIR /src
COPY DashboardIotHome/DashboardIotHome.csproj DashboardIotHome/DashboardIotHome/DashboardIotHome.csproj
RUN dotnet restore DashboardIotHome/DashboardIotHome/DashboardIotHome.csproj
COPY . ./DashboardIotHome/DashboardIotHome
WORKDIR /DashboardIotHome/src/DashboardIotHome
RUN dotnet build DashboardIotHome/DashboardIotHome.csproj -c Release -o /app

FROM dotnetbuilder AS publish
RUN dotnet publish "DashboardIotHome/DashboardIotHome.csproj" -c Release -o /app
WORKDIR /app

# base node image
FROM node:12.13 as nodebuilder
WORKDIR /app
# install and cache app dependencies
COPY ./DashboardIotHome/iot-dashboard/package.json ./package.json
COPY ./DashboardIotHome/iot-dashboard/ /app
RUN npm install
RUN npm run build

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=nodebuilder /app/build ./wwwroot
ENTRYPOINT ["dotnet", "DashboardIotHome.dll"]
