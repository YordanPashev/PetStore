namespace PetStore.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Requests;

    public class RequestsController : AdministrationController
    {
        private readonly IRequestsService requestsService;

        public RequestsController(IRequestsService requestsService)
            => this.requestsService = requestsService;

        [HttpGet]
        public IActionResult Index(string message = null)
        {
            ICollection<RequestViewModel> model = this.requestsService.GetAllActiveRequests()
                                                                      .To<RequestViewModel>()
                                                                      .ToArray();
            if (!string.IsNullOrEmpty(message))
            {
                this.ViewBag.Message = message;
            }

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveRequest(string id)
        {
            Request request = await this.requestsService.GetRequestByIdASync(id);

            if (request == null)
            {
                return this.RedirectToAction("Index", new { message = GlobalConstants.NoRequestFoundMessage });
            }

            await this.requestsService.RemoveRequestAsync(request);

            return this.RedirectToAction("Index", new { message = GlobalConstants.SuccessfullyRemovedRequest });
        }

        [HttpGet]
        public IActionResult InactiveRequests()
        {
            ICollection<RequestViewModel> model = this.requestsService.GetAllInactiveRequests()
                                                                      .To<RequestViewModel>()
                                                                      .ToArray();

            return this.View(model);
        }
    }
}
