using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web_Server.Models;

namespace Web_Server.Data
{

    public class SeedData
    {
        private static readonly PasswordHasher<object> hasher = new PasswordHasher<object>();

        public static string HashPass(string ps)
        {
            return hasher.HashPassword(null, ps);
        }
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Candidate" },
            new Role { Id = 2, Name = "Recruiter" }
            );
            // Seed Users //Password for all users is "123"
            modelBuilder.Entity<User>().HasData(
    new User { Id = 1, FullName = "Alice Johnson", Email = "alice@example.com", Password = HashPass("123"), Address = "123 Main St", PhoneNumber = "0123456789", Image = "alice.jpg", Status = 1, Description = "A software developer", CVId = 1, RoleId = 1 },
    new User { Id = 2, FullName = "Bob Smith", Email = "bob@example.com", Password = HashPass("123"), Address = "456 Elm St", PhoneNumber = "0987654321", Image = "bob.jpg", Status = 1, Description = "A data analyst", CVId = 2, RoleId = 1 },
    new User { Id = 3, FullName = "Charlie Brown", Email = "charlie@example.com", Password = HashPass("123"), Address = "789 Oak St", PhoneNumber = "0112233445", Image = "charlie.jpg", Status = 1, Description = "A project manager", CVId = 3, RoleId = 1 },
    new User { Id = 4, FullName = "Trong Hieu", Email = "tronghieutronghieu1510@gmail.com", Password = HashPass("123"), Address = "321 Pine St", PhoneNumber = "0223344556", Image = "hieu.jpg", Status = 1, Description = "A system admin", CVId = 4, RoleId = 2 }, // RoleId = 2
    new User { Id = 5, FullName = "Emma Watson", Email = "emma@example.com", Password = HashPass("123"), Address = "654 Maple St", PhoneNumber = "0334455667", Image = "emma.jpg", Status = 1, Description = "A marketing specialist", CVId = 5, RoleId = 1 },
    new User { Id = 6, FullName = "Frank Miller", Email = "frank@example.com", Password = HashPass("123"), Address = "987 Birch St", PhoneNumber = "0445566778", Image = "frank.jpg", Status = 1, Description = "A financial advisor", CVId = 6, RoleId = 1 },
    new User { Id = 7, FullName = "Grace Davis", Email = "grace@example.com", Password = HashPass("123"), Address = "159 Cedar St", PhoneNumber = "0556677889", Image = "grace.jpg", Status = 1, Description = "A UI/UX designer", CVId = 7, RoleId = 1 },
    new User { Id = 8, FullName = "Henry Wilson", Email = "henry@example.com", Password = HashPass("123"), Address = "753 Walnut St", PhoneNumber = "0667788990", Image = "henry.jpg", Status = 1, Description = "A content writer", CVId = 8, RoleId = 1 },
    new User { Id = 9, FullName = "Isabella Thomas", Email = "isabella@example.com", Password = HashPass("123"), Address = "852 Fir St", PhoneNumber = "0778899001", Image = "isabella.jpg", Status = 1, Description = "A HR manager", CVId = 9, RoleId = 1 },
    new User { Id = 10, FullName = "Jack Martinez", Email = "jack@example.com", Password = HashPass("123"), Address = "951 Palm St", PhoneNumber = "0889900112", Image = "jack.jpg", Status = 1, Description = "A sales executive", CVId = 10, RoleId = 1 }
);




