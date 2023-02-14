namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class CreateProductViewModel
    {
        public CreateProductViewModel() => this.Categories = new HashSet<CategoryShortInfoViewModel>();

        public ProductInfoViewModel ProductInfo { get; set; }

        public ICollection<CategoryShortInfoViewModel> Categories { get; set; }

        public string UserMessage { get; set; }
    }
}
