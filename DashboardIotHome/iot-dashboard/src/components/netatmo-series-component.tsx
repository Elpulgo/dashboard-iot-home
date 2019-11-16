import React from 'react';
import NetatmoApi from "../services/netatmo-api";
import { NetatmoSerie } from "../models/netatmo";
import HighchartsWrapper from "./Highcharts";
import Spinner from "../utils/spinner";

type NetatmoSeriesState = {
    series: NetatmoSerie[],
    isLoading: boolean,
    error: string | null
};


export default class NetatmoSeriesComponent extends React.Component<{}, NetatmoSeriesState> {

    private _netatmoApi: NetatmoApi;
    private _abort!: () => void;

    constructor(props: Readonly<any>) {
        super(props);
        this._netatmoApi = new NetatmoApi();
        this.state = { series: [], isLoading: true, error: null };
    }

    public async componentDidMount() {
        try {
            const [promise, abort] = this._netatmoApi.getSeries();
            this._abort = abort;

            const netatmoResponse = await promise;

            if (!netatmoResponse.data)
                return;

            this.setState({ series: netatmoResponse.data })

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

    render() {
        if (this.state.isLoading) {
            return (<Spinner isLoading={this.state.isLoading} />);
        } else if (this.state.series == null || (this.state.series.length < 1 && !this.state.isLoading)) {
            return (
                <h4>Couldn't load any series =( {this.state.error}</h4>
            );
        } else {
            return (
                <HighchartsWrapper series={this.state.series} />
            );
        }
    }
}