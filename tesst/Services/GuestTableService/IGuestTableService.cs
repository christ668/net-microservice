using common.Data.GuestTable;
using common.Data.UserData;

namespace admin.Services.GuestTableService
{
    public interface IGuestTableService
    {
        Task<List<GuestTableData>> GetAll();
        Task<GuestTableData> GetById(int Id);
        Task<GuestTableData> Add(GuestTableData guestTable);
        Task<GuestTableData> Update(GuestTableData guestTable);
        Task Delete(int Id);
    }
}
