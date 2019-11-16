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
Is configured in dashboard-iot-home/.env, which is used by docker-compose file.
Below is example of .env file, which tokens and client ids are needed.

Before launching it the first time, press the button on the Hue bridge in order for the app to connect to your Bridge ip address
and retireve a secret appkey. Which is then stored locally in the docker container, and used on subsequent runs. 


## Example configuration for docker
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
