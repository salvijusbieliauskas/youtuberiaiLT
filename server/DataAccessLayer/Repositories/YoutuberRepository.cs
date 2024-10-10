using DataAccessLayer.Interfaces;
using Domain;
using Domain.Enums;
using Domain.Models;
using Domain.Models.Youtube;
using FluentResults;
using Helpers.Mappers;
using Helpers.RequestObjects;
using Helpers.Strings;
using Microsoft.EntityFrameworkCore;
using Services.YoutubeAPI;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccessLayer.Repositories
{
    public class YoutuberRepository : IYoutuberRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IYoutubeService _ytService;
        public YoutuberRepository(AppDbContext dbContext, IYoutubeService ytService)
        {
            _dbContext = dbContext;
            _ytService = ytService;
        }

        public async Task<Result<YoutubeChannel>> RemoveCetegoryAsync(string channelId, string category)
        {
            string normalizedCategory = category.MyNormalize();
            var channel = await _dbContext.Channels
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == channelId);
            if (channel is null)
            {
                return Result.Fail(ErrorTypes.NotFound);
            }
            var categoryToRemove = channel.Categories
                .FirstOrDefault(x => x.CategoryNormalizedName == normalizedCategory);

            if (categoryToRemove != null) 
            {
                channel.Categories.Remove(categoryToRemove);
                await _dbContext.SaveChangesAsync();
            }

            return channel;
        }

        public async Task<Result<YoutubeChannel>> AddCetegoryAsync(string chanelId, string newCategory)
        {
            var channel = await _dbContext.Channels
             .Include(x => x.Categories)
             .ThenInclude(x => x.Category)
             .FirstOrDefaultAsync(x => x.Id == chanelId);
            if (channel is null)
            {
                return Result.Fail(ErrorTypes.NotFound);
            }
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.Name.ToLower() == newCategory.ToLower()
                || x.NormalizedName.ToLower() == newCategory.ToLower());
            if (category is null)
            {
                category = new Category()
                {
                    Name = newCategory,
                    NormalizedName = newCategory.MyNormalize(),
                };
                await _dbContext.Categories.AddAsync(category);
            }
            var channelCategory = new ChannelCategory()
            {
                ChannelId = chanelId,
                CategoryName = category.Name,
                CategoryNormalizedName = category.NormalizedName,
            };
            if (!channel.Categories
                .Any(x => x.CategoryName.ToLower() == category.Name.ToLower()
                || x.CategoryNormalizedName == category.NormalizedName))
            {
                channel.Categories.Add(channelCategory);
                await _dbContext.ChannelCategories.AddAsync(channelCategory);
            }

            await _dbContext.SaveChangesAsync();

            return channel;
        }

        public async Task<Result<YoutubeChannel>> CreateByIdAsync(string id)
        {
            
            var channelResponse = await _ytService.getChannelById(id);
            if (channelResponse is null)
            {
                return Result.Fail(ErrorTypes.NotFound);
            }
            var channel = channelResponse.ToYoutubeChannel();
            var channelFromDb = await _dbContext.Channels
                 .Include(x => x.Categories)
                 .ThenInclude(x => x.Category)
                 .FirstOrDefaultAsync(x => x.Id == channel.Id);
            if (channelFromDb is not null)
            {
                return Result.Fail(ErrorTypes.Exists);
            }
            await _dbContext.Channels.AddAsync(channel);
            await _dbContext.SaveChangesAsync();

            return Result.Ok(channel);
        }

        public async Task<Result> DeleteByIdAsync(string id)
        {
            var existingChannel = await _dbContext.Channels.FirstOrDefaultAsync(c => c.Id == id);
            if (existingChannel is null)
            {
                return Result.Fail(ErrorTypes.NotFound);
            }

            _dbContext.Channels.Remove(existingChannel);
            await _dbContext.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result<List<YoutubeChannel>>> GetAllAsync(SearchQueryObject query)
        {
            IQueryable<YoutubeChannel>? channels = channels = _dbContext.Channels.Include(x => x.Categories).ThenInclude(c => c.Category).AsQueryable();


            if (!string.IsNullOrWhiteSpace(query.ChannelHandle))
            {
                channels = channels.Where(x => x.Title.Contains(query.ChannelHandle) || x.CustomUrl.Contains(query.ChannelHandle));
            }

            if (query.IncludeCategories is not null && query.IncludeCategories.Any())
            {
                query.IncludeCategories = query.IncludeCategories.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                if (query.IncludeCategories.Count() > 0)
                {
                    channels = channels.Where(c => query.IncludeCategories
                        .Any(category => c.Categories
                        .Any(x => x.CategoryName.ToLower().Contains(category.ToLower())
                        || x.CategoryNormalizedName.ToLower().Contains(category.ToLower()))));
                }
            }
            if (query.ExcludeCategories is not null && query.ExcludeCategories.Any())
            {
                query.ExcludeCategories = query.ExcludeCategories.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                if (query.ExcludeCategories.Count() > 0)
                {
                    channels = channels.Where(c => !query.ExcludeCategories
                        .Any(category => c.Categories
                        .Any(x => x.CategoryName.ToLower().Contains(category.ToLower())
                        || x.CategoryNormalizedName.ToLower().Contains(category.ToLower()))));
                }
            }
            if (query.SortOrder is not null)
            {
                // By Title Sorting
                if (query.SortOrder == Domain.Enums.SortOrder.ByHandleAscending)
                {
                    channels = channels.OrderBy(x => x.Title);
                }
                if (query.SortOrder == Domain.Enums.SortOrder.ByHandleDescending)
                {
                    channels = channels.OrderByDescending(x => x.Title);
                }

                // By Sub Count Sorting
                if (query.SortOrder == Domain.Enums.SortOrder.BySubCountAscending)
                {
                    channels = channels.OrderBy(x => x.SubscriberCount);
                }
                if (query.SortOrder == Domain.Enums.SortOrder.BySubCountDescending)
                {
                    channels = channels.OrderByDescending(x => x.SubscriberCount);
                }
            }


            int skipNumber = (query.PageNumber - 1) * query.PageSize;
            return await channels.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        
        

        public async Task<Result<YoutubeChannel>> GetAndUpdateByIdAsync(string id)
        {
            var existingChannel = await _dbContext.Channels.FirstOrDefaultAsync(x => x.Id == id);
            if (existingChannel is null)
            {
                return Result.Fail(ErrorTypes.NotFound);
            }
            var channelResponse = await _ytService.getChannelById(id);
            if (channelResponse is null)
            {
                return Result.Fail(ErrorTypes.NotFound);
            }
            var channel = channelResponse.ToYoutubeChannel();
            channel = UpdateAsync(channel).Result.Value;

            return channel;
        }

        public async Task<Result<YoutubeChannel>> GetByIdAsync(string id)
        {
            var channel = await _dbContext.Channels.Include(x => x.Categories).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (channel is null)
            {
                return Result.Fail(ErrorTypes.NotFound);
            }

            return channel;
        }



        public async Task<Result<YoutubeChannel>> UpdateAsync(YoutubeChannel channel)
        {
            var existingChannel = await _dbContext.Channels.Include(x => x.Categories).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == channel.Id);
            if (existingChannel is null)
            {
                return Result.Fail(ErrorTypes.NotFound);
            }
            existingChannel.Title = channel.Title;
            existingChannel.Description = channel.Description;
            existingChannel.CustomUrl = channel.CustomUrl;
            existingChannel.Thumbnail = channel.Thumbnail;
            existingChannel.SubscriberCount = channel.SubscriberCount;
            existingChannel.UpdateDate();

            await _dbContext.SaveChangesAsync();

            return existingChannel;
        }


        public async Task<Result<YoutubeChannel>> UpdateByIdAsync(string id)
        {
            var existingChannel = await _dbContext.Channels.Include(x => x.Categories).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (existingChannel is null)
            {
                return Result.Fail(ErrorTypes.NotFound);
            }

            var channelResponse = await _ytService.getChannelById(id);

            if (channelResponse is null)
            {
                return Result.Fail(ErrorTypes.NotFound);
            }
            var updatedChannel = channelResponse.ToYoutubeChannel();

            existingChannel.Thumbnail = updatedChannel.Thumbnail;
            existingChannel.Description = updatedChannel.Description;
            existingChannel.Title = updatedChannel.Title;
            existingChannel.SubscriberCount = updatedChannel.SubscriberCount;
            existingChannel.CustomUrl = updatedChannel.CustomUrl;
            existingChannel.UpdateDate();

            await _dbContext.SaveChangesAsync();

            return existingChannel;
        }

        public async Task<Result<int>> GetEntriesCount(SearchQueryObject query)
        {

            IQueryable<YoutubeChannel>? channels = channels = _dbContext.Channels.Include(x => x.Categories).ThenInclude(c => c.Category).AsQueryable();


            if (!string.IsNullOrWhiteSpace(query.ChannelHandle))
            {
                channels = channels.Where(x => x.Title.Contains(query.ChannelHandle) || x.CustomUrl.Contains(query.ChannelHandle));
            }

            if (query.IncludeCategories is not null && query.IncludeCategories.Any())
            {
                query.IncludeCategories = query.IncludeCategories.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                if (query.IncludeCategories.Count() > 0)
                {
                    channels = channels.Where(c => query.IncludeCategories
                        .Any(category => c.Categories
                        .Any(x => x.CategoryName.ToLower().Contains(category.ToLower())
                        || x.CategoryNormalizedName.ToLower().Contains(category.ToLower()))));
                }
            }
            if (query.ExcludeCategories is not null && query.ExcludeCategories.Any())
            {
                query.ExcludeCategories = query.ExcludeCategories.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                if (query.ExcludeCategories.Count() > 0)
                {
                    channels = channels.Where(c => !query.ExcludeCategories
                        .Any(category => c.Categories
                        .Any(x => x.CategoryName.ToLower().Contains(category.ToLower())
                        || x.CategoryNormalizedName.ToLower().Contains(category.ToLower()))));
                }
            }
            if (query.SortOrder is not null)
            {
                // By Title Sorting
                if (query.SortOrder == Domain.Enums.SortOrder.ByHandleAscending)
                {
                    channels = channels.OrderBy(x => x.Title);
                }
                if (query.SortOrder == Domain.Enums.SortOrder.ByHandleDescending)
                {
                    channels = channels.OrderByDescending(x => x.Title);
                }

                // By Sub Count Sorting
                if (query.SortOrder == Domain.Enums.SortOrder.BySubCountAscending)
                {
                    channels = channels.OrderBy(x => x.SubscriberCount);
                }
                if (query.SortOrder == Domain.Enums.SortOrder.BySubCountDescending)
                {
                    channels = channels.OrderByDescending(x => x.SubscriberCount);
                }
            }

            return await channels.CountAsync();
        }
    }
}
