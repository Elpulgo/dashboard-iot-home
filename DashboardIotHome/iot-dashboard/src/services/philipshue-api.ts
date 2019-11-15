import { AxiosResponse } from 'axios';
import { getData } from "./get-data";
import { Hue } from "../models/philips-hue";

class PhilipsHueApi {

    private readonly baseUrl = "/api/v1/data/hue";

    public getHue(): [Promise<AxiosResponse<Hue>>, () => void] {
        return getData<Hue>(this.baseUrl, null);
    }
}

export default PhilipsHueApi;
