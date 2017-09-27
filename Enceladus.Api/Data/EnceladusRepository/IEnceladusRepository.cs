using Enceladus.Api.Models;
using Enceladus.Api.Models.Bots;
using System.Collections.Generic;
using System.Security.Claims;
namespace Enceladus.Api.Data.EnceladusRepository
{
    public interface IEnceladusRepository
    {
        bool SaveAll(ClaimsPrincipal userName);
        bool SaveAll(string email);
        AppUser GetAppUserByEmail(string email);

        ICollection<BotModel> GetBots(string userId);
        ICollection<ITradingModel> GetModels();
        BotModel GetBot(int botId);
        ICollection<ConfigBase> GetModelConfigs(int modelId);
    }
}