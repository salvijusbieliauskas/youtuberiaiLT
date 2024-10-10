using Domain.Models;
using Domain.Models.Youtube;
using FluentResults;
using Helpers.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IYoutuberRepository
    {
        Task<Result<YoutubeChannel>> GetAndUpdateByIdAsync(string id);
        Task<Result<YoutubeChannel>> GetByIdAsync(string id);
        Task<Result<List<YoutubeChannel>>> GetAllAsync(SearchQueryObject query);
        Task<Result<int>> GetEntriesCount(SearchQueryObject query);
        Task<Result<YoutubeChannel>> CreateByIdAsync(string id);
        Task<Result<YoutubeChannel>> UpdateAsync(YoutubeChannel channel);
        Task<Result<YoutubeChannel>> UpdateByIdAsync(string id);
        Task<Result<YoutubeChannel>> AddCetegoryAsync(string channelId, string newCategory);
        Task<Result<YoutubeChannel>> RemoveCetegoryAsync(string channelId, string newCategory);
        Task<Result> DeleteByIdAsync(string id);
    }
}
