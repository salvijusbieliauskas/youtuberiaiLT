using Domain.CSVModels;
using Domain.Models.Youtube;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Helpers.Mappers;
using Helpers.CSV;
using Services.YoutubeAPI;
using System.Diagnostics;
using Helpers.Strings;
using Domain.Models;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.JsonPatch;
using Domain.Enums;
using DataAccessLayer.Stores;
using Microsoft.AspNetCore.Authorization;
using Helpers.RequestObjects;
using Domain;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoutubersController : ControllerBase
    {
        private readonly IYoutuberRepository _ytRepository;
        private readonly DailyChannelStore _dailyChannelStore;
        public YoutubersController(IYoutuberRepository ytRepository, DailyChannelStore dailyChannelStore)
        {
            _ytRepository = ytRepository;
            _dailyChannelStore = dailyChannelStore;
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount([FromQuery] SearchQueryObject queryObject)
        {
            var result = await _ytRepository.GetEntriesCount(queryObject);
            if (result.IsSuccess)
            {                
                return Ok(result.Value);
            }
            return BadRequest();
        }

        [HttpGet("{channelId}")]
        public async Task<IActionResult> GetChannel(string channelId, [FromQuery] bool update = true)
        {
            //YoutubeChannel? channel = null;

            var result = update switch
            { 
                 true => await _ytRepository.GetAndUpdateByIdAsync(channelId),
                 false => await _ytRepository.GetByIdAsync(channelId),
            };

            if (result.IsFailed) 
            {
                switch (result.Errors[0].Message) 
                {
                    case ErrorTypes.NotFound: return NotFound(new {message= "Youtube kanalas nerastas." });
                }
            }

            if (result.IsSuccess) 
            {
                return Ok(result.Value);
            }
            return BadRequest();
        }

        [HttpGet("daily")]
        public IActionResult GetDailyChannel()
        {
            var channel = _dailyChannelStore.TodaysChannel;
            if (channel is null)
            {
                return NotFound();
            }

            return Ok(channel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SearchQueryObject queryRequest)
        {
            var result = await _ytRepository.GetAllAsync(queryRequest);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest();
        }

        [HttpPatch("{channelId}")]
        [Authorize]
        public async Task<IActionResult> PatchCategory(string channelId, [FromBody] ChannelCategoryPatchBodyObject request)
        {
            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                var result = request.PatchMethod switch
                {
                    PatchMethod.Add => await _ytRepository.AddCetegoryAsync(channelId, request.Category),
                    PatchMethod.Remove => await _ytRepository.RemoveCetegoryAsync(channelId, request.Category),
                    _ => null
                };

                if (result is not null)
                {
                    if (result.IsFailed)
                    {
                        switch (result.Errors[0].Message)
                        {
                            case ErrorTypes.NotFound: return NotFound(new { message = "Youtube kanalas nerastas." });
                        }
                    }

                    if (result.IsSuccess)
                    {
                        return Ok(result.Value);
                    }
                }
            }
            else
            {
                return BadRequest(new { message = "Neįvardinta kategorija." });                
            }

            return BadRequest();
        }

        [HttpPut("{channelId}")]
        public async Task<IActionResult> Update(string channelId)
        {
            var result = await _ytRepository.UpdateByIdAsync(channelId);
            if (result.IsFailed)
            {
                switch (result.Errors[0].Message)
                {
                    case ErrorTypes.NotFound: return NotFound(new { message = "Youtube kanalas nerastas." });
                }
            }
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest();
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] string channelId)
        {
            var result = await _ytRepository.CreateByIdAsync(channelId);
            if (result.IsSuccess)
            {                
                return Ok(result.Value);
            }
            if (result.IsFailed)
            {
                switch (result.Errors[0].Message)
                {                    
                    case ErrorTypes.NotFound: return NotFound(new { message = "Youtube kanalas nerastas." });
                    case ErrorTypes.Exists: return Conflict(new { message = "Youtube kanalas jau egzistuoja." });
                    default: return BadRequest(new { message = "Nepavyko pridėti." });
                }

            }
            return BadRequest();
        }

        [HttpDelete("{channelId}")]
        [Authorize]
        public async Task<IActionResult> Delete(string channelId)
        {
            var result = await _ytRepository.DeleteByIdAsync(channelId);
            if (result.IsFailed)
            {
                switch (result.Errors[0].Message)
                {
                    case ErrorTypes.NotFound: return NotFound(new { message = "Youtube kanalas nerastas." });
                }
            }
            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest();
        }

    }
}
