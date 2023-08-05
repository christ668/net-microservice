using common.Data.UserData;
using Microsoft.AspNetCore.Mvc;

namespace admin.Services.UserService
{
    public interface IUserService
    {
        Task<List<UserData>> GetAll();
        Task<UserData> GetUserById(int Id);
        Task<UserData> GetUsername(string username);
        Task<UserData> Add(UserData User);
        Task<UserData> Update(UserData User);
        Task Delete(int Id);
    }
}
