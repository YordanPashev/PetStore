﻿namespace PetStore.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using PetStore.Services.Data;
    using PetStore.Web.ViewModels.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
            => this.settingsService = settingsService;

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }
    }
}
