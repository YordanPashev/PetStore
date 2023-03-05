namespace PetStore.Web.Infrastructures
{
    using Microsoft.AspNetCore.Mvc;
    using PetStore.Web.Controllers;
    using PetStore.Web.ViewModels.User;

    public class UserControllerExtension : BaseController
    {
        //public IActionResult EditAndRedirectOrReturnInvalidInputMessage(EditUserViewModel editModel, UserDetailsViewModel user)
        //{
        //    if (!this.IsPetEdited(editModel, pet))
        //    {
        //        return this.RedirectToAction("Edit", new { id = editModel.Id, message = GlobalConstants.EditMessage });
        //    }

        //    await this.petsService.UpdatePetDataAsync(editModel, pet, petType);
        //    return this.RedirectToAction("Details", new { id = editModel.Id, message = GlobalConstants.SuccessfullyEditProductMessage });
        //}
    }
}
