import React from 'react';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"
import "./wunderlist.css";
import WunderlistApi from "../services/wunderlist-api";
import { Wunderlist } from "../models/wunderlist";
import Spinner from "../utils/spinner";

type WunderlistState = { wunderlists: Wunderlist[], isLoading: boolean };

export default class WunderlistComponent extends React.Component<{}, WunderlistState> {

    private _wunderlistApi: WunderlistApi;
    private _abort!: () => void;

    constructor(props: Readonly<any>) {
        super(props);
        this._wunderlistApi = new WunderlistApi();
        this.state = { wunderlists: [], isLoading: true };
    }

    public async componentDidMount() {
        try {
            const [promise, abort] = this._wunderlistApi.getWunderlist();
            this._abort = abort;

            const wunderlistResponse = await promise;

            if (!wunderlistResponse.data)
                return;

            this.setState({ wunderlists: wunderlistResponse.data })

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

    renderList(wunderList: Wunderlist) {
        return (
            <div className="list-container">
                <h4>
                    {wunderList.name}
                </h4>
                <div className="tasks">
                    {wunderList.tasks.map((task) => {
                        return <span className="task-row">{task.name} ({task.type})</span>
                    })}
                </div>
            </div>
        )
    }

    render() {
        if (this.state.isLoading) {
            return (<Spinner isLoading={this.state.isLoading} />);
        } else if (this.state.wunderlists.length < 1 && !this.state.isLoading) {
            return (
                <div className="wunderlist-container">
                    <h4>Failed to load any wunderlists =(</h4>
                </div>
            );
        } else {
            return (
                <div className="wunderlist-container">
                    {this.state.wunderlists.map(list => this.renderList(list))}
                </div>
            );
        }
    }
}