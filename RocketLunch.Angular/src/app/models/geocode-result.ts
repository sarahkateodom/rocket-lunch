export class GeocodeResult
{
    public features: Feature[];
}

export class Feature {
    public place_type: string[];
    public text: string;
}