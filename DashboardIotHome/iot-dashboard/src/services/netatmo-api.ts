import { AxiosResponse } from 'axios';
import { getData } from "./get-data";
import { NetatmoSerie, NetatmoCurrent } from "../models/netatmo";

class NetatmoApi {

    private readonly baseUrl = "/api/v1/data/netatmo";

    public getCurrent(): [Promise<AxiosResponse<NetatmoCurrent[]>>, () => void] {
        return getData<NetatmoCurrent[]>(`${this.baseUrl}/current`, null);
    }

    public getSeries(): [Promise<AxiosResponse<NetatmoSerie[]>>, () => void]{
        return getData<NetatmoSerie[]>(`${this.baseUrl}/series`, null);
    }
}

export default NetatmoApi;
