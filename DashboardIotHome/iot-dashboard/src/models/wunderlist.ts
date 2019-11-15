export interface Wunderlist {
    name: string;
    tasks: WunderTask[];
}

export interface WunderTask {
    name: string;
    type: string;
}