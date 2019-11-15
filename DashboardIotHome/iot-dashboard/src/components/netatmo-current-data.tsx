import React from "react";

type CurrentDataProp = { header: string, data: string | number };

export const NetatmoCurrentDataComp: React.FC<CurrentDataProp> = (props) => {
  return (
    <div className="current-row-container">
      <span className="header">{props.header}</span>
      <span className="data">{props.data}</span>
    </div>
  );
}