﻿using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface IRecruitmentService
    {
        Task<List<Recruitment>> GetAllRecruitments();

        Task<List<Recruitment>> GetTop2Recruitments();
        Task<List<Recruitment>> GetRecruitmentsByCompany(int id);
        Task<List<Recruitment>> GetRecruitmentsByCategory(int id);
        Task<List<Recruitment>> GetRecruitmentsByCompanyName(string company);
        Task<List<Recruitment>> GetRecruitmentsByTitle(string title);
        Task<List<Recruitment>> GetRecruitmentsByLocation(string location);
    }
}
