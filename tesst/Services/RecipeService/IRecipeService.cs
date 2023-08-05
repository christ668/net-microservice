using common.Data.RecipeData;
using common.Data.UserData;

namespace admin.Services.RecipeService
{
    public interface IRecipeService
    {
        Task<List<RecipeData>> GetAll();
        Task<RecipeData> GetById(int Id);
        Task<RecipeData> Add(RecipeData User);
        Task<RecipeData> Update(RecipeData User);
        Task Delete(int Id);
    }
}
