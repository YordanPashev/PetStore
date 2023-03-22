namespace PetStore.Web.Infrastructures
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Web.Controllers;
    using PetStore.Web.ViewModels.User;

    public class UserControllerExtension : BaseController
    {
        private readonly IUsersService userService;

        public UserControllerExtension(IUsersService userService)
            => this.userService = userService;

        public async Task<IActionResult> EditAndRedirectOrReturnInvalidInputMessage(EditUserViewModel editModel)
        {
            ApplicationUser user = await this.userService.GetActiveUserByIdForEditAsync(editModel.Id);

            if (!this.IsUserEdited(editModel, user) || editModel.Id != user.Id)
            {
                return this.RedirectToAction("Edit", new { id = editModel.Id, message = GlobalConstants.EditMessage });
            }

            await this.userService.UpdateUserDataAsync(editModel, user);

            return this.RedirectToAction("Index", "User", new { id = editModel.Id, message = GlobalConstants.SuccessfullyEditProductMessage });
        }

        private bool IsUserEdited(EditUserViewModel editModel, ApplicationUser user)
        {
            if (editModel.FirstName == user.FirstName && editModel.LastName == user.LastName &&
                editModel.Email == user.Email && editModel.PhoneNumber == user.PhoneNumber &&
                editModel.AddressText == user.Address.AddressText)
            {
                return false;
            }

            return true;
        }
    }
}
