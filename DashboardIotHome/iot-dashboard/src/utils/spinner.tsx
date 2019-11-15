import React from "react";
import GridLoader from "react-spinners/GridLoader";
import "./spinner.css";

type SpinnerState = { loading: boolean };
type SpinnerProps = { isLoading: boolean };

class Spinner extends React.Component<SpinnerProps, SpinnerState> {

    constructor(props: Readonly<SpinnerProps>) {
        super(props);

        this.state = {
            loading: props.isLoading
        };
    }
    render() {
        return (
            <div className="loader-container">
                <div className="loading">
                    <GridLoader
                        sizeUnit={"px"}
                        size={20}
                        color={"#003f5c"}
                        loading={this.state.loading}
                    />
                </div>
            </div>
        )
    }
}

export default Spinner;