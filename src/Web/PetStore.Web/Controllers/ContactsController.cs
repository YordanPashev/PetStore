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
        private readonly IRequestsService requestsService;

        public ContactsController(IRequestsService requestsService)
            => this.requestsService = requestsService;

        public IActionResult Index()
            => this.View();

        [HttpGet]
        public IActionResult CreateRequest()
            => this.View(new CreateRequestViewModel());

        [HttpPost]
        public async Task<IActionResult> CreateRequest(CreateRequestViewModel userRequestModel)
        {
            if (!this.ModelState.IsValid || userRequestModel == null)
            {
                return this.RedirectToAction("CreateRequest", "Contacts", new { message = GlobalConstants.InvalidDataErrorMessage });
            }

            Request request = AutoMapperConfig.MapperInstance.Map<Request>(userRequestModel);
            await this.requestsService.CreateRequestAsync(request);

            return this.RedirectToAction("Index", "Home", new { area = string.Empty, message = GlobalConstants.SuccessfullySendedRequestMessage });
        }
    }
}
