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
    [Route("models")]
    [Authorize(Policy = "CustomAuthorization")]
    public class ModelController : Controller
    {
        private IEnceladusRepository _repository;
        private ILogger<ModelController> _logger;

        public ModelController(IEnceladusRepository repository, ILogger<ModelController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public JsonResult GetModels()
        {
            try
            {
                var resp = _repository.GetModels();
                var vms = resp.Select(k => k.ToListItemViewModel());
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(vms);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new Message(ex));
            }
        }
        [HttpGet("{modelId}/configs")]
        public JsonResult GetModelConfigs(int modelId)
        {
            try
            {
                var resp = _repository.GetModelConfigs(modelId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(resp);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new Message(ex));
            }
        }
    }
}
