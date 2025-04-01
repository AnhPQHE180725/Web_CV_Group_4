using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Web_Server.Migrations
{
    /// <inheritdoc />
    public partial class db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CVId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FollowCompanies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recruitments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    View = table.Column<int>(type: "int", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recruitments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recruitments_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recruitments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recruitments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplyPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecruitmentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CVName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyPosts_Recruitments_RecruitmentId",
                        column: x => x.RecruitmentId,
                        principalTable: "Recruitments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplyPosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FollowJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RecruitmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowJobs_Recruitments_RecruitmentId",
                        column: x => x.RecruitmentId,
                        principalTable: "Recruitments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowJobs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Công nghệ thông tin (IT)" },
                    { 2, "Chăm sóc sức khỏe & Y tế" },
                    { 3, "Giáo dục & Giảng dạy" },
                    { 4, "Kỹ thuật & Xây dựng" },
                    { 5, "Tài chính & Kế toán" },
                    { 6, "Bán hàng & Tiếp thị" },
                    { 7, "Nhân sự (HR) & Tuyển dụng" },
                    { 8, "Dịch vụ khách hàng & Hỗ trợ" },
                    { 9, "Pháp lý & Tuân thủ" },
                    { 10, "Sáng tạo & Thiết kế" },
                    { 11, "Sản xuất & Chế tạo" },
                    { 12, "Logistics & Chuỗi cung ứng" },
                    { 13, "Bán lẻ & Thương mại điện tử" },
                    { 14, "Khách sạn & Du lịch" },
                    { 15, "Khoa học & Nghiên cứu" },
                    { 16, "Quan hệ công chúng (PR) & Truyền thông" },
                    { 17, "Công tác xã hội & Phi lợi nhuận" },
                    { 18, "Bất động sản & Quản lý tài sản" },
                    { 19, "Truyền thông & Giải trí" },
                    { 20, "Ô tô & Vận tải" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Candidate" },
                    { 2, "Recruiter" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CVId", "Description", "Email", "FullName", "Image", "Password", "PhoneNumber", "RoleId", "Status" },
                values: new object[,]
                {
                    { 1, "123 Main St", 1, "A software developer", "alice@example.com", "Alice Johnson", "alice.jpg", "AQAAAAIAAYagAAAAEOpVeVRmWyyBLJOiCemAK5aE1rpXczyHZhRqKOrAkqeEPIA8PNdXmAL3RDnZ3Kczfg==", "0123456789", 1, 1 },
                    { 2, "456 Elm St", 2, "A data analyst", "bob@example.com", "Bob Smith", "bob.jpg", "AQAAAAIAAYagAAAAED49uBUJW/uaJxWW0o5/REhZSiYq9S8ZQp7k1YjaB2T+brkmE73G8EZ6kE48wN8ZGw==", "0987654321", 1, 1 },
                    { 3, "789 Oak St", 3, "A project manager", "charlie@example.com", "Charlie Brown", "charlie.jpg", "AQAAAAIAAYagAAAAEKhOV5R+56YqM0lfLwd0pcyQmavua5DfiL5ahq7QL5+S7ycaxVIpnIP6WVzPIqbpoQ==", "0112233445", 1, 1 },
                    { 4, "321 Pine St", 4, "A system admin", "tronghieutronghieu1510@gmail.com", "Trong Hieu", "hieu.jpg", "AQAAAAIAAYagAAAAELYtHoEvP9J1txLtYs35bu/y9MtO3YXrEczQrpRPz3Gdp9IHKPKns9PhzxH/2rlqVg==", "0223344556", 2, 1 },
                    { 5, "654 Maple St", 5, "A marketing specialist", "emma@example.com", "Emma Watson", "emma.jpg", "AQAAAAIAAYagAAAAEKC+EaVueD3BNc6csceT2Mm1S2LpX2PHIgCPVUUuiQ/FD2TsKd/ZMEPI8V1uwfhw4Q==", "0334455667", 1, 1 },
                    { 6, "987 Birch St", 6, "A financial advisor", "frank@example.com", "Frank Miller", "frank.jpg", "AQAAAAIAAYagAAAAEAIrh+TkbDg2rELemhXXH2DBPl5S3d29mSHH3fySYF9kaCgjVYB9W34PPr19f5ZFDA==", "0445566778", 1, 1 },
                    { 7, "159 Cedar St", 7, "A UI/UX designer", "grace@example.com", "Grace Davis", "grace.jpg", "AQAAAAIAAYagAAAAEHWldhR5S4l/1Fi3Z6oeocztfptCO/AFvSAvA5o+R26iHsU/XwX35c+zW78ZAxCKfg==", "0556677889", 1, 1 },
                    { 8, "753 Walnut St", 8, "A content writer", "henry@example.com", "Henry Wilson", "henry.jpg", "AQAAAAIAAYagAAAAEKgnPJtttVRNL3iMylTXdbooAkPAuuU4clkxCpS0zdcRBanKIfRfl/+sBDdhC7JzUQ==", "0667788990", 1, 1 },
                    { 9, "852 Fir St", 9, "A HR manager", "isabella@example.com", "Isabella Thomas", "isabella.jpg", "AQAAAAIAAYagAAAAELmFRVm3UUGfm958SRSsPRxkmy0TK24KEQbe+6SWlUuTL3wCSBoheG0P84Jb/XvvxQ==", "0778899001", 1, 1 },
                    { 10, "951 Palm St", 10, "A sales executive", "jack@example.com", "Jack Martinez", "jack.jpg", "AQAAAAIAAYagAAAAEPRW4PTuuwOM1W8fiLVI907Upo0VUNNgJL8RZYFUfoMjf/C0VrKKOOzLeu4/uNp1Jw==", "0889900112", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "Description", "Email", "Logo", "Name", "PhoneNumber", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, "Hà Nội, Việt Nam", "Công ty phần mềm hàng đầu Việt Nam", "contact@fpt.com", "https://cdn.ketnoibongda.vn/upload/images/logo-nha-tai-tro-fpt-soft-ware-2020-05-18.png", "FPT Software", "024-73007300", 1, 4 },
                    { 2, "Cần Thơ, Việt Nam", "Công ty dược phẩm lớn nhất Việt Nam", "info@dhgpharma.com", "https://itppharma.com/wp-content/uploads/2021/03/DHG_Pharma_2.jpg", "DHG Pharma", "0292-3891433", 1, 4 },
                    { 3, "Hà Nội, Việt Nam", "Hệ thống giáo dục chất lượng cao", "info@vinschool.edu.vn", "https://apartmentvinhomes.com/wp-content/uploads/2023/03/logovins-1024x595.png", "Vinschool", "024-39757483", 1, 4 },
                    { 4, "TP. Hồ Chí Minh, Việt Nam", "Công ty xây dựng hàng đầu Việt Nam", "contact@coteccons.vn", "https://s3-symbol-logo.tradingview.com/coteccons-construction-joint-stock-company--600.png", "Coteccons", "028-38220800", 1, 4 },
                    { 5, "Hà Nội, Việt Nam", "Ngân hàng thương mại hàng đầu", "support@mbbank.com.vn", "https://static.wixstatic.com/media/9d8ed5_fc3bb8e4fd18410182baae118781f995~mv2.jpg/v1/fill/w_980,h_980,al_c,q_85,usm_0.66_1.00_0.01,enc_avif,quality_auto/9d8ed5_fc3bb8e4fd18410182baae118781f995~mv2.jpg", "MB Bank", "024-37674050", 1, 4 },
                    { 6, "TP. Hồ Chí Minh, Việt Nam", "Nền tảng thương mại điện tử lớn", "support@tiki.vn", "http://brandlogos.net/wp-content/uploads/2022/03/tiki-logo-brandlogos.net_-300x300.png", "Tiki", "1900-6035", 1, 4 },
                    { 7, "TP. Hồ Chí Minh, Việt Nam", "Công ty logistics hàng đầu", "info@gemadept.com.vn", "https://media.vneconomy.vn/w800/images/upload/2021/04/20/3cf770c4-d652-4204-bd86-a08f67114af2.jpg", "Gemadept", "028-39111333", 1, 4 },
                    { 8, "Nha Trang, Việt Nam", "Chuỗi khách sạn và nghỉ dưỡng cao cấp", "info@vinpearl.com", "https://inkythuatso.com/uploads/images/2021/09/vinpearl-logo-inkythuatso-1-13-10-21-19.jpg", "Vinpearl", "1900-232389", 1, 4 },
                    { 9, "Hà Nội, Việt Nam", "Tập đoàn đa ngành lớn nhất Việt Nam", "contact@vingroup.net", "https://s3-symbol-logo.tradingview.com/vingroup-joint-stock-company--600.png", "Vingroup", "024-39749999", 1, 4 },
                    { 10, "Hải Phòng, Việt Nam", "Hãng sản xuất ô tô hàng đầu Việt Nam", "support@vinfast.vn", "https://inkythuatso.com/uploads/images/2021/10/logo-vinfast-inkythuatso-21-11-08-55.jpg", "VinFast", "1900-232389", 1, 4 }
                });

            migrationBuilder.InsertData(
                table: "Recruitments",
                columns: new[] { "Id", "Address", "CategoryId", "CompanyId", "CreatedAt", "Deadline", "Description", "Experience", "Quantity", "Rank", "Salary", "Status", "Title", "Type", "UserId", "View" },
                values: new object[,]
                {
                    { 1, "Hà Nội", 1, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lập trình viên Java", "Senior", 5, "A", 25.0, 2, "Tuyển lập trình viên Java", "Full-time", null, 150 },
                    { 2, "TP. HCM", 1, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kỹ sư phần mềm", "Mid", 3, "B", 30.0, 3, "Tuyển kỹ sư phần mềm", "Full-time", null, 2 },
                    { 3, "Đà Nẵng", 1, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lập trình viên Fresher", "Fresher", 10, "C", 12.0, 0, "Tuyển fresher lập trình", "Intern", null, 50 },
                    { 4, "Hà Nội", 2, 2, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bác sĩ đa khoa", "Senior", 2, "A", 35.0, 5, "Tuyển bác sĩ đa khoa", "Full-time", null, 180 },
                    { 5, "TP. HCM", 2, 2, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Y tá điều dưỡng", "Mid", 5, "B", 18.0, 2, "Tuyển y tá điều dưỡng", "Full-time", null, 90 },
                    { 6, "Đà Nẵng", 2, 2, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kỹ thuật viên xét nghiệm", "Mid", 4, "B", 22.0, 3, "Tuyển kỹ thuật viên xét nghiệm", "Full-time", null, 120 },
                    { 7, "Hà Nội", 3, 3, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giáo viên tiếng Anh", "Senior", 3, "A", 30.0, 2, "Tuyển giáo viên tiếng Anh", "Full-time", null, 220 },
                    { 8, "TP. HCM", 3, 3, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trợ giảng Toán", "Junior", 4, "C", 15.0, 1, "Tuyển trợ giảng Toán", "Part-time", null, 70 },
                    { 9, "Đà Nẵng", 3, 3, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giảng viên Đại học", "Senior", 2, "A", 40.0, 5, "Tuyển giảng viên đại học", "Full-time", null, 3 },
                    { 10, "Hà Nội", 4, 4, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kỹ sư xây dựng", "Mid", 4, "B", 28.0, 3, "Tuyển kỹ sư xây dựng", "Full-time", null, 175 },
                    { 11, "TP. HCM", 4, 4, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kiến trúc sư", "Senior", 2, "A", 38.0, 4, "Tuyển kiến trúc sư", "Full-time", null, 240 },
                    { 12, "Đà Nẵng", 4, 4, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kỹ thuật viên điện", "Junior", 5, "C", 20.0, 2, "Tuyển kỹ thuật viên điện", "Full-time", null, 95 },
                    { 13, "Hà Nội", 5, 5, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kế toán trưởng", "Senior", 2, "A", 45.0, 5, "Tuyển kế toán trưởng", "Full-time", null, 260 },
                    { 14, "TP. HCM", 5, 5, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuyên viên tài chính", "Mid", 4, "B", 30.0, 3, "Tuyển chuyên viên tài chính", "Full-time", null, 180 },
                    { 15, "Đà Nẵng", 5, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nhân viên thuế", "Junior", 6, "C", 22.0, 2, "Tuyển nhân viên thuế", "Full-time", null, 110 },
                    { 16, "Hà Nội", 6, 6, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuyên viên marketing", "Mid", 4, "B", 28.0, 3, "Tuyển chuyên viên marketing", "Full-time", null, 150 },
                    { 17, "TP. HCM", 6, 6, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuyên gia SEO", "Senior", 3, "A", 35.0, 2, "Tuyển chuyên gia SEO", "Full-time", null, 2 },
                    { 18, "Đà Nẵng", 6, 6, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thực tập sinh marketing", "Fresher", 6, "C", 12.0, 1, "Tuyển thực tập sinh marketing", "Intern", null, 80 },
                    { 19, "Hà Nội", 7, 7, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chăm sóc khách hàng", "Junior", 8, "B", 18.0, 2, "Tuyển nhân viên chăm sóc khách hàng", "Full-time", null, 130 },
                    { 20, "TP. HCM", 7, 7, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nhân viên telesales", "Junior", 10, "C", 20.0, 3, "Tuyển nhân viên telesales", "Full-time", null, 110 },
                    { 21, "Đà Nẵng", 7, 9, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trưởng nhóm CSKH", "Mid", 3, "A", 28.0, 4, "Tuyển trưởng nhóm CSKH", "Full-time", null, 170 },
                    { 22, "Hà Nội", 8, 9, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quản lý kho vận", "Senior", 2, "A", 40.0, 5, "Tuyển quản lý kho vận", "Full-time", null, 250 },
                    { 23, "TP. HCM", 8, 9, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nhân viên vận hành logistics", "Mid", 6, "B", 30.0, 2, "Tuyển nhân viên vận hành logistics", "Full-time", null, 180 },
                    { 24, "Đà Nẵng", 8, 9, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tài xế giao hàng", "Junior", 10, "C", 22.0, 1, "Tuyển tài xế giao hàng", "Full-time", null, 120 },
                    { 25, "Hà Nội", 11, 9, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuyên viên pháp lý", "Mid", 4, "B", 30.0, 3, "Tuyển chuyên viên pháp lý", "Full-time", null, 90 },
                    { 26, "TP. HCM", 11, 9, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luật sư doanh nghiệp", "Senior", 2, "A", 50.0, 5, "Tuyển luật sư doanh nghiệp", "Full-time", null, 110 },
                    { 27, "Đà Nẵng", 11, 9, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trợ lý luật sư", "Junior", 6, "C", 22.0, 1, "Tuyển trợ lý luật sư", "Full-time", null, 130 },
                    { 28, "Hà Nội", 12, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế đồ họa", "Mid", 4, "B", 25.0, 2, "Tuyển thiết kế đồ họa", "Full-time", null, 150 },
                    { 29, "TP. HCM", 12, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "UI/UX Designer", "Mid", 3, "B", 30.0, 3, "Tuyển UI/UX Designer", "Full-time", null, 170 },
                    { 30, "Đà Nẵng", 12, 9, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết kế nội thất", "Mid", 4, "B", 28.0, 2, "Tuyển nhân viên thiết kế nội thất", "Full-time", null, 190 },
                    { 31, "Hà Nội", 13, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Công nhân sản xuất", "Junior", 10, "C", 15.0, 1, "Tuyển công nhân sản xuất", "Full-time", null, 210 },
                    { 32, "TP. HCM", 13, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kỹ sư cơ khí", "Mid", 4, "B", 35.0, 3, "Tuyển kỹ sư cơ khí", "Full-time", null, 230 },
                    { 33, "Đà Nẵng", 13, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giám sát sản xuất", "Senior", 3, "A", 40.0, 4, "Tuyển giám sát sản xuất", "Full-time", null, 250 },
                    { 34, "Hà Nội", 15, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nhân viên bán hàng online", "Junior", 6, "C", 14.0, 1, "Tuyển nhân viên bán hàng online", "Full-time", null, 30 },
                    { 35, "TP. HCM", 15, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quản lý cửa hàng", "Mid", 3, "B", 28.0, 3, "Tuyển quản lý cửa hàng", "Full-time", null, 50 },
                    { 36, "Đà Nẵng", 15, 1, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuyên viên TMĐT", "Mid", 4, "B", 30.0, 2, "Tuyển chuyên viên TMĐT", "Full-time", null, 70 },
                    { 37, "Hà Nội", 16, 8, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lễ tân khách sạn", "Junior", 5, "C", 16.0, 1, "Tuyển lễ tân khách sạn", "Full-time", null, 90 },
                    { 38, "TP. HCM", 16, 8, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quản lý khách sạn", "Senior", 2, "A", 45.0, 5, "Tuyển quản lý khách sạn", "Full-time", null, 110 },
                    { 39, "Đà Nẵng", 16, 8, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hướng dẫn viên du lịch", "Mid", 4, "B", 22.0, 2, "Tuyển hướng dẫn viên du lịch", "Full-time", null, 130 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyPosts_RecruitmentId",
                table: "ApplyPosts",
                column: "RecruitmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyPosts_UserId",
                table: "ApplyPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UserId",
                table: "Companies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CVs_UserId",
                table: "CVs",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FollowCompanies_CompanyId",
                table: "FollowCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowCompanies_UserId",
                table: "FollowCompanies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowJobs_RecruitmentId",
                table: "FollowJobs",
                column: "RecruitmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowJobs_UserId",
                table: "FollowJobs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recruitments_CategoryId",
                table: "Recruitments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Recruitments_CompanyId",
                table: "Recruitments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Recruitments_UserId",
                table: "Recruitments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyPosts");

            migrationBuilder.DropTable(
                name: "CVs");

            migrationBuilder.DropTable(
                name: "FollowCompanies");

            migrationBuilder.DropTable(
                name: "FollowJobs");

            migrationBuilder.DropTable(
                name: "Recruitments");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
