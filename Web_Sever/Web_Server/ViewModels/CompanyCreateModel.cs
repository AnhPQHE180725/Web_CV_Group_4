﻿namespace Web_Server.ViewModels;

public class CompanyCreateModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? Logo { get; set; }
    public int Status { get; set; }
}