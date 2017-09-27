using Enceladus.Api.Helpers;
using Enceladus.Api.Models;
using Enceladus.Api.Models.Bots;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Enceladus.Api.Data.EnceladusRepository
{
    public class EnceladusRepository : IEnceladusRepository
    {
        private EnceladusContext _context;

        public EnceladusRepository(EnceladusContext context)
        {
            _context = context;
        }

        public bool SaveAll(ClaimsPrincipal user)
        {
            var email = UserClaimHelper.Email(user.Identity);
            return SaveAll(email);
        }

        public bool SaveAll(string email)
        {
            foreach (var history in _context.ChangeTracker.Entries()
                            .Where(e => e.Entity is IModificationHistory &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified))
                            .Select(e => e.Entity as IModificationHistory))
            {
                history.ModifiedByUser = email;
                if (string.IsNullOrEmpty(history.CreatedByUser))
                {
                    history.CreatedByUser = email;
                }

                history.DateModified = DateTime.UtcNow;
                if (history.DateCreated == DateTime.MinValue)
                {
                    history.DateCreated = DateTime.UtcNow;
                }
            }

            return _context.SaveChanges() > 0;
        }

        public AppUser GetAppUserByEmail(string email)
        {
            return _context.AppUsers.Where(k => k.Email == email).FirstOrDefault();
        }

        public ICollection<BotModel> GetBots(string userId)
        {
            return AppValueDevCache.GetBots(userId);
        }

        public ICollection<ITradingModel> GetModels()
        {
            return AppValueDevCache.GetModels();
        }

        public BotModel GetBot(int botId)
        {
            var models = AppValueDevCache.GetBots("0");
            return models.FirstOrDefault(k => k.Id == botId);
        }

        public ICollection<ConfigBase> GetModelConfigs(int modelId)
        {
            var model = AppValueDevCache.GetModels().FirstOrDefault(k => k.Id == modelId);
            return model.ConfigurationQuestions;
        }
    }
}