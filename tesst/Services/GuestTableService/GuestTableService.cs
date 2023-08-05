using admin.Services.UserService;
using common.Data.Response;
using common.Data.UserData;
using common.Helper.HashGenerator;
using common.Helper;
using common.Model;
using Newtonsoft.Json;
using Redis.Constants;
using Redis.Service;
using common.Data.GuestTable;
using Microsoft.EntityFrameworkCore;

namespace admin.Services.GuestTableService
{
    public class GuestTableService : IGuestTableService
    {
        private readonly DemoDbContext _DBContext;
        private readonly IRedisService _redisService;
        public GuestTableService(DemoDbContext DBContext, IRedisService redisService)
        {
            _DBContext = DBContext;
            _redisService = redisService;
        }

        public async Task<List<GuestTableData>> GetAll()
        {
            var Data = new List<GuestTableData>();

            var UserCacheData = await _redisService.GetCacheString(RedisKeyConstants.GUEST_TABLE_MODEL);
            if (!string.IsNullOrEmpty(UserCacheData))
            {
                Data = JsonConvert.DeserializeObject<List<GuestTableData>>(UserCacheData);
                return Data!;
            }

            Data = await _DBContext.Guesttables.Select(
             s => new GuestTableData
             {
                 Id = s.Id,
                 RoomPosition = s.RoomPosition,
                 Chair = s.Chair,
                 IsActive = s.IsActive,
                 PlacementDate = s.PlacementDate,
             }
           ).ToListAsync();

            if (Data.Count < 0) return new List<GuestTableData>();
            else
            {
                await _redisService.CacheString(RedisKeyConstants.GUEST_TABLE_MODEL, JsonConvert.SerializeObject(Data), TimeSpan.FromHours(24));
                return Data;
            }

        }
        public async Task<GuestTableData> GetById(int Id)
        {
            var all = await GetAll();
            var result = all.FirstOrDefault(t => t.Id == Id);

            if (result == null) throw new ErrorException(ErrorUtil.NoUserFound);

            return result;
        }

        public async Task<GuestTableData> Add(GuestTableData guestTable)
        {
            var entityCheck = await _DBContext.Guesttables.FirstOrDefaultAsync(s => s.Id == guestTable.Id);
            if (entityCheck != null) throw new ErrorException(ErrorUtil.GuestTableExist);

            var entity = new Guesttable()
            {
                Id = guestTable.Id,
                RoomPosition = guestTable.RoomPosition,
                Chair = guestTable.Chair,
                IsActive = guestTable.IsActive,
                PlacementDate = guestTable.PlacementDate,
            };

            _DBContext.Guesttables.Add(entity);
            await _DBContext.SaveChangesAsync();

            await _redisService.DeleteKey(RedisKeyConstants.GUEST_TABLE_MODEL);

            return new GuestTableData(entity);
        }

        public async Task<GuestTableData> Update(GuestTableData guestTable)
        {
            var entity = await _DBContext.Guesttables.FirstOrDefaultAsync(s => s.Id == guestTable.Id);
            if (entity == null) throw new ErrorException(ErrorUtil.NoTableFound);


            entity.RoomPosition = guestTable.RoomPosition;
            entity.Chair = guestTable.Chair;
            entity.IsActive = guestTable.IsActive;
            entity.PlacementDate = guestTable.PlacementDate;

            await _DBContext.SaveChangesAsync(); 

            await _redisService.DeleteKey(RedisKeyConstants.GUEST_TABLE_MODEL);

            return new GuestTableData(entity);
        }
        public async Task Delete(int Id)
        {
            var check = await _DBContext.Guesttables.FirstOrDefaultAsync(s => s.Id == Id);
            if (check == null) throw new ErrorException(ErrorUtil.NoTableFound);


            _DBContext.Guesttables.Remove(check);
            await _DBContext.SaveChangesAsync();

            await _redisService.DeleteKey(RedisKeyConstants.GUEST_TABLE_MODEL);
        }
    }
}