            // Seed Companies
            modelBuilder.Entity<Company>().HasData(
    new Company { Id = 1, Name = "FPT Software", Description = "Công ty phần mềm hàng đầu Việt Nam", Address = "Hà Nội, Việt Nam", Email = "contact@fpt.com", PhoneNumber = "024-73007300", Status = 1, UserId = 4, Logo = "https://cdn.ketnoibongda.vn/upload/images/logo-nha-tai-tro-fpt-soft-ware-2020-05-18.png" },
    new Company { Id = 2, Name = "DHG Pharma", Description = "Công ty dược phẩm lớn nhất Việt Nam", Address = "Cần Thơ, Việt Nam", Email = "info@dhgpharma.com", PhoneNumber = "0292-3891433", Status = 1, UserId = 4, Logo = "https://itppharma.com/wp-content/uploads/2021/03/DHG_Pharma_2.jpg" },
    new Company { Id = 3, Name = "Vinschool", Description = "Hệ thống giáo dục chất lượng cao", Address = "Hà Nội, Việt Nam", Email = "info@vinschool.edu.vn", PhoneNumber = "024-39757483", Status = 1, UserId = 4, Logo = "https://apartmentvinhomes.com/wp-content/uploads/2023/03/logovins-1024x595.png" },
    new Company { Id = 4, Name = "Coteccons", Description = "Công ty xây dựng hàng đầu Việt Nam", Address = "TP. Hồ Chí Minh, Việt Nam", Email = "contact@coteccons.vn", PhoneNumber = "028-38220800", Status = 1, UserId = 4, Logo = "https://s3-symbol-logo.tradingview.com/coteccons-construction-joint-stock-company--600.png" },
    new Company { Id = 5, Name = "MB Bank", Description = "Ngân hàng thương mại hàng đầu", Address = "Hà Nội, Việt Nam", Email = "support@mbbank.com.vn", PhoneNumber = "024-37674050", Status = 1, UserId = 4, Logo = "https://static.wixstatic.com/media/9d8ed5_fc3bb8e4fd18410182baae118781f995~mv2.jpg/v1/fill/w_980,h_980,al_c,q_85,usm_0.66_1.00_0.01,enc_avif,quality_auto/9d8ed5_fc3bb8e4fd18410182baae118781f995~mv2.jpg" },
    new Company { Id = 6, Name = "Tiki", Description = "Nền tảng thương mại điện tử lớn", Address = "TP. Hồ Chí Minh, Việt Nam", Email = "support@tiki.vn", PhoneNumber = "1900-6035", Status = 1, UserId = 4, Logo = "http://brandlogos.net/wp-content/uploads/2022/03/tiki-logo-brandlogos.net_-300x300.png" },
    new Company { Id = 7, Name = "Gemadept", Description = "Công ty logistics hàng đầu", Address = "TP. Hồ Chí Minh, Việt Nam", Email = "info@gemadept.com.vn", PhoneNumber = "028-39111333", Status = 1, UserId = 4, Logo = "https://media.vneconomy.vn/w800/images/upload/2021/04/20/3cf770c4-d652-4204-bd86-a08f67114af2.jpg" },
    new Company { Id = 8, Name = "Vinpearl", Description = "Chuỗi khách sạn và nghỉ dưỡng cao cấp", Address = "Nha Trang, Việt Nam", Email = "info@vinpearl.com", PhoneNumber = "1900-232389", Status = 1, UserId = 4, Logo = "https://inkythuatso.com/uploads/images/2021/09/vinpearl-logo-inkythuatso-1-13-10-21-19.jpg" },
    new Company { Id = 9, Name = "Vingroup", Description = "Tập đoàn đa ngành lớn nhất Việt Nam", Address = "Hà Nội, Việt Nam", Email = "contact@vingroup.net", PhoneNumber = "024-39749999", Status = 1, UserId = 4, Logo = "https://s3-symbol-logo.tradingview.com/vingroup-joint-stock-company--600.png" },
    new Company { Id = 10, Name = "VinFast", Description = "Hãng sản xuất ô tô hàng đầu Việt Nam", Address = "Hải Phòng, Việt Nam", Email = "support@vinfast.vn", PhoneNumber = "1900-232389", Status = 1, UserId = 4, Logo = "https://inkythuatso.com/uploads/images/2021/10/logo-vinfast-inkythuatso-21-11-08-55.jpg" }
);


