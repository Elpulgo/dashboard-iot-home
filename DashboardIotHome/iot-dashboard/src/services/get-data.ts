import axios, { AxiosResponse, AxiosRequestConfig, AxiosError } from "axios";

export const getData = <T>(url: string, params: any): [Promise<AxiosResponse<T>>, () => void] => {

    const axiosCancelToken = axios.CancelToken.source()

    axios.defaults.baseURL = (process.env.NODE_ENV !== "production") ? "http://localhost:5000" : ""

    axios.interceptors.response.use(
        (response: AxiosResponse<any>) => {
            return response as AxiosResponse<T>;
        }, (error: AxiosError) => {
            return Promise.reject(error);
        });

    const fetchParams: AxiosRequestConfig = {
        ...params,
        cancelToken: axiosCancelToken.token,
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        }
    };

    const promise = axios.get<T>(url, fetchParams);

    return [
        promise,
        axiosCancelToken.cancel.bind(axiosCancelToken)
    ];
};