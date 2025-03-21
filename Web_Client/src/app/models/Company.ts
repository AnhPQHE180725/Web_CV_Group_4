import { Recruitment } from "./Recruitment";

export interface Company {
    id: number;
    name: string;
    logo: string;
    recruitments: Recruitment[];
}