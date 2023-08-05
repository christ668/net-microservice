using common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Data.RecipeData
{
    public class RecipeData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public List<RecipeComposition> Composition { get; set; }

        public RecipeData()
        {

        }
        public RecipeData(Recipe model, List<RecipeComposition> detail)
        {
            Id = model.Id;
            Name = model.Name;
            Price = model.Price;
            Composition = detail;
        }
        public RecipeData(Recipe model, List<Composition> detail)
        {
            Id = model.Id;
            Name = model.Name;
            Price = model.Price;
            Composition = detail.ConvertAll(x => new RecipeComposition { Name = x.Name , Dose =x.Dose });
        }
    }

    public class RecipeComposition
    {
        public string Name { get; set; }
        public double Dose { get; set; }

        public RecipeComposition()
        {

        }

        public RecipeComposition(Composition model)
        {
            Name = model.Name;
            Dose = model.Dose;
        }
    }
}
