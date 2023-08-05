using System;
using System.Collections.Generic;

namespace common.Model
{
    public partial class Recipe
    {
        public Recipe()
        {
            Activeorders = new HashSet<Activeorder>();
            Compositions = new HashSet<Composition>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }

        public virtual ICollection<Activeorder> Activeorders { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
    }
}
