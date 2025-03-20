import { Recruitment } from "./Recruitment";

export interface Category {
    id: number;
    name: string;
    recruitments: Recruitment[];
}