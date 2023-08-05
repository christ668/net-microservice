using System;
using System.Collections.Generic;

namespace common.Model
{
    public partial class Activeorder
    {
        public int IdRecipe { get; set; }
        public int IdGuestTable { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Guesttable IdGuestTableNavigation { get; set; } = null!;
        public virtual Recipe IdRecipeNavigation { get; set; } = null!;
    }
}