            modelBuilder.Entity<Category>().HasData(
    new Category { Id = 1, Name = "Công nghệ thông tin (IT)" },
    new Category { Id = 2, Name = "Chăm sóc sức khỏe & Y tế" },
    new Category { Id = 3, Name = "Giáo dục & Giảng dạy" },
    new Category { Id = 4, Name = "Kỹ thuật & Xây dựng" },
    new Category { Id = 5, Name = "Tài chính & Kế toán" },
    new Category { Id = 6, Name = "Bán hàng & Tiếp thị" },
    new Category { Id = 7, Name = "Nhân sự (HR) & Tuyển dụng" },
    new Category { Id = 8, Name = "Dịch vụ khách hàng & Hỗ trợ" },
    new Category { Id = 9, Name = "Pháp lý & Tuân thủ" },
    new Category { Id = 10, Name = "Sáng tạo & Thiết kế" },
    new Category { Id = 11, Name = "Sản xuất & Chế tạo" },
    new Category { Id = 12, Name = "Logistics & Chuỗi cung ứng" },
    new Category { Id = 13, Name = "Bán lẻ & Thương mại điện tử" },
    new Category { Id = 14, Name = "Khách sạn & Du lịch" },
    new Category { Id = 15, Name = "Khoa học & Nghiên cứu" },
    new Category { Id = 16, Name = "Quan hệ công chúng (PR) & Truyền thông" },
    new Category { Id = 17, Name = "Công tác xã hội & Phi lợi nhuận" },
    new Category { Id = 18, Name = "Bất động sản & Quản lý tài sản" },
    new Category { Id = 19, Name = "Truyền thông & Giải trí" },
    new Category { Id = 20, Name = "Ô tô & Vận tải" }
);


