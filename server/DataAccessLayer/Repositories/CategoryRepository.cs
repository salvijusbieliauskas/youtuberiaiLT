using DataAccessLayer.Interfaces;
using Domain.Models;
using FluentResults;
using Helpers.Strings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result<Category>> CreateAsync(string categoryName)
        {
            string normalizedName = categoryName.MyNormalize();
            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.NormalizedName == normalizedName);
            if (existingCategory is not null) 
            {
                return Result.Fail(ErrorTypes.Exists);
            }
            var newCategory = new Category()
            {
                Name = categoryName,
                NormalizedName = normalizedName
            };
            await _dbContext.Categories.AddAsync(newCategory);
            await _dbContext.SaveChangesAsync();

            return newCategory;
        }

        public async Task<Result> DeleteAsync(string categoryName)
        {
            string normalizedName = categoryName.MyNormalize();
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.NormalizedName == normalizedName);
            if (category is null)
            {
                return Result.Fail(ErrorTypes.NotFound);
            }
            var channelCategories = _dbContext.ChannelCategories.Where(x => x.CategoryNormalizedName == category.NormalizedName);
            if (channelCategories is not null)
            {                
                _dbContext.RemoveRange(channelCategories);
            }
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<List<Category>>> GetAllAsync()
        {
            var categories = await _dbContext.Categories.ToListAsync();
         
            return categories;
        }

        public async Task<Result<Category>> GetAsync(string categoryName)
        {
            string normalizedName = categoryName.MyNormalize();
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.NormalizedName == normalizedName);

            if (category is null) 
            {
                return Result.Fail(ErrorTypes.NotFound);
            }

            return category;
        }
    }
}
