import React from "react";
import NetatmoApi from "../services/netatmo-api";
import { NetatmoCurrent } from "../models/netatmo";
import "./netatmo.css";
import Spinner from "../utils/spinner";
import { NetatmoCurrentDataComp } from "./netatmo-current-data";

type NetatmoCurrentState = {
    currentData: NetatmoCurrent[],
    isLoading: boolean,
    error: string | null
};


export default class NetatmoCurrentComponent extends React.Component<{}, NetatmoCurrentState> {

    private _netatmoApi: NetatmoApi;
    private _abort!: () => void;

    constructor(props: Readonly<any>) {
        super(props);
        this._netatmoApi = new NetatmoApi();
        this.state = { currentData: [], isLoading: true, error: null };
    }

    public async componentDidMount() {
        try {
            const [promise, abort] = this._netatmoApi.getCurrent();
            this._abort = abort;

            const netatmoResponse = await promise;

            if (!netatmoResponse.data)
                return;

            this.setState({ currentData: netatmoResponse.data })

        } catch (error) {
            console.error(error);
            this.setState({ error: error.response.data });
        } finally {
            this.setState({ isLoading: false });
        }
    }

    componentWillUnmount() {
        this._abort();
    }

    renderData(data: NetatmoCurrent) {
        return (
            <div key={data.name}>
                <h2 className="module-header">{data.name}</h2>
                <NetatmoCurrentDataComp header="Current temp" data={data.temperature} />
                <NetatmoCurrentDataComp header="Min temp (24 h)" data={data.minTemp} />
                <NetatmoCurrentDataComp header="Max temp (24 h)" data={data.maxTemp} />
                <NetatmoCurrentDataComp header="Temp trend" data={data.tempTrend} />
                <NetatmoCurrentDataComp header="Humidity" data={data.humidity} />
                <NetatmoCurrentDataComp header="Absolute pressure" data={data.absolutePressure} />
                <NetatmoCurrentDataComp header="Pressure" data={data.pressure} />
            </div>
        );
    }

    render() {
        if (this.state.isLoading) {
            return (<Spinner isLoading={this.state.isLoading} />);
        } else if (this.state.currentData == null || (this.state.currentData.length < 1 && !this.state.isLoading)) {
            return (<h4>Failed to load Philips Hue status =( {this.state.error}</h4>);
        } else {
            return (
                <div className="netatmo-current-container">
                    {this.state.currentData.map((data: NetatmoCurrent) => this.renderData(data))}
                </div>
            );
        }
    }
}
