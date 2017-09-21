using Enceladus.Api.Data.EnceladusRepository;
using Enceladus.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Enceladus.Api.Controllers
{
    [Route("accounts")]
    [Authorize]
    public class AccountController : Controller
    {
        private IEnceladusRepository _repository;
        private ILogger<AccountController> _logger;
        public AccountController(IEnceladusRepository repository, ILogger<AccountController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [HttpGet("")]

        public JsonResult test()
        {
            return Json("Ok");
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<JsonResult> Login([FromBody] LoginViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // check if any values tracked in Auth0 have changed
                    LoginResponseViewModel responseVM = await Auth0Login(vm);

                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(responseVM);
                }

                else
                {
                    _logger.LogError("Invalid login model.");
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new Message(ModelState));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to retrieve user.", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new Message(ex));
            }
        }

        private async Task<LoginResponseViewModel> Auth0Login(LoginViewModel vm)
        {
            /* var authClient = new AuthenticationApiClient(new Uri(Startup.Configuration["AUTH0_DOMAIN"]));

            


            AuthorizationCodeTokenRequest request = new AuthorizationCodeTokenRequest()
            {
                ClientId = Startup.Configuration["AUTH0_CLIENT_ID"],
                ClientSecret = Startup.Configuration["AUTH0_CLIENT_SECRET"],
                

            }
            AuthenticationRequest request = new AuthenticationRequest()
            {
                Username = vm.username,
                Password = vm.password,
                ClientId = Startup.Configuration["AUTH0_CLIENT_ID"],
                Connection = "Username-Password-Authentication",
                GrantType = "password",
                Scope = "openid role email logins_count"

            };
            var resp = await authClient.GetTokenAsync(request);

            var uInfo = await authClient.GetUserInfoAsync(resp.AccessToken);


            AppUser user = _repository.GetAppUserByEmail(vm.username);

            var mgmtClient = new ManagementApiClient(Startup.Configuration["AUTH0_API_TOKEN_GLOBAL"], new Uri(Startup.Configuration["AUTH0_DOMAIN"] + "/api/v2"));
            Auth0.Core.User userResp = null;
            try
            {
                userResp = await mgmtClient.Users.GetAsync(user.AuthId);
            }
            catch (Exception ex)
            {
                throw ex; // If we got this far it means the auth0Id in the database doesn't match up in Auth0
            }*/

            LoginResponseViewModel responseVM = new LoginResponseViewModel()
            {
                AccessToken = "",
                IdToken = "",
                RefreshToken = "",
                TokenType = "token"
            };

            return responseVM;
        }


    }
}
