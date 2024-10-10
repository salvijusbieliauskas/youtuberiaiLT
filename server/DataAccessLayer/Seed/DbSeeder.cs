using Domain.CSVModels;
using Domain.Models;
using Domain.Models.Youtube;
using Helpers.CSV;
using Helpers.Strings;
using Services.YoutubeAPI;
using Helpers.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Seed
{
    public class DbSeeder
    {
        private readonly AppDbContext _dbContext;
        private readonly IYoutubeService _ytService;
        public DbSeeder(AppDbContext dbContext, IYoutubeService ytService)
        {
            _dbContext = dbContext;
            _ytService = ytService;
        }

        public async Task SeedCategoriesFromCSV()
        {
            string categoriesFilePath = @"..\LietuvosYoutubeCategories.csv";
            if (File.Exists(categoriesFilePath))
            {
                if (!_dbContext.Categories.Any())
                {
                    var csvCategories = CSVParser<CategoryCSV>.TryParse(categoriesFilePath, ";");
                    var categories = new List<Category>();
                    foreach (var csvCategory in csvCategories)
                    {
                        var newCategory = new Category()
                        {
                            Name = csvCategory.Name,
                            NormalizedName = csvCategory.Name.MyNormalize()
                        };
                        categories.Add(newCategory);
                    }

                    if (categories.Any())
                    {
                        categories = categories.DistinctBy(x => x.NormalizedName).ToList();
                        await _dbContext.Categories.AddRangeAsync(categories);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
        }
        public async Task SeedChannelsFromCSV()
        {
            string channelsFilePath = @"..\LietuvosYoutube.csv";
            if (File.Exists(channelsFilePath))
            {
                if (!_dbContext.Channels.Any())
                {
                    var csvChannels = CSVParser<ChannelCSV>.TryParse(channelsFilePath, ";");

                    List<YoutubeChannel> channels = new List<YoutubeChannel>();
                    List<ChannelCategory> channelCategories = new List<ChannelCategory>();
                    foreach (var csvChannel in csvChannels)
                    {
                        var response = _ytService.getChannel(csvChannel.Handle).GetAwaiter().GetResult();
                        if (response is null)
                        {
                            continue;
                        }
                        var channel = response.ToYoutubeChannel();
                        foreach (var category in csvChannel.Categories)
                        {
                            var newChannelCategory = new ChannelCategory()
                            {
                                ChannelId = channel.Id,
                                CategoryName = category,
                                CategoryNormalizedName = category.MyNormalize()
                            };

                            var existingCatInDB = await _dbContext.Categories
                                .FirstOrDefaultAsync(x => x.NormalizedName == newChannelCategory.CategoryNormalizedName);
                            if (existingCatInDB is null)
                            {
                                var newCategory = new Category()
                                {
                                    Name = newChannelCategory.CategoryName,
                                    NormalizedName = newChannelCategory.CategoryNormalizedName
                                };
                                await _dbContext.Categories.AddAsync(newCategory);
                                await _dbContext.SaveChangesAsync();
                            }
                            var existingCatInChannel = channel.Categories
                                .FirstOrDefault(c => c.CategoryName == newChannelCategory.CategoryName || c.CategoryNormalizedName == newChannelCategory.CategoryNormalizedName);
                            if (existingCatInChannel is null && !channel.Categories.Contains(existingCatInChannel))
                            {
                                channel.Categories.Add(newChannelCategory);
                            }
                        }

                        channels.Add(channel);
                    }


                    if (channels.Any())
                    {
                        channels = channels.DistinctBy(x => x.Id).ToList();
                        await _dbContext.Channels.AddRangeAsync(channels);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }

        }
    }
}
