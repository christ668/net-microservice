using common.Data.Response;
using common.Data.UserData;
using common.Helper;
using common.Helper.HashGenerator;
using common.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Redis.Constants;
using Redis.Service;

namespace admin.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DemoDbContext _DBContext;
        private readonly IRedisService _redisService;
        private readonly IHashGenerator _hashGenerator;
        public UserService(DemoDbContext DBContext, IRedisService redisService, IHashGenerator hashGenerator)
        {
            _DBContext = DBContext;
            _redisService = redisService;
            _hashGenerator = hashGenerator;
        }

        public async Task<List<UserData>> GetAll()
        {
            var Data = new List<UserData>();

            var UserCacheData = await _redisService.GetCacheString(RedisKeyConstants.USER_MODEL);
            if (!string.IsNullOrEmpty(UserCacheData))
            {
                Data = JsonConvert.DeserializeObject<List<UserData>>(UserCacheData);
                return Data!;
            }

             Data = await _DBContext.Users.Select(
              s => new UserData
              {
                  Id = s.Id,
                  FirstName = s.FirstName,
                  LastName = s.LastName,
                  Username = s.Username,
                  Password = s.Password,
                  EnrollmentDate = s.EnrollmentDate
              }
            ).ToListAsync();

            if (Data.Count < 0) return new List<UserData>();
            else
            {
                await _redisService.CacheString(RedisKeyConstants.USER_MODEL, JsonConvert.SerializeObject(Data), TimeSpan.FromHours(24));
                return Data;
            }   

        }
        public async Task<UserData> GetUserById(int Id)
        {
            var all = await GetAll();
            var result = all.FirstOrDefault(t => t.Id == Id);

            if (result == null) throw new ErrorException(ErrorUtil.NoUserFound);

            return result;
        }
        public async Task<UserData> GetUsername(string username)
        {
            var all = await GetAll();
            var result = all.FirstOrDefault(t => t.Username == username);

            if (result == null) throw new ErrorException(ErrorUtil.NoUserFound);

            return result;
        }
        public async Task<UserData> Add(UserData User)
        {
            var entity = new User()
            {
                Id = User.Id,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Username = User.Username,
                Password = _hashGenerator.HashPassword(User.Password),
                EnrollmentDate = User.EnrollmentDate
            };

            _DBContext.Users.Add(entity);
            await _DBContext.SaveChangesAsync();
            await _redisService.DeleteKey(RedisKeyConstants.USER_MODEL);

            return new UserData(entity);
        }

        public async Task<UserData> Update(UserData User)
        {
            var entity = await _DBContext.Users.FirstOrDefaultAsync(s => s.Id == User.Id);
            if (entity == null) throw new ErrorException(ErrorUtil.NoUserFound);

            entity.FirstName = User.FirstName;
            entity.LastName = User.LastName;
            entity.Username = User.Username;
            entity.Password = _hashGenerator.HashPassword(User.Password);
            entity.EnrollmentDate = User.EnrollmentDate;

            await _DBContext.SaveChangesAsync();
            await _redisService.DeleteKey(RedisKeyConstants.USER_MODEL);

            return new UserData(entity);
        }
        public async Task Delete(int Id)
        {
            var check = await _DBContext.Users.FirstOrDefaultAsync(s => s.Id == Id);
            if (check == null) throw new ErrorException(ErrorUtil.NoUserFound);

            _DBContext.Users.Remove(check);
            await _DBContext.SaveChangesAsync();
            await _redisService.DeleteKey(RedisKeyConstants.USER_MODEL);
        }
    }
}
