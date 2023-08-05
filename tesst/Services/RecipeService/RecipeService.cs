using common.Data.RecipeData;
using common.Data.Response;
using common.Helper;
using common.Helper.HashGenerator;
using common.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Redis.Constants;
using Redis.Service;
using static System.Net.Mime.MediaTypeNames;

namespace admin.Services.RecipeService
{
    public class RecipeService : IRecipeService
    {
        private readonly DemoDbContext _DBContext;
        private readonly IRedisService _redisService;
        private readonly IHashGenerator _hashGenerator;
        public RecipeService(DemoDbContext DBContext, IRedisService redisService, IHashGenerator hashGenerator)
        {
            _DBContext = DBContext;
            _redisService = redisService;
            _hashGenerator = hashGenerator;
        }

        public async Task<List<RecipeData>> GetAll()
        {
            var Data = new List<RecipeData>();

            var UserCacheData = await _redisService.GetCacheString(RedisKeyConstants.RECIPE_MODEL);
            if (!string.IsNullOrEmpty(UserCacheData))
            {
                Data = JsonConvert.DeserializeObject<List<RecipeData>>(UserCacheData);
                return Data!;
            }

            Data = await _DBContext.Recipes.Select(
             s => new RecipeData(s,s.Compositions.ToList())            
           ).ToListAsync();

            if (Data.Count < 0) return new List<RecipeData>();
            else
            {
                await _redisService.CacheString(RedisKeyConstants.RECIPE_MODEL, JsonConvert.SerializeObject(Data), TimeSpan.FromHours(24));
                return Data;
            }

        }
        public async Task<RecipeData> GetById(int Id)
        {
            var all = await GetAll();
            var result = all.FirstOrDefault(t => t.Id == Id);

            if (result == null) throw new ErrorException(ErrorUtil.NoRecipeFound);

            return result;
        }
        public async Task<RecipeData> Add(RecipeData recipe)
        {
            var entityCheck = await _DBContext.Recipes.FirstOrDefaultAsync(s => s.Id == recipe.Id);
            if (entityCheck != null) throw new ErrorException(ErrorUtil.RecipeExist);

            var entity = new Recipe()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Price = recipe.Price,
            };

            _DBContext.Recipes.Add(entity);
            await _DBContext.SaveChangesAsync();

            foreach (var detail in recipe.Composition)
            {
                Composition test = new Composition();

                if(_DBContext.Compositions.Count() == 0) test.Id = 0;
                else test.Id = _DBContext.Compositions.OrderBy(t => t.Id).Last().Id + 1;
                test.Dose = detail.Dose;
                test.Name = detail.Name;
                test.RecipeId = recipe.Id;

                _DBContext.Recipes.FirstOrDefault(t => t.Id == recipe.Id)!.Compositions.Add(test);
                await _DBContext.SaveChangesAsync();
            }

            await _redisService.DeleteKey(RedisKeyConstants.RECIPE_MODEL);

            return new RecipeData(entity, recipe.Composition);
        }

        public async Task<RecipeData> Update(RecipeData recipe)
        {
            var entity = await _DBContext.Recipes.FirstOrDefaultAsync(s => s.Id == recipe.Id);
            if (entity == null) throw new ErrorException(ErrorUtil.NoRecipeFound);

            entity.Name = recipe.Name;
            entity.Price = recipe.Price;

            await _DBContext.SaveChangesAsync();


            var checkDetail = await _DBContext.Compositions.Where(s => s.RecipeId == entity.Id).ToListAsync();
            foreach (var detail in checkDetail)
            {
                _DBContext.Compositions.Remove(detail);
                await _DBContext.SaveChangesAsync();
            }

            foreach (var detail in recipe.Composition)
            {
                Composition test = new Composition();
                test.Id = _DBContext.Compositions.OrderBy(t => t.Id).Last().Id + 1;
                test.Dose = detail.Dose;
                test.Name = detail.Name;
                test.RecipeId = recipe.Id;

                _DBContext.Recipes.FirstOrDefault(t => t.Id == recipe.Id)!.Compositions.Add(test);
                await _DBContext.SaveChangesAsync();
            }

            await _redisService.DeleteKey(RedisKeyConstants.RECIPE_MODEL);

            return new RecipeData(entity, recipe.Composition);
        }

        public async Task Delete(int Id)
        {
            var check = await _DBContext.Recipes.FirstOrDefaultAsync(s => s.Id == Id);
            if (check == null) throw new ErrorException(ErrorUtil.NoRecipeFound);

            var checkDetail = await _DBContext.Compositions.Where(s=> s.RecipeId == check.Id).ToListAsync();

            foreach (var detail in checkDetail)
            {
                _DBContext.Compositions.Remove(detail);
                await _DBContext.SaveChangesAsync();
            }

            _DBContext.Recipes.Remove(check);
            await _DBContext.SaveChangesAsync();

            await _redisService.DeleteKey(RedisKeyConstants.RECIPE_MODEL);
        }
    }
}