            modelBuilder.Entity<Recruitment>().HasData(
            // IT Jobs
            new Recruitment { Id = 1, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển lập trình viên Java", Status = 2, Quantity = 5, Experience = "Senior", Salary = 25, CompanyId = 1, Description = "Lập trình viên Java", Type = "Full-time", Deadline = new DateTime(2025, 4, 30), Rank = "A", CategoryId = 1, View = 150 },

            new Recruitment { Id = 2, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển kỹ sư phần mềm", Status = 3, Quantity = 3, Experience = "Mid", Salary = 30, CompanyId = 1, Description = "Kỹ sư phần mềm", Type = "Full-time", Deadline = new DateTime(2025, 5, 10), Rank = "B", CategoryId = 1, View = 2  },

            new Recruitment { Id = 3, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển fresher lập trình", Status = 0, Quantity = 10, Experience = "Fresher", Salary = 12 , CompanyId = 1, Description = "Lập trình viên Fresher", Type = "Intern", Deadline = new DateTime(2025, 4, 15), Rank = "C", CategoryId = 1, View = 50 },

            // Healthcare Jobs
            new Recruitment { Id = 4, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển bác sĩ đa khoa", Status = 5, Quantity = 2, Experience = "Senior", Salary = 35 , CompanyId = 2, Description = "Bác sĩ đa khoa", Type = "Full-time", Deadline = new DateTime(2025, 4, 20), Rank = "A", CategoryId = 2, View = 180 },

            new Recruitment { Id = 5, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển y tá điều dưỡng", Status = 2, Quantity = 5, Experience = "Mid", Salary = 18 , CompanyId = 2, Description = "Y tá điều dưỡng", Type = "Full-time", Deadline = new DateTime(2025, 4, 25), Rank = "B", CategoryId = 2, View = 90 },

            new Recruitment { Id = 6, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển kỹ thuật viên xét nghiệm", Status = 3, Quantity = 4, Experience = "Mid", Salary = 22 , CompanyId = 2, Description = "Kỹ thuật viên xét nghiệm", Type = "Full-time", Deadline = new DateTime(2025, 4, 18), Rank = "B", CategoryId = 2, View = 120 },

            // Education Jobs
            new Recruitment { Id = 7, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển giáo viên tiếng Anh", Status = 2, Quantity = 3, Experience = "Senior", Salary = 30, CompanyId = 3, Description = "Giáo viên tiếng Anh", Type = "Full-time", Deadline = new DateTime(2025, 5, 5), Rank = "A", CategoryId = 3, View = 220 },

            new Recruitment { Id = 8, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển trợ giảng Toán", Status = 1, Quantity = 4, Experience = "Junior", Salary = 15 , CompanyId = 3, Description = "Trợ giảng Toán", Type = "Part-time", Deadline = new DateTime(2025, 4, 28), Rank = "C", CategoryId = 3, View = 70 },

            new Recruitment { Id = 9, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển giảng viên đại học", Status = 5, Quantity = 2, Experience = "Senior", Salary = 40, CompanyId = 3, Description = "Giảng viên Đại học", Type = "Full-time", Deadline = new DateTime(2025, 4, 30), Rank = "A", CategoryId = 3, View = 3 },

            // Engineering Jobs
            new Recruitment { Id = 10, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển kỹ sư xây dựng", Status = 3, Quantity = 4, Experience = "Mid", Salary = 28, CompanyId = 4, Description = "Kỹ sư xây dựng", Type = "Full-time", Deadline = new DateTime(2025, 4, 22), Rank = "B", CategoryId = 4, View = 175 },

            new Recruitment { Id = 11, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển kiến trúc sư", Status = 4, Quantity = 2, Experience = "Senior", Salary = 38, CompanyId = 4, Description = "Kiến trúc sư", Type = "Full-time", Deadline = new DateTime(2025, 5, 10), Rank = "A", CategoryId = 4, View = 240 },

            new Recruitment { Id = 12, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển kỹ thuật viên điện", Status = 2, Quantity = 5, Experience = "Junior", Salary = 20, CompanyId = 4, Description = "Kỹ thuật viên điện", Type = "Full-time", Deadline = new DateTime(2025, 4, 30), Rank = "C", CategoryId = 4, View = 95 },

            // Finance Jobs
            new Recruitment { Id = 13, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển kế toán trưởng", Status = 5, Quantity = 2, Experience = "Senior", Salary = 45, CompanyId = 5, Description = "Kế toán trưởng", Type = "Full-time", Deadline = new DateTime(2025, 4, 20), Rank = "A", CategoryId = 5, View = 260 },

            new Recruitment { Id = 14, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển chuyên viên tài chính", Status = 3, Quantity = 4, Experience = "Mid", Salary = 30, CompanyId = 5, Description = "Chuyên viên tài chính", Type = "Full-time", Deadline = new DateTime(2025, 4, 25), Rank = "B", CategoryId = 5, View = 180 },

            new Recruitment { Id = 15, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển nhân viên thuế", Status = 2, Quantity = 6, Experience = "Junior", Salary = 22, CompanyId = 1, Description = "Nhân viên thuế", Type = "Full-time", Deadline = new DateTime(2025, 4, 18), Rank = "C", CategoryId = 5, View = 110 },
             new Recruitment { Id = 16, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển chuyên viên marketing", Status = 3, Quantity = 4, Experience = "Mid", Salary = 28, CompanyId = 6, Description = "Chuyên viên marketing", Type = "Full-time", Deadline = new DateTime(2025, 4, 30), Rank = "B", CategoryId = 6, View = 150 },

    new Recruitment { Id = 17, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển chuyên gia SEO", Status = 2, Quantity = 3, Experience = "Senior", Salary = 35, CompanyId = 6, Description = "Chuyên gia SEO", Type = "Full-time", Deadline = new DateTime(2025, 5, 5), Rank = "A", CategoryId = 6, View = 2 },

    new Recruitment { Id = 18, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển thực tập sinh marketing", Status = 1, Quantity = 6, Experience = "Fresher", Salary = 12, CompanyId = 6, Description = "Thực tập sinh marketing", Type = "Intern", Deadline = new DateTime(2025, 4, 28), Rank = "C", CategoryId = 6, View = 80 },

    // Customer Service Jobs
    new Recruitment { Id = 19, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển nhân viên chăm sóc khách hàng", Status = 2, Quantity = 8, Experience = "Junior", Salary = 18, CompanyId = 7, Description = "Chăm sóc khách hàng", Type = "Full-time", Deadline = new DateTime(2025, 4, 22), Rank = "B", CategoryId = 7, View = 130 },

    new Recruitment { Id = 20, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển nhân viên telesales", Status = 3, Quantity = 10, Experience = "Junior", Salary = 20, CompanyId = 7, Description = "Nhân viên telesales", Type = "Full-time", Deadline = new DateTime(2025, 4, 25), Rank = "C", CategoryId = 7, View = 110 },

    new Recruitment { Id = 21, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển trưởng nhóm CSKH", Status = 4, Quantity = 3, Experience = "Mid", Salary = 28, CompanyId = 9, Description = "Trưởng nhóm CSKH", Type = "Full-time", Deadline = new DateTime(2025, 5, 10), Rank = "A", CategoryId = 7, View = 170 },

    // Logistics & Supply Chain Jobs
    new Recruitment { Id = 22, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển quản lý kho vận", Status = 5, Quantity = 2, Experience = "Senior", Salary = 40, CompanyId = 9, Description = "Quản lý kho vận", Type = "Full-time", Deadline = new DateTime(2025, 4, 18), Rank = "A", CategoryId = 8, View = 250 },

    new Recruitment { Id = 23, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển nhân viên vận hành logistics", Status = 2, Quantity = 6, Experience = "Mid", Salary = 30, CompanyId = 9, Description = "Nhân viên vận hành logistics", Type = "Full-time", Deadline = new DateTime(2025, 4, 30), Rank = "B", CategoryId = 8, View = 180 },

    new Recruitment { Id = 24, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển tài xế giao hàng", Status = 1, Quantity = 10, Experience = "Junior", Salary = 22, CompanyId = 9, Description = "Tài xế giao hàng", Type = "Full-time", Deadline = new DateTime(2025, 5, 10), Rank = "C", CategoryId = 8, View = 120 },
// Legal & Compliance Jobs
new Recruitment { Id = 25, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển chuyên viên pháp lý", Status = 3, Quantity = 4, Experience = "Mid", Salary = 30, CompanyId = 9, Description = "Chuyên viên pháp lý", Type = "Full-time", Deadline = new DateTime(2025, 4, 30), Rank = "B", CategoryId = 11, View = 90 },

new Recruitment { Id = 26, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển luật sư doanh nghiệp", Status = 5, Quantity = 2, Experience = "Senior", Salary = 50, CompanyId = 9, Description = "Luật sư doanh nghiệp", Type = "Full-time", Deadline = new DateTime(2025, 5, 15), Rank = "A", CategoryId = 11, View = 110 },

new Recruitment { Id = 27, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển trợ lý luật sư", Status = 1, Quantity = 6, Experience = "Junior", Salary = 22, CompanyId = 9, Description = "Trợ lý luật sư", Type = "Full-time", Deadline = new DateTime(2025, 5, 10), Rank = "C", CategoryId = 11, View = 130 },

// Creative & Design Jobs
new Recruitment { Id = 28, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển thiết kế đồ họa", Status = 2, Quantity = 4, Experience = "Mid", Salary = 25, CompanyId = 1, Description = "Thiết kế đồ họa", Type = "Full-time", Deadline = new DateTime(2025, 5, 1), Rank = "B", CategoryId = 12, View = 150 },

new Recruitment { Id = 29, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển UI/UX Designer", Status = 3, Quantity = 3, Experience = "Mid", Salary = 30, CompanyId = 1, Description = "UI/UX Designer", Type = "Full-time", Deadline = new DateTime(2025, 5, 12), Rank = "B", CategoryId = 12, View = 170 },

new Recruitment { Id = 30, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển nhân viên thiết kế nội thất", Status = 2, Quantity = 4, Experience = "Mid", Salary = 28, CompanyId = 9, Description = "Thiết kế nội thất", Type = "Full-time", Deadline = new DateTime(2025, 4, 25), Rank = "B", CategoryId = 12, View = 190 },

// Manufacturing & Production Jobs
new Recruitment { Id = 31, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển công nhân sản xuất", Status = 1, Quantity = 10, Experience = "Junior", Salary = 15, CompanyId = 1, Description = "Công nhân sản xuất", Type = "Full-time", Deadline = new DateTime(2025, 5, 2), Rank = "C", CategoryId = 13, View = 210 },

new Recruitment { Id = 32, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển kỹ sư cơ khí", Status = 3, Quantity = 4, Experience = "Mid", Salary = 35, CompanyId = 1, Description = "Kỹ sư cơ khí", Type = "Full-time", Deadline = new DateTime(2025, 4, 30), Rank = "B", CategoryId = 13, View = 230 },

new Recruitment { Id = 33, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển giám sát sản xuất", Status = 4, Quantity = 3, Experience = "Senior", Salary = 40, CompanyId = 1, Description = "Giám sát sản xuất", Type = "Full-time", Deadline = new DateTime(2025, 5, 10), Rank = "A", CategoryId = 13, View = 250 },

// Retail & E-Commerce Jobs
new Recruitment { Id = 34, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển nhân viên bán hàng online", Status = 1, Quantity = 6, Experience = "Junior", Salary = 14, CompanyId = 1, Description = "Nhân viên bán hàng online", Type = "Full-time", Deadline = new DateTime(2025, 5, 1), Rank = "C", CategoryId = 15, View = 30 },

new Recruitment { Id = 35, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển quản lý cửa hàng", Status = 3, Quantity = 3, Experience = "Mid", Salary = 28, CompanyId = 1, Description = "Quản lý cửa hàng", Type = "Full-time", Deadline = new DateTime(2025, 4, 28), Rank = "B", CategoryId = 15, View = 50 },

new Recruitment { Id = 36, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển chuyên viên TMĐT", Status = 2, Quantity = 4, Experience = "Mid", Salary = 30, CompanyId = 1, Description = "Chuyên viên TMĐT", Type = "Full-time", Deadline = new DateTime(2025, 5, 10), Rank = "B", CategoryId = 15, View = 70 },

// Hospitality & Tourism Jobs
new Recruitment { Id = 37, Address = "Hà Nội", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển lễ tân khách sạn", Status = 1, Quantity = 5, Experience = "Junior", Salary = 16, CompanyId = 8, Description = "Lễ tân khách sạn", Type = "Full-time", Deadline = new DateTime(2025, 5, 5), Rank = "C", CategoryId = 16, View = 90 },

new Recruitment { Id = 38, Address = "TP. HCM", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển quản lý khách sạn", Status = 5, Quantity = 2, Experience = "Senior", Salary = 45, CompanyId = 8, Description = "Quản lý khách sạn", Type = "Full-time", Deadline = new DateTime(2025, 5, 15), Rank = "A", CategoryId = 16, View = 110 },

new Recruitment { Id = 39, Address = "Đà Nẵng", CreatedAt = new DateTime(2025, 3, 26), Title = "Tuyển hướng dẫn viên du lịch", Status = 2, Quantity = 4, Experience = "Mid", Salary = 22, CompanyId = 8, Description = "Hướng dẫn viên du lịch", Type = "Full-time", Deadline = new DateTime(2025, 4, 28), Rank = "B", CategoryId = 16, View = 130 }

        );


        }

    }
}
