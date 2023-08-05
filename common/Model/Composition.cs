using System;
using System.Collections.Generic;

namespace common.Model
{
    public partial class Composition
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double Dose { get; set; }
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; } = null!;
    }
}
