import { Recruitment } from "./Recruitment";

export interface Company {
    id: number;
    name: string;
    description?: string;
    address?: string;
    email?: string;
    phoneNumber?: string;
    logo: string;
    status?: number;
    recruitments: Recruitment[];
}