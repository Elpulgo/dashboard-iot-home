import * as React from "react";
import Highcharts from "highcharts";
import HighchartsReact from "highcharts-react-official";

import "./Highcharts.css";
import { NetatmoSerie, SeriesType } from "../models/netatmo";

type HighchartsWrapperState = {
    chartOptions: any,
    height: string;
    width: string;
};

type HighchartsWrapperProps = {
    series: NetatmoSerie[];
}

class HighchartsWrapper extends React.Component<HighchartsWrapperProps, HighchartsWrapperState> {

    private chartRef: any;

    constructor(props: Readonly<any>) {
        super(props);

        this.state = {
            chartOptions: {
                chart: {
                    zoomType: "xy",
                },
                xAxis: {
                    type: "datetime"
                },
                yAxis: [
                    {
                        labels: {
                            format: "{value}°C"
                        },
                        title: {
                            text: "Temperature"
                        },
                        opposite: false
                    },
                    {
                        labels: {
                            format: "{value} %"
                        },
                        title: {
                            text: "Humidity"
                        },
                        min: 0,
                        max: 100,
                        opposite: true
                    }
                ],
                series: this.props.series.map(serie => {
                    return {
                        data: serie.values.map(value => [this.getTicksFromTimestamp(value.timestamp), value.value]),
                        name: serie.name,
                        yAxis: serie.type === SeriesType.Temperature ? 0 : 1,
                        dashStyle: serie.type === SeriesType.Humidity ? "shortdot" : "solid"
                    };
                }),
                credits: {
                    enabled: false
                }
            },
            height: "100%",
            width: "100%"
        };

        this.chartRef = React.createRef();
    }

    private getTicksFromTimestamp(date: Date) {
        return new Date(date).getTime();
    }

    private updateDimensions() {
        this.chartRef.current.chart.reflow();
    }

    componentDidMount() {
        window.addEventListener("resize", this.updateDimensions.bind(this));
    }

    componentWillUnmount() {

    }


    render() {
        const { chartOptions, height, width } = this.state;
        return (
            <div className="highcharts-container">
                <HighchartsReact
                    ref={this.chartRef}
                    containerProps={{ style: { height: height, width: width } }}
                    highcharts={Highcharts}
                    options={chartOptions}
                    {...this.props}
                />
            </div>
        );
    }
}

export default HighchartsWrapper;