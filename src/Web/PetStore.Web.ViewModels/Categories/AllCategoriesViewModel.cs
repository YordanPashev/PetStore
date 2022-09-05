namespace PetStore.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class AllCategoriesViewModel
    {
        public ICollection<CategoryProdutsViewModel> AllCategories { get; set; }
    }
}
