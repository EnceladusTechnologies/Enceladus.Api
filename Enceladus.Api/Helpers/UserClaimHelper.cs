using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Enceladus.Api.Helpers
{

    public static class UserClaimHelper
    {
        public static List<string> Roles(IIdentity Identity)
        {
            var cId = (ClaimsIdentity)Identity;
            var roles = cId.Claims.Where(k => k.Type == "roles").Select(k => k.Value).ToList();
            return roles;
        }
        public static string UserId(IIdentity Identity)
        {
            var cId = (ClaimsIdentity)Identity;
            var claimId = cId.Claims.FirstOrDefault(k => k.Type.ToLower() == ClaimTypes.NameIdentifier);
            if (claimId != null)
                return claimId.Value;
            else
                return null;
        }
        public static Guid SubscriberId(IIdentity Identity)
        {
            var cId = (ClaimsIdentity)Identity;
            var claimId = cId.Claims.FirstOrDefault(k => k.Type.ToLower() == "subscriber_id");
            if (claimId != null)
                return Guid.Parse(claimId.Value);
            else
                return Guid.Empty;
        }
        public static List<Guid> TenantIds(IIdentity Identity)
        {
            var cId = (ClaimsIdentity)Identity;
            var facility_ids = cId.Claims.Where(k => k.Type == "facility_ids").Select(k => Guid.Parse(k.Value)).ToList();
            return facility_ids;//new List<Guid>();//Guid.Parse(cId.Claims.First(k => k.Type.ToLower() == "facility_id").Value);
        }
        public static string Email(IIdentity Identity)
        {
            var cId = (ClaimsIdentity)Identity;
            var claimEmail = cId.Claims.FirstOrDefault(k => k.Type.ToLower() == "email");
            if (claimEmail != null)
                return claimEmail.Value;
            else
                return null;
        }

        internal static string UserAuth0Id(IIdentity Identity)
        {
            var cId = (ClaimsIdentity)Identity;
            return cId.Claims.First(k => k.Type.ToLower() == "sub").Value;
        }
    }
}

