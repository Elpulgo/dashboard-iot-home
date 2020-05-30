![Build Windows Latest](https://github.com/Elpulgo/dashboard-iot-home/workflows/Build%20Windows%20Latest/badge.svg)

# dashboard-iot-home
Dashboard for IoT API:s

A dashboard built in React with typescript to learn React served by dotnetcore 3 web app.
Support for running as docker container.

Dashboard consists of 
- Netatmo current data (indoor/outdoor module)
- Netatmo historic series from last week (indoor/outdoor module)
- Philips Hue lights from Hue Bridge
- Wunderlists from up to 5 different lists

___
### Configuration (.env)
Configure Netatmo, Wunderlist and Philips Hue bridge in order for all dashboard functionality to work.
Is configured in dashboard-iot-home/.env, which is used by docker-compose file for local development.
For production from Docker Hub see <a name="sample-docker-file"></a>
Below is example of .env file, which tokens and client ids are needed.

Before launching it the first time, press the button on the Hue bridge in order for the app to connect to your Bridge ip address
and retireve a secret appkey. Which is then stored locally in the docker container, and used on subsequent runs. 


## Example configuration for docker (local dev in .env file)
```
Netatmo__ClientId=""
Netatmo__ClientSecret=""
Netatmo__DeviceId=""
Netatmo__UserName=""
Netatmo__Password=""
Netatmo__OutdoorModuleId=""
Wunderlist__ClientId=""
Wunderlist__AccessToken=""
WunderList__ListFirst=""
WunderList__ListSecond=""
PhilipsHue__BridgeIp=""
```

### [Sample prod docker-compose file](#sample-docker-file)

```
version: '3.5'

services:
  backend:
    restart: always
    privileged: true
    image: elpulgo/dashboard_iot_home:__VERSION__
    ports:      
      - "8080:80"
    networks:
      - overlay
    build:
      context: .
      dockerfile: DashboardIotHome/Dockerfile
    volumes:
      - ~/iot-dashboard/data:/app/data
    environment:
      - Netatmo__ClientId=
      - Netatmo__ClientSecret=
      - Netatmo__DeviceId=
      - Netatmo__UserName=
      - Netatmo__Password=
      - Netatmo__OutdoorModuleId=
      - Wunderlist__ClientId=
      - Wunderlist__AccessToken=
      - WunderList__ListFirst=
      - WunderList__ListSecond=
      - WunderList__ListThird=
      - WunderList__ListFourth=
      - WunderList__ListFifth=
      - PhilipsHue__BridgeIp=

networks:
  overlay:
```
