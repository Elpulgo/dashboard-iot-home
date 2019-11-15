import React, { CSSProperties } from 'react';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"
import PhilipsHueApi from "../services/philipshue-api";
import { Light } from "../models/philips-hue";
import "./philips-hue.css";
import Spinner from "../utils/spinner";

type HueState = { lights: Light[], isLoading: boolean; };

export default class HueComponent extends React.Component<{}, HueState> {

    private _hueApi: PhilipsHueApi;
    private _abort!: () => void;

    constructor(props: Readonly<any>) {
        super(props);
        this._hueApi = new PhilipsHueApi();
        this.state = { lights: [], isLoading: true };        
    }

    public async componentDidMount() {
        try {
            const [promise, abort] = this._hueApi.getHue();
            this._abort = abort;

            const hueResponse = await promise;
            if (!hueResponse.data)
                return;

            this.setState({ lights: hueResponse.data.lights })
        } catch (error) {
            console.error(error);
        } finally {
            setTimeout(() => {
                this.setState({ isLoading: false });
            }, 1000);
        }
    }

    componentWillUnmount() {
        this._abort();
    }

    getHueStyle(hexColor: string): CSSProperties {
        return {
            width: "50px",
            height: "50px",
            borderRadius: "50%",
            backgroundColor: `#${hexColor}`
        };
    }

    render() {
        if (this.state.isLoading) {
            return (<Spinner isLoading={this.state.isLoading} />);
        } else if (this.state.lights.length < 1 && !this.state.isLoading) {
            return (
                <div className="lights-container">
                    <h4>Failed to load hue lights =(</h4>
                </div>
            );
        } else {
            const hueLights = this.state.lights
                .map((light: Light) => {
                    return (
                        <div className="light-row">
                            <FontAwesomeIcon color={light.on ? "green" : "red"} size="3x" icon="lightbulb" />
                            <span className="light-name">{light.name}</span>
                            <span style={this.getHueStyle(light.hexColor)}></span>
                        </div>
                    )
                });

            return (
                <div className="lights-container">
                    {hueLights}
                </div>
            );
        }
    }
}