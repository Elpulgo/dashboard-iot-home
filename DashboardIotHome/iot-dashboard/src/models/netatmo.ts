export interface NetatmoCurrent {
    id: string;
    name: string;
    temperature: number;
    co2: number;
    humidity: number;
    pressure: number;
    absolutePressure: number;
    minTemp: number;
    maxTemp: number;
    tempTrend: string;
    pressureTrend: string;
}

export interface NetatmoSerie {
    id: string;
    name: string;
    values: NetatmoValue[];
    type: SeriesType;
}

export interface NetatmoValue {
    value: number;
    timestamp: Date;
}

export enum SeriesType {
    Temperature,
    Humidity
}