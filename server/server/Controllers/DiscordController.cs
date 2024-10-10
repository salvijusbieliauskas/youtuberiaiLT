using Discord;
using Discord.WebSocket;
using Google.Apis.YouTube.v3.Data;
using Helpers.RequestObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Services.DiscordSevice;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscordController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly string _token = "";
        private readonly ulong _channelId = 0;
        private readonly DiscordSocketClient _client = new DiscordSocketClient();
        public DiscordController(IConfiguration config)
        {
            _config = config;
            _token = config["Discord:BotToken"];
            _channelId = UInt64.Parse(config["Discord:ChannelId"]);
        }

        [HttpPost]
        public async Task<IActionResult> Message([FromBody] DiscordMsgBodyObject requestObject)
        {
            string categoriesString = "";
            if (requestObject.Categories is not null)
            {
                if (string.IsNullOrWhiteSpace(requestObject.Message) || requestObject.Categories.Count() > 5)
                {
                    return BadRequest(new { message = "Pasiūlymo laukas yra privalomas." });
                }
                categoriesString = "\ntags: " + stringifyCategories(requestObject.Categories);
            }
            else
            {
                categoriesString = "No tags provided.";
            }

            string suggestionString = "pasiūlymas:   " + requestObject.Message;

            string completeMessage =
                suggestionString
                + "\n--------------------------"
                + categoriesString
                + "----------------------------";

            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();

            var messageChannel = await _client.GetChannelAsync(_channelId) as IMessageChannel;

            var response = await messageChannel!.SendMessageAsync(completeMessage);

            await _client.DisposeAsync();
            if (response is not null)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        string stringifyCategories(List<string> tags)
        {
            string stringyTags = "";
            int index = 1;
            foreach (var tag in tags)
            {
                if (index == 1)
                {
                    stringyTags = stringyTags + "       " + tag + "\n";
                }
                else
                {
                    stringyTags = stringyTags + "                  " + tag + "\n";
                }
                index++;
            }
            return stringyTags;
        }
    }
}
