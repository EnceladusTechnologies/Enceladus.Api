using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Enceladus.Api.Models
{
    public static class UserClaimHelper
    {
            public static string Email(IIdentity Identity)
            {
                var cId = (ClaimsIdentity)Identity;
                return cId.Claims.First(k => k.Type.ToLower() == "email").Value;
            }

            internal static string UserAuth0Id(IIdentity Identity)
            {
                var cId = (ClaimsIdentity)Identity;
                return cId.Claims.First(k => k.Type.ToLower() == "sub").Value;
            }
    }
}
