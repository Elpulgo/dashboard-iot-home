export interface Hue {
    lights: Light[];
}

export interface Light {
    name: string;
    hexColor: string;
    saturation?: number;
    on: boolean;
}