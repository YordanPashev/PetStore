namespace PetStore.Web.ViewModels.Products
{
    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class EditProductViewModel : CreateProductViewModel, IMapFrom<Product>, IMapTo<Product>
    {
    }
}
