using System;
using System.Collections.Generic;

namespace common.Model
{
    public partial class Guesttable
    {
        public int Id { get; set; }
        public string RoomPosition { get; set; } = null!;
        public int Chair { get; set; }
        public bool IsActive { get; set; }
        public DateTime PlacementDate { get; set; }
    }
}
