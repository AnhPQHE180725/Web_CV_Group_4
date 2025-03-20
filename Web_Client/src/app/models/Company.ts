import { Recruitment } from "./Recruitment";

export interface Company {
    id: number;
    name: number;
    logo: string;
    recruitments: Recruitment[];
}