namespace admin.Services.UserAuthService
{
    public interface IUserAuthService
    {
        Task<(string, DateTime)> Login(string username, string password);
        public bool ValidateToken(string token);
    }
}
