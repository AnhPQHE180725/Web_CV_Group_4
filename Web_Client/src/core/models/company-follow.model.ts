export interface CompanyFollow {
    id: string;
    userId: string;
    companyId: string;
    createdAt: Date;
}

export interface CompanyFollowResponse {
    followed: boolean;
    message: string;
} 