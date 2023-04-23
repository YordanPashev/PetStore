namespace PetStore.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    public class OrderInfoViewModel
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public string ProductId { get; set; }
    }
}
