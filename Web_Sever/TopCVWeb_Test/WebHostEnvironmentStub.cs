using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace TopCVWeb_Test
{
    public class WebHostEnvironmentStub : IWebHostEnvironment
    {
        public string WebRootPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        public string EnvironmentName { get; set; } = "Development";
        public string ApplicationName { get; set; } = "Web_Server";
        public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
        public IFileProvider WebRootFileProvider { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }

    }
}
