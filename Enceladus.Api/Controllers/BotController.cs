using Enceladus.Api.Data.EnceladusRepository;
using Enceladus.Api.Helpers;
using Enceladus.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Enceladus.Api.Controllers
{
    [Route("bots")]
    [Authorize(Policy = "CustomAuthorization")]
    public class BotController : Controller
    {
        private IEnceladusRepository _repository;
        private ILogger<BotController> _logger;

        public BotController(IEnceladusRepository repository, ILogger<BotController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public JsonResult GetBots()
        {
            try
            {
                var resp = AppValueDevCache.GetBots(UserClaimHelper.UserId(User.Identity));
                var vms = resp.Select(k => k.ToListItemViewModel());
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(vms);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new Message(ex));
            }
        }

        [HttpGet("{botId}/simulate")]
        public async Task<JsonResult> SimulateBot(int botId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var bot = AppValueDevCache.GetBots(UserClaimHelper.UserId(User.Identity))
                                            .FirstOrDefault(k => k.Id == botId);
                    var simulator = new BotSimulator();
                    bot = await simulator.Simulate(bot);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(bot.ToViewModel());
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new Message(ModelState));
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new Message(ex));
            }
        }
    }
}
