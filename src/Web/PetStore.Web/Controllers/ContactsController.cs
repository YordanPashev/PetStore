namespace PetStore.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Requests;

    public class ContactsController : BaseController
    {
        private readonly IRequestsService requestsServices;

        public ContactsController(IRequestsService requestsServices)
            => this.requestsServices = requestsServices;

        public IActionResult Index()
            => this.View();

        [HttpGet]
        public IActionResult CreateRequest()
        {
            CreateRequestViewModel model = new CreateRequestViewModel();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(CreateRequestViewModel userRequestModel)
        {
            if (!this.ModelState.IsValid || userRequestModel == null)
            {
                return this.RedirectToAction("CreateRequest", "Contacts", new { message = GlobalConstants.InvalidDataErrorMessage });
            }

            Request request = AutoMapperConfig.MapperInstance.Map<Request>(userRequestModel);
            await this.requestsServices.CreateRequest(request);

            return this.RedirectToAction("Index", "Home", new { area = string.Empty, message = GlobalConstants.SuccessfullySendedRequestMessage });
        }
    }
}
