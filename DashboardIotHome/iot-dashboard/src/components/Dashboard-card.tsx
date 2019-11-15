
import { CardType } from "../models/card-type";
import "./Dashboard-card.css";
import WunderlistComponent from "./wunderlist-component";
import NetatmoSeriesComponent from "./netatmo-series-component";
import NetatmoCurrentComponent from "./netatmo-current-component";
import HueComponent from "./hue-component";
import React from "react";

type DashboardState = { type: CardType, title: string };

class DashboardCard extends React.Component<{ type: CardType }, DashboardState> {

  constructor(props: Readonly<any>) {
    super(props);
    this.state = { type: props.type, title: "" };
  }

  componentDidMount() {
    switch (this.state.type) {
      case CardType.NetatmoCurrent:
        this.setState({ title: "Current temperature data" });
        break;
      case CardType.NetatmoSeries:
        this.setState({ title: "Temperature/Humidity last week" });
        break;
      case CardType.PhilipsHue:
        this.setState({ title: "Philips Hue status" });
        break;
      case CardType.Wunderlist:
        this.setState({ title: "Wunderlists" });
        break;

      default:
        break;
    }
  }

  componentWillUnmount() {

  }

  renderCardType() {
    switch (this.state.type) {
      case CardType.NetatmoCurrent:
        return (<NetatmoCurrentComponent />);
      case CardType.NetatmoSeries:
        return (<NetatmoSeriesComponent />);
      case CardType.PhilipsHue:
        return (<HueComponent />);
      case CardType.Wunderlist:
        return (<WunderlistComponent />);

      default:
        return (<div>This type is not supported ... Where did you get it?</div>)
    }
  }

  render() {
    return (
      <div className="dashboard-card-container">
        <div className="dashboard-card">
          <div className="card-header">
            {this.state.title}
          </div>
          <div className="card-body">
            {this.renderCardType()}
          </div>
        </div>
      </div>
    );
  }
}

export default DashboardCard;
