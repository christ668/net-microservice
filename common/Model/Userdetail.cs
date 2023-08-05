using System;
using System.Collections.Generic;

namespace common.Model
{
    public partial class Userdetail
    {
        public int Id { get; set; }
        public string? ActiveAddress { get; set; }
        public string? Nik { get; set; }
        public string MaritalStatus { get; set; } = null!;

        public virtual User IdNavigation { get; set; } = null!;
    }
}
