using Enceladus.Api.Helpers;
using Enceladus.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
    }
}