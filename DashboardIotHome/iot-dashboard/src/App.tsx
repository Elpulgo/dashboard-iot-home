import React from 'react';
import './App.css';

import { CardType } from "./models/card-type";

import { library } from "@fortawesome/fontawesome-svg-core"
import { faList, faLightbulb, faCloudSun } from '@fortawesome/free-solid-svg-icons'
import DashboardCard from "./components/Dashboard-card";


const App: React.FC = () => {

  library.add(faList, faLightbulb, faCloudSun);

  return (
    <div className="App">
      <DashboardCard type={CardType.NetatmoCurrent}></DashboardCard>
      <DashboardCard type={CardType.NetatmoSeries}></DashboardCard>
      <DashboardCard type={CardType.PhilipsHue}></DashboardCard>
      <DashboardCard type={CardType.Wunderlist}></DashboardCard>
    </div>
  );
}

export default App;
