import { AxiosResponse } from 'axios';
import { Wunderlist } from './../models/wunderlist';
import { getData } from "./get-data";

class WunderlistApi {

    private readonly baseUrl = "/api/v1/data/wunderlist";

    public getWunderlist(): [Promise<AxiosResponse<Wunderlist[]>>, () => void] {
        return getData<Wunderlist[]>(this.baseUrl, null);
    }
}

export default WunderlistApi;
