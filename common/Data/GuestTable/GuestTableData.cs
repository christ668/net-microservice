using common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Data.GuestTable
{
    public class GuestTableData
    {
        public int Id { get; set; }
        public string RoomPosition { get; set; } = null!;
        public int Chair { get; set; }
        public bool IsActive { get; set; }
        public DateTime PlacementDate { get; set; }

        public GuestTableData()
        {

        }
        public GuestTableData(Guesttable model)
        {
            Id = model.Id;
            RoomPosition = model.RoomPosition;
            Chair = model.Chair;
            IsActive = model.IsActive;
            PlacementDate = model.PlacementDate;
        }
    }
}
