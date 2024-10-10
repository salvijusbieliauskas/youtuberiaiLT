using DataAccessLayer.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class DailyChannelSelectionRepository : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DailyChannelStore _dailyChannelStore;
        public DailyChannelSelectionRepository(IServiceProvider serviceProvider, DailyChannelStore dailyChannelStore)
        {
            _dailyChannelStore = dailyChannelStore;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var LtuTimeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
                var LtuNowTime = TimeZoneInfo.ConvertTimeFromUtc(now, LtuTimeZone);
                var addedTime = LtuNowTime.AddDays(1);

                var timeUntilNextRun = addedTime - LtuNowTime;
                try
                {
                    await SelectDailyChannel();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }

                await Task.Delay(timeUntilNextRun, cancellationToken);
            }
        }

        private async Task SelectDailyChannel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                int channelsCount = await dbContext.Channels.CountAsync();
                if (channelsCount > 0)
                {
                    var random = new Random();
                    bool changed = false;
                    while (!changed)
                    {
                        int randomIndex = random.Next(0, channelsCount);
                        var randomChannel = await dbContext.Channels.Skip(randomIndex)
                                .Include(x => x.Categories)
                                .ThenInclude(x => x.Category)
                                .FirstOrDefaultAsync();

                        if (randomChannel is not null)
                        {
                            if (randomChannel.Id != _dailyChannelStore.TodaysChannel.Id && randomChannel.Id != _dailyChannelStore.YesterdaysChannel.Id)
                            {
                                _dailyChannelStore.SetSelectedChannel(randomChannel);
                                changed = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
