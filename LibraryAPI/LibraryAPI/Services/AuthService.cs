using LibraryAPI.Interfaces;
using LibraryAPI.Models;

namespace LibraryAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(
            ILogger<AuthService> logger,
            AppDbContext context,
            IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }


        public string Login()
        {
            throw new NotImplementedException();
        }

        public Task<string> Logout()
        {
            throw new NotImplementedException();
        }
    }
}