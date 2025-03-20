export interface Recruitment {
    id: number;
    address: string;
    createdAt: Date;
    description: string;
    experience: string;
    quantity: number;
    rank: string;
    salary: number;
    status: number;
    title: string;
    type: string;
    view: number;
    deadline: string;
    categoryId?: number;
    companyId?: number;
    companyName: string;
    categoryName: string;
}